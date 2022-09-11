namespace MaxSoftTechAssignment.BLL.DTOs.ManageShopDtos;

public class ProductsListViewModel
{
    public int? Id { get; set; } 
    public string Name { get; set; } = null!;
    public int Quantity { get; set; }
    public decimal Price { get; set; }
}