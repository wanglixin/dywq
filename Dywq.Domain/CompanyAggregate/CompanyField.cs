using Dywq.Domain.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dywq.Domain.CompanyAggregate
{

    /// <summary>
    /// 企业信息字段
    /// </summary>
    public class CompanyField : Entity<int>, IAggregateRoot
    {
        /// <summary>
        /// 组id
        /// </summary>
        public int GroupId { get; set; }

        /// <summary>
        /// 字段名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        public int Sort { get; set; }

        /// <summary>
        /// 字段类型：0：文本框字符串 1：文本框数字   2：下拉框
        /// </summary>
        public int Type { get; set; }

        /// <summary>
        /// 别名，方便查询
        /// </summary>
        public string Alias { get; set; }


    }
}
