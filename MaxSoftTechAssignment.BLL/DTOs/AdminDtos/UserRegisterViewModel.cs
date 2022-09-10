using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MaxSoftTechAssignment.BLL.DTOs.AdminDtos;

public class UserRegisterViewModel
{
    [Required, MinLength(3), MaxLength(60)] public string Name { get; set; } = null!;
    [Required, MinLength(3), MaxLength(60)] public string UserName { get; set; } = null!;
    [Required, EmailAddress] public string Email { get; set; } = null!;
    [Required, PasswordPropertyText] public string Password { get; set; } = null!;
    [Required, MaxLength(60)] public string[] Roles { get; set; } = null!;
}