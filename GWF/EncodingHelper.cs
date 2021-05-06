using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GWF
{
    /// <summary>
    /// Url Encode, Base64 Encode
    /// </summary>
    public class EncodingHelper
    {
        /// <summary>
        /// Plaintext문자열을 URL Encode
        /// </summary>
        /// <param name="plaintext"></param>
        /// <returns></returns>
        public static string URLEncodedValue(string plaintext)
        {
            return System.Web.HttpUtility.UrlEncode(plaintext, System.Text.Encoding.UTF8);
        }

        /// <summary>
        /// Urlencode문자열을 Plaintext로 디코드
        /// </summary>
        /// <param name="encodedtext">Urlencode된 문자열</param>
        /// <returns></returns>
        public static string URLDecodedValue(string encodedtext)
        {
            return System.Web.HttpUtility.UrlDecode(encodedtext, System.Text.Encoding.UTF8);
        }

        /// <summary>
        /// Plaintext문자열을 Base64로 인코드
        /// </summary>
        /// <param name="encodedtext"></param>
        /// <returns></returns>
        public static string Base64Encode(string encodedtext)
        {
            return Convert.ToBase64String(Encoding.UTF8.GetBytes(encodedtext));
        }

        /// <summary>
        /// Base64문자열을 Plaintext로 디코드
        /// </summary>
        /// <param name="encodedtext">Base64문자열</param>
        /// <returns></returns>
        public static string Base64Decode(string encodedtext)
        {
            return Encoding.UTF8.GetString(Convert.FromBase64String(encodedtext));
        }
    }
}
