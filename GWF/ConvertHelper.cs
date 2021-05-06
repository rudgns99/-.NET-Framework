using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GWF
{
    public class ConvertHelper
    {
        /// <summary>
        /// object에서 문자열 값 리턴
        /// </summary>
        /// <param name="data">object</param>
        /// <returns>Empty</returns>
        public static string ToString(object data)
        {
            return ToString(data, string.Empty);
        }

        /// <summary>
        /// object에서 문자열 값 리턴
        /// </summary>
        /// <param name="data">object</param>
        /// <param name="defaultvalue">기본값</param>
        /// <returns></returns>
        public static string ToString(object data, string defaultvalue)
        {
            try
            {
                return data != DBNull.Value && null != data ? Convert.ToString(data) : defaultvalue;
            }
            catch
            {
                return defaultvalue;
            }
        }

        /// <summary>
        /// object에서 byte 값 리턴(오류시 0 리턴)
        /// </summary>
        /// <param name="data">object</param>
        /// <returns>오류시 0 리턴</returns>
        public static byte ToByte(object data)
        {
            return ToByte(data, (byte)0);
        }

        /// <summary>
        /// object에서 byte 값 리턴
        /// </summary>
        /// <param name="data">object</param>
        /// <param name="defaultvalue">기본값</param>
        /// <returns></returns>
        public static byte ToByte(object data, byte defaultvalue)
        {
            try
            {
                return data != DBNull.Value && null != data ? Convert.ToByte(data) : defaultvalue;
            }
            catch
            {
                return defaultvalue;
            }
        }

        /// <summary>
        /// object에서 bool 값 리턴, 실패시 false 리턴
        /// </summary>
        /// <param name="data">object</param>
        /// <returns></returns>
        public static bool ToBool(object data)
        {
            return ToBool(data, false);
        }

        /// <summary>
        /// object에서 bool 값 리턴
        /// </summary>
        /// <param name="data">object</param>
        /// <param name="defaultvalue">기본값</param>
        /// <returns></returns>
        public static bool ToBool(object data, bool defaultvalue)
        {
            try
            {
                return data != DBNull.Value && null != data ? Convert.ToBoolean(data) : defaultvalue;
            }
            catch
            {
                return defaultvalue;
            }
        }

        /// <summary>
        /// object에서 정수 값 리턴(오류시 0 리턴)
        /// </summary>
        /// <param name="data">object</param>
        /// <returns>오류시 0 리턴</returns>
        public static int ToInt(object data)
        {
            return ToInt(data, 0);
        }

        /// <summary>
        /// object에서 정수 값 리턴
        /// </summary>
        /// <param name="data">object</param>
        /// <param name="defaultvalue">기본값</param>
        /// <returns></returns>
        public static int ToInt(object data, int defaultvalue)
        {
            try
            {
                return data != DBNull.Value && null != data ? Convert.ToInt32(data) : defaultvalue;
            }
            catch
            {
                return defaultvalue;
            }
        }

        /// <summary>
        /// object에서 정수 값 리턴(오류시 0 리턴)
        /// </summary>
        /// <param name="data">object</param>
        /// <returns>오류시 0 리턴</returns>
        public static Int64 ToInt64(object data)
        {
            return ToInt64(data, 0);
        }

        /// <summary>
        /// object에서 정수 값 리턴
        /// </summary>
        /// <param name="data">object</param>
        /// <param name="defaultvalue">기본값</param>
        /// <returns></returns>
        public static Int64 ToInt64(object data, Int64 defaultvalue)
        {
            try
            {
                return data != DBNull.Value && null != data ? Convert.ToInt64(data) : defaultvalue;
            }
            catch
            {
                return defaultvalue;
            }
        }

        /// <summary>
        /// object에서 decimal 값 리턴, 오류 시 0 리턴
        /// </summary>
        /// <param name="data">object</param>
        /// <returns></returns>
        public static decimal ToDecimal(object data)
        {
            return ToDecimal(data, 0);
        }

        /// <summary>
        /// object에서 decimal 값 리턴
        /// </summary>
        /// <param name="data">object</param>
        /// <param name="defaultvalue">기본값</param>
        /// <returns></returns>
        public static decimal ToDecimal(object data, decimal defaultvalue)
        {
            try
            {
                return data != DBNull.Value && null != data ? Convert.ToDecimal(data) : defaultvalue;
            }
            catch
            {
                return defaultvalue;
            }
        }

        /// <summary>
        /// object에서 Double 값 리턴(오류시 0 리턴)
        /// </summary>
        /// <param name="data">object</param>
        /// <returns>오류시 0 리턴</returns>
        public static double ToDouble(object data)
        {
            return ToDouble(data, 0.0);
        }

        /// <summary>
        /// object에서 Double 값 리턴
        /// </summary>
        /// <param name="data">object</param>
        /// <param name="defaultvalue">기본값</param>
        /// <returns></returns>
        public static double ToDouble(object data, double defaultvalue)
        {
            try
            {
                return data != DBNull.Value && null != data ? Convert.ToDouble(data) : defaultvalue;
            }
            catch
            {
                return defaultvalue;
            }
        }

        /// <summary>
        /// object에서 Single 값 리턴(오류시 0 리턴)
        /// </summary>
        /// <param name="data">object</param>
        /// <returns>오류시 0 리턴</returns>
        public static double ToSingle(object data)
        {
            return ToSingle(data, 0.0);
        }

        /// <summary>
        /// object에서 Single 값 리턴
        /// </summary>
        /// <param name="data">object</param>
        /// <param name="defaultvalue">기본값</param>
        /// <returns></returns>
        public static double ToSingle(object data, double defaultvalue)
        {
            try
            {
                return data != DBNull.Value && null != data ? Convert.ToSingle(data) : defaultvalue;
            }
            catch
            {
                return defaultvalue;
            }
        }

        /// <summary>
        /// object에서 날짜형 값 리턴
        /// </summary>
        /// <param name="data">object</param>
        /// <returns>DateTime.MinValue</returns>
        public static DateTime ToDateTime(object data)
        {
            return ToDateTime(data, DateTime.MinValue);
        }

        /// <summary>
        /// object에서 날짜형 값 리턴
        /// </summary>
        /// <param name="data">object</param>
        /// <param name="defaultvalue">기본값</param>
        /// <returns></returns>
        public static DateTime ToDateTime(object data, DateTime defaultvalue)
        {
            if (IsDateTimeString(ToString(data)))
                return Convert.ToDateTime(data);

            return data != DBNull.Value && DateTime.TryParse(data as string, out defaultvalue) ? defaultvalue : defaultvalue;
        }

        /// <summary>
        /// object에서 날짜형 값을 지정한 포맷으로 리턴
        /// </summary>
        /// <param name="data">object</param>
        /// <param name="OutputFormat">날짜 형식</param>
        /// <param name="defaultvalue">기본값</param>
        /// <returns></returns>
        public static string ToDateTimeFormatString(object data, string OutputFormat, DateTime defaultvalue)
        {
            return ToDateTime(data, defaultvalue).ToString(OutputFormat);
        }

        /// <summary>
        /// 날짜 형식 여부 판별
        /// </summary>
        /// <param name="data">날짜 문자열</param>
        /// <returns></returns>
        public static bool IsDateTimeString(string data)
        {
            try
            {
                DateTime tm;
                return DateTime.TryParse(data, out tm) ? true : false;
            }
            catch
            {
            }
            return false;
        }

        public static DataSet ToDataSet(DbDataReader reader)
        {
            DataSet ds = new DataSet();
            DataTable schemaDt = reader.GetSchemaTable();
            DataTable dataDt = new DataTable();
            for (int i = 0; i <= schemaDt.Rows.Count - 1; i++)
            {
                DataRow dataRow = schemaDt.Rows[i];
                string columnName = dataRow["ColumnName"].ToString(); DataColumn column = new DataColumn(columnName, dataRow["DataType"].GetType());
                dataDt.Columns.Add(column);
            }
            while (reader.Read())
            {
                DataRow dataRow = dataDt.NewRow();

                for (int i = 0; i <= reader.FieldCount - 1; i++)
                {
                    dataRow[i] = reader.GetValue(i);
                }
                dataDt.Rows.Add(dataRow);
            }
            ds.Tables.Add(dataDt);
            return ds;
        }
    }
}
