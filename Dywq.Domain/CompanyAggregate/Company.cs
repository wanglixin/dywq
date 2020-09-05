using Dywq.Domain.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dywq.Domain.CompanyAggregate
{

    /// <summary>
    /// 企业，暂时没有多余字段
    /// </summary>
    public class Company : Entity<int>, IAggregateRoot
    {
        public string Logo { get; set; }
    }
}
