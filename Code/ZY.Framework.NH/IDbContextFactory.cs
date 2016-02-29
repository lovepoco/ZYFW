using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NHibernate;
using NHibernate.Context;

namespace ZY.Framework.Data.EF
{
    /// <summary>
    /// 数据上下文工厂接口
    /// </summary>
    /// <typeparam name="TContext"></typeparam>
    public interface IDbContextFactory<TContext> where TContext : StringComparer, new()
    {
        TContext GetDbContext();
    }
}
