﻿using Dywq.Domain.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dywq.Domain.Expert
{
    /// <summary>
    /// 专家类型
    /// </summary>
    public class ExpertType : Entity<int>, IAggregateRoot
    {

        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }


        /// <summary>
        /// 排序
        /// </summary>
        public int Sort { get; set; }
    }
}