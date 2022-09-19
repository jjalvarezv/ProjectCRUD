using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DomainLayer.DomainModels
{
    public class Product
    {
        public string Name { get; set; } = null!;
        public string ProductNumber { get; set; } = null!;
        public decimal StandardCost { get; set; }
        // Selling price
        public decimal ListPrice { get; set; }
        public string? Size { get; set; }
        public decimal? Weight { get; set; }
        public DateTime SellStartDate { get; set; }
        public DateTime? ModifiedDate { get; set; }

        public Product()
        {
            this.ModifiedDate = DateTime.Today;
        }

    }
}