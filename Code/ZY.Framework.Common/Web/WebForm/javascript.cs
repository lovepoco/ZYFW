using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZY.Framework.Common.Web.WebForm
{
    /// <summary>
    /// 前台javascript交互脚本控件
    /// </summary>
    public class javascript
    {
        #region 引入脚本文件
        /// <summary>
        /// 引入脚本文件到页面
        /// *JS代码嵌入在页面的顶部、表单的最前，适用于要在控件加载前执行的JS代码
        /// </summary>
        /// <param name="page">System.Web.UI.Page   如果是当前页写this</param>
        /// <param name="filepath">文件地址</param>
        public static void Register(System.Web.UI.Page page, string filepath)
        {
            if (!page.ClientScript.IsStartupScriptRegistered(page.GetType(), "clientScript"))
            {
                page.ClientScript.RegisterStartupScript(page.GetType(), "clientScript", "<script type=\"text/javascript\" src=\"" + filepath + "\"></script>");
            }
        }
        /// <summary>
        /// 引入脚本文件到页面
        /// *JS代码嵌入在页面的底部、表单的最后)，适用于要在页面控件加载完成后运行的JS代码
        /// </summary>
        /// <param name="page">System.Web.UI.Page   如果是当前页写this</param>
        /// <param name="filepath">文件地址</param>
        public static void RegisterEnd(System.Web.UI.Page page, string filepath)
        {
            if (!page.ClientScript.IsClientScriptBlockRegistered(page.GetType(), "clientScript"))
            {
                page.ClientScript.RegisterClientScriptBlock(page.GetType(), "clientScript", "<script type=\"text/javascript\" src=\"" + filepath + "\"></script>");
            }
        }
        /// <summary>
        /// 引入脚本文件到页面
        /// </summary>
        /// <param name="page">System.Web.UI.Page   如果是当前页写this</param>
        /// <param name="filepath">文件地址</param>
        /// <param name="isEnd">代码是否是嵌入在页面的底部、表单的最后,为true时适用于要在页面控件加载完成后运行的JS代码</param>
        public static void Register(System.Web.UI.Page page, string filepath, bool isEnd)
        {
            if (isEnd)
            {
                RegisterEnd(page, filepath);
            }
            else
            {
                Register(page, filepath);
            }
        }
        #endregion 引入脚本文件

        #region 自定义脚本代码

        /// <summary>
        /// 自定义脚本
        /// *JS代码嵌入在页面的顶部、表单的最前，适用于要在控件加载前执行的JS代码
        /// </summary>
        /// <param name="content">脚本内容</param>
        public static void Custom(string content)
        {
            System.Web.UI.Page page = System.Web.HttpContext.Current.Handler as System.Web.UI.Page;
            if (!page.ClientScript.IsStartupScriptRegistered(page.GetType(), "clientScript"))
            {
                page.ClientScript.RegisterStartupScript(page.GetType(), "clientScript", "<script type=\"text/javascript\">" + content + "</script>");
            }
        }
        /// <summary>
        /// 自定义脚本
        /// *JS代码嵌入在页面的顶部、表单的最前，适用于要在控件加载前执行的JS代码
        /// </summary>
        /// <param name="page">System.Web.UI.Page   如果是当前页写this</param>
        /// <param name="content">脚本内容</param>
        public static void Custom(System.Web.UI.Page page, string content)
        {
            if (!page.ClientScript.IsStartupScriptRegistered(page.GetType(), "clientScript"))
            {
                page.ClientScript.RegisterStartupScript(page.GetType(), "clientScript", "<script type=\"text/javascript\">" + content + "</script>");
            }
        }
        /// <summary>
        /// 自定义脚本
        /// *JS代码嵌入在页面的底部、表单的最后，适用于要在页面控件加载完成后运行的JS代码
        /// </summary>
        /// <param name="content">脚本内容</param>
        public static void CustomEnd(string content)
        {
            System.Web.UI.Page page = System.Web.HttpContext.Current.Handler as System.Web.UI.Page;
            if (!page.ClientScript.IsClientScriptBlockRegistered(page.GetType(), "clientScript"))
            {
                page.ClientScript.RegisterClientScriptBlock(page.GetType(), "clientScript", "<script type=\"text/javascript\">" + content + "</script>");
            }
        }
        /// <summary>
        /// 自定义脚本
        /// *JS代码嵌入在页面的底部、表单的最后，适用于要在页面控件加载完成后运行的JS代码
        /// </summary>
        /// <param name="page">System.Web.UI.Page   如果是当前页写this</param>
        /// <param name="content">脚本内容</param>
        public static void CustomEnd(System.Web.UI.Page page, string content)
        {
            if (!page.ClientScript.IsClientScriptBlockRegistered(page.GetType(), "clientScript"))
            {
                page.ClientScript.RegisterClientScriptBlock(page.GetType(), "clientScript", "<script type=\"text/javascript\">" + content + "</script>");
            }
        }
        /// <summary>
        /// 自定义脚本
        /// </summary>
        /// <param name="page">System.Web.UI.Page   如果是当前页写this</param>
        /// <param name="content">脚本内容</param>
        /// <param name="isEnd">代码是否是嵌入在页面的底部、表单的最后,为true时适用于要在页面控件加载完成后运行的JS代码</param>
        public static void Custom(System.Web.UI.Page page, string content, bool isEnd)
        {
            if (isEnd)
            {
                CustomEnd(page, content);
            }
            else
            {
                Custom(page, content);
            }
        }
        #endregion 自定义脚本代码

        #region 提示框(Alert)
        /// <summary>
        /// 提示框(Alert)
        /// *JS代码嵌入在页面的顶部、表单的最前，适用于要在控件加载前执行的JS代码
        /// </summary>
        /// <param name="content">用户提示信息</param>
        public static void Alert(string content)
        {
            Alert(System.Web.HttpContext.Current.Handler as System.Web.UI.Page, content, "", "", false);
        }
        /// <summary>
        /// 提示框(Alert)
        /// *JS代码嵌入在页面的顶部、表单的最前，适用于要在控件加载前执行的JS代码
        /// </summary>
        /// <param name="content">用户提示信息</param>
        /// <param name="toPage">跳转页面地址,为空表示返回上一页</param>
        public static void Alert(string content, string toPage)
        {
            System.Web.UI.Page page = System.Web.HttpContext.Current.Handler as System.Web.UI.Page;
            if (string.IsNullOrEmpty(toPage))
            {
                Alert(page, content, "~h", "~h", false);
            }
            else
            {
                Alert(page, content, toPage, "", false);
            }
        }
        /// <summary>
        /// 提示框(Alert)
        /// *JS代码嵌入在页面的顶部、表单的最前，适用于要在控件加载前执行的JS代码
        /// </summary>
        /// <param name="content">用户提示信息</param>
        /// <param name="toPage">跳转页面地址,为空表示返回上一页</param>
        /// <param name="frameName">跳转框架名,~top表示顶层框架,~h表示返回上一页</param>
        public static void Alert(string content, string toPage, string frameName)
        {
            System.Web.UI.Page page = System.Web.HttpContext.Current.Handler as System.Web.UI.Page;
            if (string.IsNullOrEmpty(toPage))
            {
                Alert(page, content, "~h", "~h", false);
            }
            else
            {
                Alert(page, content, toPage, frameName, false);
            }
        }
        /// <summary>
        /// 提示框(Alert)
        /// *JS代码嵌入在页面的顶部、表单的最前，适用于要在控件加载前执行的JS代码
        /// </summary>
        /// <param name="page">System.Web.UI.Page   如果是当前页写this</param>
        /// <param name="content">用户提示信息</param>
        public static void Alert(System.Web.UI.Page page, string content)
        {
            Alert(page, content, "", "", false);
        }
        /// <summary>
        /// 提示框(Alert)
        /// *JS代码嵌入在页面的顶部、表单的最前，适用于要在控件加载前执行的JS代码
        /// </summary>
        /// <param name="page">System.Web.UI.Page   如果是当前页写this</param>
        /// <param name="content">用户提示信息</param>
        /// <param name="toPage">跳转页面地址,为空表示返回上一页</param>
        public static void Alert(System.Web.UI.Page page, string content, string toPage)
        {
            if (string.IsNullOrEmpty(toPage))
            {
                Alert(page, content, "~h", "~h", false);
            }
            else
            {
                Alert(page, content, toPage, "", false);
            }
        }
        /// <summary>
        /// 提示框(Alert)
        /// *JS代码嵌入在页面的顶部、表单的最前，适用于要在控件加载前执行的JS代码
        /// </summary>
        /// <param name="page">System.Web.UI.Page   如果是当前页写this</param>
        /// <param name="content">用户提示信息</param>
        /// <param name="toPage">跳转页面地址,为空表示返回上一页</param>
        /// <param name="frameName">跳转框架名,~top表示顶层框架,~h表示返回上一页</param>
        public static void Alert(System.Web.UI.Page page, string content, string toPage, string frameName)
        {
            if (string.IsNullOrEmpty(toPage))
            {
                Alert(page, content, "~h", "~h", false);
            }
            else
            {
                Alert(page, content, toPage, frameName, false);
            }
        }
        /// <summary>
        /// 提示框(Alert)
        /// *JS代码嵌入在页面的底部、表单的最后，适用于要在页面控件加载完成后运行的JS代码
        /// </summary>
        /// <param name="content">用户提示信息</param>
        public static void AlertEnd(string content)
        {
            Alert(System.Web.HttpContext.Current.Handler as System.Web.UI.Page, content, "", "", true);
        }
        /// <summary>
        /// 提示框(Alert)
        /// *JS代码嵌入在页面的底部、表单的最后，适用于要在页面控件加载完成后运行的JS代码
        /// </summary>
        /// <param name="content">用户提示信息</param>
        /// <param name="toPage">跳转页面地址,为空表示返回上一页</param>
        public static void AlertEnd(string content, string toPage)
        {
            System.Web.UI.Page page = System.Web.HttpContext.Current.Handler as System.Web.UI.Page;
            if (string.IsNullOrEmpty(toPage))
            {
                Alert(page, content, "~h", "~h", true);
            }
            else
            {
                Alert(page, content, toPage, "", true);
            }
        }
        /// <summary>
        /// 提示框(Alert)
        /// *JS代码嵌入在页面的底部、表单的最后，适用于要在页面控件加载完成后运行的JS代码
        /// </summary>
        /// <param name="content">用户提示信息</param>
        /// <param name="toPage">跳转页面地址,为空表示返回上一页</param>
        /// <param name="frameName">跳转框架名,~top表示顶层框架,~h表示返回上一页</param>
        public static void AlertEnd(string content, string toPage, string frameName)
        {
            System.Web.UI.Page page = System.Web.HttpContext.Current.Handler as System.Web.UI.Page;
            if (string.IsNullOrEmpty(toPage))
            {
                Alert(page, content, "~h", "~h", true);
            }
            else
            {
                Alert(page, content, toPage, frameName, true);
            }
        }
        /// <summary>
        /// 提示框(Alert)
        /// *JS代码嵌入在页面的底部、表单的最后，适用于要在页面控件加载完成后运行的JS代码
        /// </summary>
        /// <param name="page">System.Web.UI.Page   如果是当前页写this</param>
        /// <param name="content">用户提示信息</param>
        public static void AlertEnd(System.Web.UI.Page page, string content)
        {
            Alert(page, content, "", "", true);
        }
        /// <summary>
        /// 提示框(Alert)
        /// *JS代码嵌入在页面的底部、表单的最后，适用于要在页面控件加载完成后运行的JS代码
        /// </summary>
        /// <param name="page">System.Web.UI.Page   如果是当前页写this</param>
        /// <param name="content">用户提示信息</param>
        /// <param name="toPage">跳转页面地址,为空表示返回上一页</param>
        public static void AlertEnd(System.Web.UI.Page page, string content, string toPage)
        {
            if (string.IsNullOrEmpty(toPage))
            {
                Alert(page, content, "~h", "~h", true);
            }
            else
            {
                Alert(page, content, toPage, "", true);
            }
        }
        /// <summary>
        /// 提示框(Alert)
        /// *JS代码嵌入在页面的底部、表单的最后，适用于要在页面控件加载完成后运行的JS代码
        /// </summary>
        /// <param name="page">System.Web.UI.Page   如果是当前页写this</param>
        /// <param name="content">用户提示信息</param>
        /// <param name="toPage">跳转页面地址,为空表示返回上一页</param>
        /// <param name="frameName">跳转框架名,~top表示顶层框架,~h表示返回上一页</param>
        public static void AlertEnd(System.Web.UI.Page page, string content, string toPage, string frameName)
        {
            if (string.IsNullOrEmpty(toPage))
            {
                Alert(page, content, "~h", "~h", true);
            }
            else
            {
                Alert(page, content, toPage, frameName, true);
            }
        }
        /// <summary>
        /// 提示框(Alert)
        /// </summary>
        /// <param name="page">System.Web.UI.Page   如果是当前页写this</param>
        /// <param name="content">用户提示信息</param>
        /// <param name="toPage">跳转页面地址</param>
        /// <param name="frameName">跳转框架名,~top表示顶层框架,~h表示返回上一页</param>
        /// <param name="isEnd">代码是否是嵌入在页面的底部、表单的最后,为true时适用于要在页面控件加载完成后运行的JS代码</param>
        public static void Alert(System.Web.UI.Page page, string content, string toPage, string frameName, bool isEnd)
        {
            if (!string.IsNullOrEmpty(content))
            {
                string scriptcontent;
                content = content.Replace("\"", "\\\"");
                toPage = toPage.Replace("\"", "\\\"");
                scriptcontent = "alert(\"" + content + "\");";
                if (!string.IsNullOrEmpty(toPage))
                {
                    if (!string.IsNullOrEmpty(frameName))
                    {
                        switch (frameName)
                        {
                            case "~top":
                                scriptcontent += "top.location.href=\"";
                                break;
                            case "~h":
                                scriptcontent += "history.back(1);";
                                break;
                            default:
                                scriptcontent += "parent." + frameName + ".location.href=\"";
                                break;
                        }
                    }
                    else
                    {
                        scriptcontent += "location.href=\"";
                    }
                    if (frameName != "~h")
                    {
                        scriptcontent += toPage + "\"";
                    }
                }
                if (isEnd)
                {
                    Custom(page, scriptcontent);
                }
                else
                {
                    CustomEnd(page, scriptcontent);
                }
            }
        }
        #endregion 提示框(Alert)

        #region 页面跳转
        /// <summary>
        /// 页面跳转
        /// *JS代码嵌入在页面的顶部、表单的最前，适用于要在控件加载前执行的JS代码
        /// </summary>
        /// <param name="toPage">跳转页面地址</param>
        public static void ToPage(string toPage)
        {
            ToPage(System.Web.HttpContext.Current.Handler as System.Web.UI.Page, toPage, "", false);
        }
        /// <summary>
        /// 页面跳转
        /// *JS代码嵌入在页面的顶部、表单的最前，适用于要在控件加载前执行的JS代码
        /// </summary>
        /// <param name="toPage">跳转页面地址</param>
        /// <param name="frameName">跳转框架名,~top表示顶层框架,~h表示返回上一页,为空表示当前框架跳转</param>
        public static void ToPage(string toPage, string frameName)
        {
            ToPage(System.Web.HttpContext.Current.Handler as System.Web.UI.Page, toPage, frameName, false);
        }
        /// <summary>
        /// 页面跳转
        /// *JS代码嵌入在页面的顶部、表单的最前，适用于要在控件加载前执行的JS代码
        /// </summary>
        /// <param name="page">System.Web.UI.Page   如果是当前页写this</param>
        /// <param name="toPage">跳转页面地址</param>
        public static void ToPage(System.Web.UI.Page page, string toPage)
        {
            ToPage(page, toPage, "", false);
        }
        /// <summary>
        /// 页面跳转
        /// *JS代码嵌入在页面的顶部、表单的最前，适用于要在控件加载前执行的JS代码
        /// </summary>
        /// <param name="page">System.Web.UI.Page   如果是当前页写this</param>
        /// <param name="toPage">跳转页面地址</param>
        /// <param name="frameName">跳转框架名,~top表示顶层框架,~h表示返回上一页,为空表示当前框架跳转</param>
        public static void ToPage(System.Web.UI.Page page, string toPage, string frameName)
        {
            ToPage(page, toPage, frameName, false);
        }
        /// <summary>
        /// 页面跳转
        /// *JS代码嵌入在页面的底部、表单的最后，适用于要在页面控件加载完成后运行的JS代码
        /// </summary>
        /// <param name="toPage">跳转页面地址</param>
        public static void ToPageEnd(string toPage)
        {
            ToPage(System.Web.HttpContext.Current.Handler as System.Web.UI.Page, toPage, "", true);
        }
        /// <summary>
        /// 页面跳转
        /// *JS代码嵌入在页面的底部、表单的最后，适用于要在页面控件加载完成后运行的JS代码
        /// </summary>
        /// <param name="toPage">跳转页面地址</param>
        /// <param name="frameName">跳转框架名,~top表示顶层框架,~h表示返回上一页,为空表示当前框架跳转</param>
        public static void ToPageEnd(string toPage, string frameName)
        {
            ToPage(System.Web.HttpContext.Current.Handler as System.Web.UI.Page, toPage, frameName, true);
        }
        /// <summary>
        /// 页面跳转
        /// *JS代码嵌入在页面的底部、表单的最后，适用于要在页面控件加载完成后运行的JS代码
        /// </summary>
        /// <param name="page">System.Web.UI.Page   如果是当前页写this</param>
        /// <param name="toPage">跳转页面地址</param>
        public static void ToPageEnd(System.Web.UI.Page page, string toPage)
        {
            ToPage(page, toPage, "", true);
        }
        /// <summary>
        /// 页面跳转
        /// *JS代码嵌入在页面的底部、表单的最后，适用于要在页面控件加载完成后运行的JS代码
        /// </summary>
        /// <param name="page">System.Web.UI.Page   如果是当前页写this</param>
        /// <param name="toPage">跳转页面地址</param>
        /// <param name="frameName">跳转框架名,~top表示顶层框架,~h表示返回上一页,为空表示当前框架跳转</param>
        public static void ToPageEnd(System.Web.UI.Page page, string toPage, string frameName)
        {
            ToPage(page, toPage, frameName, true);
        }
        /// <summary>
        /// 页面跳转
        /// </summary>
        /// <param name="page">System.Web.UI.Page   如果是当前页写this</param>
        /// <param name="toPage">跳转页面地址</param>
        /// <param name="frameName">跳转框架名,~top表示顶层框架,~h表示返回上一页,为空表示当前框架跳转</param>
        /// <param name="isEnd">代码是否是嵌入在页面的底部、表单的最后,为true时适用于要在页面控件加载完成后运行的JS代码</param>
        public static void ToPage(System.Web.UI.Page page, string toPage, string frameName, bool isEnd)
        {
            if (!string.IsNullOrEmpty(toPage))
            {
                string scriptcontent;
                toPage = toPage.Replace("\"", "\\\"");
                scriptcontent = "";
                if (!string.IsNullOrEmpty(toPage))
                {
                    if (!string.IsNullOrEmpty(frameName))
                    {
                        switch (frameName)
                        {
                            case "~top":
                                scriptcontent += "top.location.href=\"";
                                break;
                            case "~h":
                                scriptcontent += "history.back(1);";
                                break;
                            default:
                                scriptcontent += "parent." + frameName + ".location.href=\"";
                                break;
                        }
                    }
                    else
                    {
                        scriptcontent += "location.href=\"";
                    }
                    if (frameName != "~h")
                    {
                        scriptcontent += toPage + "\"";
                    }
                }
                if (isEnd)
                {
                    Custom(page, scriptcontent);
                }
                else
                {
                    CustomEnd(page, scriptcontent);
                }
            }
        }
        #endregion 页面跳转
    }
}
