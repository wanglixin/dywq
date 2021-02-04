using Dywq.Domain.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dywq.Domain.InvestmentAggregate
{
    /// <summary>
    /// 企业对接合作类别
    /// </summary>
    public class InvestmentType : Entity<int>, IAggregateRoot
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
