using MaxSoftTechAssignment.BLL.DTOs.AccountDtos;
using MaxSoftTechAssignment.DAL;
using MaxSoftTechAssignment.DAL.Data;
using MaxSoftTechAssignment.DAL.Entities;
using Microsoft.AspNetCore.Identity;

namespace MaxSoftTechAssignment.WEB;

public static class DataSeeding
{
    public static async Task SeedDataAsync(UserManager<User> userManager,RoleManager<Role> roleManager
        ,ApplicationDbContext dbContext,IConfiguration configuration)
    {
        IConfigurationSection adminSection = configuration.GetSection("Admin");

        var user = new SignIn
        {
            UserName = adminSection["Username"],
            Password = adminSection["Password"]
        };
        string email = adminSection["Email"];

        var adminUser = await userManager.FindByNameAsync(user.UserName);
        if ( adminUser != null)
        {
            var hashedPassword = userManager.PasswordHasher.HashPassword(adminUser, user.Password);

            if(adminUser.PasswordHash == hashedPassword && adminUser.Email == email) return;

            adminUser.PasswordHash = hashedPassword;
            adminUser.Email = email;

            var updateResult = await userManager.UpdateAsync(adminUser);

            if (!updateResult.Succeeded) throw new Exception("Admin password is not updated");
            
            return;
        }

        adminUser = new User()
        {
            UserName = user.UserName,
            Email = email,
        };
        
        var result = await userManager.CreateAsync(adminUser, user.Password);

        if (!result.Succeeded) throw new Exception("Admin is not created");

        var role = await roleManager.FindByNameAsync(Constants.Admin);

        if (role == null) throw new Exception("Role not found");

        await userManager.AddToRoleAsync(adminUser, role.Name);
    
    }
}