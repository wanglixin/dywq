using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

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
    }
}
