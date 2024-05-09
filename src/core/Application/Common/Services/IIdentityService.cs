using Application.Requests.Authentication.Models;
using Application.Requests.Users.Models;
using MediatR;
using Shared.ServiceContracts;

namespace Application.Common.Services;

public interface IIdentityService : IScopedService
{
    Task<IResponse<AuthenticateResponse>> SignInUserASync(LoginUserRequest loginUserRequest);
    Task<IResponse<Unit>> SignOutUserASync(string userId);
    Task<IResponse<string>> GetUserFullNameAsync(string userId);
    Task<IResponse<string>> GetUserEmailAsync(string userId);
    Task<IResponse<List<UserVm>>> GetUsersAsync();
    Task<IResponse<UserVm>> GetUserAsync(string id);
    Task<IResponse<string>> CreateUserASync(CreateUserRequest createUserRequest);
    Task<IResponse<UserVm>> UpdateUserAsync(UpdateUserRequest updateUserRequest);
    Task<IResponse<Unit>> DeleteUserAsync(string id);
}