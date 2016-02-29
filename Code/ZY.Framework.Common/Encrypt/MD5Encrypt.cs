using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;

namespace ZY.Framework.Common.Encrypt
{
    /// <summary>
    /// MD5加密处理模块
    /// </summary>
    public sealed class MD5Encrypt
    {
        #region Md5加密函数
        /// <summary>
        /// MD5字符加密
        /// </summary>
        /// <param name="str">需要进行MD5加密的字符串</param>
        /// <returns>MD5加密后的字符串 32位</returns>
        public static string GetMd5(string str)
        {
            return Get32Md5(str, Encoding.UTF8);
        }
        /// <summary>
        /// MD5字符加密
        /// </summary>
        /// <param name="str">需要进行MD5加密的字符串</param>
        /// <param name="num">返回位数，16或32</param>
        /// <returns>MD5加密后的字符串</returns>
        public static string GetMd5(string str, int num)
        {
            string RStr;
            if (num == 32)
            {
                RStr = Get32Md5(str, Encoding.UTF8);
            }
            else if (num == 16)
            {
                RStr = Get16Md5(str, Encoding.UTF8);
            }
            else
            {
                RStr = GetMd5(str);
            }
            return RStr;
        }
        /// <summary>
        /// MD5字符加密
        /// </summary>
        /// <param name="str">需要进行MD5加密的字符串</param>
        /// <param name="num">返回位数，16或32</param>
        /// <param name="charset">字符集,GB2312与UTF8</param>
        /// <returns>MD5加密后的字符串</returns>
        public static string GetMd5(string str, int num, Encoding charset)
        {
            string RStr;
            if (num == 32)
            {
                RStr = Get32Md5(str, charset);
            }
            else if (num == 16)
            {
                RStr = Get16Md5(str, charset);
            }
            else
            {
                RStr = str;
            }
            return RStr;
        }

        /// <summary>
        /// 取得16位加密结果
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        private static string Get16Md5(string str, Encoding charset)
        {
            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
            byte[] fromData;
            fromData = charset.GetBytes(str);
            string RStr = BitConverter.ToString(md5.ComputeHash(fromData), 4, 8);
            RStr = RStr.Replace("-", "").ToLower();
            return RStr;
        }

        /// <summary>
        /// 取得32位加密结果
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        private static string Get32Md5(string str, Encoding charset)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] fromData;
            fromData = charset.GetBytes(str);
            byte[] targetData = md5.ComputeHash(fromData);
            string byte2String = null;
            for (int i = 0; i < targetData.Length; i++)
            {
                byte2String = byte2String + targetData[i].ToString("x2");
            }
            return byte2String;
        }

        #endregion Md5加密函数
    }
}
