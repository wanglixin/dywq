using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dywq.Web.Dto.Guestbook
{
    public class GuestbookDTO
    {

        public int Id { get; set; }

        /// <summary>
        /// 留言用户，或管理员id
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// 内容
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// 留言类型：0：用户留言 1：管理员回复
        /// </summary>
        public int Type { get; set; }


        /// <summary>
        /// 回复的留言id，默认为0
        /// </summary>
        public int ReplyId { get; set; }

        public DateTime CreatedTime { get; set; }
        public DateTime? UpdatedTime { get; set; }

        /// <summary>
        /// 企业logo
        /// </summary>
        public string CompanyLogo { get; set; }


        /// <summary>
        /// 企业名称
        /// </summary>
        public string CompanyName { get; set; }


        /// <summary>
        /// 回复内容
        /// </summary>
        public GuestbookDTO GuestbookReply { get; set; }

    }
}
