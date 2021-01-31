using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dywq.Web.Dto.Commpany
{
    public class CompanyNewsDto
    {
        public int Id { get; set; }
        public int CompanyId { get; set; }

        public string CompanyName { get; set; }

        /// <summary>
        /// 标题
        /// </summary>
        public string Title { get; set; }



        /// <summary>
        /// 类型id
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
        /// 审核状态，0：企业用户新增给编辑审核，失败后可以编辑  1：编辑提交给管理员待审核，失败后可以编辑，2：审核通过   -1：审核失败
        /// </summary>
        public int Status { get; set; }


        public string StatusStr
        {
            get
            {
                return Status switch
                {
                    0 => "等待编辑审核",
                    1 => "等待管理员审核",
                    2 => "审核通过",
                    -1 => "审核失败",
                    _ => "未知状态",
                };
            }
        }


        /// <summary>
        /// 排序
        /// </summary>
        public int Sort { get; set; }


        public DateTime CreatedTime { get; set; }
        public DateTime? UpdatedTime { get; set; }
    }
}
