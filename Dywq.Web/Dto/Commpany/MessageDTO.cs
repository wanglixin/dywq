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
                switch (this.Type)
                {
                    case 0: return "惠企政策";
                    default:return "";
                }
            }
        }
    }
}
