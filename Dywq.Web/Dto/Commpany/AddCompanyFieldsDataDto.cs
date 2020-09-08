using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dywq.Web.Dto.Commpany
{

    public class FieldDataItemDto
    {
        public int FieldId { get; set; }
        public string Value { get; set; }
        public int Type { get; set; }
        public string Alias { get; set; }

        public bool Required { get; set; }

        public string Name { get; set; }
    }
}
