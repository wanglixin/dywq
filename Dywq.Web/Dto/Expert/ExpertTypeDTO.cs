using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dywq.Web.Dto.Expert
{
    public class ExpertTypeDTO
    {
        public int Id { get; set; }

        public string Name { get; set; }
        public int Sort { get; set; }

        public DateTime CreatedTime { get; set; }


        public DateTime? UpdatedTime { get; set; }
    }
}
