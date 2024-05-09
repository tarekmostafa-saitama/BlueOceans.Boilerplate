using Application.Common.Authentication;
using Application.Common.Exceptions;
using Application.Common.Models;
using Application.Common.Services;
using Application.Repositories;
using Application.Requests.Authentication.Models;
using Application.Requests.Users.Models;
using DocumentFormat.OpenXml.Office2010.Excel;
using Forbids;
using k8s.KubeConfigModels;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace Infrastructure.Identity.Services;

public class IdentityService(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager,
        IForbid forbid, IAuthenticateService authenticateService, IUnitOfWork unitOfWork)
    : IIdentityService
{
    private readonly IForbid _forbid = forbid;
    private readonly UserManager<ApplicationUser> _userManager = userManager;


    public async Task<IResponse<AuthenticateResponse>> SignInUserASync(LoginUserRequest loginUserRequest)
    {
        var user = await _userManager.FindByEmailAsync(loginUserRequest.Email);
        if (user == null)
            return Response.Fail<AuthenticateResponse>("Failed login attempt");
        var signInResult =
            await signInManager.PasswordSignInAsync(user, loginUserRequest.Password, false, false);

        return signInResult.Succeeded
            ? Response.Success(await authenticateService.Authenticate(user.Id, CancellationToken.None))
            : Response.Fail<AuthenticateResponse>("Failed login attempt");
    }

    public async Task<IResponse<Unit>> SignOutUserASync(string userId)
    {
        await signInManager.SignOutAsync();
        unitOfWork.RefreshTokensRepository.RemoveRange(x => x.UserId == userId);
        await unitOfWork.CommitAsync();
        return Response.Success(Unit.Value);
    }


    public async Task<IResponse<string>> GetUserFullNameAsync(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
            return Response.Fail<string>("user not found with id = " + userId);
        return Response.Success(user.FullName);
    }

    public async Task<IResponse<string>> GetUserEmailAsync(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
            return Response.Fail<string>("user not found with id = " + userId);
        return Response.Success(user.Email);
    }

    public async Task<IResponse<List<UserVm>>> GetUsersAsync()
    {
        var users = await userManager.Users.ToListAsync();
        var adaptedUsersList = users.Adapt<List<UserVm>>();

        return Response.Success(adaptedUsersList);

    }

    public async Task<IResponse<UserVm>> GetUserAsync(string id)
    {
        var user = await userManager.Users.FirstOrDefaultAsync(x=>x.Id==id);
        if (user == null)
            return Response.Fail<UserVm>("user not found with id = " + id);
        var adaptedUser = user.Adapt<UserVm>();

        return Response.Success(adaptedUser);

    }
    public async Task<IResponse<string>> CreateUserASync(CreateUserRequest createUserRequest)
    {
        //TODO: Where is the tenant id
        var user = new ApplicationUser
        {
            Email = createUserRequest.Email,
            FullName = createUserRequest.FullName,
            UserName = createUserRequest.Email,
            
        };
        var createResult = await _userManager.CreateAsync(user, createUserRequest.Password);
        return createResult.Succeeded
            ? Response.Success(user.Id)
            : Response.Fail<string>(createResult.Errors.Select(error => error.Description).ToList());
    }

    public async Task<IResponse<UserVm>> UpdateUserAsync(UpdateUserRequest updateUserRequest)
    {

        var user = await userManager.Users.FirstOrDefaultAsync(x => x.Id==updateUserRequest.Id);
        if (user == null)
            return Response.Fail<UserVm>("user not found with id = " + updateUserRequest.Id);

        //TODO: where is the tenant id
        user.FullName=updateUserRequest.FullName;
        user.Email=updateUserRequest.Email;
        user.UserName=updateUserRequest.UserName;

        var updateResult= await userManager.UpdateAsync(user);
        var adaptedUser = user.Adapt<UserVm>();
        if(updateResult.Succeeded)
            return Response.Success(adaptedUser);

        throw new NotFoundException("user not found with id = " + updateUserRequest.Id);
    }
    public async Task<IResponse<Unit>> DeleteUserAsync(string id)
    {
        var user = await userManager.Users.FirstOrDefaultAsync(x => x.Id == id);
        if (user == null)
            return Response.Fail<Unit>("user not found with id = " + id);

        var deleteResult= await userManager.DeleteAsync(user);
        if(deleteResult.Succeeded)
             return Response.Success(Unit.Value);

        throw new Exception(deleteResult.Errors.Select(error => error.Description).FirstOrDefault());


    }
}