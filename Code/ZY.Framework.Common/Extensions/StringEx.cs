using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace System
{
    public static class StringEx
    {
        /// <summary>
        /// 对字符进行安全处理
        /// <para>替换对sql不利字符</para>
        /// <para>';&#39;char</para>
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string SafeReplace(this string str)
        {
            if (!string.IsNullOrEmpty(str))
            {
                str=str.Replace("'", "");
                str = str.Replace(";", "");
                str = str.Replace("&#39;", "");
                //处理char
                str = Regex.Replace(str, @"char\(\s*\d+\s*\)", ""); ;
            }
            return str;
        }
        /// <summary>
        /// 补全左右逗号
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string CompleteComma(this string str)
        {
            if (!string.IsNullOrEmpty(str))
            {
                if (str.Substring(0, 1) != ",") str = "," + str;
                if (str.Substring(str.Length - 1, 1) != ",") str += ",";
            }
            return str;
        }
        /// <summary>
        /// 清理左右逗号
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string ClearComma(this string str)
        {
            if (!string.IsNullOrEmpty(str))
            {
                str = Regex.Replace(str, "^,+", "");
                str = Regex.Replace(str, ",+$", "");
            }
            return str;
        }
        /// <summary>
        /// 生成重复字符串
        /// </summary>
        /// <param name="str">字符</param>
        /// <param name="n">数量</param>
        /// <returns></returns>
        public static string MakeRepeatChar(this string str, int n)
        {
            if (n > 0)
            {
                string ostr = str;
                for (int i = 1; i < n; i++)
                {
                    str += ostr;
                }
                return str;
            }
            else
            {
                return "";
            }
        }
        /// <summary>
        /// 计算字符串字节数汉字为2字节字母1字节
        /// </summary>
        /// <param name="str">字符串</param>
        /// <returns>int字符串字节数</returns>
        public static int GetNlength(this string str)
        {
            Char[] cc = str.ToCharArray();
            int intLen = str.Length;
            int i;
            for (i = 0; i < cc.Length; i++)
            {
                if (cc[i] > 255)
                {
                    intLen++;
                }
            }
            return intLen;
        }
        /// <summary>
        /// 截取字符,并加上后缀
        /// </summary>
        /// <param name="str"></param>
        /// <param name="n">长度</param>
        /// <param name="Suffix">后缀</param>
        /// <returns></returns>
        public static string Cat(this string str, int n, string Suffix="...")
        {
            if (!string.IsNullOrEmpty(str))
            {
                if (str.Length > n)
                {
                    str = str.Substring(0, n);
                    str += Suffix;
                }
            }
            return str;
        }
        /// <summary>
        /// 截取字符,并加上后缀,中文双字符算一个
        /// </summary>
        /// <param name="str"></param>
        /// <param name="n">长度</param>
        /// <param name="Suffix">后缀</param>
        /// <returns></returns>
        public static string Ncat(this string str, int n, string Suffix="...")
        {
            Char[] c = str.ToCharArray();
            int i, intLen = 0;
            string newStr = "";
            for (i = 0; i < c.Length; i++)
            {
                if (intLen >= n)
                {
                    return newStr;
                }
                newStr += c[i];

                if (c[i] > 255)
                {
                    intLen += 2;
                }
                else 
                {
                    intLen++;
                }
            }
            newStr += Suffix;
            return newStr;
        }
        /// <summary>
        /// br换为换行(默认\n)
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string BR2NewLine(this string str, string brstr = "\n")
        {
            if (!string.IsNullOrEmpty(str))
            {
                str = Regex.Replace(str, "<br[^>]*>", brstr);
            }
            return str;
        }
        /// <summary>
        /// \n换行换成br
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string NewLine2BR(this string str)
        {
            if (!string.IsNullOrEmpty(str))
            {
                str = Regex.Replace(str, "\r\n", "<br />");
                str = Regex.Replace(str, "\r", "<br />");
                str = Regex.Replace(str, "\n", "<br />");
            }
            return str;
        }
        /// <summary>
        /// 清理换行
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string ClearNewLine(this string str)
        {
            if (!string.IsNullOrEmpty(str))
            {
                str = Regex.Replace(str, "\r\n", "");
                str = Regex.Replace(str, "\r", "");
                str = Regex.Replace(str, "\n", "");
            }
            return str;
        }
        /// <summary>
        /// split后转整型数组
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static int[] Split2Int(this string str,char charsplit)
        {
            string[] StrArr = str.Split(charsplit);
            int[] NewArr = new int[StrArr.Length];

            for (int i = 0; i < StrArr.Length; i++)
            {
                NewArr[i] = int.Parse(StrArr[i]);
            }
            return NewArr;
        }
        /// <summary>
        /// 清除HTML标记
        /// <para>br换行不处理</para>
        /// </summary>
        /// <param name="Htmlstring"></param>
        /// <returns></returns>
        public static string RemoveHTML(this string Htmlstring)
        {
            //remove the header
            Htmlstring = Regex.Replace(Htmlstring, "(<head>).*(</head>)", string.Empty, RegexOptions.IgnoreCase);
            //remove all styles
            Htmlstring = Regex.Replace(Htmlstring, @"<style([^>])*?>", "<style>", RegexOptions.IgnoreCase);
            //删除脚本   
            Htmlstring = Regex.Replace(Htmlstring, @"<script[^>]*?>.*?</script>", "", RegexOptions.IgnoreCase);
            //处理br
            Htmlstring = Regex.Replace(Htmlstring, @"<br([^>]*)>", "{-br-}", RegexOptions.IgnoreCase);
            //删除HTML
            Htmlstring = Regex.Replace(Htmlstring, @"<[^>]*?>", "", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"([\r\n])[\s]+", "", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"-->", "", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"<!--.*", "", RegexOptions.IgnoreCase);
            //br回归
            Htmlstring = Htmlstring.Replace("{-br-}", "<br/>");

            Htmlstring = Regex.Replace(Htmlstring, @"&(quot|#34);", "\"", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(amp|#38);", "&", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(lt|#60);", "<", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(gt|#62);", ">", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(nbsp|#160);", "   ", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(iexcl|#161);", "\xa1", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(cent|#162);", "\xa2", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(pound|#163);", "\xa3", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(copy|#169);", "\xa9", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&#(\d+);", "", RegexOptions.IgnoreCase);

            //Htmlstring.Replace("<", "");
            //Htmlstring.Replace(">", "");
            //Htmlstring.Replace("\r\n", "");
            //Htmlstring = HttpContext.Current.Server.HtmlEncode(Htmlstring).Trim();

            return Htmlstring;
        }
        /// <summary>
        /// 清除所有空格
        /// <para>包括html空格</para>
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string RemoveSpace(this string str)
        {
            str = Regex.Replace(str, @"\s", string.Empty, RegexOptions.IgnoreCase);
            str = Regex.Replace(str, @"&nbsp;", string.Empty, RegexOptions.IgnoreCase);
            return str;
        }
        /// <summary>
        /// 编码汉字
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string Escape(this string str)
        {
            StringBuilder sb = new StringBuilder();
            foreach (char c in str) {
                sb.Append((Char.IsLetterOrDigit(c) || c == '-' || c == '_' || c == '\\' || c == '/' || c == '.') ? c.ToString() : Uri.HexEscape(c)); 
            } 
            return sb.ToString();
        }
        /// <summary>
        /// 解码汉字
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string UnEscape(this string str)
        {
            StringBuilder sb = new StringBuilder();
            int len = str.Length;
            int i = 0;
            while (i != len)
            {
                if (Uri.IsHexEncoding(str, i)) sb.Append(Uri.HexUnescape(str, ref i));
                else sb.Append(str[i++]);
            }
            return sb.ToString();
        }
        /// <summary>
        /// 邮箱格式验证
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool isEmail(this string str)
        {
            bool ismail = false;
            if (!string.IsNullOrWhiteSpace(str))
            {
                string emailpattren = @"^((([a-z]|\d|[!#\$%&'\*\+\-\/=\?\^_`{\|}~]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])+(\.([a-z]|\d|[!#\$%&'\*\+\-\/=\?\^_`{\|}~]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])+)*)|((\x22)((((\x20|\x09)*(\x0d\x0a))?(\x20|\x09)+)?(([\x01-\x08\x0b\x0c\x0e-\x1f\x7f]|\x21|[\x23-\x5b]|[\x5d-\x7e]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(\\([\x01-\x09\x0b\x0c\x0d-\x7f]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]))))*(((\x20|\x09)*(\x0d\x0a))?(\x20|\x09)+)?(\x22)))@((([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])*([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])))\.)+(([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])*([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])))";
                if (Regex.IsMatch(str, emailpattren))
                {
                    ismail = true;
                }
            }
            return ismail;
        }

        #region Html 编码
        /// <summary>
        /// 对文本框中的字符进行HTML编码
        /// </summary>
        /// <param name="str">源字符串</param>
        /// <returns></returns>
        public static string HtmlEncode(string str)
        {
            str = str.Replace("&", "&amp;");
            str = str.Replace("'", "''");
            str = str.Replace("\"", "&quot;");
            str = str.Replace(" ", "&nbsp;");
            str = str.Replace("<", "&lt;");
            str = str.Replace(">", "&gt;");
            str = str.Replace("\n", "<br>");
            str = str.Replace("\r", "<br>");
            str = str.Replace("\r\n", "<br>");
            return str;
        }
        /// <summary>
        /// 对字符串进行HTML解码,解析为可为页面识别的代码
        /// </summary>
        /// <param name="str">要解码的字符串</param>
        /// <returns></returns>
        public static string HtmlDecode(string str)
        {
            str = str.Replace("<br>", "\n");
            str = str.Replace("&gt;", ">");
            str = str.Replace("&lt;", "<");
            str = str.Replace("&nbsp;", " ");
            str = str.Replace("&quot;", "\"");
            return str;
        }
        #endregion
    }
}
