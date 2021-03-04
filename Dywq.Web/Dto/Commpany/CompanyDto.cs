using Dywq.Web.Dto.User;
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

        /// <summary>
        /// 审核状态，0：待审核  1：审核通过   -1：审核失败
        /// [舍弃：审核状态，0:企业未提交信息状态，待提交信息（不需要审核） 1：提交信息审核，待审核 2：审核通过 -1：审核失败]
        /// </summary>
        public int Status { get; set; }

        public string StatusStr
        {
            get
            {
                return Status switch
                {
                    0 => "待审核",
                    1 => "审核通过",
                    -1 => "审核失败",
                    _ => "未知状态"
                };


            }

        }


        public bool IsEdit(LoginUserDTO loginUser)
        {
            return ((Status == 0 || Status == -1) && (loginUser.Type == 0 || loginUser.Type == 2)) || loginUser.Type == 1;
        }

        public bool IsAdmin(LoginUserDTO loginUser)
        {
            return loginUser.Type == 1;
        }
    }
}
