using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dywq.Web.Dto.Commpany
{
    public class CompanyInfoDTO
    {

        /// <summary>
        /// 企业id
        /// </summary>
        public int CompanyId { get; set; }

        public string CompanyName { get; set; }


        /// <summary>
        /// 企业类型id
        /// </summary>
        public int CompanyTypeId { get; set; }

        /// <summary>
        /// 企业类型名称
        /// </summary>
        public string CompanyTypeName { get; set; }

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
        /// 审核状态，0:企业未提交信息状态，待提交信息（不需要审核） 1：提交信息审核，待审核 2：审核通过 -1：审核失败
        /// </summary>
        public int Status { get; set; }


        public string StatusStr
        {
            get
            {
                return Status switch
                {
                    0 => "等待提交信息",
                    1 => "待审核",
                    2 => "审核通过",
                    -1 => "审核失败，请重新提交",
                    _ => "未知状态",
                };
            }
        }

        /// <summary>
        /// 排序
        /// </summary>
        public int Sort { get; set; }

        /// <summary>
        /// 是否展示
        /// </summary>
        public bool Show { get; set; }

        public DateTime? UpdatedTime { get; set; }



    }
}
