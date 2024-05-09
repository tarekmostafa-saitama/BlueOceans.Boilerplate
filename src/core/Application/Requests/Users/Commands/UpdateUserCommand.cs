using Application.Common.Services;
using Application.Requests.Users.Models;
using MediatR;

namespace Application.Requests.Users.Commands;

public record UpdateUserCommand(UpdateUserRequest UpdateUserRequest) :IRequest<IResponse<UserVm>>;

internal sealed class UpdateUserCommandHandler(IIdentityService identityService) : IRequestHandler<UpdateUserCommand,IResponse<UserVm>>
{
    public async Task<IResponse<UserVm>> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        return await  identityService.UpdateUserAsync(request.UpdateUserRequest);
    }
}