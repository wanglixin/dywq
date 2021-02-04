using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dywq.Web.Dto.Purchase
{
    public class PurchaseDTO
    {
        public int Id { get; set; }

        public int CompanyId { get; set; }


        public string CompanyName { get; set; }

        /// <summary>
        /// 手机号
        /// </summary>
        public string Mobile { get; set; }


        public string EncryptedMobile
        {
            get
            {
                return $"1*****{ Mobile.Substring(9, 2)}";
            }
        }

        /// <summary>
        /// 产品名称
        /// </summary>
        public string ProductName { get; set; }

        /// <summary>
        /// 0:采购  1：生产
        /// </summary>
        public int Type { get; set; }

        /// <summary>
        /// 联系人名称
        /// </summary>
        public string Contacts { get; set; }

        /// <summary>
        /// 详细说明
        /// </summary>
        public string Content { get; set; }


        /// <summary>
        /// 是否显示
        /// </summary>
        public bool Show { get; set; }


        /// <summary>
        /// 排序
        /// </summary>
        public int Sort { get; set; }


        /// <summary>
        /// 审核状态，0：企业用户新增给编辑审核，失败后可以编辑  1：审核通过   -1：审核失败
        /// </summary>
        public int Status { get; set; }

        public string StatusStr
        {
            get
            {
                return Status switch
                {
                    0 => "等待审核",
                    1 => "审核通过",
                    //1 => "等待管理员审核",
                    //2 => "审核通过",
                    -1 => "审核失败",
                    _ => "未知状态"
                };
            }
        }

        public DateTime CreatedTime { get; set; }

        public DateTime? UpdatedTime { get; set; }
    }
}
