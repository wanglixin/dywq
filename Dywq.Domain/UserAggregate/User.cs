using Dywq.Domain.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dywq.Domain.UserAggregate
{

    /// <summary>
    /// 用户
    /// </summary>
    public class User: Entity<int>, IAggregateRoot
    {
        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// 用户类型 0：企业用户  1：管理员
        /// </summary>
        public int Type { get; set; }
    }
}
