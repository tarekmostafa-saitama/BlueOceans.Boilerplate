using Application.Common.Services;
using Forbids;
using MediatR;

namespace Application.Requests.Authentication.Commands;

public record LogOutCommand : IRequest<IResponse<Unit>>;

internal sealed class LogOutCommandHandler : IRequestHandler<LogOutCommand, IResponse<Unit>>
{
    private readonly ICurrentUserService _currentUserService;
    private readonly IForbid _forbid;
    private readonly IIdentityService _identityService;


    public LogOutCommandHandler(ICurrentUserService currentUserService, IForbid forbid,
        IIdentityService identityService)
    {
        _currentUserService = currentUserService;
        _forbid = forbid;
        _identityService = identityService;
    }

    public async Task<IResponse<Unit>> Handle(LogOutCommand request, CancellationToken cancellationToken)
    {
        var userId = _currentUserService.Id;
        _forbid.NullOrEmpty(userId);
        return await _identityService.SignOutUserASync(userId);
    }
}