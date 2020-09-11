using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dywq.Web.Dto.Commpany
{
    public class CompanyDTO
    {
        public string Logo { get; set; }
        public int CompanyId { get; set; }

        public Int64 Rowid { get; set; }

        public string Name { get; set; }

        public string Industry { get; set; }
        public string BusinessNature { get; set; }
        public string IsPartyBranch { get; set; }
        public DateTime CreatedTime { get; set; }

    }
}
