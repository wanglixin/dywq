using Dywq.Infrastructure.Core;
using Dywq.Web.Dto.Article;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dywq.Web.Dto.Commpany
{
    public class StatisticInfoDto
    {
        /// <summary>
        /// 融资贷款
        /// </summary>
        public int FinancingCount { get; set; }

        /// <summary>
        /// 企业信息审核
        /// </summary>
        public int CompanyInfoCount { get; set; }

        /// <summary>
        /// 企业对接信息 
        /// </summary>
        public int CooperationInfoCount { get; set; }

        /// <summary>
        /// 采购需求
        /// </summary>
        public int PurchaseCount0 { get; set; }

        /// <summary>
        /// 生产需求
        /// </summary>
        public int PurchaseCount1 { get; set; }




    }



    public class AdminStatisticInfoDto: StatisticInfoDto
    {
        /// <summary>
        /// 惠企政策文章数
        /// </summary>
        public int PolicyArticleTotalCount { get; set; }

        /// <summary>
        /// 企业对接信息总数
        /// </summary>
        public int CooperationInfoTotalCount { get; set; }
    }
}
