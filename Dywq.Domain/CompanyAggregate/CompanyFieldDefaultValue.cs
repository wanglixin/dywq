using Dywq.Domain.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dywq.Domain.CompanyAggregate
{
    /// <summary>
    /// 字段默认值 提供数据源绑定
    /// </summary>
    public class CompanyFieldDefaultValue : Entity<int>, IAggregateRoot
    {
        /// <summary>
        /// 字段id
        /// </summary>
        public int CompanyFieldId { get; set; }

        /// <summary>
        /// value
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        /// text
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        public int Sort { get; set; }
    }
}
