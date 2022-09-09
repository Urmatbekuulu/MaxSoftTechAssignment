using System.ComponentModel.DataAnnotations.Schema;

namespace MaxSoftTechAssignment.DAL.Entities;

public class Shop:BaseEntity
{
    public string Name { get; set; } = null!;
    public string? ManagerId { get; set; } = null!;
    public string? SalesmanId { get; set; } = null!;
    [ForeignKey("ManagerId")]
    public User? Manager { get; set; }
    [ForeignKey("SalesmanId")]
    public User? Salesman { get; set; }

}