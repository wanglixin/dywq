using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dywq.Web.Dto.Commpany
{
    public class CompanyFieldsDto
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

                public int Type { get; set; }

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
}
