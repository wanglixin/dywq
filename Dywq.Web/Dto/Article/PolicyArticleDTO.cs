using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dywq.Web.Dto.Article
{
    public class PolicyArticleDTO
    {
        public int Id { get; set; }


        public DateTime CreatedTime { get; set; }

        public DateTime? UpdatedTime { get; set; }

        /// <summary>
        /// 主题
        /// </summary>
        public string ThemeTitle { get; set; }

        /// <summary>
        /// 政策类型id
        /// </summary>
        public int PolicyTypeId { get; set; }


        /// <summary>
        /// 政策类型名称
        /// </summary>
        public string PolicyTypeName { get; set; }

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
        /// 来源
        /// </summary>
        public string Source { get; set; }

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

        public string Describe { get; set; }
    }
}
