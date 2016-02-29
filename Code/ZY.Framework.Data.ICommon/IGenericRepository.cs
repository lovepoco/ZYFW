using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ZY.Framework.Data.ICommon
{
    /// <summary>
    /// 数据仓库接口(基类)
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public interface IGenericRepository<TEntity> : IDisposable
           where TEntity : class
    {
        #region 属性
        /// <summary>
        /// 总记录数
        /// </summary>
        int TotalCount { get; }
        #endregion

        #region List
        /// <summary>
        /// 所有数据
        /// </summary>
        IQueryable<TEntity> GetAll();

        /// <summary>
        /// 过滤记录
        /// <para>相当于Where功能</para>
        /// </summary>
        /// <param name="predicate">过滤条件</param>
        IQueryable<TEntity> Filter(Expression<Func<TEntity, bool>> predicate);

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
        IQueryable<TEntity> Filter(Expression<Func<TEntity, bool>> predicate,
            out int total, int index = 0, int size = 50);

        /// <summary>
        /// 获取数据
        /// </summary>
        /// <param name="filter">过滤条件</param>
        /// <param name="orderBy">排充条件</param>
        /// <param name="selector">查询字段  请使继承实体的子类</param>
        /// <param name="includeProperties">同步加载数据</param>
        /// <returns></returns>
        IQueryable<TEntity> Get(Expression<Func<TEntity, bool>> filter = null, Func<IQueryable<TEntity>,
            IOrderedQueryable<TEntity>> orderBy = null, Expression<Func<TEntity, TEntity>> selector = null, string includeProperties = "");

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
        IQueryable<TEntity> GetPaged(out int total, int index = 0, int size = 20, Expression<Func<TEntity, bool>> filter = null, Func<IQueryable<TEntity>,
           IOrderedQueryable<TEntity>> orderBy = null, Expression<Func<TEntity, TEntity>> selector = null, string includeProperties = "");
        #endregion

        #region CURD
        /// <summary>
        /// 统计总数
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        int Count(Expression<Func<TEntity, bool>> predicate);
        /// <summary>
        /// 是否存在
        /// </summary>
        /// <param name="predicate">条件</param>
        bool Contains(Expression<Func<TEntity, bool>> predicate);

        /// <summary>
        /// 是否存在
        /// </summary>
        /// <param name="predicate">条件</param>
        bool isExist(Expression<Func<TEntity, bool>> predicate);

        /// <summary>
        /// 取一条记录
        /// </summary>
        /// <param name="keys"></param>
        TEntity Find(params object[] keys);

        /// <summary>
        /// 取一条记录
        /// </summary>
        /// <param name="predicate"></param>
        /// <param name="isNoTracking"></param>
        /// <returns></returns>
        TEntity Find(Expression<Func<TEntity, bool>> predicate, bool isNoTracking = false);

        /// <summary>
        /// 添加
        /// <para>请及时执行Save</para>
        /// </summary>
        /// <param name="model"></param>
        void Add(TEntity model);

        /// <summary>
        /// 更新
        /// <para>请及时执行Save</para>
        /// </summary>
        /// <param name="model"></param>
        void Update(TEntity model);

        /// <summary>
        /// 删除
        /// <para>请及时执行Save</para>
        /// </summary>
        /// <param name="t"></param>
        void Delete(TEntity model);

        /// <summary>
        /// 删除多条
        /// <para>请及时执行Save</para>
        /// </summary>
        /// <param name="predicate">条件</param>
        void Delete(Expression<Func<TEntity, bool>> predicate);

        /// <summary>
        /// 保存
        /// <para>此处执行的是所有保存，同库操作一般只需要执行一次保存</para>
        /// </summary>
        /// <returns></returns>
        int Save();
        #endregion

        #region 系统底层
        /// <summary>
        /// 执行原生SQL(查询)
        /// </summary>
        /// <param name="query">语句</param>
        /// <param name="parameters">参数</param>
        /// <returns>实体</returns>
        IQueryable<TEntity> GetWithRawSql(string query, params object[] parameters);
        /// <summary>
        /// 执行原生SQL
        /// </summary>
        /// <param name="sql">语句</param>
        /// <param name="parameters">参数</param>
        /// <returns></returns>
        int ExecuteRawSql(string sql, params object[] parameters);
        #endregion
    }
}
