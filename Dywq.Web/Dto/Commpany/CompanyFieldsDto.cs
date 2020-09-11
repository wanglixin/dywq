using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dywq.Web.Dto.Commpany
{
    public class CompanyFieldsDTO
    {
        public IList<CompanyFieldGroupItem> Groups { get; set; }

        public class CompanyFieldGroupItem
        {
            public string Name { get; set; }
            public int Id { get; set; }
            public int Sort { get; set; }

            public IList<CompanyFieldItem> CompanyFieldItems { get; set; }

            public class CompanyFieldItem
            {
                public int Sort { get; set; }
                public int Id { get; set; }

                /// <summary>
                /// 显示的名称
                /// </summary>
                public string Name { get; set; }

                public int GroupId { get; set; }

                public string Alias { get; set; }


                /// <summary>
                /// 字段类型：0文本框字符串 1文本框数字 2日期  3单选框 4复选框 5 下拉框
                /// </summary>
                public int Type { get; set; }


                /// <summary>
                /// 是否必填 必上传
                /// </summary>
                public bool Required { get; set; }

                /// <summary>
                /// 值
                /// </summary>
                public string Value { get; set; }


                public IList<CompanyFieldDefaultValueItem> CompanyFieldDefaultValues { get; set; }

                public class CompanyFieldDefaultValueItem
                {
                    public int Sort { get; set; }
                    public int Id { get; set; }


                    /// <summary>
                    /// value
                    /// </summary>
                    public string Value { get; set; }

                    /// <summary>
                    /// text
                    /// </summary>

                    public string Text { get; set; }

                    public int FieldId { get; set; }

                }
            }
        }

    }



    public class CompanyEditDto
    {
        public CompanyFieldsDTO Fields { get; set; }
        public string Logo { get; set; }
        public int CompanyId { get; set; }

        public string Name { get; set; }
    }

}
