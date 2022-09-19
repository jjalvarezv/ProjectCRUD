using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DomainLayer.DTOs;
public class ProductDto
{
    public int ProductId { get; set; }
    public string Name { get; set; } = null!;
    public string ProductNumber { get; set; } = null!;
    public string? Size { get; set; }
    public decimal? Weight { get; set; }  
    public decimal StandardCost { get; set; }
    public decimal ListPrice { get; set; }
    public DateTime SellStartDate { get; set; }
}
