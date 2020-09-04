using Dywq.Domain.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dywq.Domain.CompanyAggregate
{
    /// <summary>
    /// 企业信息
    /// </summary>
    public class CompanyFieldData : Entity<int>, IAggregateRoot
    {
        /// <summary>
        /// 企业id
        /// </summary>
        public int CompanyId { get; set; }

        /// <summary>
        /// 字段id
        /// </summary>
        public int FieldId { get; set; }

        /// <summary>
        /// 选择的CompanyFieldDefaultValue.Id 或 填写的内容
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        /// 字段类型：0：文本框数字  1：文本框字符串 2：下拉框
        /// </summary>
        public int Type { get; set; }

        /// <summary>
        /// 别名，方便查询
        /// </summary>
        public string Alias { get; set; }
    }
}
