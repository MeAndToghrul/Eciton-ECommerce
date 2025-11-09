using Eciton.Domain.Entities.Common;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eciton.Domain.Entities.Entity
{
    public class Category : BaseEntity
    {
        public string Name { get; set; } 
        public string CategoryImage { get; set; }
        public ICollection<CategoryField> Fields { get; set; }
        public ICollection<Product> Products { get; set; }
    }
}
