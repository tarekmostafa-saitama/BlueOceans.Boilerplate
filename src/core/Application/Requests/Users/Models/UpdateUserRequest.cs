namespace Application.Requests.Users.Models;

public class UpdateUserRequest
{
    public string Id { get; set; }
    public string FullName { get; set; }
    public string Email { get; set; }
    public string UserName { get; set; }

}