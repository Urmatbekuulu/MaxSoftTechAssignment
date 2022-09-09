using Microsoft.AspNetCore.Identity;

namespace MaxSoftTechAssignment.DAL.Entities;

public class User:IdentityUser
{
    public string Name { get; set; } = null!;
}