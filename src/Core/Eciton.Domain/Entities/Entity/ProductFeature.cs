using Eciton.Domain.Entities.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eciton.Domain.Entities.Entity
{
    public class ProductFeature : BaseEntity
    {
        public string ProductId { get; set; }
        public Product Product { get; set; }

        public string Key { get; set; }         
        public string Value { get; set; }       
        public string DataType { get; set; }    
    }
}
