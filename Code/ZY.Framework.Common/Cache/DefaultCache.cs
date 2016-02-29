using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Caching;

namespace ZY.Framework.Common.Cache
{
    /// <summary>
    /// 默认缓存 - aspnet缓存方式
    /// </summary>
    public class DefaultCache:CacheBase
    {
        private System.Web.Caching.Cache cache = HttpRuntime.Cache;

        #region 构造
        /// <summary>
        /// 构造函数
        /// </summary>
        public DefaultCache()
            : this("Common.Cache")
        {

        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="prefix">前缀</param>
        public DefaultCache(string prefix)
        {
            this.Prefix = prefix;
        }
        #endregion

        #region 添加
        /// <summary>
        /// 添加
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="duration"></param>
        /// <returns></returns>
        public override bool Add<T>(string key, T value, TimeSpan duration)
        {
            bool result = false;
            if (value != null)
            {
                if (duration <= TimeSpan.Zero)
                {
                    duration = this.MaxDuration;
                }
                result = this.cache.Add(this.GetFullName(key), value, null, DateTime.Now.Add(duration), System.Web.Caching.Cache.NoSlidingExpiration, CacheItemPriority.Default, null) == null;
            }

            return result;
        }
        #endregion

        #region 获取
        /// <summary>
        /// 获取
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public override T Get<T>(string key)
        {
            T result = default(T);
            object value = this.cache.Get(this.GetFullName(key));
            if (value is T)
            {
                result = (T)value;
            }

            return result;
        }
        /// <summary>
        /// 获取多条
        /// </summary>
        /// <param name="keys"></param>
        /// <returns></returns>
        public override IDictionary<string, object> MultiGet(IList<string> keys)
        {
            IDictionary<string, object> result = new Dictionary<string, object>();
            foreach (string key in keys)
            {
                result.Add(key, this.Get<object>(key));
            }

            return result;
        }
        #endregion

        #region 设置
        /// <summary>
        /// 设置
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="duration"></param>
        /// <returns></returns>
        public override bool Set<T>(string key, T value, TimeSpan duration)
        {
            bool result = false;
            if (value != null)
            {
                if (duration <= TimeSpan.Zero)
                {
                    duration = this.MaxDuration;
                }
                this.cache.Insert(this.GetFullName(key), value, null, DateTime.Now.Add(duration), System.Web.Caching.Cache.NoSlidingExpiration);
                result = true;
            }

            return result;
        }
        #endregion

        #region 清理
        /// <summary>
        /// 清理
        /// </summary>
        /// <param name="key"></param>
        public override void Remove(string key)
        {
            this.cache.Remove(this.GetFullName(key));
        }
        /// <summary>
        /// 清理所有
        /// </summary>
        public override void Clear()
        {
            //	获取键集合
            IList<string> keys = new List<string>();
            IDictionaryEnumerator caches = this.cache.GetEnumerator();
            while (caches.MoveNext())
            {
                string key = caches.Key.ToString();
                if (key.StartsWith(this.Prefix))
                {
                    keys.Add(key);
                }
            }
            //	移除全部
            foreach (string key in keys)
            {
                this.cache.Remove(key);
            }
        }
        #endregion
    }
}
