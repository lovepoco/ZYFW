using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZY.Framework.Common.Cache
{
    /// <summary>
    /// 缓存基类
    /// </summary>
    public abstract class CacheBase : ICache
    {
        #region 属性
        private TimeSpan maxDuration = TimeSpan.FromDays(15);
        /// <summary>
        /// 最长持续时间
        /// </summary>
        public TimeSpan MaxDuration
        {
            get
            {
                return this.maxDuration;
            }
            set
            {
                this.maxDuration = value;
            }
        }
        /// <summary>
        /// 前缀
        /// </summary>
        public string Prefix
        {
            get;
            set;
        }
        #endregion

        #region 添加
        /// <summary>
        /// 添加
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool Add<T>(string key, T value)
        {
            return this.Add<T>(key, value, TimeSpan.FromHours(1));
        }
        /// <summary>
        /// 添加
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="duration"></param>
        /// <returns></returns>
        public abstract bool Add<T>(string key, T value, TimeSpan duration);
        #endregion

        #region 获取
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public abstract T Get<T>(string key);
        /// <summary>
        ///  获取全名
        /// </summary>
        /// <param name="key">键</param>
        /// <returns>全名</returns>
        public virtual string GetFullName(string key)
        {
            string result = key;
            if (!string.IsNullOrWhiteSpace(this.Prefix))
            {
                result = string.Format("{0}.{1}", this.Prefix, key);
            }

            return result;
        }
        /// <summary>
        /// 列表获取
        /// </summary>
        /// <param name="keys"></param>
        /// <returns></returns>
        public abstract IDictionary<string, object> MultiGet(IList<string> keys);
        #endregion

        #region 设置
        /// <summary>
        /// 设置
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool Set<T>(string key, T value)
        {
            return this.Set<T>(key, value, TimeSpan.FromHours(1));
        }
        /// <summary>
        /// 设置
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="duration"></param>
        /// <returns></returns>
        public abstract bool Set<T>(string key, T value, TimeSpan duration);
        #endregion

        #region 清理
        /// <summary>
        /// 单条删除
        /// </summary>
        /// <param name="key"></param>
        public abstract void Remove(string key);
        /// <summary>
        /// 清理
        /// </summary>
        public abstract void Clear();
        #endregion
    }
}
