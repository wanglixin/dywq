using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dywq.Web.Dto.Site
{
    public class AboutUsDTO
    {
        public int Id { get; set; }

        /// <summary>
        /// 主题
        /// </summary>
        public string Title { get; set; }


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

        public DateTime CreatedTime { get; set; }

        public DateTime? UpdatedTime { get; set; }

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
