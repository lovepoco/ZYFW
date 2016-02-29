using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using ZY.Framework.Data.ICommon;

namespace ZY.Framework.Data.EF
{
    /// <summary>
    /// 数据仓库
    /// <para>功能实现实体</para>
    /// </summary>
    /// <typeparam name="TContext">数据库上下文</typeparam>
    /// <typeparam name="TEntity">实体</typeparam>
    public abstract class GenericRepository<TContext, TEntity> : IGenericRepository<TEntity>
        where TContext : DbContext, new()
        where TEntity : class
    {
        #region 属性
        /// <summary>
        /// 上下文
        /// </summary>
        protected TContext _context;
        /// <summary>
        /// 上下文工厂
        /// </summary>
        protected ZY.Framework.Data.ICommon.IDbContextFactory<TContext> _dbContextFactory;
        private IDbSet<TEntity> __set;
        protected IDbSet<TEntity> _set
        {
            get
            {
                return __set;
            }
        }
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
            __set = _context.Set<TEntity>();
        }
        /// <summary>
        /// 初始上下文工厂
        /// </summary>
        protected void initDBContextFactory()
        {
            if (_dbContextFactory != null)
            {
                _context = _dbContextFactory.GetDbContext();
                __set = _context.Set<TEntity>();
            }
            else
            {
                throw new Exception("错误的上下文工厂，请与开发人员联系");
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
        /// 数据集
        /// </summary>
        protected IDbSet<TEntity> DbSet
        {
            get
            {
                return _set == null ? _context.Set<TEntity>() : _set;
            }
        }
        /// <summary>
        /// where条件器
        /// </summary>
        protected Expression<Func<TEntity, bool>> _Where = PredicateExtensions.True<TEntity>();
        /// <summary>
        /// 总记录数
        /// </summary>
        public int TotalCount
        {
            get { return DbSet.Count(); }
        }
        #endregion

        #region 功能
        /// <summary>
        /// 初始where条件器
        /// </summary>
        public void InitWhere()
        {
            _Where = PredicateExtensions.True<TEntity>();
        }
        #endregion

        #region List
        /// <summary>
        /// 所有记录
        /// </summary>
        /// <returns></returns>
        public IQueryable<TEntity> GetAll()
        {
            return DbSet.AsQueryable();
        }

        /// <summary>
        /// 过滤记录
        /// <para>相当于Where功能</para>
        /// </summary>
        /// <param name="predicate">过滤条件</param>
        public virtual IQueryable<TEntity> Filter(Expression<Func<TEntity, bool>> predicate)
        {
            if (predicate == null) throw new Exception("错误的筛选条件，筛选条件不能为空！");
            return DbSet.Where(predicate).AsQueryable<TEntity>();
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
        public virtual IQueryable<TEntity> Filter(Expression<Func<TEntity, bool>> filter, out int total, int index = 0, int size = 50)
        {
            int skipCount = index * size;
            var _resetSet = filter != null ? DbSet.Where(filter).AsQueryable() : DbSet.AsQueryable();
            _resetSet = skipCount == 0 ? _resetSet.Take(size) : _resetSet.Skip(skipCount).Take(size);
            total = _resetSet.Count();
            return _resetSet.AsQueryable();
        }

        /// <summary>
        /// 获取数据
        /// </summary>
        /// <param name="filter">过滤条件</param>
        /// <param name="orderBy">排充条件</param>
        /// <param name="selector">查询字段  请使继承实体的子类</param>
        /// <param name="includeProperties">同步加载数据</param>
        /// <returns></returns>
        public virtual IQueryable<TEntity> Get(Expression<Func<TEntity, bool>> filter = null, Func<IQueryable<TEntity>,
            IOrderedQueryable<TEntity>> orderBy = null, Expression<Func<TEntity, TEntity>> selector = null, string includeProperties = "")
        {
            IQueryable<TEntity> query = DbSet;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            if (selector != null)
            {
                query = query.Select(selector);
            }

            foreach (var includeProperty in includeProperties.Split
                (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }

            if (orderBy != null)
            {
                return orderBy(query).AsQueryable();
            }
            else
            {
                return query.AsQueryable();
            }
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
        #endregion

        #region CURD
        /// <summary>
        /// 统计总数
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public virtual int Count(Expression<Func<TEntity, bool>> predicate)
        {
            int result = 0;
            if (predicate != null)
            {
                result = DbSet.Count(predicate);
            }
            else
            {
                result = TotalCount;
            }
            return result;
        }
        /// <summary>
        /// 是否存在
        /// </summary>
        /// <param name="predicate">条件</param>
        public virtual bool Contains(Expression<Func<TEntity, bool>> predicate)
        {
            return DbSet.Any(predicate); ;
        }

        /// <summary>
        /// 是否存在
        /// </summary>
        /// <param name="predicate">条件</param>
        public virtual bool isExist(Expression<Func<TEntity, bool>> predicate)
        {
            return Contains(predicate);
        }

        /// <summary>
        /// 取一条记录
        /// </summary>
        /// <param name="keys"></param>
        public virtual TEntity Find(params object[] keys)
        {
            return DbSet.Find(keys);
        }
        /// <summary>
        /// 取一条记录
        /// </summary>
        /// <param name="predicate"></param>
        /// <param name="isNoTracking"></param>
        /// <returns></returns>
        public virtual TEntity Find(Expression<Func<TEntity, bool>> predicate = null, bool isNoTracking = false)
        {
            TEntity model;
            var dbs = DbSet.AsQueryable();
            if (isNoTracking) dbs = dbs.AsNoTracking();
            if (predicate == null)
            {
                model = dbs.FirstOrDefault();
            }
            else
            {
                model = dbs.FirstOrDefault(predicate);
            }
            return model;
        }

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="model"></param>
        public virtual void Add(TEntity model)
        {
            DbSet.Add(model);
        }

        /// <summary>
        /// 更新
        /// <para>请及时执行Save</para>
        /// </summary>
        /// <param name="model"></param>
        public virtual void Update(TEntity model)
        {
            RemoveHoldingEntityInContext<TEntity>(model);
            DbSet.Attach(model);
            var entry = Context.Entry(model);
            entry.State = EntityState.Modified;
        }

        /// <summary>
        /// 删除
        /// <para>请及时执行Save</para>
        /// </summary>
        /// <param name="t"></param>
        public virtual void Delete(TEntity model)
        {
            if (Context.Entry(model).State == EntityState.Detached)
            {
                DbSet.Attach(model);
            }
            DbSet.Remove(model);
        }

        /// <summary>
        /// 删除多条
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
        /// 保存
        /// <para>-1表示失败</para>
        /// <para>此处执行的是所有保存，同库操作一般只需要执行一次保存</para>
        /// </summary>
        /// <returns></returns>
        public int Save()
        {
            return Context.SaveChanges();
        }
        #endregion

        #region 系统底层
        /// <summary>
        /// 执行原生SQL(查询)
        /// <para>用于查询执行</para>
        /// </summary>
        /// <param name="query"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public IQueryable<TEntity> GetWithRawSql(string query, params object[] parameters)
        {
            return Context.Database.SqlQuery<TEntity>(query, parameters).AsQueryable();
        }
        /// <summary>
        /// 执行原生
        /// <para>危险底层方法，只对Service层提供</para>
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public int ExecuteRawSql(string sql, params object[] parameters)
        {
            return Context.Database.ExecuteSqlCommand(sql, parameters);
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
        /// <summary>
        /// 清理实体跟踪关系
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="_context"></param>
        /// <param name="entity"></param>
        /// <returns></returns>
        private Boolean RemoveHoldingEntityInContext<TEntity>(TEntity entity)
            where TEntity : class
        {
            var objContext = ((IObjectContextAdapter)_context).ObjectContext;
            var objSet = objContext.CreateObjectSet<TEntity>();
            var entityKey = objContext.CreateEntityKey(objSet.EntitySet.Name, entity);

            Object foundEntity;
            var exists = objContext.TryGetObjectByKey(entityKey, out foundEntity);

            if (exists)
            {
                objContext.Detach(foundEntity);
            }

            return (exists);
        }
        #endregion
    }
}
