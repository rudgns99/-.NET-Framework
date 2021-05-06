using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization.Json;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Data;

namespace GWF
{
    public class JsonHelper
    {
        private JObject parser;
        public int colCount;

        /// <summary>
        /// JSON 형식 데이터를 받아서 사용한다
        /// </summary>
        /// <param name="data">JSON 형식 데이터</param>
        public JsonHelper(object data)
        {
            if (ConvertHelper.ToString(data).Length == 0)
                return;

            parser = JObject.Parse(data.ToString());
            colCount = parser.Count;
        }

        public static T Deserialize<T>(string json)
        {
            return JsonConvert.DeserializeObject<T>(json);
        }

        public static T Deserialize<T>(DataTable dt)
        {
            return JsonConvert.DeserializeObject<T>(DataHelper.JsonStringDataTable(dt));
        }

        public static T Deserialize<T>(DataRow dr)
        {
            return JsonConvert.DeserializeObject<T>(DataHelper.JsonStringDataRow(dr));
        }

        public static string Serialize<T>(T obj)
        {
            return JsonConvert.SerializeObject(obj, Formatting.Indented);
        }

        public static string getFormatter(string json)
        {
            var sr = new StringReader(json);
            var sb = new StringBuilder();
            var preChar = new char();
            int parenthesisCnt = 0;
            int x = 0;

            while(true)
            {
                x = sr.Read();
                if (x.Equals(-1)) break;
                var c = (char)x;
                
                sb.Append(c);

                if (":".IndexOf(c) > -1)
                {
                    sb.Remove(sb.Length - 1, 1);
                    sb.Append(string.Format(" {0} ", c));
                }

                if (",".IndexOf(c) > -1)
                {
                    sb.Append(Environment.NewLine);
                    for (int i = 0; i < parenthesisCnt; i++)
                        sb.Append(string.Empty.PadRight(4, ' '));
                }

                if ("{[".IndexOf(c) > -1)
                {
                    parenthesisCnt++;
                    sb.Append(Environment.NewLine);
                    for (int i = 0; i < parenthesisCnt; i++)
                        sb.Append(string.Empty.PadRight(4, ' '));
                }

                if ("]".IndexOf(c) > -1)
                {
                    if (preChar.Equals('['))
                    {
                        sb.Remove(sb.ToString().LastIndexOf('[') + 1, sb.Length - sb.ToString().LastIndexOf('[') - 1);                        
                        sb.Append(c);
                        parenthesisCnt--;
                    }
                    else
                    {
                        sb.Remove(sb.Length - 1, 1);
                        sb.Append(Environment.NewLine);
                        parenthesisCnt--;
                        for (int i = 0; i < parenthesisCnt; i++)
                            sb.Append(string.Empty.PadRight(4, ' '));
                        sb.Append(c);
                    }
                }

                if ("}".IndexOf(c) > -1)
                {
                    sb.Remove(sb.Length - 1, 1);
                    sb.Append(Environment.NewLine);
                    parenthesisCnt--;
                    for (int i = 0; i < parenthesisCnt; i++)
                        sb.Append(string.Empty.PadRight(4, ' '));
                    sb.Append(c);
                }
                preChar = c;
            }

            return sb.ToString();
        }

        /// <summary>
        /// JsonHelper로 생성해서, 사용.
        /// Key를 넣으면 Value 값을 리턴함.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public string GetJsonValue(string name)
        {
            string result = string.Empty;

            try
            {
                result = parser.GetValue(name).ToString();
            }
            catch (Exception ex)
            {
                string error = ex.Message;
                // TODO : save log
            }

            return result;
        }

        /// <summary>
        /// JsonHelper로 생성해서, 사용.
        /// Key를 넣으면 리스트 형식의 Value 값을 리턴함.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public List<string> GetJsonObjects(string name)
        {
            List<string> results = new List<string>();

            try
            {
                var jsonCollection = JArray.Parse(GetJsonValue(name));

                foreach (var jsonObject in jsonCollection)
                {
                    results.Add(jsonObject.ToString());
                }
            }
            catch (Exception ex)
            {
                string error = ex.Message;
                // TODO : save log
            }

            return results;
        }

        /// <summary>
        /// JsonHelper로 생성해서, 사용.
        /// Key를 넣으면 Dictionary 형식의 Value 값을 리턴함.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public Dictionary<string, string> GetJsonCollectionValue(string name)
        {
            Dictionary<string, string> results = null;

            try
            {
                results = JsonConvert.DeserializeObject<Dictionary<string, string>>(parser.GetValue(name).ToString());
            }
            catch (Exception ex)
            {
                results = null;
                string error = ex.Message;
                // TODO : save log
            }

            return results;
        }
    }
}
