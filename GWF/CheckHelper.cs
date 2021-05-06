using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GWF
{
    public class CheckHelper
    {
        /// <summary>
        /// 오브젝트가 널이거나 아무것도 없을경우
        /// </summary>
        /// <param name="str">문자열 또는 객체 숫자는 0이면 true를 반환함</param>
        /// <returns></returns>
        public static bool isNaN(object str)
        {
            if (str == null)
                return true;

            if(str.GetType().Equals(typeof(string)))
            {
                if(str.ToString().Length == 0)
                    return true;
            }
            else if (str.GetType().Equals(typeof(int)))
            {
                if (Convert.ToInt32(str) == 0)
                    return true;
            }

            return false;
        }

    }
}
