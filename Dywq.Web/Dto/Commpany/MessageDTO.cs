using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dywq.Web.Dto.Commpany
{
    public class MessageDTO
    {

        public int Id { get; set; }

        public int CompanyId { get; set; }

        /// <summary>
        /// 0 惠企政策 
        /// </summary>
        public int Type { get; set; }

        /// <summary>
        /// 0 默认 未阅读  1：已阅读
        /// </summary>
        public int IsRead { get; set; }

        /// <summary>
        /// 关联id 对应内容的id
        /// </summary>
        public int AssociationId { get; set; }

        /// <summary>
        /// 消息内容
        /// </summary>
        public string Content { get; set; }

        public DateTime CreatedTime { get; set; }

        public DateTime? UpdatedTime { get; set; }

        public string TypeStr
        {
            get
            {
                return this.Type switch
                {
                    0 => "惠企政策",
                    1 => "通知公告",
                    _ => "",
                };
            }
        }


        public string Link
        {
            get
            {
                return this.Type switch
                {
                    0 => $"/article/policyDetail/{AssociationId}?_source=user",
                    1 => $"/news/detail/{AssociationId}?_source=user",
                    _ => "",
                };
            }
        }
    }
}
