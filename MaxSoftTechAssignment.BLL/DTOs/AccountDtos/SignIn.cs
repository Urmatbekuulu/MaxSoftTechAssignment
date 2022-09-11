using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MaxSoftTechAssignment.BLL.DTOs.AccountDtos;

public class SignIn
{
    [Required,MinLength(5),MaxLength(60)]
    public string UserName { get; set; } = null!;

    [Required,MinLength(3),MaxLength(60),PasswordPropertyText]
    public string Password { get; set; } = null!;

}