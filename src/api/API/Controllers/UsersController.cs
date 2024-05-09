using Application.Common.Models;
using Application.Requests.Authentication.Commands;
using Application.Requests.Users.Commands;
using Application.Requests.Users.Models;
using Application.Requests.Users.Queries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    public class UsersController : ApiControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<IResponse<List<UserVm>>>> GetUsersAsync()
        {
            var x = Request;
            return Ok(await Mediator.Send(new GetUsersQuery()));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<IResponse<UserVm>>> GetUsersAsync(string id)
        {
            return Ok(await Mediator.Send(new GetUserQuery(id)));
        }

        [HttpPost]
        public async Task<ActionResult<IResponse<string>>> CreateUserAsync(CreateUserRequest registerUserRequest)
        {

           await Mediator.Send(new CreateUserCommand(registerUserRequest));
            return new CreatedAtActionResult(nameof(CreateUserAsync),
                "Users" , null,null); 
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<IResponse<UserVm>>> UpdateUserAsync(UpdateUserRequest updateUserRequest)
        {

           
            return Ok(await Mediator.Send(new UpdateUserCommand(updateUserRequest)));
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<IResponse<string>>> DeleteUserAsync(string id)
        {
            return Ok(await Mediator.Send(new DeleteUserCommand(id)));
        }

    }
}
