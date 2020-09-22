using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dywq.Web.Dto.Expert
{
    public class ExpertGroupDTO
    {
        public ExpertTypeDTO ExpertType { get; set; }
        public IEnumerable<ExpertDTO> Experts { get; set; }
    }
}
