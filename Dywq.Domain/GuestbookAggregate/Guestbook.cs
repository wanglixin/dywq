using Dywq.Domain.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dywq.Domain.GuestbookAggregate
{
    /// <summary>
    /// 留言簿：目前留言只回复一条，回复过就不回复了
    /// </summary>
    public class Guestbook : Entity<int>, IAggregateRoot
    {
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



    }
}
