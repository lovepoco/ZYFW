using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace System
{
    public static class DateTimeEx
    {
        /// <summary>
        /// 显示简单时间
        /// <para>一天内显示时分秒，大于一天显示年月日</para>
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static string ToSimpDate(this DateTime date, string timeformat = "HH:mm:ss", string dateformat = "yy/MM/dd")
        {
            TimeSpan ts = DateTime.Now - date;
            if (ts.Days == 0)
            {
                return date.ToString(timeformat);
            }
            else
            {
                return date.ToString(dateformat);
            }
        }
        /// <summary>
        /// 按格式显示时间
        /// </summary>
        /// <param name="date"></param>
        /// <param name="format"></param>
        /// <returns></returns>
        public static string ToStringByFormat(this DateTime? date, string format)
        {
            string result = "";
            if (date.HasValue)
            {
                result = date.Value.ToString(format);
            }
            return result;
        }
    }
}
