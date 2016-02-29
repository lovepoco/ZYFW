using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZY.Framework.Data.ICommon
{
    /// <summary>
    /// 数据上下文工厂接口
    /// </summary>
    /// <typeparam name="TContext"></typeparam>
    public interface IDbContextFactory<TContext> where TContext : new()
    {
        TContext GetDbContext();
    }
}
