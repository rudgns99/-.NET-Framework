using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace GWF
{
    public class RegexHelper
    {

        /// <summary>
        /// 특수문자 삭제
        /// </summary>
        /// <param name="str">입력문자열</param>
        /// <returns></returns>
        public static string DelSymbolString(string str)
        {
            return Regex.Replace(str, @"[^0-9a-zA-Zㄱ-힗]+", "");
        }
        
        /// <summary>
        /// 이메일 체크
        /// </summary>
        /// <param name="str">입력문자열</param>
        /// <returns></returns>
        public static bool isEmail(string str)
        {
            return Regex.IsMatch(str, @"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$");
        }

        /// <summary>
        /// URL 체크
        /// </summary>
        /// <param name="str">입력문자열</param>
        /// <returns></returns>
        public static bool isURL(string str)
        {
            return Regex.IsMatch(str, @"http://([\w-]+\.)+[\w-]+(/[\w- ./?%&=]*)?");
        }

        /// <summary>
        /// 사용자정의 정규식 체크
        /// </summary>
        /// <param name="str">입력문자열</param>
        /// <param name="pattern">정규식패턴</param>
        /// <returns></returns>
        public static bool isPattern(string str, string pattern)
        {
            return Regex.IsMatch(str, pattern);
        }

        /// <summary>
        /// 숫자로만 이루어져 있는지 확인.
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool isNumberString(string str)
        {
            if (String.IsNullOrEmpty(str)) return false;
            return Regex.IsMatch(str, "^([0-9]+)$");
        }

        /// <summary>
        /// 숫자인지 확인한다.
        /// 음수기호(-)를 포함하거나
        /// 소수점이 있어도 확인이 된다.
        /// 양수(+)기호가 있는경우 False
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool isFloatString(string str)
        {
            if (String.IsNullOrEmpty(str)) return false;
            return Regex.IsMatch(str, "^(-?0[.]\\d+)$|^(-?[1-9]+\\d*([.]\\d+)?)$|^0$");
        }
    }
}
