using Microsoft.AspNetCore.Identity;

namespace MaxSoftTechAssignment.DAL.Entities;

public class User:IdentityUser
{

    public string? Name { get; set; } = null!;
    
    public virtual List<Role>? Roles { get; set; }
}