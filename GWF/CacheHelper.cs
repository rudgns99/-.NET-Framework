using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Caching;

namespace GWF
{
    /// <summary>
    /// 캐시 관련
    /// </summary>
    public class CacheHelper
    {
        /// <summary>
        /// key에 해당하는 캐시 존재 여부
        /// </summary>
        /// <param name="key">캐시명</param>
        /// <returns></returns>
        public static bool HasCache(string key)
        {
            if (HttpContext.Current != null && HttpContext.Current.Cache != null && HttpContext.Current.Cache[key] != null)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// key에 해당하는 캐시 가져오기
        /// </summary>
        /// <param name="key">캐시명</param>
        /// <returns></returns>
        public static object GetCache(string key)
        {
            if (HttpContext.Current != null && HttpContext.Current.Cache != null && HttpContext.Current.Cache[key] != null)
            {
                return HttpContext.Current.Cache[key];
            }
            return null;
        }

        /// <summary>
        /// 캐시 추가
        /// </summary>
        /// <param name="key">캐시명</param>
        /// <param name="data">캐시값</param>
        /// <returns></returns>
        public static bool AddCache(string key, object data)
        {
            if (HttpContext.Current != null && HttpContext.Current.Cache != null)
            {
                HttpContext.Current.Cache.Insert(key, data, null, Cache.NoAbsoluteExpiration, new TimeSpan(0, 30, 0));
                return true;
            }
            return false;
        }

        /// <summary>
        /// 캐시 삭제
        /// </summary>
        /// <param name="key">캐시명</param>
        /// <returns></returns>
        public static bool DeleteCache(string key)
        {
            if (HttpContext.Current != null && HttpContext.Current.Cache != null)
            {
                HttpContext.Current.Cache.Remove(key);
                return true;
            }
            return false;
        }
    }
}
