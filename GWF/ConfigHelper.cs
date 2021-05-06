using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GWF
{
    /// <summary>
    /// Config Helper class
    /// </summary>
    public class ConfigHelper
    {
        /// <summary>
        /// App키 int값으로 가져오기
        /// </summary>
        /// <param name="strKey">key</param>
        /// <returns>오류시 기본값</returns>
        public static int AppConfigInt(string strKey)
        {
            return ConvertHelper.ToInt(AppConfig(strKey), 0);
        }

        /// <summary>
        /// App키 int값으로 가져오기
        /// </summary>
        /// <param name="strKey">key</param>
        /// <param name="defaultvalue">기본값</param>
        /// <returns>오류시 기본값</returns>
        public static int AppConfigInt(string strKey, int defaultvalue)
        {
            return ConvertHelper.ToInt(AppConfig(strKey), defaultvalue);
        }

        /// <summary>
        /// App키 byte값으로 가져오기
        /// </summary>
        /// <param name="strKey">key</param>
        /// <param name="defaultvalue">기본값</param>
        /// <returns>오류시 기본값</returns>
        public static byte AppConfigByte(string strKey, byte defaultvalue)
        {
            return ConvertHelper.ToByte(AppConfig(strKey), defaultvalue);
        }

        /// <summary>
        /// App키 문자열 값으로 가져오기
        /// </summary>
        /// <param name="strKey">key</param>
        /// <param name="defaultvalue">기본값</param>
        /// <returns>오류시 기본값</returns>
        public static string AppConfig(string strKey, string defaultvalue)
        {
            return ConvertHelper.ToString(AppConfig(strKey), defaultvalue);
        }

        /// <summary>
        /// App키 값 가져오기
        /// </summary>
        /// <param name="strKey">key</param>
        /// <returns>오류시 emtpy</returns>
        public static string AppConfig(string strKey)
        {
            string res = string.Empty;

            try
            {
                if (System.Configuration.ConfigurationManager.AppSettings[strKey] != null)
                    res = System.Configuration.ConfigurationManager.AppSettings[strKey].ToString();
            }
            catch (Exception ex)
            {
                Log4netHelper.Warn(ref ex);
            }

            return res;
        }

        /// <summary>
        /// 연결 문자열 가져오기
        /// </summary>
        /// <param name="strKey">key</param>
        /// <returns></returns>
        public static string ConnectionString(string strKey)
        {
            string res = string.Empty;

            if (System.Configuration.ConfigurationManager.ConnectionStrings[strKey] != null)
                res = System.Configuration.ConfigurationManager.ConnectionStrings[strKey].ConnectionString;

            return res;
        }
    }
}
