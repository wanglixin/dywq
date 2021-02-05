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



    public class AdminStatisticInfoDto : StatisticInfoDto
    {
        /// <summary>
        /// 惠企政策文章数
        /// </summary>
        public int PolicyArticleTotalCount { get; set; }

        /// <summary>
        /// 企业对接信息总数
        /// </summary>
        public int CooperationInfoTotalCount { get; set; }



        /// <summary>
        /// 企业名录
        /// </summary>
        public int CompanyCount { get; set; }

        /// <summary>
        /// 通知公告
        /// </summary>
        public int NewsCount { get; set; }

        /// <summary>
        /// 招建安环
        /// </summary>
        public int InvestmentCount { get; set; }

        /// <summary>
        /// 企业党建
        /// </summary>
        public int PartyBuildingArticleCount { get; set; }

        /// <summary>
        /// 惠企政策
        /// </summary>
        public int PolicyArticleCount { get; set; }

        /// <summary>
        /// 专家智库
        /// </summary>
        public int ExpertCount { get; set; }


        /// <summary>
        /// 关于我们
        /// </summary>
        public int AboutusCount { get; set; }

    }



    public class EditorStatisticInfoDto : AdminStatisticInfoDto
    {

    }
}
