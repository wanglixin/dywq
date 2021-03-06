﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dywq.Web.Dto.Investment
{
    public class InvestmentInfoDTO
    {
        public int Id { get; set; }


        public int CompanyId { get; set; }


        public string CompanyName { get; set; }

        /// <summary>
        /// 类型id
        /// </summary>
        public int InvestmentTypeId { get; set; }

        public string InvestmentTypeName { get; set; }


        /// <summary>
        /// 标题
        /// </summary>
        public string Title { get; set; }

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

        public DateTime CreatedTime { get; set; }

        public DateTime? UpdatedTime { get; set; }

        public int Status { get; set; }

        public string StatusStr
        {
            get
            {
                return Status switch
                {
                    0 => "等待审核",
                    1 => "审核通过",
                    //1 => "等待管理员审核",
                    //2 => "审核通过",
                    -1 => "审核失败",
                    _ => "未知状态",
                };
            }
        }


        public string Describe { get; set; }

    }
}
