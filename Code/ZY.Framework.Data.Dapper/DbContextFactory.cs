using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using Dapper;


namespace ZY.Framework.Data.Dapper
{
    /// <summary>
    /// 数据上下文工厂
    /// </summary>
    /// <typeparam name="TContext"></typeparam>
    public class DbContextFactory<TContext> : ZY.Framework.Data.ICommon.IDbContextFactory<TContext>, IDisposable
        where TContext : IDbConnection, new()
    {
        private string _connectionString;
        private string __configname = "dbContext";
        private string _configname
        {
            set
            {
                __configname = value;
            }
            get
            {
                if (string.IsNullOrWhiteSpace(__configname))
                {
                    __configname = "dbContext";
                }
                return __configname;
            }
        }
        private TContext _context;

        public DbContextFactory()
            : this("", true)
        {
        }
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="val">值(默认为配置名称)</param>
        /// <param name="isConfigName">是表示配置名称 否表示为连接语句 默认：是</param>
        public DbContextFactory(string val, bool isConfigName = true)
        {
            if (isConfigName)
            {
                _configname = val;
                try
                {
                    _connectionString = ConfigurationManager.ConnectionStrings[_configname].ConnectionString;
                }
                catch
                {
                    throw new Exception("连接字符配置名有误，请与开发人员联系");
                }
            }
            else
            {
                _connectionString = val;
                initContext();
            }
        }
        /// <summary>
        /// 初始上下文
        /// </summary>
        private void initContext(bool isLazyLoad = false)
        {
            if (!string.IsNullOrWhiteSpace(_connectionString))
            {
                try
                {
                    _context = Activator.CreateInstance<TContext>();
                    _context.ConnectionString = _connectionString;
                    if (_context.State != ConnectionState.Open)
                    {
                        _context.Open();
                    }
                }
                catch
                {
                    throw new Exception("连接字符有误，请与开发人员联系");
                }
            }
            else
            {
                throw new Exception("错误的连接字符串");
            }
        }
        /// <summary>
        /// 获取数据上下文对象
        /// </summary>
        /// <returns></returns>
        public TContext GetDbContext()
        {
            if (_context == null)
            {
                initContext();
            }
            return _context == null ? Activator.CreateInstance<TContext>() : _context;
        }
        /// <summary>
        /// 释放对象
        /// </summary>
        public void Dispose()
        {
            if (_context != null)
                _context.Dispose();

            GC.SuppressFinalize(this);
        }
    }
}
