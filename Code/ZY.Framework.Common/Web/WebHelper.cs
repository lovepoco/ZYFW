using System;
using System.Web;
using System.Collections;
using System.Text;
using System.Text.RegularExpressions;

namespace ZY.Framework.Common.Web
{
    /// <summary>
    /// 网页辅助
    /// </summary>
    public partial class WebHelper
    {
        #region IP地址
        /// <summary>
        /// IP地址
        /// </summary>
        public static string IP
        {
            get
            {
                string result = String.Empty;
                result = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
                if (result != null && result != String.Empty)
                {
                    //可能有代理
                    if (result.IndexOf(".") == -1) //没有"."肯定是非IPv4格式
                        result = null;
                    else
                    {
                        if (result.IndexOf(",") != -1)
                        {
                            //有","，估计多个代理。取第一个不是内网的IP。
                            result = result.Replace(" ", "").Replace("", "");
                            string[] temparyip = result.Split(",;".ToCharArray());
                            for (int i = 0; i < temparyip.Length; i++)
                            {
                                if (IsIPAddress(temparyip[i])
                                        && temparyip[i].Substring(0, 3) != "10."
                                        && temparyip[i].Substring(0, 7) != "192.168"
                                        && temparyip[i].Substring(0, 7) != "172.16.")
                                {
                                    return temparyip[i]; //找到不是内网的地址
                                }
                            }
                        }
                        else if (IsIPAddress(result)) //代理即是IP格式
                            return result;
                        else
                            result = null; //代理中的内容 非IP，取IP
                    }

                }

                string IpAddress = (HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"] != null && HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"] != String.Empty) ? HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"] : HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];

                if (null == result || result == String.Empty)
                    result = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];

                if (result == null || result == String.Empty)
                    result = HttpContext.Current.Request.UserHostAddress;

                return result;
            }
        }
        /// <summary>
        /// 转换10进制IP
        /// </summary>
        /// <param name="ip"></param>
        /// <returns></returns>
        public static Int64 toDenaryIp(string ip)
        {
            Int64 _Int64 = 0;
            string _ip = ip;
            if (_ip.LastIndexOf(".") > -1)
            {
                string[] _iparray = _ip.Split('.');

                _Int64 = Int64.Parse(_iparray.GetValue(0).ToString()) * 256 * 256 * 256 + Int64.Parse(_iparray.GetValue(1).ToString()) * 256 * 256 + Int64.Parse(_iparray.GetValue(2).ToString()) * 256 + Int64.Parse(_iparray.GetValue(3).ToString()) - 1;
            }
            return _Int64;
        }
        /// <summary>
        /// ip十进制
        /// </summary>
        public static Int64 DenaryIp
        {
            get
            {
                Int64 _Int64 = 0;

                string _ip = IP;
                if (_ip.LastIndexOf(".") > -1)
                {
                    string[] _iparray = _ip.Split('.');

                    _Int64 = Int64.Parse(_iparray.GetValue(0).ToString()) * 256 * 256 * 256 + Int64.Parse(_iparray.GetValue(1).ToString()) * 256 * 256 + Int64.Parse(_iparray.GetValue(2).ToString()) * 256 + Int64.Parse(_iparray.GetValue(3).ToString()) - 1;
                }
                return _Int64;
            }
        }
        /// <summary>
        /// 是否ip格式
        /// </summary>
        /// <param name="str1"></param>
        /// <returns></returns>
        public static bool IsIPAddress(string str1)
        {
            if (str1 == null || str1 == string.Empty || str1.Length < 7 || str1.Length > 15) return false;

            string regformat = @"^\d{1,3}[\.]\d{1,3}[\.]\d{1,3}[\.]\d{1,3}$";

            Regex regex = new Regex(regformat, RegexOptions.IgnoreCase);
            return regex.IsMatch(str1);
        }
        /// <summary>
        /// 获取当前客户IP
        /// </summary>
        /// <returns></returns>
        public static string GetIp()
        {
            string result = String.Empty;
            result = IP;
            if (string.IsNullOrEmpty(result))
            {
                result = HttpContext.Current.Request.UserHostAddress;
            }
            if (string.IsNullOrEmpty(result))
            {
                return "127.0.0.1";
            }
            return result;
        }
        #endregion

        #region 用于对URL地址参数进行处理
        /// <summary>
        /// 用于对网址进行参数处理,只处理有这个参数的地址
        /// </summary>
        /// <param name="url">源站地址</param>
        /// <param name="fname">字段名</param>
        /// <param name="val">值</param>
        /// <returns>处理过的网址值</returns>
        public static string ReplaceUrl(string url, string fname, string val)
        {
            string tmpstr = "";
            tmpstr = Regex.Replace(url, @"([\?&])" + fname + "=[^&]*(&?)", "$1" + fname + "=" + val + "$2");
            return tmpstr;
        }
        /// <summary>
        /// 删除网址中的参数
        /// </summary>
        /// <param name="url">源站地址</param>
        /// <param name="fname">字段名</param>
        /// <param name="val">值</param>
        /// <returns>处理过的网址值</returns>
        public static string DelUrlParam(string url, string fname)
        {
            string tmpstr = "";
            tmpstr = Regex.Replace(url, @"([\?&])" + fname + "=[^&]*(&?)", "$1");
            tmpstr = Regex.Replace(tmpstr, @"&$", "");
            return tmpstr;
        }
        /// <summary>
        /// 用于对网址进行参数处理,只处理有这个参数的地址
        /// </summary>
        /// <param name="url">源站地址</param>
        /// <param name="fname">字段名</param>
        /// <param name="val">值</param>
        /// <returns>处理过的网址值</returns>
        public static string ReplaceAddUrl(string url, string fname, string val)
        {
            string tmpstr = "";
            string regstr = @"([\?&])" + fname + "=[^&]*(&?)";
            tmpstr = Regex.Replace(url, regstr, "$1");
            if ("?&".IndexOf(tmpstr.Substring(tmpstr.Length - 1)) > -1) { tmpstr = tmpstr.Substring(0, tmpstr.Length - 1); }
            if (tmpstr.IndexOf("?") > -1)
            {
                tmpstr += "&";
            }
            else
            {
                tmpstr += "?";
            }
            tmpstr += fname + "=" + val;
            return tmpstr;
        }
        /// <summary>
        /// 取得当前页的查询参数
        /// </summary>
        /// <returns>不带?开头的参数&联接</returns>
        public static string GetCurrentQueryParams()
        {
            StringBuilder stringBuilder = new StringBuilder();
            string currentPath = HttpContext.Current.Request.Url.PathAndQuery;
            int startIndex = currentPath.IndexOf("?");
            if (startIndex <= 0) return string.Empty;
            string[] nameValues = currentPath.Substring(startIndex + 1).Split('&');
            foreach (string param in nameValues)
            {
                stringBuilder.Append(param);
                stringBuilder.Append("&");
            }
            return stringBuilder.ToString().TrimEnd('&');
        }
        #endregion
    }
}