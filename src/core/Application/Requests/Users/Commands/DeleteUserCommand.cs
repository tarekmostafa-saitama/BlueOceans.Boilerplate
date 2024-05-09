using Application.Common.Services;
using MediatR;

namespace Application.Requests.Users.Commands;

public record DeleteUserCommand(string UserId) :IRequest<IResponse<Unit>> ;

internal sealed class DeleteUserCommandHandler(IIdentityService identityService) : IRequestHandler<DeleteUserCommand, IResponse<Unit>>
{
    public async Task<IResponse<Unit>> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
    {
        return await identityService.DeleteUserAsync(request.UserId); 
    }
}