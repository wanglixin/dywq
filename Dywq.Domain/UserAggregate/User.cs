using Dywq.Domain.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dywq.Domain.UserAggregate
{

    /// <summary>
    /// 用户
    /// </summary>
    public class User : Entity<int>, IAggregateRoot
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


        /// <summary>
        /// 真实姓名
        /// </summary>
        public string RealName { get; set; }

        /// <summary>
        /// 手机号码
        /// </summary>
        public string Mobile { get; set; }

        /// <summary>
        /// 身份证
        /// </summary>
        public string IDCard { get; set; }

        /// <summary>
        /// 登陆次数
        /// </summary>
        public int LoginCount { get; set; }
    }
}
