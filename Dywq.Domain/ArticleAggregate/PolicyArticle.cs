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
    }
}
