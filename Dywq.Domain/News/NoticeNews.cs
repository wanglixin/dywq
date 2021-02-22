using Dywq.Domain.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dywq.Domain.News
{
    /// <summary>
    /// 通知新闻
    /// </summary>
    public class NoticeNews : Entity<int>, IAggregateRoot
    {
        /// <summary>
        /// 主题
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 来源
        /// </summary>
        public string Source { get; set; }

        /// <summary>
        /// 内容
        /// </summary>
        public string Content { get; set; }


        /// <summary>
        /// 图片
        /// </summary>
        public string Pic { get; set; }


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
