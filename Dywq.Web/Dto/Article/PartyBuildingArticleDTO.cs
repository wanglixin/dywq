﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dywq.Web.Dto.Article
{
    public class PartyBuildingArticleDTO
    {
        public int Id { get; set; }


        public DateTime CreatedTime { get; set; }

        public DateTime? UpdatedTime { get; set; }

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
        ///  0:默认 待审核 1：审核成功 -1：审核失败
        /// </summary>
        public int Status { get; set; }


        public string StatusStr
        {
            get
            {
                return Status switch
                {
                    0 => "待审核",
                    1 => "审核通过",
                    -1 => "审核失败",
                    _ => "未知状态",
                };
            }
        }
    }
}
