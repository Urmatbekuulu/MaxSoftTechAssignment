namespace MaxSoftTechAssignment.DAL.Entities;

public class Product:BaseEntity
{
    public string Name { get; set; } = null!;
    public int Quantity { get; set; }
    public decimal Price { get; set; }
    public int ShopId { get; set; }
    public Shop Shop { get; set; } = null!;

}