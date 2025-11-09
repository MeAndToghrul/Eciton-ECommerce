using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eciton.Application.ReadModels
{
    public class CategoryFieldReadModel
    {
        public string Id { get; set; }
        public string CategoryId { get; set; }
        public string FieldName { get; set; }
        public string DataType { get; set; }        
    }
}
