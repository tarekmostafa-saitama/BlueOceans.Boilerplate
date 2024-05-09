using Application.Common.Services;
using Application.Requests.Users.Models;
using MediatR;

namespace Application.Requests.Users.Queries;

public record GetUserQuery(string Id) : IRequest<IResponse<UserVm>>;

internal sealed class GetUserQueryHandler(IIdentityService identityService) : IRequestHandler<GetUserQuery, IResponse<UserVm>>
{
    public async Task<IResponse<UserVm>> Handle(GetUserQuery request, CancellationToken cancellationToken)
    {
        return await identityService.GetUserAsync(request.Id);
    }
}
