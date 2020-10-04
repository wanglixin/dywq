using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dywq.Web.Dto
{
    public class SearchDTO
    {
        /// <summary>
        /// 字段id
        /// </summary>
        public int FieldId { get; set; }


        /// <summary>
        /// 字段值
        /// </summary>
        public string Value { get; set; }


        /// <summary>
        /// 字段类型：0文本框字符串 1文本框数字 2日期  3单选框 4复选框 5 下拉框 ,-1:特殊类型 企业名称
        /// </summary>
        public int Type { get; set; }

       
    }
}
