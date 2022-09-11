using System.ComponentModel.DataAnnotations;

namespace MaxSoftTechAssignment.BLL.DTOs.ManageShopDtos;

public class ProductViewModel
{
    public int? Id { get; set; }


    [Required,MinLength(1),MaxLength(100)]
    public string Name { get; set; } = null!;

    [Range(0,int.MaxValue)]
    public int Quantity { get; set; }
    [Range(0,Double.MaxValue)]
    public decimal Price { get; set; }
    [Required]
    public int ShopId { get; set; }
}