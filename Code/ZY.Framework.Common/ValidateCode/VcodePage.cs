﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.SessionState;
using System.Drawing;
using System.Drawing.Imaging;
using JD.ValidateCode;

namespace ZY.Framework.Common.ValidateCode
{
    public class VcodePage : IHttpHandler, IRequiresSessionState
    {

        /// <summary>
        /// 要使用验证码的类型
        /// </summary>
        private int VcodeType
        {
            get
            {
                int i = 0;
                int.TryParse(HttpContext.Current.Request["t"], out i);

                return i;
            }
        }

        #region VcodeImageCreator
        // 随机对象
        private static readonly Random g_random = new Random();

        /// <summary>
        /// 获取图片宽度
        /// </summary>
        public virtual int ImageWidth
        {
            get
            {
                if (HttpContext.Current.Request.QueryString["w"] == null)
                    return 180;

                return Convert.ToInt32(HttpContext.Current.Request.QueryString["w"]);
            }
        }

        /// <summary>
        /// 获取图片高度
        /// </summary>
        public virtual int ImageHeight
        {
            get
            {
                if (HttpContext.Current.Request.QueryString["h"] == null)
                    return 60;

                return Convert.ToInt32(HttpContext.Current.Request.QueryString["h"]);
            }
        }

        /// <summary>
        /// 获取最少字符个数
        /// </summary>
        public virtual int MinChars
        {
            get
            {
                if (HttpContext.Current.Request.QueryString["MinChars"] == null)
                    return 5;

                return Convert.ToInt32(HttpContext.Current.Request.QueryString["MinChars"]);
            }
        }

        /// <summary>
        /// 获取最多字符个数
        /// </summary>
        public virtual int MaxChars
        {
            get
            {
                if (HttpContext.Current.Request.QueryString["MaxChars"] == null)
                    return 5;

                return Convert.ToInt32(HttpContext.Current.Request.QueryString["MaxChars"]);
            }
        }

        /// <summary>
        /// 获取最小字符尺寸
        /// </summary>
        public virtual int CharMinSize
        {
            get
            {
                if (HttpContext.Current.Request.QueryString["CharMinSize"] == null)
                    return 18;

                return Convert.ToInt32(HttpContext.Current.Request.QueryString["CharMinSize"]);
            }
        }

        /// <summary>
        /// 获取最大字符尺寸
        /// </summary>
        public virtual int CharMaxSize
        {
            get
            {
                if (HttpContext.Current.Request.QueryString["CharMaxSize"] == null)
                    return 32;

                return Convert.ToInt32(HttpContext.Current.Request.QueryString["CharMaxSize"]);
            }
        }

        /// <summary>
        /// 随机生成字符串
        /// </summary>
        /// <param name="minChars"></param>
        /// <param name="maxChars"></param>
        /// <returns></returns>
        public string NextString(int minChars, int maxChars)
        {
            string value = "";

            // 随机产生字符串长度
            int strLen = g_random.Next(minChars, maxChars);
            // 计数器
            int count = 0;

            while (count < strLen)
            {
                // 产生随机字符 0..9, a..z, A..Z
                int n = g_random.Next(48, 121);

                // 如果不是数字或者字母, 则跳过
                if ((n >= 58 && n <= 64) || (n >= 91 && n <= 96))
                    continue;

                // 不为 I, o, O
                if (n == '0' || n == '1' ||
                    n == 'o' || n == 'O' || n == 'r')
                    continue;

                value += Convert.ToChar(n);

                count++;
            }

            return value;
        }

        private void BuildVcode1()
        {
            // 获取随机字符串
            string vcStr = this.NextString(this.MinChars, this.MaxChars).ToUpper();

            // 创建验证码图片生成器
            VcodeImageCreator vcImgCreator = new VcodeImageCreator();

            // 设置字符尺寸
            vcImgCreator.CharMinSize = this.CharMinSize;
            vcImgCreator.CharMaxSize = this.CharMaxSize;

            // 创建验证码图片
            Image vcImg = vcImgCreator.CreateImage(this.ImageWidth, this.ImageHeight, vcStr);

            // 保存验证码
            HttpContext.Current.Session["__validatecodeimage"] = vcStr;
            // 保存验证码图片
            using (System.IO.MemoryStream ms = new System.IO.MemoryStream())
            {
                HttpContext.Current.Response.ContentType = "Image/PNG";
                HttpContext.Current.Response.Clear();
                HttpContext.Current.Response.BufferOutput = true;
                vcImg.Save(ms, ImageFormat.Png);
                ms.Flush();
                HttpContext.Current.Response.BinaryWrite(ms.GetBuffer());
                HttpContext.Current.Response.End();
            }
        }


        #endregion


        public void BuildVcode()
        {
            switch (VcodeType)
            {
                case 0:
                case 1:
                    BuildVcode1();
                    break;
                default:
                    BuildVcode();
                    break;
            }
        }


        public static bool Validation(string vcode)
        {
            if (HttpContext.Current.Session["__validatecodeimage"] != null)
            {
                return vcode.ToLower() == HttpContext.Current.Session["__validatecodeimage"].ToString().ToLower();
            }
            else
                return false;
        }


        public bool IsReusable
        {
            get { return true; }
        }

        public void ProcessRequest(HttpContext context)
        {
            if (context.Request.Path.IndexOf(".hxl", System.StringComparison.Ordinal) > -1)
                BuildVcode();
        }
    }
}
