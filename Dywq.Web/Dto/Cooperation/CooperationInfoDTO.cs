using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dywq.Web.Dto.Cooperation
{
    public class CooperationInfoDTO
    {
        public int Id { get; set; }


        public int CompanyId { get; set; }


        public string CompanyName { get; set; }

        /// <summary>
        /// 类型id
        /// </summary>
        public int CooperationTypeId { get; set; }

        public string CooperationTypeName { get; set; }


        /// <summary>
        /// 标题
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 内容
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

        public DateTime CreatedTime { get; set; }

        public DateTime? UpdatedTime { get; set; }

        public int Status { get; set; }

        public string StatusStr
        {
            get
            {
                return Status switch
                {
                    0 => "待审核",
                    -1 => "未通过",
                    1 => "审核通过",
                    _ => "未知状态",
                };
            }
        }
    }
}
