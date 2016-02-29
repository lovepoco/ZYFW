using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace ZY.Framework.Common.Utility
{
    /// <summary>
    /// 分页计算
    /// </summary>
    public class Pagination
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="recordcount">总记录条数</param>
        public Pagination(int recordcount)
            : this(recordcount,1,20)
        {
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="recordcount">总记录条数</param>
        /// <param name="curpage">当前页</param>
        public Pagination(int recordcount, int curpage)
            : this(recordcount, curpage, 20)
        {            
        }
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="recordcount">总记录条数</param>
        /// <param name="curpage">当前页</param>
        /// <param name="pagesize">每页显示数量</param>
        public Pagination(int recordcount,int curpage,int pagesize)
        {
            this.RecordCount = recordcount;
            if (curpage == 0)
            {
                curpage = GetCurrentPage();
            }
            this.CurrentPage = curpage;

            this.PageSize = pagesize;
        }
        /// <summary>
        /// 每页显示数量(私有)
        /// </summary>
        private int _PageSize = 20;
        /// <summary>
        /// 每页显示数量
        /// </summary>
        public int PageSize{
            get {
                return _PageSize;
            }
            set
            {
                if (value > 0)
                {
                    _PageSize = value;
                }
                else
                {
                    _PageSize = 20;
                    throw new Exception("每页显示数不可为零或负数");
                }
            }
        }
        /// <summary>
        /// 总记录条数(私有)
        /// </summary>
        private int _RecordCount = 0;
        /// <summary>
        /// 总记录条数
        /// </summary>
        public int RecordCount{
            get {
                if (_RecordCount < 0) _RecordCount = 0;
                return _RecordCount;
            }
            set {
                if (value >= 0)
                {
                    _RecordCount = value;
                }
                else
                {
                    _RecordCount = 0;
                }
            }
        }
        /// <summary>
        /// 最大页数
        /// </summary>
        public int MaxPage
        {
            get
            {
                int tmp=(int)Math.Ceiling(RecordCount/(double)PageSize);
                if (tmp < 1) tmp = 1;
                return tmp;
            }
        }
        /// <summary>
        /// 当前页(私有)
        /// </summary>
        private int _CurrentPage = 1;
        /// <summary>
        /// 当前页
        /// </summary>
        public int CurrentPage
        {
            get
            {
                //处理超上限
                int _maxpage = MaxPage;
                if (_CurrentPage > _maxpage) _CurrentPage = _maxpage;
                //处理下限
                if (_CurrentPage < 1) _CurrentPage = 1;
                return _CurrentPage;                
            }
            set
            {
                //处理超上限
                if (value > 0)
                {
                    int _maxpage = MaxPage;
                    if (value > _maxpage) value = _maxpage;
                    _CurrentPage = value;
                }
                else
                {
                    _CurrentPage = 1;
                }
            }
        }
        /// <summary>
        /// 数据获取开始索引
        /// </summary>
        public int StartIndex{
            get
            {
                int result = 0;
                result = (CurrentPage - 1) * PageSize;
                return result;
            }
        }
        /// <summary>
        /// 数据获取结束索引
        /// </summary>
        public int EndIndex
        {
            get
            {
                int result = 0;
                result = CurrentPage* PageSize;
                if (result > RecordCount) result = RecordCount;
                return result;
            }
        }
        /// <summary>
        /// 获取当前页
        /// </summary>
        /// <param name="pname">参数对应名，默认page</param>
        /// <param name="psource">参数获取方式，默认get</param>
        /// <returns></returns>
        public int GetCurrentPage(string pname = "page",ParametersSource psource=ParametersSource.Get)
        {
            string _tmp = "";
            int _cpage=_CurrentPage;
            HttpContext hc = HttpContext.Current;
            switch (psource)
            {
                case ParametersSource.Get:
                    _tmp = hc.Request.QueryString[pname];
                    if (int.TryParse(_tmp, out _cpage))
                    {
                        _CurrentPage = _cpage;
                    }
                    break;
                case ParametersSource.Post:
                    _tmp = hc.Request.Form[pname];
                    if (int.TryParse(_tmp, out _cpage))
                    {
                        _CurrentPage = _cpage;
                    }
                    break;
                case ParametersSource.Cookie:
                    _tmp = hc.Request.Cookies[pname].Value;
                    if (int.TryParse(_tmp, out _cpage))
                    {
                        _CurrentPage = _cpage;
                    }
                    break;
                default:
                    _tmp = hc.Request.QueryString[pname];
                    if (string.IsNullOrWhiteSpace(_tmp))
                    {
                        //如果get为空则取post
                        _tmp = hc.Request.Form[pname];
                    }
                    if (int.TryParse(_tmp, out _cpage))
                    {
                        _CurrentPage = _cpage;
                    }
                    break;
            }
            return CurrentPage;
        }
    }
    /// <summary>
    /// 参数来源
    /// </summary>
    public enum ParametersSource
    {
        /// <summary>
        /// get、post方式兼容
        /// </summary>
        All=0,
        /// <summary>
        /// get方式
        /// </summary>
        Get=1,
        /// <summary>
        /// 表单post
        /// </summary>
        Post=2,
        /// <summary>
        /// Cookie
        /// </summary>
        Cookie=3
    }
}
