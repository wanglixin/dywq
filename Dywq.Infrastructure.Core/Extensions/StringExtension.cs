using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace Dywq.Infrastructure.Core.Extensions
{
    public static class StringExtension
    {
        /// <summary>
        /// 大写的md5
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string Md5(this string input)
        {
            return MD5Encrypt(input).ToUpper();
        }

        static string MD5Encrypt(string input)
        {
            MD5CryptoServiceProvider mD5CryptoServiceProvider = new MD5CryptoServiceProvider();
            byte[] array = mD5CryptoServiceProvider.ComputeHash(Encoding.GetEncoding("gb2312").GetBytes(input));
            StringBuilder stringBuilder = new StringBuilder();
            byte[] array2 = array;
            for (int i = 0; i < array2.Length; i++)
            {
                byte b = array2[i];
                stringBuilder.Append(b.ToString("x2"));
            }
            return stringBuilder.ToString();

        }


        public static string Cut(this string input, int count)
        {
            if (string.IsNullOrWhiteSpace(input)) return "";
            var length = input.Length;
            if (length <= count) return input;
            return input.Substring(0, count) + "...";
        }


        ///   <summary>
        ///   去除HTML标记
        ///   </summary>
        ///   <param   name=”NoHTML”>包括HTML的源码   </param>
        ///   <returns>已经去除后的文字</returns>
        public static string FilterHtml(this string input)
        {
            if (string.IsNullOrWhiteSpace(input)) return "";
            //删除脚本
            input = Regex.Replace(input, @"<script[^>]*?>.*?</script>", "",
            RegexOptions.IgnoreCase);
            //删除HTML
            input = Regex.Replace(input, @"<(.[^>]*)>", "",
            RegexOptions.IgnoreCase);
            input = Regex.Replace(input, @"([\r\n])[\s]+", "",
            RegexOptions.IgnoreCase);
            input = Regex.Replace(input, @"–>", "", RegexOptions.IgnoreCase);
            input = Regex.Replace(input, @"<!–.*", "", RegexOptions.IgnoreCase);
            input = Regex.Replace(input, @"&(quot|#34);", "\"",
            RegexOptions.IgnoreCase);
            input = Regex.Replace(input, @"&(amp|#38);", "&",
            RegexOptions.IgnoreCase);
            input = Regex.Replace(input, @"&(lt|#60);", "<",
            RegexOptions.IgnoreCase);
            input = Regex.Replace(input, @"&(gt|#62);", ">",
            RegexOptions.IgnoreCase);
            input = Regex.Replace(input, @"&(nbsp|#160);", "   ",
            RegexOptions.IgnoreCase);
            input = Regex.Replace(input, @"&(iexcl|#161);", "\xa1", RegexOptions.IgnoreCase);
            input = Regex.Replace(input, @"&(cent|#162);", "\xa2", RegexOptions.IgnoreCase);
            input = Regex.Replace(input, @"&(pound|#163);", "\xa3", RegexOptions.IgnoreCase);
            input = Regex.Replace(input, @"&(copy|#169);", "\xa9", RegexOptions.IgnoreCase);
            input = Regex.Replace(input, @"&#(\d+);", "", RegexOptions.IgnoreCase);
            input.Replace("<", "");
            input.Replace(">", "");
            input.Replace("\r\n", "");
            return input;
        }

    }
}
