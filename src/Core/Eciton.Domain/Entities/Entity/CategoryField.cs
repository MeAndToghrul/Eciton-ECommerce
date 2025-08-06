using Eciton.Domain.Entities.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eciton.Domain.Entities.Entity
{
    public class CategoryField : BaseEntity
    {
        public string CategoryId { get; set; }
        public Category Category { get; set; }

        public string FieldName { get; set; }   
        public string DataType { get; set; }    
    }

}
