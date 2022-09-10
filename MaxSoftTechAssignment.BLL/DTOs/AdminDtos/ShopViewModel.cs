using System.ComponentModel.DataAnnotations;

namespace MaxSoftTechAssignment.BLL.DTOs.AdminDtos;

public class ShopViewModel
{
    [Required] public string Name { get; set; } = null!;
    public string? ManagerId { get; set; }
    public string? SalesmanId { get; set; }
}