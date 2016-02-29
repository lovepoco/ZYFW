using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using ZY.Framework.Data.ICommon;

namespace ZY.Framework.Data.EF
{
    /// <summary>
    /// 数据服务
    /// <para>功能实现实体</para>
    /// </summary>
    /// <typeparam name="TRepository"></typeparam>
    /// <typeparam name="TEntity"></typeparam>
    public abstract class GenericService<TRepository, TEntity> : IGenericService<TRepository, TEntity>
        where TRepository : IGenericRepository<TEntity>, new()
        where TEntity : class
    {
        #region 属性
        /// <summary>
        /// 仓库类
        /// </summary>
        protected TRepository _rep = new TRepository();
        /// <summary>
        /// where条件器
        /// </summary>
        protected Expression<Func<TEntity, bool>> _Where = PredicateExtensions.True<TEntity>();
        /// <summary>
        /// 表总记录数.
        /// </summary>
        public int TotalCount
        {
            get
            {
                return _rep.TotalCount;
            }
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
        /// 取得所有记录
        /// </summary>
        public List<TEntity> GetAll()
        {
            return _rep.GetAll().ToList();
        }
        /// <summary>
        /// 过滤记录
        /// <para>相当于Where功能</para>
        /// </summary>
        /// <param name="predicate">过滤条件</param>
        public List<TEntity> Filter(Expression<Func<TEntity, bool>> predicate)
        {
            return _rep.Filter(predicate).ToList();
        }
        /// <summary>
        /// 获取数据
        /// </summary>
        /// <param name="filter">过滤条件</param>
        /// <param name="orderBy">排充条件</param>
        /// <param name="selector">查询字段  请使继承实体的子类</param>
        /// <param name="includeProperties">同步加载数据</param>
        /// <returns></returns>
        public virtual List<TEntity> Get(Expression<Func<TEntity, bool>> filter = null, Func<IQueryable<TEntity>,
            IOrderedQueryable<TEntity>> orderBy = null, Expression<Func<TEntity, TEntity>> selector = null, string includeProperties = "")
        {
            return _rep.Get(filter, orderBy, selector, includeProperties).ToList();
        }
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
        public virtual List<TEntity> GetTop(int num = 10, Expression<Func<TEntity, bool>> filter = null, Func<IQueryable<TEntity>,
           IOrderedQueryable<TEntity>> orderBy = null, Expression<Func<TEntity, TEntity>> selector = null, string includeProperties = "")
        {
            if (num < 0) throw new Exception("不可取负数量条数数据");
            return _rep.Get(filter, orderBy, selector, includeProperties).Take(num).ToList();
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
        public virtual List<TEntity> GetPaged(out int total, int index = 0, int size = 20, Expression<Func<TEntity, bool>> filter = null, Func<IQueryable<TEntity>,
           IOrderedQueryable<TEntity>> orderBy = null, Expression<Func<TEntity, TEntity>> selector = null, string includeProperties = "")
        {
            return _rep.GetPaged(out total, index, size, filter, orderBy, selector, includeProperties).ToList();
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
            result = _rep.Count(predicate);
            return result;
        }
        /// <summary>
        /// 是否存在
        /// </summary>
        /// <param name="predicate">条件</param>
        public virtual bool Contains(Expression<Func<TEntity, bool>> predicate)
        {
            return _rep.Contains(predicate);
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
        /// <param name="predicate"></param>
        /// <param name="isNoTracking"></param>
        /// <returns></returns>
        public virtual TEntity Find(Expression<Func<TEntity, bool>> predicate = null, bool isNoTracking = false)
        {
            return _rep.Find(predicate, isNoTracking);
        }
        /// <summary>
        /// 添加
        /// <para>请及时执行Save</para>
        /// </summary>
        /// <param name="t"></param>
        public virtual void Add(TEntity model)
        {
            _rep.Add(model);
        }
        /// <summary>
        /// 删除
        /// <para>请及时执行Save</para>
        /// </summary>
        /// <param name="t"></param>        
        public virtual void Delete(TEntity model)
        {
            _rep.Delete(model);
        }
        /// <summary>
        /// 删除多条
        /// <para>请及时执行Save</para>
        /// </summary>
        /// <param name="predicate">条件</param>
        public virtual void Delete(Expression<Func<TEntity, bool>> predicate)
        {
            _rep.Delete(predicate);
        }
        /// <summary>
        /// 更新
        /// <para>请及时执行Save</para>
        /// </summary>
        /// <param name="t"></param>
        public virtual void Update(TEntity model)
        {
            _rep.Update(model);
        }
        /// <summary>
        /// 保存
        /// <para>此处执行的是所有保存，同库操作一般只需要执行一次保存</para>
        /// </summary>
        /// <returns></returns>
        public virtual int Save()
        {
            return _rep.Save();
        }
        #endregion

        #region 系统底层
        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose()
        {
            if (_rep != null)
                _rep.Dispose();
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}
