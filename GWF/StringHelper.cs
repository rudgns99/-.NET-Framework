using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GWF
{
    /// <summary>
    /// 문자열 처리 헬퍼
    /// </summary>
    public class StringHelper
    {
        /// <summary>
        /// 문자열 Padding 처리
        /// </summary>
        /// <param name="padstring">pad할 문자/문자열</param>
        /// <param name="count">반복수</param>
        /// <returns>문자열</returns>
        public static string Padding(string padstring, int count)
        {
            string ret = string.Empty;
            while (count-- > 0)
                ret += padstring;

            return ret;
        }

        /// <summary>
        /// 랜덤한 숫자 문자열 만들기 
        /// </summary>
        /// <param name="length">길이(최대 8자리)</param>
        /// <returns>문자열</returns>
        public static string RandomNumberString(int length)
        {
            System.Random ranNum = new System.Random();
            return ranNum.Next(100000000, 999999999).ToString().Substring(0, length > 8 ? 8 : length);
        }

        /// <summary>
        /// 랜덤한 문자열 만들기
        /// </summary>
        /// <param name="length">길이</param>
        /// <returns>문자열</returns>
        public static string RandomString(int length)
        {
            int rnum = 0;
            int i, j;
            string ranStr = null;

            System.Random ranNum = new System.Random();

            for (i = 0; i <= length; i++)
            {
                for (j = 48; j <= 122; j++)
                {
                    rnum = ranNum.Next(48, 123);
                    if (rnum >= 48 && rnum <= 122 && (rnum <= 57 || rnum >= 65) && (rnum <= 90 || rnum >= 97))
                    {
                        break;
                    }
                }

                ranStr += Convert.ToChar(rnum);
            }

            return (ranStr);
        }

        /// <summary>
        /// 지정된 문자열 길이만큼 문자열 반환(영문1바이트,한글2바이트 처리)
        /// </summary>
        /// <param name="orgText">문자열</param>
        /// <param name="max">최대 길이</param>
        /// <param name="overString">최대 길이가 넘을 때, 생략 표시 문자열</param>
        /// <returns></returns>
        public static string CutString(string orgText, int max, string overString)
        {
            string result = string.Empty;
            if (orgText != null && orgText != string.Empty)
            {
                int nByte = 0;
                char[] cArr = orgText.ToCharArray();
                for (int i = 0; i < cArr.Length; i++)
                {
                    int keyCode = Convert.ToInt32(cArr[i]);
                    if (keyCode < 0 || keyCode >= 128)
                    {
                        nByte += 2; //한글 2byte
                    }
                    else
                    {
                        nByte++;  //영문은 1바이트
                    }



                    if (nByte <= max)
                    {
                        result += orgText.Substring(i, 1);
                    }
                    else
                    {
                        result = result.Trim();
                        if (!string.IsNullOrEmpty(overString))
                            result += overString;
                        break;
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// 텍스트 길이를 가져온다. 한글은 2Byte 처리
        /// </summary>
        /// <param name="orgText">문자열</param>
        /// <returns>byte 문자열 길이</returns>
        public static int ByteLength(string orgText)
        {
            string result = string.Empty;
            int nByte = 0;

            if (orgText != null && orgText != string.Empty)
            {
                char[] cArr = orgText.ToCharArray();
                for (int i = 0; i < cArr.Length; i++)
                {
                    int keyCode = Convert.ToInt32(cArr[i]);
                    if (keyCode < 0 || keyCode >= 128)
                    {
                        nByte += 2; //한글 2byte
                    }
                    else
                    {
                        nByte++;  //영문은 1바이트
                    }
                }
            }

            return nByte;
        }

        /// <summary>
        /// QueryString 형태의 문자열을 Dictionary 형태로 변환
        /// </summary>
        /// <param name="param">QueryString 형태의 문자열</param>
        /// <returns></returns>
        public static Dictionary<string, string> QueryStringToArray(string param)
        {
            Dictionary<string, string> query_string = new Dictionary<string, string>();
            string query = string.Empty;
            try
            {
                if (string.IsNullOrEmpty(param))
                    return null;

                string[] components = param.Split('&');
                foreach (string kv in components)
                {
                    int pos = kv.IndexOf('=');
                    if (pos != -1)
                    {
                        string key = EncodingHelper.URLDecodedValue(kv.Substring(0, pos).ToLower());
                        string val = string.Empty;

                        if (!query_string.ContainsKey(key))
                            val = EncodingHelper.URLDecodedValue(kv.Substring(pos + 1));

                        if (!string.IsNullOrEmpty(val))
                            query_string.Add(key, val);
                    }
                }
            }
            catch (Exception ex)
            {
                Log4netHelper.Error(ref ex);
            }

            return query_string;
        }

        public static string FirstCharToUpper(object input)
        {
            switch (ConvertHelper.ToString(input, string.Empty))
            {
                case null: throw new ArgumentNullException(nameof(input));
                case "": throw new ArgumentException($"{nameof(input)} cannot be empty", nameof(input));
                default: return ConvertHelper.ToString(input, string.Empty).First().ToString().ToUpper() + ConvertHelper.ToString(input, string.Empty).Substring(1);
            }
        }

    }

}
