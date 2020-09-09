using System;
using System.Collections.Generic;
using System.Text;

namespace Dywq.Infrastructure.Core
{
    public class Pager
    {
        public static string Create(int pageSize, int pageIndex, int totalCount, string linkUrl, int centSize)
        {
            if ((totalCount < 1) || (pageSize < 1))
            {
                return "";
            }
            int num = totalCount / pageSize;
            if (num < 1)
            {
                return "";
            }
            if ((totalCount % pageSize) > 0)
            {
                num++;
            }
            if (num <= 1)
            {
                return "";
            }
            StringBuilder builder = new StringBuilder();
            string oldStr = "__id__";
            int num5 = pageIndex - 1;
            string str2 = "<a href=\"" + ReplaceStr(linkUrl, oldStr, num5.ToString()) + "\">\x00ab上一页</a>";
            num5 = pageIndex + 1;
            string str3 = "<a href=\"" + ReplaceStr(linkUrl, oldStr, num5.ToString()) + "\">下一页\x00bb</a>";
            string str4 = "<a href=\"" + ReplaceStr(linkUrl, oldStr, "1") + "\">1</a>";
            string str5 = "<a href=\"" + ReplaceStr(linkUrl, oldStr, num.ToString()) + "\">" + num.ToString() + "</a>";
            if (pageIndex <= 1)
            {
                str2 = "<span class=\"disabled\">\x00ab上一页</span>";
            }
            if (pageIndex >= num)
            {
                str3 = "<span class=\"disabled\">下一页\x00bb</span>";
            }
            if (pageIndex == 1)
            {
                str4 = "<span class=\"current\">1</span>";
            }
            if (pageIndex == num)
            {
                str5 = "<span class=\"current\">" + num.ToString() + "</span>";
            }
            int num2 = pageIndex - (centSize / 2);
            if (pageIndex < centSize)
            {
                num2 = 2;
            }
            int num3 = (pageIndex + centSize) - ((centSize / 2) + 1);
            if (num3 >= num)
            {
                num3 = num - 1;
            }
            builder.Append("<span>共" + totalCount + "记录</span>");
            builder.Append(str2 + str4);
            if (pageIndex >= centSize)
            {
                builder.Append("<span>...</span>\n");
            }
            for (int i = num2; i <= num3; i++)
            {
                if (i == pageIndex)
                {
                    builder.Append("<span class=\"current\">" + i + "</span>");
                }
                else
                {
                    builder.Append(string.Concat(new object[] { "<a href=\"", ReplaceStr(linkUrl, oldStr, i.ToString()), "\">", i, "</a>" }));
                }
            }
            if ((num - pageIndex) > (centSize - (centSize / 2)))
            {
                builder.Append("<span>...</span>");
            }
            builder.Append(str5 + str3);
            return builder.ToString();
        }


        private static string ReplaceStr(string originalStr, string oldStr, string newStr)
        {
            if (string.IsNullOrEmpty(oldStr))
            {
                return "";
            }
            return originalStr.Replace(oldStr, newStr);
        }
    }
}
