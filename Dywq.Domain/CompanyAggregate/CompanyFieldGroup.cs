using Dywq.Domain.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dywq.Domain.CompanyAggregate
{
    /// <summary>
    /// 字段分组
    /// </summary>
    public class CompanyFieldGroup : Entity<int>, IAggregateRoot
    {
        /// <summary>
        /// 组名
        /// </summary>
        public string Name { get; set; }


        /// <summary>
        /// 排序
        /// </summary>
        public int Sort { get; set; }
    }
}
