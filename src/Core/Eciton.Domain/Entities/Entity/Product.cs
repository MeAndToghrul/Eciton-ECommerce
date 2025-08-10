using Eciton.Domain.Entities.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eciton.Domain.Entities.Entity
{
    public class Product : BaseEntity
    {
        public string Brand { get; set; }
        public decimal Price { get; set; }
        public string Model { get; set; }
        public bool IsStock { get; set; } = true;
        public int StockQuantity { get; set; } 
        public string ProductImage { get; set; }
        public string CategoryId { get; set; }
        public Category Category { get; set; }
        public ICollection<ProductFeature> Features { get; set; }
        public decimal? DiscountPercentage { get; set; } 
        public DateTime? DiscountStartDate { get; set; } 
        public DateTime? DiscountEndDate { get; set; }   
    }

}
