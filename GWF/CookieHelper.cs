using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace GWF
{
    /// <summary>
    /// 쿠키 생성 및 가져오기 클래스
    /// </summary>
    public class CookieHelper
    {
        /// <summary>
        /// 쿠키에서 값 리턴
        /// </summary>
        /// <param name="id">id</param>
        /// <returns></returns>
        public static string GetCookie(string id)
        {
            try
            {
                return (null != HttpContext.Current && null != HttpContext.Current.Request && null != HttpContext.Current.Request.Cookies && null != HttpContext.Current.Request.Cookies[id]) ? HttpContext.Current.Request.Cookies[id].Value : string.Empty;
            }
            catch
            {
            }

            return string.Empty;
        }

        /// <summary>
        /// 쿠키 설정
        /// </summary>
        /// <param name="id">쿠키명</param>
        /// <param name="value">쿠키 값</param>
        /// <param name="path">쿠키 경로</param>
        /// <param name="CookieDomain">쿠키 도메인</param>
        /// <param name="HttpOnly">클라이언트측 스크립트를 통해 쿠키에 액세스할 수 없으면 true이고, 그렇지 않으면 false</param>
        /// <param name="Secure">SSL 연결(HTTPS)을 통해 쿠키를 전송하면 true이고, 그렇지 않으면 false</param>
        /// <returns></returns>
        public static bool SetCookie(string id, string value, string path, string CookieDomain, bool HttpOnly, bool Secure)
        {
            try
            {
                if (null != HttpContext.Current && null != HttpContext.Current.Response && null != HttpContext.Current.Response.Cookies)
                {
                    HttpCookie cookie = new HttpCookie(id, value);
                    if (!string.IsNullOrEmpty(path)) cookie.Path = path;
                    if (!string.IsNullOrEmpty(CookieDomain)) cookie.Domain = CookieDomain;
                    cookie.HttpOnly = HttpOnly;
                    cookie.Secure = Secure;
                    HttpContext.Current.Response.Cookies.Add(cookie);

                    return true;
                }
            }
            catch
            {
            }
            return false;
        }

        /// <summary>
        /// 쿠키 설정
        /// </summary>
        /// <param name="id">쿠키명</param>
        /// <param name="value">쿠키 값</param>
        /// <param name="path">쿠키 경로</param>
        /// <param name="CookieDomain">쿠키 도메인</param>
        /// <param name="expiredDateTime">쿠키 만료 날짜</param>
        /// <param name="HttpOnly">클라이언트측 스크립트를 통해 쿠키에 액세스할 수 없으면 true이고, 그렇지 않으면 false</param>
        /// <param name="Secure">SSL 연결(HTTPS)을 통해 쿠키를 전송하면 true이고, 그렇지 않으면 false</param>
        /// <returns></returns>
        public static bool SetCookie(string id, string value, string path, string CookieDomain, DateTime expiredDateTime, bool HttpOnly, bool Secure)
        {
            try
            {
                if (null != HttpContext.Current && null != HttpContext.Current.Response && null != HttpContext.Current.Response.Cookies)
                {
                    HttpCookie cookie = new HttpCookie(id, value);
                    if (!string.IsNullOrEmpty(path)) cookie.Path = path;
                    if (!string.IsNullOrEmpty(CookieDomain)) cookie.Domain = CookieDomain;
                    cookie.Expires = expiredDateTime;
                    cookie.HttpOnly = HttpOnly;
                    cookie.Secure = Secure;
                    HttpContext.Current.Response.Cookies.Add(cookie);

                    return true;
                }
            }
            catch
            {
            }
            return false;
        }
    }
}
