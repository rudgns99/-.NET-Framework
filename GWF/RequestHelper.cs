using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;

namespace GWF
{
    /// <summary>
    /// Request 관련 Helper
    /// </summary>
    public class RequestHelper
    {
        #region Query관련
        /// <summary>
        /// QueryString 에서 데이터 가져오기 없으면 string.Empty
        /// </summary>
        /// <param name="id">id</param>
        /// <returns></returns>
        public static string QueryString(string id)
        {
            return QueryString(id, string.Empty);
        }

        /// <summary>
        /// QueryString 에서 문자열 값 리턴
        /// </summary>
        /// <param name="id">id</param>
        /// <param name="defaultvalue">실패시 기본값</param>
        /// <returns></returns>
        public static string QueryString(string id, string defaultvalue)
        {
            try
            {
                return (null != HttpContext.Current && null != HttpContext.Current.Request && null != HttpContext.Current.Request.QueryString && !string.IsNullOrEmpty(HttpContext.Current.Request.QueryString[id])) ? HttpContext.Current.Request.QueryString[id] : defaultvalue;
            }
            catch
            {
                return defaultvalue;
            }
        }

        /// <summary>
        /// QueryString에서 int값 리턴,실패 시 0 리턴
        /// </summary>
        /// <param name="id">id</param>
        /// <returns></returns>
        public static int QueryInt(string id)
        {
            return QueryInt(id, 0);
        }

        /// <summary>
        /// QueryString에서 int값 리턴
        /// </summary>
        /// <param name="id">id</param>
        /// <param name="defaultvalue">실패시 기본값</param>
        /// <returns></returns>
        public static int QueryInt(string id, int defaultvalue)
        {
            string value = QueryString(id);

            try
            {
                return !string.IsNullOrEmpty(value) ? Convert.ToInt32(value.Trim()) : defaultvalue;
            }
            catch
            {
            }

            return defaultvalue;
        }

        /// <summary>
        /// QueryString에서 int64값 리턴, 실패 시 0 리턴
        /// </summary>
        /// <param name="id">id</param>
        /// <returns></returns>
        public static Int64 QueryInt64(string id)
        {
            return QueryInt64(id, 0);
        }

        /// <summary>
        /// QueryString에서 int64값 리턴
        /// </summary>
        /// <param name="id">id</param>
        /// <param name="defaultvalue">실패시 기본값</param>
        /// <returns></returns>
        public static Int64 QueryInt64(string id, Int64 defaultvalue)
        {
            string value = QueryString(id);

            try
            {
                return !string.IsNullOrEmpty(value) ? Convert.ToInt64(value.Trim()) : defaultvalue;
            }
            catch
            {
            }

            return defaultvalue;
        }

        /// <summary>
        /// QueryString에서 Decimal값 리턴, 실패 시 0 리턴
        /// </summary>
        /// <param name="id">id</param>
        /// <returns></returns>
        public static decimal QueryDecimal(string id)
        {
            return QueryDecimal(id, 0);
        }

        /// <summary>
        /// QueryString에서 Decimal값 리턴
        /// </summary>
        /// <param name="id">id</param>
        /// <param name="defaultvalue">실패시 기본값</param>
        /// <returns></returns>
        public static decimal QueryDecimal(string id, decimal defaultvalue)
        {
            string value = QueryString(id);

            try
            {
                return !string.IsNullOrEmpty(value) ? Convert.ToDecimal(value.Trim()) : defaultvalue;
            }
            catch
            {
            }

            return defaultvalue;
        }

        /// <summary>
        /// QueryString에서 Double값 리턴, 실패 시 0 리턴
        /// </summary>
        /// <param name="id">id</param>
        /// <returns></returns>
        public static double QueryDouble(string id)
        {
            return QueryDouble(id, 0);
        }

        /// <summary>
        /// QueryString에서 Double값 리턴
        /// </summary>
        /// <param name="id">id</param>
        /// <param name="defaultvalue">실패시 기본값</param>
        /// <returns></returns>
        public static double QueryDouble(string id, double defaultvalue)
        {
            string value = QueryString(id);

            try
            {
                return !string.IsNullOrEmpty(value) ? Convert.ToDouble(value.Trim()) : defaultvalue;
            }
            catch
            {
            }
            
            return defaultvalue;
        }

        /// <summary>
        /// QueryString에서 DateTime값 리턴
        /// </summary>
        /// <param name="id">id</param>
        /// <param name="parseFormat">id값으로 들어오는 날짜포멧</param>
        /// <param name="cultureInfo">문화스펙</param>
        /// <param name="defaultvalue">실패시 기본값</param>
        /// <returns></returns>
        public static DateTime? QueryDate(string id, string parseFormat, System.Globalization.CultureInfo cultureInfo)
        {
            string value = QueryString(id);

            try
            {
                if(!string.IsNullOrEmpty(value))
                {
                    return DateTime.ParseExact(value.Trim(), parseFormat, cultureInfo);
                }
                else
                {
                    return null;
                }
            }
            catch
            {
            }

            return null;
        }

        /// <summary>
        /// QueryString에서 DateTime값 리턴
        /// </summary>
        /// <param name="id">id</param>
        /// <param name="parseFormat">id값으로 들어오는 날짜포멧</param>
        /// <param name="cultureInfo">문화스펙</param>
        /// <param name="defaultvalue">실패시 기본값</param>
        /// <returns></returns>
        public static DateTime QueryDate(string id, string parseFormat, System.Globalization.CultureInfo cultureInfo, DateTime defaultvalue)
        {
            string value = QueryString(id);

            try
            {
                return !string.IsNullOrEmpty(value) ? DateTime.ParseExact(value.Trim(), parseFormat, cultureInfo) : defaultvalue;
            }
            catch
            {
            }

            return defaultvalue;
        }

        /// <summary>
        /// QueryString에서 Single값 리턴, 실패 시 0 리턴
        /// </summary>
        /// <param name="id">id</param>
        /// <returns></returns>
        public static float QuerySingle(string id)
        {
            return QuerySingle(id, 0);
        }

        /// <summary>
        /// QueryString에서 Single값 리턴
        /// </summary>
        /// <param name="id">id</param>
        /// <param name="defaultvalue">실패시 기본값</param>
        /// <returns></returns>
        public static float QuerySingle(string id, float defaultvalue)
        {
            string value = QueryString(id);

            try
            {
                return !string.IsNullOrEmpty(value) ? Convert.ToSingle(value.Trim()) : defaultvalue;
            }
            catch
            {
            }

            return defaultvalue;
        }
        #endregion

        #region Form관련
        /// <summary>
        /// Form 에서 문자열값 리턴,실패 시 string.empty 리턴
        /// </summary>
        /// <param name="id">id</param>
        /// <returns></returns>
        public static string FormString(string id)
        {
            return FormString(id, string.Empty);
        }

        /// <summary>
        /// Form 에서 문자열값 리턴,실패 시 string.empty 리턴
        /// </summary>
        /// <param name="id">id</param>
        /// <param name="defaultvalue">실패시 기본값</param>
        /// <returns></returns>
        public static string FormString(string id, string defaultvalue)
        {
            try
            {
                return (null != HttpContext.Current && null != HttpContext.Current.Request && null != HttpContext.Current.Request.Form && !string.IsNullOrEmpty(HttpContext.Current.Request.Form[id])) ? HttpContext.Current.Request.Form[id] : defaultvalue;
            }
            catch
            {
                return defaultvalue;
            }
        }

        /// <summary>
        /// Form에서 int값 리턴, 실패 시 0 리턴
        /// </summary>
        /// <param name="id">id</param>
        /// <returns></returns>
        public static int FormInt(string id)
        {
            return FormInt(id, 0);
        }

        /// <summary>
        /// Form에서 int값 리턴
        /// </summary>
        /// <param name="id">id</param>
        /// <param name="defaultvalue">실패시 기본값</param>
        /// <returns></returns>
        public static int FormInt(string id, int defaultvalue)
        {
            string value = FormString(id);

            try
            {
                return !string.IsNullOrEmpty(value) ? Convert.ToInt32(value.Trim()) : defaultvalue;
            }
            catch
            {
            }

            return defaultvalue;
        }

        /// <summary>
        /// Form에서 int64값 리턴, 실패 시 0리턴
        /// </summary>
        /// <param name="id">id</param>
        /// <returns></returns>
        public static Int64 FormInt64(string id)
        {
            return FormInt64(id, 0);
        }

        /// <summary>
        /// Form에서 int64값 리턴
        /// </summary>
        /// <param name="id">id</param>
        /// <param name="defaultvalue">실패시 기본값</param>
        /// <returns></returns>
        public static Int64 FormInt64(string id, Int64 defaultvalue)
        {
            string value = FormString(id);

            try
            {
                return !string.IsNullOrEmpty(value) ? Convert.ToInt64(value.Trim()) : defaultvalue;
            }
            catch
            {
            }

            return defaultvalue;
        }

        /// <summary>
        /// Form에서 Decimal값 리턴, 실패 시 0 리턴
        /// </summary>
        /// <param name="id">id</param>
        /// <returns></returns>
        public static decimal FormDecimal(string id)
        {
            return FormDecimal(id, 0);
        }

        /// <summary>
        /// Form에서 Decimal값 리턴
        /// </summary>
        /// <param name="id">id</param>
        /// <param name="defaultvalue">실패시 기본값</param>
        /// <returns></returns>
        public static decimal FormDecimal(string id, decimal defaultvalue)
        {
            string value = FormString(id);

            try
            {
                return !string.IsNullOrEmpty(value) ? Convert.ToDecimal(value.Trim()) : defaultvalue;
            }
            catch
            {
            }

            return defaultvalue;
        }

        /// <summary>
        /// Form에서 Double값 리턴, 실패시 0 리턴
        /// </summary>
        /// <param name="id">id</param>
        /// <returns></returns>
        public static double FormDouble(string id)
        {
            return FormDouble(id, 0);
        }

        /// <summary>
        /// Form에서 Double값 리턴
        /// </summary>
        /// <param name="id">id</param>
        /// <param name="defaultvalue">실패시 기본값</param>
        /// <returns></returns>
        public static double FormDouble(string id, double defaultvalue)
        {
            string value = FormString(id);

            try
            {
                return !string.IsNullOrEmpty(value) ? Convert.ToDouble(value.Trim()) : defaultvalue;
            }
            catch
            {
            }

            return defaultvalue;
        }

        /// <summary>
        /// Form에서 DateTime값 리턴
        /// </summary>
        /// <param name="id">id</param>
        /// <param name="parseFormat">id값으로 들어오는 날짜포멧</param>
        /// <param name="cultureInfo">문화스펙</param>
        /// <param name="defaultvalue">실패시 기본값</param>
        /// <returns></returns>
        public static DateTime? FormDate(string id, string parseFormat, System.Globalization.CultureInfo cultureInfo)
        {
            string value = FormString(id);

            try
            {
                if (!string.IsNullOrEmpty(value))
                {
                    return DateTime.ParseExact(value.Trim(), parseFormat, cultureInfo);
                }
                else
                {
                    return null;
                }
            }
            catch
            {
            }

            return null;
        }

        /// <summary>
        /// Form에서 DateTime값 리턴
        /// </summary>
        /// <param name="id">id</param>
        /// <param name="parseFormat">id값으로 들어오는 날짜포멧</param>
        /// <param name="cultureInfo">문화스펙</param>
        /// <param name="defaultvalue">실패시 기본값</param>
        /// <returns></returns>
        public static DateTime FormDate(string id, string parseFormat, System.Globalization.CultureInfo cultureInfo, DateTime defaultvalue)
        {
            string value = FormString(id);

            try
            {
                return !string.IsNullOrEmpty(value) ? DateTime.ParseExact(value.Trim(), parseFormat, cultureInfo) : defaultvalue;
            }
            catch
            {
            }

            return defaultvalue;
        }

        /// <summary>
        /// Form에서 Single(float)값 리턴, 실패시 0 리턴
        /// </summary>
        /// <param name="id">id</param>
        /// <returns></returns>
        public static float FormSingle(string id)
        {
            return FormSingle(id, 0);
        }

        /// <summary>
        /// Form에서 Single(float)값 리턴
        /// </summary>
        /// <param name="id">id</param>
        /// <param name="defaultvalue">실패시 기본값</param>
        /// <returns></returns>
        public static float FormSingle(string id, float defaultvalue)
        {
            string value = FormString(id);

            try
            {
                return !string.IsNullOrEmpty(value) ? Convert.ToSingle(value.Trim()) : defaultvalue;
            }
            catch
            {
            }

            return defaultvalue;
        }
        #endregion

        /// <summary>
        /// 프록시 거처오는 Ip 가져오기, 없을 경우 Request.UserHostAddress 리턴
        /// </summary>
        /// <param name="ProxyHeaderName">IP가 담겨있는 해더 이름</param>
        /// <returns></returns>
        public static string Ip(string ProxyHeaderName)
        {
            string ip = ConvertHelper.ToString(HttpContext.Current.Request.ServerVariables[ProxyHeaderName]);
            Regex regex = new Regex(@"^(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])\.(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])\.(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])\.(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])$");
            return regex.IsMatch(ip) ? ip : HttpContext.Current.Request.UserHostAddress;
        }

        /// <summary>
        /// Referer 체크
        /// </summary>
        /// <param name="Hosts">비교할 Host(여러명일 경우 ; 구분)</param>
        /// <returns>같으면 true, 다르면 false</returns>
        public static bool ConfirmReferer(string Hosts)
        {
            try
            {
                Uri ReferUri = HttpContext.Current.Request.UrlReferrer;
                string[] Host = Hosts.Split(';');
                for (int i = 0; i < Host.Length; i++)
                {
                    if (ReferUri.Host.ToLower() == Host[i].ToLower())
                        return true;
                }
                return false;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 현재 Url Host와 Refere Host 비교
        /// </summary>
        /// <returns>같으면 true, 다르면 false</returns>
        public static bool ConfirmReferer()
        {
            try
            {
                if (HttpContext.Current.Request.UrlReferrer.Host == HttpContext.Current.Request.Url.Host) return true;
                else return false;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 모든 From을 가져온다.
        /// 결과는 UTF8 전용
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public static string AllForm(bool isEncoding)
        {
            try
            {
                if (HttpContext.Current.Request.Form.Count < 1)
                    return string.Empty;

                StringBuilder sb = new StringBuilder(128);

                foreach (string key in HttpContext.Current.Request.Form.AllKeys)
                {
                    if (sb.Length > 0) sb.Append("&");
                    if(isEncoding)
                        sb.AppendFormat("{0}={1}", key, HttpUtility.UrlEncode(HttpContext.Current.Request.Form[key], Encoding.UTF8));
                    else
                        sb.AppendFormat("{0}={1}", key, HttpContext.Current.Request.QueryString[key]);
                }

                return sb.ToString();
            }
            catch//(Exception ex)
            {
            }
            return string.Empty;
        }

        /// <summary>
        /// 모든 From을 Json형태로 가져온다.
        /// 결과는 UTF8 전용
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public static string AllFormJson()
        {
            try
            {
                if (HttpContext.Current.Request.Form.Count < 1)
                    return string.Empty;

                StringBuilder sb = new StringBuilder(128);
                sb.Append("{");
                foreach (string key in HttpContext.Current.Request.Form.AllKeys)
                {
                    if (sb.Length > 1) sb.Append(",");
                    sb.AppendFormat(@"{0} : ""{1}""", key, getJsonString(HttpContext.Current.Request.Form[key]));
                }
                sb.Append("}");
                return sb.ToString();
            }
            catch//(Exception ex)
            {
            }
            return string.Empty;
        }

        private static string getJsonString(object val)
        {
            string retVal = string.Empty;
            retVal = ConvertHelper.ToString(val);
            retVal = retVal.Replace(@"\", @"\\");
            retVal = retVal.Replace("\"", "\\\"");
            return retVal;
        }

        /// <summary>
        /// 모든 From을 Json형태로 가져온다.
        /// 결과는 UTF8 전용
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public static string AllQueryStringJson()
        {
            try
            {
                if (HttpContext.Current.Request.QueryString.Count < 1)
                    return string.Empty;

                StringBuilder sb = new StringBuilder(128);
                sb.Append("{");
                foreach (string key in HttpContext.Current.Request.QueryString.AllKeys)
                {
                    if (sb.Length > 1) sb.Append(",");
                    sb.AppendFormat(@"{0} : ""{1}""", key, getJsonString(HttpContext.Current.Request.QueryString[key]));
                }
                sb.Append("}");
                return sb.ToString();
            }
            catch//(Exception ex)
            {
            }
            return string.Empty;
        }


        /// <summary>
        /// 모든 From을 Json형태로 가져온다.
        /// 결과는 UTF8 전용
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public static string AllQueryStringJson(Dictionary<string,string> addQueryString)
        {
            try
            {
                if (HttpContext.Current.Request.QueryString.Count < 1)
                    return string.Empty;

                StringBuilder sb = new StringBuilder(128);
                sb.Append("{");

                foreach (string key in HttpContext.Current.Request.QueryString.AllKeys)
                {
                    if (sb.Length > 1) sb.Append(",");
                    sb.AppendFormat(@"{0} : ""{1}""", key, HttpContext.Current.Request.QueryString[key]);
                }

                foreach (KeyValuePair<string, string> kvp in addQueryString)
                {
                    if (sb.Length > 1) sb.Append(",");
                    sb.AppendFormat(@"{0} : ""{1}""", kvp.Key, kvp.Value);
                }

                sb.Append("}");
                return sb.ToString();
            }
            catch//(Exception ex)
            {
            }
            return string.Empty;
        }

        /// <summary>
        /// 모든 QueryString를 가져온다.
        /// 결과는 UTF8 전용
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public static string AllQueryString(bool isEncoding)
        {
            try
            {
                if (HttpContext.Current.Request.QueryString.Count < 1)
                    return string.Empty;

                StringBuilder sb = new StringBuilder(128);

                foreach (string key in HttpContext.Current.Request.Form.AllKeys)
                {
                    if (sb.Length > 0) sb.Append("&");
                    if(isEncoding)
                        sb.AppendFormat("{0}={1}", key, HttpUtility.UrlEncode(HttpContext.Current.Request.QueryString[key], Encoding.UTF8));
                    else
                        sb.AppendFormat("{0}={1}", key, HttpContext.Current.Request.Form[key]);
                }

                return sb.ToString();
            }
            catch//(Exception ex)
            {
            }
            return string.Empty;
        }


        /// <summary>
        /// QueryString에서 원하는 QueryString을 가져온다.
        /// 결과는 UTF8 전용
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public static string SelectedQueryString(List<string> query)
        {
            try
            {
                if (HttpContext.Current.Request.QueryString.Count < 1)
                    return string.Empty;

                StringBuilder sb = new StringBuilder(128);

                foreach (string key in HttpContext.Current.Request.QueryString.AllKeys)
                {
                    if (query.Contains(key))
                    {
                        if (sb.Length > 0) sb.Append("&");
                        sb.AppendFormat("{0}={1}", key, HttpUtility.UrlEncode(HttpContext.Current.Request.QueryString[key], Encoding.UTF8));
                    }
                }

                return sb.ToString();
            }
            catch//(Exception ex)
            {
            }
            return string.Empty;
        }

        /// <summary>
        /// Post parameter에서 원하는 parameter를 가져온다.
        /// 결과는 UTF8 전용
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public static string SelectedFormString(List<string> query)
        {
            try
            {
                if (HttpContext.Current.Request.Form.Count < 1)
                    return string.Empty;

                StringBuilder sb = new StringBuilder(128);

                foreach (string key in HttpContext.Current.Request.Form.AllKeys)
                {
                    if (query.Contains(key))
                    {
                        if (sb.Length > 0) sb.Append("&");
                        sb.AppendFormat("{0}={1}", key, HttpUtility.UrlEncode(HttpContext.Current.Request.Form[key], Encoding.UTF8));
                    }
                }

                return sb.ToString();
            }
            catch//(Exception ex)
            {
            }
            return string.Empty;
        }

        /// <summary>
        /// QueryString에서 해당 QueryString을 제거하고 가져온다.
        /// 결과는 UTF8 전용
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public static string RemovedQueryString(List<string> query)
        {
            try
            {
                if (HttpContext.Current.Request.QueryString.Count < 1)
                    return string.Empty;

                StringBuilder sb = new StringBuilder(128);


                NameValueCollection ResultParam = HttpContext.Current.Request.QueryString;

                for (int i = 0; i < ResultParam.Count; i++)
                {
                    if (!query.Contains(ResultParam.GetKey(i)))
                    {
                        if (i > 0 && !string.IsNullOrEmpty(sb.ToString())) sb.Append("&");
                        sb.AppendFormat("{0}={1}", ResultParam.GetKey(i), EncodingHelper.URLEncodedValue(ResultParam[i]));
                    }
                }
                return sb.ToString();
            }
            catch//(Exception ex)
            {
            }
            return string.Empty;
        }

        /// <summary>
        /// Post parameter에서 해당 parameter를 제거하고 가져온다.
        /// 결과는 UTF8 전용
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public static string RemovedFormString(List<string> query)
        {
            try
            {
                if (HttpContext.Current.Request.Form.Count < 1)
                    return string.Empty;

                StringBuilder sb = new StringBuilder(128);


                NameValueCollection ResultParam = HttpContext.Current.Request.Form;
                for (int i = 0; i < ResultParam.Count; i++)
                {
                    if (!query.Contains(ResultParam.GetKey(i)))
                    {
                        if (i > 0 && !string.IsNullOrEmpty(sb.ToString())) sb.Append("&");
                        sb.AppendFormat("{0}={1}", ResultParam.GetKey(i), ResultParam[i]);
                    }
                }
                return sb.ToString();
            }
            catch//(Exception ex)
            {
            }
            return string.Empty;
        }

        public static string GetIpAddress(HttpRequest Request)
        {
            string ipAddressString = Request.UserHostAddress;

            if (ipAddressString == null)
                return null;

            System.Net.IPAddress ipAddress;
            System.Net.IPAddress.TryParse(ipAddressString, out ipAddress);

            // If we got an IPV6 address, then we need to ask the network for the IPV4 address 
            // This usually only happens when the browser is on the same machine as the server.
            if (ipAddress.AddressFamily == System.Net.Sockets.AddressFamily.InterNetworkV6)
            {
                ipAddress = System.Net.Dns.GetHostEntry(ipAddress).AddressList
                    .First(x => x.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork);
            }

            return ipAddress.ToString();
        }
    }
}
