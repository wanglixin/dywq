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
        /// <summary>
        /// 企业名称：新增的字段，最终考虑还是放在这里
        /// </summary>
        public string Name { get; set; }


        public string Logo { get; set; }

        /// <summary>
        /// 企业类型id
        /// </summary>
        public int CompanyTypeId { get; set; }


        /// <summary>
        /// 企业介绍图片
        /// </summary>
        public string IntroduceImage { get; set; }

        /// <summary>
        /// 企业介绍，概况
        /// </summary>
        public string Introduce { get; set; }

        /// <summary>
        /// 主营业务
        /// </summary>
        public string MainBusiness { get; set; }

        /// <summary>
        /// 合作内容
        /// </summary>
        public string CooperationContent { get; set; }

        /// <summary>
        /// 联系方式，联系信息
        /// </summary>
        public string Contact { get; set; }

        /// <summary>
        /// 是否展示
        /// </summary>
        public bool Show { get; set; }

        /// <summary>
        /// 审核状态，0:企业未提交信息状态，待提交信息（不需要审核） 1：提交信息审核，待审核 2：审核通过 -1：审核失败
        /// </summary>
        public int Status { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        public int Sort { get; set; }


        /// <summary>
        /// 企业登陆次数，所有绑定用户的之和
        /// </summary>
        public int LoginCount { get; set; }
    }
}
