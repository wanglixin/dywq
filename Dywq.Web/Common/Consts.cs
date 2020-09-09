using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dywq.Web.Common
{
    public static class CompanyFieldAlias
    {
        /// <summary>
        /// 企业名称
        /// </summary>
        public const string CompanyName = "CompanyName";

        /// <summary>
        /// 行业
        /// </summary>
        public const string Industry = "Industry";

        /// <summary>
        /// 注册地址
        /// </summary>
        public const string RegisteredAddress = "RegisteredAddress";

        /// <summary>
        /// 主营业务
        /// </summary>
        public const string MainBusiness = "MainBusiness";

        /// <summary>
        /// 主要产品
        /// </summary>
        public const string MainProducts = "MainProducts";

        /// <summary>
        /// 党员人数
        /// </summary>
        public const string PartyMemberCount = "PartyMemberCount";

        /// <summary>
        /// 是否有党员
        /// </summary>
        public const string IsPartyMember = "IsPartyMember";

        /// <summary>
        /// 是否党支部
        /// </summary>
        public const string IsPartyBranch = "IsPartyBranch";


        /// <summary>
        /// 企业性质
        /// </summary>
        public const string BusinessNature = "BusinessNature";

    }


    public static class Role
    {
        public const string User = "User";
        public const string Admin = "Admin";
    }
}
