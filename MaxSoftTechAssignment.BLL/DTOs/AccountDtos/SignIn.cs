using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MaxSoftTechAssignment.BLL.DTOs.AccountDtos;

public class SignIn
{
    [Required,MinLength(5),MaxLength(60)]
    public string Username { get; set; } = null!;

    [Required,MinLength(8),MaxLength(60),PasswordPropertyText]
    public string Password { get; set; } = null!;

}