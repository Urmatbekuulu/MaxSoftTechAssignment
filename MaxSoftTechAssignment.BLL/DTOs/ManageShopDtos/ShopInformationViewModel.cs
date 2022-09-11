namespace MaxSoftTechAssignment.BLL.DTOs.ManageShopDtos;

public class ShopInformationViewModel
{
    public string Name { get; set; } = null!;
    public string? ManagerUsername { get; set; }
    public string? SalesmanUsername { get; set; }
    public int ProductTypesCount { get; set; }
    
}