﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dywq.Web.Dto.Expert
{
    public class ExpertDTO
    {

        public int Id { get; set; }

        /// <summary>
        /// 名字
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 头像图片
        /// </summary>
        public string Pic { get; set; }

        /// <summary>
        /// 职称
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 简介
        /// </summary>
        public string Introduction { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        public int Sort { get; set; }

        /// <summary>
        /// 是否显示
        /// </summary>
        public bool Show { get; set; }

        public int ExpertTypeId { get; set; }


        public string ExpertTypeName { get; set; }


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
