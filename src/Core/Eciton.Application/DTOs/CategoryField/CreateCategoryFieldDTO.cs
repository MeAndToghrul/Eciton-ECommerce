using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eciton.Application.DTOs.CategoryField
{
    public class CreateCategoryFieldDTO
    {
        public string CategoryId { get; set; }
        public string FieldName { get; set; }
        public string DataType { get; set; }
    }
}
