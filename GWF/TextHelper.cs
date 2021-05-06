using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GWF
{
    /// <summary>
    /// 텍스트 파일 읽기 쓰기
    /// </summary>
    public class TextHelper
    {
        /// <summary>
        /// 주어진 경로의 텍스트 파일을 읽음
        /// </summary>
        /// <param name="sPath"></param>
        /// <returns></returns>
        public static string TextFileRead(string sPath)
        {
            string sFileText = string.Empty;

            try
            {
                if (File.Exists(sPath))
                {
                    StreamReader sr = new StreamReader(sPath, Encoding.Default);

                    sFileText = sr.ReadToEnd();

                    sr.Close();
                }
            }
            catch (Exception ex)
            {
                Log4netHelper.Warn(ref ex);
            }

            return sFileText;
        }

        /// <summary>
        /// 주어진 경로에 문자열을 텍스트 파일로 저장
        /// </summary>
        /// <param name="sPath"></param>
        /// <param name="sMsg"></param>
        public static bool TextFileWrite(string sPath, string sMsg)
        {
            try
            {
                FileStream aFile = new FileStream(sPath, FileMode.OpenOrCreate);
                aFile.Position = aFile.Length;
                StreamWriter sw = new StreamWriter(aFile);

                sw.WriteLine(sMsg);
                sw.Close();

                return true;
            }
            catch (Exception ex)
            {
                Log4netHelper.Warn(ref ex);
            }

            return false;
        }
    }
}
