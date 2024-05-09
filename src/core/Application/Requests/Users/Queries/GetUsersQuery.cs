using Application.Common.Services;
using Application.Repositories;
using Application.Requests.Users.Models;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Application.Requests.Users.Queries;

public record GetUsersQuery : IRequest<IResponse<List<UserVm>>>;

internal sealed class GetUsersQueryHandler(IIdentityService identityService) : IRequestHandler<GetUsersQuery, IResponse<List<UserVm>>>
{
    public async Task<IResponse<List<UserVm>>> Handle(GetUsersQuery request, CancellationToken cancellationToken)
    {
        return await identityService.GetUsersAsync();

    }
}
