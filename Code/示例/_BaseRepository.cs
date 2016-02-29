using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.Entity;
using JD.Framework.Data.EF;
using CNGJRC.Data.Model;

namespace CNGJRC.Data.Repository
{
    /// <summary>
    /// 仓库基类
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public abstract class _BaseRepository<TEntity> : GenericRepository<CNGJRCDBEntities, TEntity>
        where TEntity : class
    {
        protected _BaseRepository()
        {
            DbContextFactory<CNGJRCDBEntities> _dbc = new DbContextFactory<CNGJRCDBEntities>("CNGJRCDBEntities");
            base._dbContextFactory = _dbc;
            base.initDBContextFactory();
        }
    }
}
