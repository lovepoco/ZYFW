using System;
using System.Web;

namespace ZY.Framework.Common.Web
{
    /// <summary>
    /// 网页辅助
    /// </summary>
    public partial class WebHelper
    {
        /// <summary>
        /// 清除指定Cookie
        /// </summary>
        /// <param name="cookiename">cookiename</param>
        public static void ClearCookie(string cookiename)
        {
            HttpCookie cookie = HttpContext.Current.Request.Cookies[cookiename];
            if (cookie != null)
            {
                cookie.Value = "";   //值清空
                cookie.Expires = DateTime.Now.AddYears(-3);
                HttpContext.Current.Response.Cookies.Add(cookie);
            }
        }
        /// <summary>
        /// 获取指定Cookie值
        /// </summary>
        /// <param name="cookiename">cookiename</param>
        /// <returns></returns>
        public static string GetCookieValue(string cookiename)
        {
            HttpCookie cookie = HttpContext.Current.Request.Cookies[cookiename];
            string str = string.Empty;
            if (cookie != null)
            {
                str = cookie.Value;
            }
            return str;
        }
        /// <summary>
        /// 添加一个Cookie
        /// </summary>
        /// <param name="cookiename">cookie名</param>
        /// <param name="cookievalue">cookie值</param>
        /// <param name="expires">过期时间 DateTime</param>
        /// <param name="cookieDomain">域</param>
        public static void SetCookie(string cookiename, string cookievalue, DateTime? expires=null, string cookieDomain="")
        {
            HttpCookie cookie = new HttpCookie(cookiename);
            cookie.Value = cookievalue;
            if (expires != null)
            {
                cookie.Expires = (DateTime)expires;
            }
            if (!string.IsNullOrWhiteSpace(cookieDomain))
            {
                cookie.Domain = cookieDomain;
            }
            HttpContext.Current.Response.Cookies.Add(cookie);
        }
    }
}
