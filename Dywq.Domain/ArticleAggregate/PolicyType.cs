using Dywq.Domain.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dywq.Domain.ArticleAggregate
{
    /// <summary>
    /// 政策类型
    /// </summary>
    public class PolicyType : Entity<int>, IAggregateRoot
    {
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }


        /// <summary>
        /// 排序
        /// </summary>
        public int Sort { get; set; }
    }
}
