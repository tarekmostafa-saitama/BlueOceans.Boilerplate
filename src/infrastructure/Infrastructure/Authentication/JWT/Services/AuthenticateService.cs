using Application.Common.Authentication;
using Application.Requests.Authentication.Models;
using Domain.Entities;
using Infrastructure.Identity;
using Infrastructure.Persistence;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Authentication.JWT.Services;

internal sealed class AuthenticateService : IAuthenticateService
{
    private readonly IAccessTokenService _accessTokenService;
    private readonly ApplicationDbContext _context;
    private readonly IRefreshTokenService _refreshTokenService;
    private readonly UserManager<ApplicationUser> _userManager;

    public AuthenticateService(IAccessTokenService accessTokenService, IRefreshTokenService refreshTokenService,
        ApplicationDbContext context, UserManager<ApplicationUser> userManager)
    {
        _accessTokenService = accessTokenService;
        _refreshTokenService = refreshTokenService;
        _context = context;
        _userManager = userManager;
    }

    public async Task<AuthenticateResponse> Authenticate(string userId, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByIdAsync(userId);

        var refreshToken = await _refreshTokenService.Generate(user.Id);
        await _context.RefreshTokens.AddAsync(new RefreshToken { Token = refreshToken, UserId = user.Id },
            cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return new AuthenticateResponse
        {
            AccessToken = await _accessTokenService.Generate(user.Id),
            RefreshToken = refreshToken
        };
    }
}