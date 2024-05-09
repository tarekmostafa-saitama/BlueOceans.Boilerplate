using Application.Common.Services;
using Application.Requests.Users.Models;
using MediatR;

namespace Application.Requests.Users.Commands;

public record CreateUserCommand(CreateUserRequest RegisterUserRequest) : IRequest<IResponse<string>>;

internal sealed class RegisterUserCommandHandler : IRequestHandler<CreateUserCommand, IResponse<string>>
{
    private readonly IIdentityService _identityService;

    public RegisterUserCommandHandler(IIdentityService identityService)
    {
        _identityService = identityService;
    }

    public async Task<IResponse<string>> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        return await _identityService.CreateUserASync(request.RegisterUserRequest);
    }
}