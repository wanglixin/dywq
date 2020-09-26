using Dywq.Domain.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dywq.Domain.CompanyAggregate
{
    /// <summary>
    /// 消息
    /// </summary>
    public class Message : Entity<int>, IAggregateRoot
    {
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
    }
}
