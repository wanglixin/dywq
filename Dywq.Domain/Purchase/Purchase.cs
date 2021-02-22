using Dywq.Domain.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dywq.Domain.Purchase
{
    /// <summary>
    /// 采购生产需求
    /// </summary>
    public class Purchase : Entity<int>, IAggregateRoot
    {

        public int CompanyId { get; set; }
        /// <summary>
        /// 手机号
        /// </summary>
        public string Mobile { get; set; }

        /// <summary>
        /// 产品名称
        /// </summary>
        public string ProductName { get; set; }

        /// <summary>
        /// 0:采购  1：生产
        /// </summary>
        public int Type { get; set; }

        /// <summary>
        /// 联系人名称
        /// </summary>
        public string Contacts { get; set; }

        /// <summary>
        /// 详细说明
        /// </summary>
        public string Content { get; set; }


        /// <summary>
        /// 是否显示
        /// </summary>
        public bool Show { get; set; }


        /// <summary>
        /// 排序
        /// </summary>
        public int Sort { get; set; }


        /// <summary>
        /// 审核状态，0：企业用户新增给编辑审核，失败后可以编辑  1：审核通过   -1：审核失败
        /// </summary>
        public int Status { get; set; }

        public int UserId { get; set; }
    }
}
