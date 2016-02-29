using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using ZY.Framework.Data.ICommon;
using Dapper;

namespace ZY.Framework.Data.Dapper
{
    /// <summary>
    /// 数据仓库
    /// <para>功能实现实体</para>
    /// </summary>
    /// <typeparam name="TContext">数据库上下文</typeparam>
    /// <typeparam name="TEntity">实体</typeparam>
    public abstract class GenericRepository<TContext, TEntity> : IGenericRepository<TEntity>
        where TContext : IDbConnection, new()
        where TEntity : class
    {
        #region 临时变量
        private StringBuilder _tmpsb = new StringBuilder();
        private string _tmpstr = "";
        #endregion

        #region 属性
        /// <summary>
        /// 上下文
        /// </summary>
        protected TContext _context;
        /// <summary>
        /// 上下文工厂
        /// </summary>
        protected ZY.Framework.Data.ICommon.IDbContextFactory<TContext> _dbContextFactory;

        /// <summary>
        /// 空构造
        /// <para>继承类需要初始此构造，初始上下文与上下文工厂</para>
        /// </summary>
        protected GenericRepository()
        {
        }
        /// <summary>
        /// 构造
        /// </summary>
        /// <param name="dbContextFactory">上下文工厂</param>
        protected GenericRepository(ZY.Framework.Data.ICommon.IDbContextFactory<TContext> dbContextFactory)
        {
            _dbContextFactory = dbContextFactory;
            _context = _dbContextFactory.GetDbContext();
        }
        /// <summary>
        /// 初始上下文工厂
        /// </summary>
        protected void initDBContextFactory()
        {
            if (_dbContextFactory != null)
            {
                _context = _dbContextFactory.GetDbContext();
            }
            else
            {
                throw new Exception("错误的连接，请与开发人员联系");
            }
        }
        /// <summary>
        /// 上下文工厂
        /// </summary>
        protected TContext Context
        {
            get
            {
                return _context;
            }
        }

        /// <summary>
        /// 对应表名
        /// <para>不可为空</para>
        /// </summary>
        protected string Table = "";
        /// <summary>
        /// 表前缀
        /// </summary>
        protected string TablePrefix = "";
        /// <summary>
        /// 表后缀
        /// </summary>
        protected string TableSubfix = "";
        /// <summary>
        /// 表全称
        /// <para>前缀+表名+后缀</para>
        /// </summary>
        protected string TableFullName
        {
            get
            {
                return TablePrefix + Table + TableSubfix;
            }
        }
        /// <summary>
        /// 表全称带分隔符
        /// <para>前缀+表名+后缀</para>
        /// </summary>
        protected string TableFullNameAndSeparator
        {
            get
            {
                return SeparatorPrefix + TablePrefix + Table + TableSubfix + SeparatorSubfix;
            }
        }
        /// <summary>
        /// 分隔符前
        /// <para>表名列名分隔</para>
        /// </summary>
        protected string SeparatorPrefix = "";
        /// <summary>
        /// 分隔符后
        /// <para>表名列名分隔</para>
        /// </summary>
        protected string SeparatorSubfix = "";
        /// <summary>
        /// 总记录数
        /// </summary>
        public int TotalCount
        {
            get
            {
                int result = 0;
                _tmpsb = new StringBuilder();
                _tmpsb.Append("select count(*) as cnt from " + TableFullNameAndSeparator);
                result = Context.Query<int>(_tmpsb.ToString()).FirstOrDefault();
                return result;
            }
        }
        #endregion

        #region List
        /// <summary>
        /// 所有记录
        /// </summary>
        /// <returns></returns>
        public IQueryable<TEntity> GetAll()
        {
            _tmpsb = new StringBuilder();
            _tmpsb.Append("select * from " + TableFullNameAndSeparator);
            return Context.Query<TEntity>(_tmpsb.ToString()).AsQueryable();
        }
        /// <summary>
        /// 过滤记录【未用】
        /// <para>相当于Where功能</para>
        /// </summary>
        /// <param name="predicate">过滤条件</param>
        public virtual IQueryable<TEntity> Filter(Expression<Func<TEntity, bool>> predicate)
        {
            throw new NotImplementedException("未实现方法");
        }
        /// <summary>
        /// 过滤记录
        /// <para>相当于Where功能</para>
        /// </summary>
        /// <param name="predicate">过滤条件</param>
        public virtual IQueryable<TEntity> Filter(string predicate)
        {
            if (string.IsNullOrWhiteSpace(predicate)) throw new Exception("错误的筛选条件，筛选条件不能为空！");

            _tmpsb = new StringBuilder();
            _tmpsb.Append("select * from " + TableFullNameAndSeparator + " where 1=1 and " + predicate);
            return Context.Query<TEntity>(_tmpsb.ToString()).AsQueryable();
        }

        /// <summary>
        /// 过滤记录(分页)【未用】
        /// <para>相当于Where功能</para>
        /// </summary>
        /// <typeparam name="Key"></typeparam>
        /// <param name="predicate">过滤条件</param>
        /// <param name="total">总记录数</param>
        /// <param name="index">当前页</param>
        /// <param name="size">每页多少</param>
        /// <returns></returns>
        public virtual IQueryable<TEntity> Filter(Expression<Func<TEntity, bool>> filter, out int total, int index = 0, int size = 50)
        {
            throw new NotImplementedException("方法未实现");
        }
        /// <summary>
        /// 过滤记录(分页)
        /// <para>相当于Where功能</para>
        /// </summary>
        /// <typeparam name="Key"></typeparam>
        /// <param name="predicate">过滤条件</param>
        /// <param name="total">总记录数</param>
        /// <param name="index">当前页</param>
        /// <param name="size">每页多少</param>
        /// <returns></returns>
        public virtual IQueryable<TEntity> Filter(string filter, out int total, int index = 0, int size = 50)
        {
            int skipCount = index * size;
            throw new NotImplementedException("方法未实现");
        }

        /// <summary>
        /// 获取数据【未用】
        /// </summary>
        /// <param name="filter">过滤条件</param>
        /// <param name="orderBy">排充条件</param>
        /// <param name="selector">查询字段  请使继承实体的子类</param>
        /// <param name="includeProperties">同步加载数据</param>
        /// <returns></returns>
        public virtual IQueryable<TEntity> Get(Expression<Func<TEntity, bool>> filter = null, Func<IQueryable<TEntity>,
            IOrderedQueryable<TEntity>> orderBy = null, Expression<Func<TEntity, TEntity>> selector = null, string includeProperties = "")
        {
            throw new NotImplementedException("方法未实现");
        }
        /// <summary>
        /// 获取数据
        /// </summary>
        /// <param name="filter">过滤条件</param>
        /// <param name="orderBy">排充条件</param>
        /// <param name="selector">查询字段  请使继承实体的子类</param>
        /// <param name="includeProperties">同步加载数据</param>
        /// <returns></returns>
        public virtual IQueryable<TEntity> Get(string filter = null, string orderBy = null, string selector = null, string includeProperties = "")
        {
            throw new NotImplementedException("方法未实现");
        }

        /// <summary>
        /// 获取分页数据
        /// </summary>
        /// <param name="total">记录数</param>
        /// <param name="index">当前页索引</param>
        /// <param name="size">获取数量</param>
        /// <param name="filter">过滤条件</param>
        /// <param name="orderBy">排充条件</param>
        /// <param name="selector">查询字段  请使继承实体的子类</param>
        /// <param name="includeProperties">同步加载数据</param>
        /// <returns></returns>
        public virtual IQueryable<TEntity> GetPaged(out int total, int index = 0, int size = 20, Expression<Func<TEntity, bool>> filter = null, Func<IQueryable<TEntity>,
           IOrderedQueryable<TEntity>> orderBy = null, Expression<Func<TEntity, TEntity>> selector = null, string includeProperties = "")
        {
            int skipCount = (index - 1) * size;
            var _reset = Get(filter, orderBy, selector, includeProperties);
            total = _reset.Count();
            _reset = skipCount <= 0 ? _reset.Take(size) : _reset.Skip(skipCount).Take(size);
            return _reset.AsQueryable();
        }

        /// <summary>
        /// 获取分页数据
        /// </summary>
        /// <param name="total">记录数</param>
        /// <param name="index">当前页索引</param>
        /// <param name="size">获取数量</param>
        /// <param name="filter">过滤条件</param>
        /// <param name="orderBy">排充条件</param>
        /// <param name="selector">查询字段  请使继承实体的子类</param>
        /// <param name="includeProperties">同步加载数据</param>
        /// <returns></returns>
        public virtual IQueryable<TEntity> GetPaged(out int total, int index = 0, int size = 20, string filter = null, string orderBy = null, string selector = null, string includeProperties = "")
        {
            int skipCount = (index - 1) * size;
            var _reset = Get(filter, orderBy, selector, includeProperties);
            total = _reset.Count();
            _reset = skipCount <= 0 ? _reset.Take(size) : _reset.Skip(skipCount).Take(size);
            return _reset.AsQueryable();
        }
        #endregion

        #region CURD
        /// <summary>
        /// 统计总数【未用】
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public virtual int Count(Expression<Func<TEntity, bool>> predicate)
        {
            throw new NotImplementedException("方法未实现");
        }
        /// <summary>
        /// 统计总数
        /// </summary>
        /// <param name="predicate">直接and拼接</param>
        /// <returns></returns>
        public virtual int Count(string predicate)
        {
            int result = 0;
            if (!string.IsNullOrWhiteSpace(predicate))
            {
                _tmpsb = new StringBuilder();
                _tmpsb.Append("select count(*) as cnt from " + TableFullNameAndSeparator + " where 1=1 and " + predicate);
                result = Context.Query<int>(_tmpsb.ToString()).FirstOrDefault();
            }
            else
            {
                result = TotalCount;
            }
            return result;
        }

        /// <summary>
        /// 是否存在【未用】
        /// </summary>
        /// <param name="predicate">条件</param>
        public virtual bool Contains(Expression<Func<TEntity, bool>> predicate)
        {
            throw new NotImplementedException("方法未实现");
        }
        /// <summary>
        /// 是否存在
        /// </summary>
        /// <param name="predicate">条件</param>
        public virtual bool Contains(string predicate)
        {
            return Count(predicate) > 0;
        }

        /// <summary>
        /// 是否存在【未用】
        /// </summary>
        /// <param name="predicate">条件</param>
        public virtual bool isExist(Expression<Func<TEntity, bool>> predicate)
        {
            throw new NotImplementedException("方法未实现");
        }
        /// <summary>
        /// 是否存在
        /// </summary>
        /// <param name="predicate">条件</param>
        public virtual bool isExist(string predicate)
        {
            return Count(predicate) > 0;
        }

        /// <summary>
        /// 取一条记录
        /// <para>方法禁用</para>
        /// </summary>
        /// <param name="keys"></param>
        public virtual TEntity Find(params object[] keys)
        {
            throw new NotImplementedException("方法禁用");
        }
        /// <summary>
        /// 取一条记录【未用】
        /// </summary>
        /// <param name="predicate"></param>
        /// <param name="isNoTracking"></param>
        /// <returns></returns>
        public virtual TEntity Find(Expression<Func<TEntity, bool>> predicate = null, bool isNoTracking = false)
        {
            throw new NotImplementedException("方法未实现");
        }
        /// <summary>
        /// 取一条记录
        /// </summary>
        /// <param name="predicate"></param>
        /// <param name="isNoTracking">属性无效</param>
        /// <returns></returns>
        public virtual TEntity Find(string predicate = null, bool isNoTracking = false)
        {
            TEntity model;
            _tmpsb = new StringBuilder();
            _tmpsb.Append("select * from " + TableFullNameAndSeparator + " where 1=1 and " + predicate);
            model = Context.Query<TEntity>(_tmpsb.ToString()).FirstOrDefault();
            return model;
        }

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="model"></param>
        public virtual void Add(TEntity model)
        {
            throw new NotImplementedException("方法未实现");
        }

        /// <summary>
        /// 更新
        /// <para>请及时执行Save</para>
        /// </summary>
        /// <param name="model"></param>
        public virtual void Update(TEntity model)
        {
            throw new NotImplementedException("方法未实现");
        }

        /// <summary>
        /// 删除
        /// <para>请及时执行Save</para>
        /// </summary>
        /// <param name="t"></param>
        public virtual void Delete(TEntity model)
        {
            throw new NotImplementedException("方法未实现");
        }

        /// <summary>
        /// 删除多条【未用】
        /// <para>请及时执行Save</para>
        /// </summary>
        /// <param name="predicate">条件</param>
        public virtual void Delete(Expression<Func<TEntity, bool>> predicate)
        {
            var toDelete = Filter(predicate);
            foreach (var obj in toDelete)
            {
                Delete(obj);
            }
        }
        /// <summary>
        /// 删除多条
        /// <para>请及时执行Save</para>
        /// </summary>
        /// <param name="predicate">条件</param>
        public virtual void Delete(string predicate)
        {
            _tmpsb = new StringBuilder();
            _tmpsb.Append("delete * from " + TableFullNameAndSeparator + " where 1=1 and " + predicate);
            Context.Execute(_tmpsb.ToString());
        }

        /// <summary>
        /// 保存
        /// <para>不适应Dapper</para>
        /// </summary>
        /// <returns></returns>
        public int Save()
        {
            throw new NotImplementedException("此模式下此方法不适应：Dapper");
        }
        #endregion

        #region 系统底层
        /// <summary>
        /// 执行原生SQL(查询)
        /// <para>用于查询执行</para>
        /// </summary>
        /// <param name="query"></param>
        /// <param name="parameters">未使用</param>
        /// <returns></returns>
        public IQueryable<TEntity> GetWithRawSql(string query, params object[] parameters)
        {
            return Context.Query<TEntity>(query).AsQueryable();
        }
        /// <summary>
        /// 执行原生
        /// <para>危险底层方法，只对Service层提供</para>
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="parameters">未使用</param>
        /// <returns></returns>
        public int ExecuteRawSql(string sql, params object[] parameters)
        {
            return Context.Execute(sql);
        }
        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose()
        {
            if (Context != null)
                Context.Dispose();
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}
