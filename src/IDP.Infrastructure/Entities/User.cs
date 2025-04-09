using Microsoft.AspNetCore.Identity;

namespace IDP.Infrastructure.Entities;

public class User : IdentityUser
{
    public string FirstName { get; set; } = default!;
    public string LastName { get; set; } = default!;
    public string Address { get; set; } = default!;
}
