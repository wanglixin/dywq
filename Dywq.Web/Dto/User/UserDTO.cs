using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dywq.Web.Dto.User
{
    public class UserDTO
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public DateTime CreatedTime { get; set; }

        public int Type { get; set; }

        public string CompanyName { get; set; }

        public int CompanyId { get; set; }


        public string RealName { get; set; }
        public string IDCard { get; set; }
        public string Mobile { get; set; }
    }
}
