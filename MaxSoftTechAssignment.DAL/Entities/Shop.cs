using System.ComponentModel.DataAnnotations.Schema;

namespace MaxSoftTechAssignment.DAL.Entities;

public class Shop:BaseEntity
{
    public string Name { get; set; } = null!;
    public string? ManagerId { get; set; } = null!;
    public string? SalesmanId { get; set; } = null!;
    public List<Product> Products { get; set; } = new();
    [ForeignKey("ManagerId")]
    public User? Manager { get; set; }
    [ForeignKey("SalesmanId")]
    public User? Salesman { get; set; }

}