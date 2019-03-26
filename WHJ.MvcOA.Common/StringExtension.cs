using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace WHJ.MvcOA.Common
{
   public static class StringExtension
    {
        #region 1.0 返回字符串的md5值
        /// <summary>
        /// 返回字符串的md5值
        /// </summary>
        /// <param name="strOri">需要加密的字符串</param>
        /// <returns></returns>
        public static string MD5(this string strOri)
        {
            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
            byte[] bytValue, bytHash;
            bytValue = System.Text.Encoding.UTF8.GetBytes(strOri);
            bytHash = md5.ComputeHash(bytValue);
            md5.Clear();
            string sTemp = string.Empty;
            for (int i = 0; i < bytHash.Length; i++)
            {
                sTemp += bytHash[i].ToString("X").PadLeft(2, '0');
            }
            return sTemp.ToString();
        } 
        #endregion

        #region 2.0 判断本字符串是否和 strNew相等（忽略大小写）
        /// <summary>
        /// 判断本字符串是否和 strNew相等（忽略大小写）
        /// </summary>
        /// <param name="strOri">本字符串</param>
        /// <param name="strNew">要比较的字符串</param>
        /// <returns></returns>
        public static bool IsSameStr(this string strOri, string strNew)
        {
            return string.Equals(strOri, strNew, StringComparison.CurrentCultureIgnoreCase);
        } 
        #endregion

        #region 3.0 判断本字符串 是否为空
        /// <summary>
        /// 判断本字符串 是否为空
        /// </summary>
        /// <param name="strOri">要判断的字符串</param>
        /// <returns></returns>
        public static bool IsNullOrEmpty(this string strOri)
        {
            return string.IsNullOrEmpty(strOri);
        } 
        #endregion

    }
}
