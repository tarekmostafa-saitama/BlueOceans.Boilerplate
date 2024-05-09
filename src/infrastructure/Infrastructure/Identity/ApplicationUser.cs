using Microsoft.AspNetCore.Identity;
using Shared.Contracts;

namespace Infrastructure.Identity;

public class ApplicationUser : IdentityUser
{
    public string FullName { get; set; }
}