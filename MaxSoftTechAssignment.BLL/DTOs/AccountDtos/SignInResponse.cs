namespace MaxSoftTechAssignment.BLL.DTOs.AccountDtos;

public class SignInResponse
{
    public string Token { get; set; } = null!;
    public DateTime ExpireDate { get; set; }
}