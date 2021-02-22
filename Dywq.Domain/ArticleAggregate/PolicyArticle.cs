using Dywq.Domain.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dywq.Domain.ArticleAggregate
{
    /// <summary>
    /// 政策文章
    /// </summary>
    public class PolicyArticle : Entity<int>, IAggregateRoot
    {
        /// <summary>
        /// 政策主题
        /// </summary>
        public string ThemeTitle { get; set; }

        /// <summary>
        /// 来源
        /// </summary>
        public string Source { get; set; }

        /// <summary>
        /// 内容
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// 政策类型id
        /// </summary>
        public int PolicyTypeId { get; set; }


        /// <summary>
        /// 是否显示
        /// </summary>
        public bool Show { get; set; }


        /// <summary>
        /// 排序
        /// </summary>
        public int Sort { get; set; }

        /// <summary>
        /// 0:默认 待审核 1：审核成功 -1：审核失败
        /// </summary>
        public int Status { get; set; }

        public int UserId { get; set; }

        /// <summary>
        /// 用来显示列表
        /// </summary>
        public string Describe { get; set; }
    }
}
