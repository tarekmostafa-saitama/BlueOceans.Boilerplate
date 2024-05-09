using System.Security.Claims;
using Application.Common.Authentication;
using Infrastructure.Authentication.JWT.Models;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Authentication.JWT.Services;

internal sealed class AccessTokenService : IAccessTokenService
{
    private readonly JwtSettings _jwtSettings;
    private readonly ITokenGenerator _tokenGenerator;
    private readonly UserManager<ApplicationUser> _userManager;

    public AccessTokenService(ITokenGenerator tokenGenerator, JwtSettings jwtSettings,
        UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
        (_tokenGenerator, _jwtSettings) = (tokenGenerator, jwtSettings);
    }

    public async Task<string> Generate(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);

        List<Claim> claims = new()
        {
            new Claim("id", user.Id),
            new Claim(ClaimTypes.GivenName, user.FullName),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Name, user.UserName)
        };
        return _tokenGenerator.Generate(new GenerateTokenRequest(_jwtSettings.AccessTokenSecret, _jwtSettings.Issuer,
            _jwtSettings.Audience,
            _jwtSettings.AccessTokenExpirationMinutes, claims)).Token;
    }
}