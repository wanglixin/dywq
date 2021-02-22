using Dywq.Domain.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dywq.Domain.ArticleAggregate
{
    /// <summary>
    /// 党建文章
    /// </summary>
    public class PartyBuildingArticle : Entity<int>, IAggregateRoot
    {
        /// <summary>
        /// 党建主题
        /// </summary>
        public string ThemeTitle { get; set; }

        /// <summary>
        /// 副标题
        /// </summary>
        public string Subtitle { get; set; }

        /// <summary>
        /// 来源
        /// </summary>
        public string Source { get; set; }

        /// <summary>
        /// 图片
        /// </summary>
        public string Pic { get; set; }

        /// <summary>
        /// 内容
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
        /// 0:默认 待审核 1：审核成功 -1：审核失败
        /// </summary>
        public int Status { get; set; }

        public int UserId { get; set; }
    }
}
