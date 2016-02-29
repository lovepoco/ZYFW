using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using JD.Framework.Data.EF;

namespace CNGJRC.Services
{
    /// <summary>
    /// 服务基类
    /// </summary>
    /// <typeparam name="TRepository">仓库类</typeparam>
    /// <typeparam name="TEntity">实体</typeparam>
    public abstract class _BaseServices<TRepository, TEntity> : GenericService<TRepository, TEntity>
        where TRepository : IGenericRepository<TEntity>, new()
        where TEntity : class
    {
    }
}
