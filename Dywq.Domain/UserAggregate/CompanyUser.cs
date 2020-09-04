using Dywq.Domain.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dywq.Domain.UserAggregate
{
    /// <summary>
    /// 企业用户关系
    /// </summary>
    public class CompanyUser : Entity<int>, IAggregateRoot
    {

        /// <summary>
        /// 用户id
        /// </summary>
        public int UserId { get; set; }


        /// <summary>
        /// 企业id
        /// </summary>
        public int CompanyId { get; set; }

    }
}
