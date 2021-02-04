using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dywq.Web.Dto.User
{
    public class LoginUserDTO
    {
        public int Id { get; set; }
        public string UserName { get; set; }

        /// <summary>
        /// 0 普通用户 1管理员 2编辑
        /// </summary>
        public int Type { get; set; }

        public int CompanyId { get; set; }

        public string CompanyName { get; set; }

        public string Logo { get; set; }


        public string Role
        {
            get
            {
                if (this.Type == 0) return Dywq.Web.Common.Role.User;
                else if (this.Type == 1) return Dywq.Web.Common.Role.Admin;
                else if (this.Type == 2) return Dywq.Web.Common.Role.Editor;
                else return Dywq.Web.Common.Role.User;
            }
        }

        /// <summary>
        /// 展示名称
        /// </summary>
        public string ShowName
        {
            get
            {
                if (this.Type == 0 || this.Type == 2) return CompanyName;
                else if (this.Type == 1) return "管理员";
                else return "";
            }
        }



    }
}
