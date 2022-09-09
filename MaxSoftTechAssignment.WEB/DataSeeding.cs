using MaxSoftTechAssignment.BLL.DTOs.AccountDtos;
using MaxSoftTechAssignment.DAL;
using MaxSoftTechAssignment.DAL.Data;
using MaxSoftTechAssignment.DAL.Entities;
using Microsoft.AspNetCore.Identity;

namespace MaxSoftTechAssignment.WEB;

public static class DataSeeding
{
    public static async Task SeedDataAsync(UserManager<User> userManager,RoleManager<Role> roleManager,ApplicationDbContext dbContext,IConfiguration configuration)
    {
        IConfigurationSection adminSection = configuration.GetSection("Admin");

        var user = new SignIn
        {
            Username = adminSection["Username"],
            Password = adminSection["Password"]
        };
        string email = adminSection["Email"];
        
        if (await userManager.FindByNameAsync(user.Username) == null)
        {
            var adminUser = new User()
            {
                Name = user.Username,
                UserName = user.Username,
                Email = email
               
            };
            var result = await userManager.CreateAsync(adminUser, user.Password);

            if (!result.Succeeded) throw new Exception("Admin is not created");

            var role = await roleManager.FindByNameAsync(Constants.Admin);

            if (role == null) throw new Exception("Role not found");

            await userManager.AddToRoleAsync(adminUser, role.Name);
        }
    }
}