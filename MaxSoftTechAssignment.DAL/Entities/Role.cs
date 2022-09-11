using Microsoft.AspNetCore.Identity;

namespace MaxSoftTechAssignment.DAL.Entities;

public class Role:IdentityRole
{
    
    public Role(){}
    public Role(string role)
    {
        NormalizedName = role;
        Name = role;
    }
    public List<User>? Users { get; set; }

}