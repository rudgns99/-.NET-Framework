using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace GWF
{
    public class LogHelper
    {

        public static void AshxLogger(string path, object obj)
        {
            if(obj.GetType().Equals(typeof(string)))
            {
                AshxLogger(path, ConvertHelper.ToString(obj));
            }
            else
            {
                AshxLogger(path, obj as Exception);
            }
        }

        private static void AshxLogger(string path, string message)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("[{0}] {1}{2}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), message, Environment.NewLine);
            string FileName = string.Format("AshxHandler [{0}].log", DateTime.Now.ToString("yyyy-MM-dd"));
            string PathFileName = Path.Combine(path, FileName);
            if (!Directory.Exists(path)) { Directory.CreateDirectory(path); }
            IOHelper.TextFileWrite(PathFileName, sb.ToString(), Encoding.UTF8);
        }

        private static void AshxLogger(string path, Exception ex)
        {
            if (ex == null)
                return;

            string time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            string msg = ex.Message;
            string stt = ex.StackTrace;
            string sc = ex.Source;
            string msgd = string.Empty;
            string sttd = string.Empty;
            string scd = string.Empty;
            if (ex.InnerException != null)
            {
                msgd = ex.InnerException.Message;
                sttd = ex.InnerException.StackTrace;
                scd = ex.InnerException.Source;
            }
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("[{0}]┌ {1}{2}", time, msg, Environment.NewLine);
            sb.AppendFormat("[{0}]│ {1}{2}", time, stt, Environment.NewLine);
            sb.AppendFormat("[{0}]│ {1}{2}", time, sc, Environment.NewLine);
            sb.AppendFormat("[{0}]│ {1}{2}", time, msgd, Environment.NewLine);
            sb.AppendFormat("[{0}]│ {1}{2}", time, sttd, Environment.NewLine);
            sb.AppendFormat("[{0}]└ {1}{2}", time, scd, Environment.NewLine);
            string FileName = string.Format("AshxHandler [{0}].log", DateTime.Now.ToString("yyyy-MM-dd"));
            string PathFileName = Path.Combine(path, FileName);
            if (!Directory.Exists(path)) { Directory.CreateDirectory(path); }
             IOHelper.TextFileWrite(PathFileName, sb.ToString(), Encoding.UTF8);
        }
    }
}
