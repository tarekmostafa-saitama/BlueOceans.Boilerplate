using Application.Common.Authentication;
using Application.Repositories;
using Application.Requests.Authentication.Models;
using Forbids;
using MediatR;

namespace Application.Requests.Authentication.Commands;

public record RefreshCommand(RefreshRequest RefreshRequest) : IRequest<IResponse<AuthenticateResponse>>;

internal sealed class RefreshCommandHandler : IRequestHandler<RefreshCommand, IResponse<AuthenticateResponse>>
{
    private readonly IAuthenticateService _authenticateService;
    private readonly IForbid _forbid;
    private readonly IRefreshTokenValidator _refreshTokenValidator;
    private readonly IUnitOfWork _unitOfWork;

    public RefreshCommandHandler(IRefreshTokenValidator refreshTokenValidator,
        IAuthenticateService authenticateService,
        IUnitOfWork unitOfWork,
        IForbid forbid)
    {
        _refreshTokenValidator = refreshTokenValidator;
        _authenticateService = authenticateService;
        _unitOfWork = unitOfWork;
        _forbid = forbid;
    }

    public async Task<IResponse<AuthenticateResponse>> Handle(RefreshCommand request,
        CancellationToken cancellationToken)
    {
        var refreshRequest = request.RefreshRequest;
        var isValidRefreshToken = _refreshTokenValidator.Validate(refreshRequest.RefreshToken);
        //TODO: Make it a Clear Error 
        _forbid.False(isValidRefreshToken);

        var refreshToken =
            await _unitOfWork.RefreshTokensRepository.GetSingleAsync(x => x.Token == refreshRequest.RefreshToken,
                false); 


        _forbid.Null(refreshToken);

        _unitOfWork.RefreshTokensRepository.Remove(refreshToken);
        await _unitOfWork.CommitAsync();

        return Response.Success(await _authenticateService.Authenticate(refreshToken.UserId, cancellationToken));
    }
}