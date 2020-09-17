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
    }
}
