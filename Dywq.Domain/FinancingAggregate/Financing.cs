using Dywq.Domain.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dywq.Domain.FinancingAggregate
{
    /// <summary>
    /// 融资信息
    /// </summary>
    public class Financing : Entity<int>, IAggregateRoot
    {
        /// <summary>
        /// 对接的企业id
        /// </summary>
        public int CompanyId { get; set; }

        /// <summary>
        /// 银行
        /// </summary>
        public string Bank { get; set; }


        /// <summary>
        /// 标题
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 内容
        /// </summary>
        public string Content { get; set; }

        public string Pic { get; set; }

        /// <summary>
        /// 是否显示
        /// </summary>
        public bool Show { get; set; }


        /// <summary>
        /// 排序
        /// </summary>
        public int Sort { get; set; }


        /// <summary>
        /// 审核状态，0:企业未提交信息状态，待提交信息（不需要审核） 1：提交信息审核，待审核 2：审核通过 -1：审核失败
        /// </summary>
        public int Status { get; set; }
    }
}
