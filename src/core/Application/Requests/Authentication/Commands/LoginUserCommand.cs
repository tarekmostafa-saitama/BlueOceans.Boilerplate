using Application.Common.Services;
using Application.Requests.Authentication.Models;
using MediatR;

namespace Application.Requests.Authentication.Commands;

public record LoginUserCommand(LoginUserRequest LoginUserRequest) : IRequest<IResponse<AuthenticateResponse>>;

internal sealed class LoginUserCommandHandler : IRequestHandler<LoginUserCommand, IResponse<AuthenticateResponse>>
{
    private readonly IIdentityService _identityService;


    public LoginUserCommandHandler(IIdentityService identityService)
    {
        _identityService = identityService;
    }

    public async Task<IResponse<AuthenticateResponse>> Handle(LoginUserCommand request,
        CancellationToken cancellationToken)
    {
        return await _identityService.SignInUserASync(request.LoginUserRequest);
    }
}