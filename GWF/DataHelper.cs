using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Net.Json;
using System.Reflection;
using System.Xml;

namespace GWF
{
    public class DataHelper
    {
        /// <summary>
        /// DataSet에서 번호에 해당하는 테이블이 존재하는지 여부(행의 갯수가 0개 이상이어야 true)
        /// </summary>
        /// <param name="ds">데이타셋</param>
        /// <param name="TableNumber">원하는 테이블 번호(최대 갯수가 아님)</param>
        /// <returns>존재하면 true</returns>
        public static bool HasRow(DataSet ds, int TableNumber)
        {
            if (ds != null && ds.Tables.Count > TableNumber && ds.Tables[TableNumber].Rows != null && ds.Tables[TableNumber].Rows.Count > 0)
            {
                return true;
            }
            else
                return false;
        }

        /// <summary>
        /// DataSet에서 0번 테이블이 존재하는지 여부(행의 갯수가 0개 이상이어야 true)
        /// </summary>
        /// <param name="ds">데이타셋</param>
        /// <returns>존재하면 true</returns>
        public static bool HasRow(DataSet ds)
        {
            return HasRow(ds, 0);
        }

        /// <summary>
        /// DataTable에서 데이터가 존재하는지 여부
        /// </summary>
        /// <param name="dt">데이타테이블</param>
        /// <returns>존재하면 true</returns>
        public static bool HasRow(DataTable dt)
        {
            return (dt != null && dt.Rows.Count > 0);
        }

        /// <summary>
        /// DataSet에서 지정한 번호의 Table을 가져온다
        /// </summary>
        /// <param name="ds">데이타셋</param>
        /// <param name="TableNumber">원하는 테이블 번호(최대 갯수가 아님)</param>
        /// <returns>존재하면 해당 table, 아니면 null</returns>
        public static DataTable Table(DataSet ds, int TableNumber)
        {
            if (ds != null && ds.Tables.Count > TableNumber && ds.Tables[TableNumber].Rows != null && ds.Tables[TableNumber].Rows.Count > 0)
            {
                return ds.Tables[TableNumber];
            }
            else
                return null;
        }

        /// <summary>
        /// DataSet에서 0번 Table을 가져온다
        /// </summary>
        /// <param name="ds">데이타셋</param>
        /// <returns>존재하면 해당 table, 아니면 null</returns>
        public static DataTable Table(DataSet ds)
        {
            return Table(ds, 0);
        }

        /// <summary>
        /// DataSet에서 DataTable 수
        /// </summary>
        /// <param name="ds">데이타셋</param>
        /// <returns>DataTable 수</returns>
        public static int TableCount(DataSet ds)
        {
            try
            {
                if (ds != null)
                {
                    return ds.Tables.Count;
                }
            }
            catch
            {
            }
            return 0;
        }

        /// <summary>
        /// DataSet에서 지정한 테이블의 Row 수
        /// </summary>
        /// <param name="ds">데이타셋</param>
        /// <param name="TableNumber">원하는 테이블 번호(최대 갯수가 아님)</param>
        /// <returns>Row 수</returns>
        public static int TableRowCount(DataSet ds, int TableNumber)
        {
            if (ds != null && ds.Tables.Count > TableNumber && ds.Tables[TableNumber].Rows != null && ds.Tables[TableNumber].Rows.Count > 0)
            {
                return ds.Tables[TableNumber].Rows.Count;
            }
            else
                return 0;
        }

        /// <summary>
        /// DataSet에서 0번째 테이블의 Row 수
        /// </summary>
        /// <param name="ds">데이타셋</param>
        /// <returns>Row 수</returns>
        public static int TableRowCount(DataSet ds)
        {
            return TableRowCount(ds, 0);
        }

        /// <summary>
        /// DataTable의 Row 수
        /// </summary>
        /// <param name="dt">테이블</param>
        /// <returns>Row 수</returns>
        public static int TableRowCount(DataTable dt)
        {
            try
            {
                if (dt != null)
                {
                    return dt.Rows.Count;
                }
            }
            catch
            {
            }
            return 0;
        }

        /// <summary>
        /// DataSet에서 지정된 테이블, 행 및 컬럼이 DB NULL 또는 값을 읽을 수 없는지 확인
        /// </summary>
        /// <param name="ds">데이타셋</param>
        /// <param name="TableNumber">원하는 테이블 번호(최대 갯수가 아님)</param>
        /// <param name="RowNumber">원하는 행</param>
        /// <param name="ColumnName">컬럼명</param>
        /// <returns>NULL 일 경우 및 값이 없는 경우 true</returns>
        public static bool IsNULL(DataSet ds, int TableNumber, int RowNumber, string ColumnName)
        {
            try
            {
                if (HasRow(ds, TableNumber))
                {
                    return ds.Tables[TableNumber].Rows[RowNumber][ColumnName] == System.DBNull.Value;
                }
            }
            catch
            {
            }
            return true;
        }

        /// <summary>
        /// DataSet에서 0번째 테이블, 행 및 컬럼이 DB NULL 또는 값을 읽을 수 없는지 확인
        /// </summary>
        /// <param name="ds">데이타셋</param>
        /// <param name="RowNumber">원하는 행</param>
        /// <param name="ColumnName">컬럼명</param>
        /// <returns>NULL 일 경우 및 값이 없는 경우 true</returns>
        public static bool IsNULL(DataSet ds, int RowNumber, string ColumnName)
        {
            try
            {
                if (HasRow(ds))
                {
                    return ds.Tables[0].Rows[RowNumber][ColumnName] == System.DBNull.Value;
                }
            }
            catch
            {
            }
            return true;
        }

        public static DataRowCollection TableRows(DataSet ds, int TableNumber = 0)
        {
            try
            {
                if (HasRow(Table(ds, TableNumber)))
                {
                    return Table(ds, TableNumber).Rows;
                }
            }
            catch
            {
            }
            return null;
        }

        public static DataRow TableRow(DataSet ds, int TableNumber, int RowNumber)
        {
            try
            {
                if (HasRow(Table(ds, TableNumber)) && Table(ds, TableNumber).Rows.Count <= RowNumber + 1)
                {
                    return Table(ds, TableNumber).Rows[RowNumber];
                }
            }
            catch
            {
            }
            return null;
        }


        public static object ObjectColumnValue(DataSet ds, int TableNumber, int RowNumber, string ColumnName)
        {
            try
            {
                if (HasRow(ds, TableNumber))
                {
                    return ds.Tables[TableNumber].Rows[RowNumber][ColumnName];
                }
            }
            catch
            {
            }
            return DBNull.Value;
        }

        public static object ObjectColumnValue(DataSet ds, int RowNumber, string ColumnName)
        {
            return ObjectColumnValue(ds, 0, RowNumber, ColumnName);
        }

        public static bool? BooleanColumnValue(DataSet ds, int TableNumber, int RowNumber, string ColumnName)
        {
            try
            {
                if (HasRow(ds, TableNumber))
                {
                    if (ds.Tables[TableNumber].Rows[RowNumber][ColumnName] == DBNull.Value)
                        return null;
                    else
                        return ConvertHelper.ToBool(ds.Tables[TableNumber].Rows[RowNumber][ColumnName]);
                }
            }
            catch
            {
            }
            return null;
        }

        public static bool? BooleanColumnValue(DataSet ds, int RowNumber, string ColumnName)
        {
            return BooleanColumnValue(ds, 0, RowNumber, ColumnName);
        }

        /// <summary>
        /// DataSet에서 지정된 테이블, 행 및 컬럼에 해당하는 문자열 리턴
        /// </summary>
        /// <param name="ds">데이타셋</param>
        /// <param name="TableNumber">원하는 테이블 번호(최대 갯수가 아님)</param>
        /// <param name="RowNumber">원하는 행</param>
        /// <param name="ColumnName">컬럼명</param>
        /// <returns>값</returns>
        public static string StringColumnValue(DataSet ds, int TableNumber, int RowNumber, string ColumnName)
        {
            try
            {
                if (HasRow(ds, TableNumber))
                {
                    return ConvertHelper.ToString(ds.Tables[TableNumber].Rows[RowNumber][ColumnName]);
                }
            }
            catch
            {
            }
            return string.Empty;
        }

        /// <summary>
        /// DataSet에서 0번째 테이블, 행 및 컬럼에 해당하는 문자열 리턴
        /// </summary>
        /// <param name="ds">데이타셋</param>
        /// <param name="RowNumber">원하는 행</param>
        /// <param name="ColumnName">컬럼명</param>
        /// <returns>값</returns>
        public static string StringColumnValue(DataSet ds, int RowNumber, string ColumnName)
        {
            return StringColumnValue(ds, 0, RowNumber, ColumnName);
        }

        /// <summary>
        /// DataTable에서 지정된 행 및 컬럼에 해당하는 문자열 리턴
        /// </summary>
        /// <param name="dt">테이블</param>
        /// <param name="RowNumber">원하는 행</param>
        /// <param name="ColumnName">컬럼명</param>
        /// <returns>값</returns>
        public static string StringColumnValue(DataTable dt, int RowNumber, string ColumnName)
        {
            try
            {
                if (null != dt && dt.Rows.Count > 0)
                {
                    return ConvertHelper.ToString(dt.Rows[RowNumber][ColumnName]);
                }
            }
            catch
            {
            }
            return string.Empty;
        }

        /// <summary>
        /// DataSet에서 지정된 테이블, 행 및 컬럼에 해당하는 문자열 리턴
        /// </summary>
        /// <param name="ds">데이타셋</param>
        /// <param name="TableNumber">원하는 테이블 번호(최대 갯수가 아님)</param>
        /// <param name="RowNumber">원하는 행</param>
        /// <param name="ColumnIndex">컬럼인덱스</param>
        /// <returns>값</returns>
        public static string StringColumnValue(DataSet ds, int TableNumber, int RowNumber, int ColumnIndex)
        {
            try
            {
                if (HasRow(ds, TableNumber))
                {
                    return ConvertHelper.ToString(ds.Tables[TableNumber].Rows[RowNumber][ColumnIndex]);
                }
            }
            catch
            {
            }
            return string.Empty;
        }

        /// <summary>
        /// DataSet에서 0번째 테이블, 행 및 컬럼에 해당하는 문자열 리턴
        /// </summary>
        /// <param name="ds">데이타셋</param>
        /// <param name="RowNumber">원하는 행</param>
        /// <param name="ColumnIndex">컬럼인덱스</param>
        /// <returns>값</returns>
        public static string StringColumnValue(DataSet ds, int RowNumber, int ColumnIndex)
        {
            return StringColumnValue(ds, 0, RowNumber, ColumnIndex);
        }

        /// <summary>
        /// DataTable에서 지정된 행 및 컬럼에 해당하는 문자열 리턴
        /// </summary>
        /// <param name="dt">테이블</param>
        /// <param name="RowNumber">원하는 행</param>
        /// <param name="ColumnIndex">컬럼인덱스</param>
        /// <returns>값</returns>
        public static string StringColumnValue(DataTable dt, int RowNumber, int ColumnIndex)
        {
            try
            {
                if (null != dt && dt.Rows.Count > 0)
                {
                    return ConvertHelper.ToString(dt.Rows[RowNumber][ColumnIndex]);
                }
            }
            catch
            {
            }
            return string.Empty;
        }

        /// <summary>
        /// DataSet에서 지정된 테이블, 행 및 컬럼에 해당하는 날짜 리턴
        /// </summary>
        /// <param name="ds">데이타셋</param>
        /// <param name="TableNumber">원하는 테이블 번호(최대 갯수가 아님)</param>
        /// <param name="RowNumber">원하는 행</param>
        /// <param name="ColumnName">컬럼명</param>
        /// <returns>값</returns>
        public static DateTime DateTimeColumnValue(DataSet ds, int TableNumber, int RowNumber, string ColumnName)
        {
            try
            {
                if (HasRow(ds, TableNumber))
                {
                    return (DateTime)ds.Tables[TableNumber].Rows[RowNumber][ColumnName];
                }
            }
            catch
            {
            }
            return DateTime.MinValue;
        }

        /// <summary>
        /// DataSet에서 0번째 테이블, 행 및 컬럼에 해당하는 날짜 리턴
        /// </summary>
        /// <param name="ds">데이타셋</param>
        /// <param name="RowNumber">원하는 행</param>
        /// <param name="ColumnName">컬럼명</param>
        /// <returns>값</returns>
        public static DateTime DateTimeColumnValue(DataSet ds, int RowNumber, string ColumnName)
        {
            return DateTimeColumnValue(ds, 0, RowNumber, ColumnName);
        }

        /// <summary>
        /// DataTable에서 지정된 행 및 컬럼에 해당하는 날짜 리턴
        /// </summary>
        /// <param name="dt">테이블</param>
        /// <param name="RowNumber">원하는 행</param>
        /// <param name="ColumnName">컬럼명</param>
        /// <returns>값</returns>
        public static DateTime DateTimeColumnValue(DataTable dt, int RowNumber, string ColumnName)
        {
            try
            {
                if (null != dt && dt.Rows.Count > 0)
                {
                    return (DateTime)dt.Rows[RowNumber][ColumnName];
                }
            }
            catch
            {
            }
            return DateTime.MinValue;
        }

        /// <summary>
        /// DataSet에서 지정된 테이블, 행 및 컬럼에 해당하는 날짜 리턴
        /// </summary>
        /// <param name="ds">데이타셋</param>
        /// <param name="TableNumber">원하는 테이블 번호(최대 갯수가 아님)</param>
        /// <param name="RowNumber">원하는 행</param>
        /// <param name="ColumnIndex">컬럼인덱스</param>
        /// <returns>값</returns>
        public static DateTime DateTimeColumnValue(DataSet ds, int TableNumber, int RowNumber, int ColumnIndex)
        {
            try
            {
                if (HasRow(ds, TableNumber))
                {
                    return (DateTime)ds.Tables[TableNumber].Rows[RowNumber][ColumnIndex];
                }
            }
            catch
            {
            }
            return DateTime.MinValue;
        }

        /// <summary>
        /// DataSet에서 0번째 테이블, 행 및 컬럼에 해당하는 날짜 리턴
        /// </summary>
        /// <param name="ds">데이타셋</param>
        /// <param name="RowNumber">원하는 행</param>
        /// <param name="ColumnIndex">컬럼인덱스</param>
        /// <returns>값</returns>
        public static DateTime DateTimeColumnValue(DataSet ds, int RowNumber, int ColumnIndex)
        {
            return DateTimeColumnValue(ds, 0, RowNumber, ColumnIndex);
        }

        /// <summary>
        /// DataTable에서 지정된 행 및 컬럼에 해당하는 날짜 리턴
        /// </summary>
        /// <param name="dt">테이블</param>
        /// <param name="RowNumber">원하는 행</param>
        /// <param name="ColumnIndex">컬럼인덱스</param>
        /// <returns>값</returns>
        public static DateTime DateTimeColumnValue(DataTable dt, int RowNumber, int ColumnIndex)
        {
            try
            {
                if (null != dt && dt.Rows.Count > 0)
                {
                    return (DateTime)dt.Rows[RowNumber][ColumnIndex];
                }
            }
            catch
            {
            }
            return DateTime.MinValue;
        }

        /// <summary>
        /// DataSet에서 지정된 테이블, 행 및 컬럼에 해당하는 정수 리턴
        /// </summary>
        /// <param name="ds">데이타셋</param>
        /// <param name="TableNumber">원하는 테이블 번호(최대 갯수가 아님)</param>
        /// <param name="RowNumber">원하는 행</param>
        /// <param name="ColumnName">컬럼명</param>
        /// <returns>값</returns>
        public static int IntColumnValue(DataSet ds, int TableNumber, int RowNumber, string ColumnName)
        {
            try
            {
                if (HasRow(ds, TableNumber))
                {
                    return ConvertHelper.ToInt(ds.Tables[TableNumber].Rows[RowNumber][ColumnName]);
                }
            }
            catch
            {
            }
            return 0;
        }

        /// <summary>
        /// DataSet에서 0번째 테이블, 행 및 컬럼에 해당하는 정수 리턴
        /// </summary>
        /// <param name="ds">데이타셋</param>
        /// <param name="RowNumber">원하는 행</param>
        /// <param name="ColumnName">컬럼명</param>
        /// <returns>값</returns>
        public static int IntColumnValue(DataSet ds, int RowNumber, string ColumnName)
        {
            return IntColumnValue(ds, 0, RowNumber, ColumnName);
        }

        /// <summary>
        /// DataTable에서 지정된 행 및 컬럼에 해당하는 정수 리턴
        /// </summary>
        /// <param name="dt">테이블</param>
        /// <param name="RowNumber">원하는 행</param>
        /// <param name="ColumnName">컬럼명</param>
        /// <returns>값</returns>
        public static int IntColumnValue(DataTable dt, int RowNumber, string ColumnName)
        {
            try
            {
                if (null != dt && dt.Rows.Count > 0)
                {
                    return ConvertHelper.ToInt(dt.Rows[RowNumber][ColumnName]);
                }
            }
            catch
            {
            }
            return 0;
        }

        /// <summary>
        /// DataSet에서 지정된 테이블, 행 및 컬럼에 해당하는 정수 리턴
        /// </summary>
        /// <param name="ds">데이타셋</param>
        /// <param name="TableNumber">원하는 테이블 번호(최대 갯수가 아님)</param>
        /// <param name="RowNumber">원하는 행</param>
        /// <param name="ColumnIndex">컬럼인덱스</param>
        /// <returns>값</returns>
        public static int IntColumnValue(DataSet ds, int TableNumber, int RowNumber, int ColumnIndex)
        {
            try
            {
                if (HasRow(ds, TableNumber))
                {
                    return ConvertHelper.ToInt(ds.Tables[TableNumber].Rows[RowNumber][ColumnIndex]);
                }
            }
            catch
            {
            }
            return 0;
        }

        /// <summary>
        /// DataSet에서 0번째 테이블, 행 및 컬럼에 해당하는 정수 리턴
        /// </summary>
        /// <param name="ds">데이타셋</param>
        /// <param name="RowNumber">원하는 행</param>
        /// <param name="ColumnIndex">컬럼인덱스</param>
        /// <returns>값</returns>
        public static int IntColumnValue(DataSet ds, int RowNumber, int ColumnIndex)
        {
            return IntColumnValue(ds, 0, RowNumber, ColumnIndex);
        }

        /// <summary>
        /// DataTable에서 지정된 행 및 컬럼에 해당하는 정수 리턴
        /// </summary>
        /// <param name="dt">테이블</param>
        /// <param name="RowNumber">원하는 행</param>
        /// <param name="ColumnIndex">컬럼인덱스</param>
        /// <returns>값</returns>
        public static int IntColumnValue(DataTable dt, int RowNumber, int ColumnIndex)
        {
            try
            {
                if (null != dt && dt.Rows.Count > 0)
                {
                    return ConvertHelper.ToInt(dt.Rows[RowNumber][ColumnIndex]);
                }
            }
            catch
            {
            }
            return 0;
        }

        /// <summary>
        /// DataSet에서 지정된 테이블, 행 및 컬럼에 해당하는 double 리턴
        /// </summary>
        /// <param name="ds">데이타셋</param>
        /// <param name="TableNumber">원하는 테이블 번호(최대 갯수가 아님)</param>
        /// <param name="RowNumber">원하는 행</param>
        /// <param name="ColumnName">컬럼명</param>
        /// <returns>값</returns>
        public static double DoubleColumnValue(DataSet ds, int TableNumber, int RowNumber, string ColumnName)
        {
            try
            {
                if (HasRow(ds, TableNumber))
                {
                    return ConvertHelper.ToDouble(ds.Tables[TableNumber].Rows[RowNumber][ColumnName]);
                }
            }
            catch
            {
            }
            return 0;
        }

        /// <summary>
        /// DataSet에서 0번째 테이블, 행 및 컬럼에 해당하는 double 리턴
        /// </summary>
        /// <param name="ds">데이타셋</param>
        /// <param name="RowNumber">원하는 행</param>
        /// <param name="ColumnName">컬럼명</param>
        /// <returns>값</returns>
        public static double DoubleColumnValue(DataSet ds, int RowNumber, string ColumnName)
        {
            return DoubleColumnValue(ds, 0, RowNumber, ColumnName);
        }

        /// <summary>
        /// DataTable에서 지정된 행 및 컬럼에 해당하는 double 리턴
        /// </summary>
        /// <param name="dt">테이블</param>
        /// <param name="RowNumber">원하는 행</param>
        /// <param name="ColumnName">컬럼명</param>
        /// <returns>값</returns>
        public static double DoubleColumnValue(DataTable dt, int RowNumber, string ColumnName)
        {
            try
            {
                if (null != dt && dt.Rows.Count > 0)
                {
                    return ConvertHelper.ToDouble(dt.Rows[RowNumber][ColumnName]);
                }
            }
            catch
            {
            }
            return 0;
        }

        /// <summary>
        /// DataSet에서 지정된 테이블, 행 및 컬럼에 해당하는 double 리턴
        /// </summary>
        /// <param name="ds">데이타셋</param>
        /// <param name="TableNumber">원하는 테이블 번호(최대 갯수가 아님)</param>
        /// <param name="RowNumber">원하는 행</param>
        /// <param name="ColumnIndex">컬럼인덱스</param>
        /// <returns>값</returns>
        public static double DoubleColumnValue(DataSet ds, int TableNumber, int RowNumber, int ColumnIndex)
        {
            try
            {
                if (HasRow(ds, TableNumber))
                {
                    return ConvertHelper.ToDouble(ds.Tables[TableNumber].Rows[RowNumber][ColumnIndex]);
                }
            }
            catch
            {
            }
            return 0.0;
        }

        /// <summary>
        /// DataSet에서 0번째 테이블, 행 및 컬럼에 해당하는 double 리턴
        /// </summary>
        /// <param name="ds">데이타셋</param>
        /// <param name="RowNumber">원하는 행</param>
        /// <param name="ColumnIndex">컬럼인덱스</param>
        /// <returns>값</returns>
        public static double DoubleColumnValue(DataSet ds, int RowNumber, int ColumnIndex)
        {
            return DoubleColumnValue(ds, 0, RowNumber, ColumnIndex);
        }

        /// <summary>
        /// DataTable에서 지정된 행 및 컬럼에 해당하는 double 리턴
        /// </summary>
        /// <param name="dt">테이블</param>
        /// <param name="RowNumber">원하는 행</param>
        /// <param name="ColumnIndex">컬럼인덱스</param>
        /// <returns>값</returns>
        public static double DoubleColumnValue(DataTable dt, int RowNumber, int ColumnIndex)
        {
            try
            {
                if (null != dt && dt.Rows.Count > 0)
                {
                    return ConvertHelper.ToDouble(dt.Rows[RowNumber][ColumnIndex]);
                }
            }
            catch
            {
            }
            return 0;
        }

        /// <summary>
        /// DataSet에서 지정된 테이블, 행 및 컬럼에 해당하는 byte 리턴
        /// </summary>
        /// <param name="ds">데이타셋</param>
        /// <param name="TableNumber">원하는 테이블 번호(최대 갯수가 아님)</param>
        /// <param name="RowNumber">원하는 행</param>
        /// <param name="ColumnName">컬럼명</param>
        /// <returns>값</returns>
        public static byte ByteColumnValue(DataSet ds, int TableNumber, int RowNumber, string ColumnName)
        {
            try
            {
                if (HasRow(ds, TableNumber))
                {
                    return ConvertHelper.ToByte(ds.Tables[TableNumber].Rows[RowNumber][ColumnName]);
                }
            }
            catch
            {
            }
            return 0;
        }

        /// <summary>
        /// DataSet에서 0번째 테이블, 행 및 컬럼에 해당하는 byte 리턴
        /// </summary>
        /// <param name="ds">데이타셋</param>
        /// <param name="RowNumber">원하는 행</param>
        /// <param name="ColumnName">컬럼명</param>
        /// <returns>값</returns>
        public static byte ByteColumnValue(DataSet ds, int RowNumber, string ColumnName)
        {
            return ByteColumnValue(ds, 0, RowNumber, ColumnName);
        }

        /// <summary>
        /// DataTable에서 지정된 행 및 컬럼에 해당하는 byte 리턴
        /// </summary>
        /// <param name="dt">테이블</param>
        /// <param name="RowNumber">원하는 행</param>
        /// <param name="ColumnName">컬럼명</param>
        /// <returns>값</returns>
        public static byte ByteColumnValue(DataTable dt, int RowNumber, string ColumnName)
        {
            try
            {
                if (null != dt && dt.Rows.Count > 0)
                {
                    return ConvertHelper.ToByte(dt.Rows[RowNumber][ColumnName]);
                }
            }
            catch
            {
            }
            return 0;
        }

        /// <summary>
        /// DataSet에서 지정된 테이블, 행 및 컬럼에 해당하는 byte 리턴
        /// </summary>
        /// <param name="ds">데이타셋</param>
        /// <param name="TableNumber">원하는 테이블 번호(최대 갯수가 아님)</param>
        /// <param name="RowNumber">원하는 행</param>
        /// <param name="ColumnIndex">컬럼인덱스</param>
        /// <returns>값</returns>
        public static byte ByteColumnValue(DataSet ds, int TableNumber, int RowNumber, int ColumnIndex)
        {
            try
            {
                if (HasRow(ds, TableNumber))
                {
                    return ConvertHelper.ToByte(ds.Tables[TableNumber].Rows[RowNumber][ColumnIndex]);
                }
            }
            catch
            {
            }
            return 0;
        }

        /// <summary>
        /// DataSet에서 0번째 테이블, 행 및 컬럼에 해당하는 byte 리턴
        /// </summary>
        /// <param name="ds">데이타셋</param>
        /// <param name="RowNumber">원하는 행</param>
        /// <param name="ColumnIndex">컬럼인덱스</param>
        /// <returns>값</returns>
        public static byte ByteColumnValue(DataSet ds, int RowNumber, int ColumnIndex)
        {
            return ByteColumnValue(ds, 0, RowNumber, ColumnIndex);
        }

        /// <summary>
        /// DataTable에서 지정된 행 및 컬럼에 해당하는 byte 리턴
        /// </summary>
        /// <param name="dt">테이블</param>
        /// <param name="RowNumber">원하는 행</param>
        /// <param name="ColumnIndex">컬럼인덱스</param>
        /// <returns>값</returns>
        public static byte ByteColumnValue(DataTable dt, int RowNumber, int ColumnIndex)
        {
            try
            {
                if (null != dt && dt.Rows.Count > 0)
                {
                    return ConvertHelper.ToByte(dt.Rows[RowNumber][ColumnIndex]);
                }
            }
            catch
            {
            }
            return 0;
        }

        /// <summary>
        /// DataSet에서 지정된 테이블, 행 및 컬럼에 해당하는 Decimal 리턴
        /// </summary>
        /// <param name="ds">데이타셋</param>
        /// <param name="TableNumber">원하는 테이블 번호(최대 갯수가 아님)</param>
        /// <param name="RowNumber">원하는 행</param>
        /// <param name="ColumnName">컬럼명</param>
        /// <returns>값</returns>
        public static decimal DecimalColumnValue(DataSet ds, int TableNumber, int RowNumber, string ColumnName)
        {
            try
            {
                if (HasRow(ds, TableNumber))
                {
                    return ConvertHelper.ToDecimal(ds.Tables[TableNumber].Rows[RowNumber][ColumnName], 0.0m);
                }
            }
            catch
            {
            }
            return 0.0m;
        }

        /// <summary>
        /// DataSet에서 0번째 테이블, 행 및 컬럼에 해당하는 Decimal 리턴
        /// </summary>
        /// <param name="ds">데이타셋</param>
        /// <param name="RowNumber">원하는 행</param>
        /// <param name="ColumnName">컬럼명</param>
        /// <returns>값</returns>
        public static decimal DecimalColumnValue(DataSet ds, int RowNumber, string ColumnName)
        {
            return DecimalColumnValue(ds, 0, RowNumber, ColumnName);
        }

        /// <summary>
        /// DataTable에서 지정된 행 및 컬럼에 해당하는 Decimal 리턴
        /// </summary>
        /// <param name="dt">테이블</param>
        /// <param name="RowNumber">원하는 행</param>
        /// <param name="ColumnName">컬럼명</param>
        /// <returns>값</returns>
        public static decimal DecimalColumnValue(DataTable dt, int RowNumber, string ColumnName)
        {
            try
            {
                if (null != dt && dt.Rows.Count > 0)
                {
                    return ConvertHelper.ToDecimal(dt.Rows[RowNumber][ColumnName], 0.0m);
                }
            }
            catch
            {
            }
            return 0.0m;
        }

        /// <summary>
        /// DataSet에서 지정된 테이블, 행 및 컬럼에 해당하는 Decimal 리턴
        /// </summary>
        /// <param name="ds">데이타셋</param>
        /// <param name="TableNumber">원하는 테이블 번호(최대 갯수가 아님)</param>
        /// <param name="RowNumber">원하는 행</param>
        /// <param name="ColumnIndex">컬럼인덱스</param>
        /// <returns>값</returns>
        public static decimal DecimalColumnValue(DataSet ds, int TableNumber, int RowNumber, int ColumnIndex)
        {
            try
            {
                if (HasRow(ds, TableNumber))
                {
                    return ConvertHelper.ToDecimal(ds.Tables[TableNumber].Rows[RowNumber][ColumnIndex], 0.0m);
                }
            }
            catch
            {
            }
            return 0.0m;
        }

        /// <summary>
        /// DataTable에서 지정된 행 및 컬럼에 해당하는 Decimal 리턴
        /// </summary>
        /// <param name="dt">테이블</param>
        /// <param name="RowNumber">원하는 행</param>
        /// <param name="ColumnIndex">컬럼인덱스</param>
        /// <returns>값</returns>
        public static decimal DecimalColumnValue(DataTable dt, int RowNumber, int ColumnIndex)
        {
            try
            {
                if (null != dt && dt.Rows.Count > 0)
                {
                    return ConvertHelper.ToDecimal(dt.Rows[RowNumber][ColumnIndex], 0.0m);
                }
            }
            catch
            {
            }
            return 0.0m;
        }

        /// <summary>
        /// DataSet에서 지정된 테이블, 행 및 컬럼에 해당하는 Int64 리턴
        /// </summary>
        /// <param name="ds">데이타셋</param>
        /// <param name="TableNumber">원하는 테이블 번호(최대 갯수가 아님)</param>
        /// <param name="RowNumber">원하는 행</param>
        /// <param name="ColumnName">컬럼명</param>
        /// <returns>값</returns>
        public static Int64 Int64ColumnValue(DataSet ds, int TableNumber, int RowNumber, string ColumnName)
        {
            try
            {
                if (HasRow(ds, TableNumber))
                {
                    return ConvertHelper.ToInt64(ds.Tables[TableNumber].Rows[RowNumber][ColumnName], 0);
                }
            }
            catch
            {
            }
            return 0;
        }

        /// <summary>
        /// DataSet에서 0번째 테이블, 행 및 컬럼에 해당하는 Int64 리턴
        /// </summary>
        /// <param name="ds">데이타셋</param>
        /// <param name="RowNumber">원하는 행</param>
        /// <param name="ColumnName">컬럼명</param>
        /// <returns>값</returns>
        public static Int64 Int64ColumnValue(DataSet ds, int RowNumber, string ColumnName)
        {
            return Int64ColumnValue(ds, 0, RowNumber, ColumnName);
        }

        /// <summary>
        /// DataTable에서 지정된 행 및 컬럼에 해당하는 Int64 리턴
        /// </summary>
        /// <param name="dt">테이블</param>
        /// <param name="RowNumber">원하는 행</param>
        /// <param name="ColumnName">컬럼명</param>
        /// <returns>값</returns>
        public static Int64 Int64ColumnValue(DataTable dt, int RowNumber, string ColumnName)
        {
            try
            {
                if (null != dt && dt.Rows.Count > 0)
                {
                    return ConvertHelper.ToInt64(dt.Rows[RowNumber][ColumnName], 0);
                }
            }
            catch
            {
            }
            return 0;
        }

        /// <summary>
        /// DataSet에서 지정된 테이블, 행 및 컬럼에 해당하는 Int64 리턴
        /// </summary>
        /// <param name="ds">데이타셋</param>
        /// <param name="TableNumber">원하는 테이블 번호(최대 갯수가 아님)</param>
        /// <param name="RowNumber">원하는 행</param>
        /// <param name="ColumnIndex">컬럼인덱스</param>
        /// <returns>값</returns>
        public static Int64 Int64ColumnValue(DataSet ds, int TableNumber, int RowNumber, int ColumnIndex)
        {
            try
            {
                if (HasRow(ds, TableNumber))
                {
                    return ConvertHelper.ToInt64(ds.Tables[TableNumber].Rows[RowNumber][ColumnIndex], 0);
                }
            }
            catch
            {
            }
            return 0;
        }

        /// <summary>
        /// DataSet에서 0번째 테이블, 행 및 컬럼에 해당하는 Int64 리턴
        /// </summary>
        /// <param name="ds">데이타셋</param>
        /// <param name="RowNumber">원하는 행</param>
        /// <param name="ColumnIndex">컬럼인덱스</param>
        /// <returns>값</returns>
        public static Int64 Int64ColumnValue(DataSet ds, int RowNumber, int ColumnIndex)
        {
            return Int64ColumnValue(ds, 0, RowNumber, ColumnIndex);
        }

        /// <summary>
        /// DataTable에서 지정된 행 및 컬럼에 해당하는 Int64 리턴
        /// </summary>
        /// <param name="dt">테이블</param>
        /// <param name="RowNumber">원하는 행</param>
        /// <param name="ColumnIndex">컬럼인덱스</param>
        /// <returns>값</returns>
        public static Int64 Int64ColumnValue(DataTable dt, int RowNumber, int ColumnIndex)
        {
            try
            {
                if (null != dt && dt.Rows.Count > 0)
                {
                    return ConvertHelper.ToInt64(dt.Rows[RowNumber][ColumnIndex], 0);
                }
            }
            catch
            {
            }
            return 0;
        }

        /// <summary>
        /// DataSet을 Json 스트링으로 생성
        /// </summary>
        /// <param name="ds">DataSet</param>
        /// <returns>Json 스트링</returns>
        public static string JsonStringDataSet(DataSet ds)
        {
            try
            {
                if (TableCount(ds) == 0 || !HasRow(ds))
                {
                    return "[]";
                }
                if (TableCount(ds) == 1)
                {
                    return JsonStringDataTable(Table(ds, 0));
                }
                JsonArrayCollection jsonFullColl = new JsonArrayCollection(); ;
                JsonTextParser jtp;
                JsonArrayCollection jsonColl;
                JsonObjectCollection jjc;

                for (int i = 0; i < TableCount(ds); i++)
                {
                    if (ds.Tables.Count < i)
                        break;

                    if (ds.Tables[i].Rows.Count == 0)
                    {
                        jsonColl = new JsonArrayCollection(ds.Tables[i].TableName);
                        jjc = new JsonObjectCollection();
                        jjc.Add(jsonColl);
                        jsonFullColl.Add(jjc);
                    }
                    else
                    {
                        jtp = new JsonTextParser();
                        jsonColl = new JsonArrayCollection(ds.Tables[i].TableName, (JsonArrayCollection)jtp.Parse(JsonStringDataTable(ds.Tables[i])));
                        jjc = new JsonObjectCollection();
                        jjc.Add(jsonColl);
                        jsonFullColl.Add(jjc);
                    }
                }
                return jsonFullColl.ToString();
            }
            catch
            {
                return "[]";
            }
        }

        /// <summary>
        /// DataTable을 Json 스트링으로 생성
        /// </summary>
        /// <param name="dt">DataTable</param>
        /// <returns>Json 스트링</returns>
        public static string JsonStringDataTable(DataTable dt)
        {
            return JsonDataTable(dt).ToString();
        }

        public static JsonArrayCollection JsonDataTable(DataTable dt)
        {
            try
            {
                if (dt == null || dt.Rows == null || dt.Rows.Count < 1)
                {
                    return new JsonArrayCollection();
                }

                JsonArrayCollection JsonArray = new JsonArrayCollection();
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    JsonObjectCollection jsonColl = new JsonObjectCollection();
                    for (int j = 0; j < dt.Columns.Count; j++)
                    {
                        switch (dt.Columns[j].DataType.FullName)
                        {
                            case "System.Double": jsonColl.Add(new JsonNumericValue(dt.Columns[j].ColumnName, DoubleColumnValue(dt, i, j))); break;
                            case "System.Int64": jsonColl.Add(new JsonNumericValue(dt.Columns[j].ColumnName, Int64ColumnValue(dt, i, j))); break;
                            case "System.Int32": jsonColl.Add(new JsonNumericValue(dt.Columns[j].ColumnName, IntColumnValue(dt, i, j))); break;
                            case "System.Byte": jsonColl.Add(new JsonNumericValue(dt.Columns[j].ColumnName, IntColumnValue(dt, i, j))); break;
                            case "System.DateTime": jsonColl.Add(new JsonStringValue(dt.Columns[j].ColumnName, StringColumnValue(dt, i, j))); break;
                            case "System.String": jsonColl.Add(new JsonStringValue(dt.Columns[j].ColumnName, StringColumnValue(dt, i, j))); break;
                            case "System.Boolean": jsonColl.Add(new JsonBooleanValue(dt.Columns[j].ColumnName, StringColumnValue(dt, i, j))); break;
                            default: jsonColl.Add(new JsonStringValue(dt.Columns[j].ColumnName, StringColumnValue(dt, i, j))); break;
                        }
                    }
                    JsonArray.Add(jsonColl);
                }
                return JsonArray;
            }
            catch
            {
                return new JsonArrayCollection();
            }
        }

        /// <summary>
        /// DataRow를 Json 스트링으로 생성
        /// </summary>
        /// <param name="dr">DataRow</param>
        /// <returns>Json 스트링</returns>
        public static string JsonStringDataRow(DataRow dr, string dateformat = "yyyy-MM-dd HH:mm:ss")
        {
            try
            {
                DataTable dt = dr.Table;
                JsonObjectCollection jsonColl = new JsonObjectCollection();
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    switch (dt.Columns[j].DataType.FullName)
                    {
                        case "System.Double": jsonColl.Add(new JsonNumericValue(dt.Columns[j].ColumnName, ConvertHelper.ToDouble(dr[j]))); break;
                        case "System.Int64": jsonColl.Add(new JsonNumericValue(dt.Columns[j].ColumnName, ConvertHelper.ToInt64(dr[j]))); break;
                        case "System.Int32":
                        case "System.Byte":
                            jsonColl.Add(new JsonNumericValue(dt.Columns[j].ColumnName, ConvertHelper.ToInt(dr[j]))); break;
                        case "System.DateTime": jsonColl.Add(new JsonStringValue(dt.Columns[j].ColumnName, ConvertHelper.ToDateTime(dr[j]).ToString(dateformat))); break;
                        case "System.Boolean": jsonColl.Add(new JsonBooleanValue(dt.Columns[j].ColumnName, ConvertHelper.ToString(dr[j]))); break;
                        case "System.String": 
                        default:
                            jsonColl.Add(new JsonStringValue(dt.Columns[j].ColumnName, ConvertHelper.ToString(dr[j]))); break;
                    }
                }
                return jsonColl.ToString();
            }
            catch
            {
                return "";
            }
        }

        public static List<T> ToClass<T>(DataTable dt) where T : new()
        {
            var dataList = new List<T>();

            if (HasRow(dt))
            {
                foreach (DataRow dataRow in dt.AsEnumerable().ToList())
                {
                    dataList.Add(ToClass<T>(dataRow));
                }
            }

            return dataList;
        }

        public static T ToClass<T>(DataRow dr) where T : new()
        {
            var classObj = new T();

            if (dr == null)
                return classObj;

            var objFieldNames = typeof(T).GetFields().Cast<FieldInfo>().
                Select(item => new
                {
                    Name = item.Name,
                    Type = Nullable.GetUnderlyingType(item.FieldType) ?? item.FieldType
                }).ToList();

            var dtlFieldNames = dr.Table.Columns.Cast<DataColumn>().Select(item => new {
                Name = item.ColumnName,
                Type = item.DataType
            }).ToList();

            foreach (var dtField in dtlFieldNames)
            {
                FieldInfo fieldInfos = classObj.GetType().GetField(dtField.Name);

                var field = objFieldNames.Find(x => x.Name == dtField.Name);

                if (field != null)
                {
                    if (fieldInfos.FieldType == typeof(Nullable<DateTime>))
                    {
                        if(dr[dtField.Name] == DBNull.Value)
                            fieldInfos.SetValue(classObj, null);
                        else
                            fieldInfos.SetValue(classObj, Convert.ToDateTime(dr[dtField.Name]));
                    }
                    else if (fieldInfos.FieldType == typeof(Nullable<bool>))
                    {
                        if (dr[dtField.Name] == DBNull.Value)
                            fieldInfos.SetValue(classObj, null);
                        else
                            fieldInfos.SetValue(classObj, Convert.ToBoolean(dr[dtField.Name]));
                    }
                    else if (fieldInfos.FieldType == typeof(Nullable<int>))
                    {
                        if (dr[dtField.Name] == DBNull.Value)
                            fieldInfos.SetValue(classObj, null);
                        else
                            fieldInfos.SetValue(classObj, Convert.ToInt32(dr[dtField.Name]));
                    }
                    else if (fieldInfos.FieldType == typeof(DateTime))
                    {
                        fieldInfos.SetValue(classObj, Convert.ToDateTime(dr[dtField.Name]));
                    }
                    else if (fieldInfos.FieldType == typeof(int))
                    {
                        fieldInfos.SetValue(classObj, Convert.ToInt32(dr[dtField.Name]));
                    }
                    else if (fieldInfos.FieldType == typeof(float))
                    {
                        fieldInfos.SetValue(classObj, Convert.ToSingle(dr[dtField.Name]));
                    }
                    else if (fieldInfos.FieldType == typeof(bool))
                    {
                        fieldInfos.SetValue(classObj, Convert.ToBoolean(dr[dtField.Name]));
                    }
                    else if (fieldInfos.FieldType == typeof(long))
                    {
                        fieldInfos.SetValue(classObj, Convert.ToInt64(dr[dtField.Name]));
                    }
                    else if (fieldInfos.FieldType == typeof(decimal))
                    {
                        fieldInfos.SetValue(classObj, Convert.ToDecimal(dr[dtField.Name]));
                    }
                    else if (fieldInfos.FieldType == typeof(bool))
                    {
                        fieldInfos.SetValue(classObj, Convert.ToBoolean(dr[dtField.Name]));
                    }
                    else if (fieldInfos.FieldType == typeof(String))
                    {
                        if (dr[dtField.Name].GetType() == typeof(DateTime))
                        {
                            fieldInfos.SetValue(classObj, Convert.ToDateTime(dr[dtField.Name]));
                        }
                        else
                        {
                            fieldInfos.SetValue(classObj, Convert.ToString(dr[dtField.Name]));
                        }
                    }
                    else if (fieldInfos.FieldType == typeof(Guid) || fieldInfos.FieldType == typeof(Nullable<Guid>))
                    {
                        if (!string.IsNullOrEmpty(Convert.ToString(dr[dtField.Name])))
                            fieldInfos.SetValue(classObj, Guid.Parse(Convert.ToString(dr[dtField.Name])));
                    }
                }
            }

            return classObj;
        }

        public static string StringXmlSelectSingleNode(XmlDocument xmldoc, string xpath)
        {
            if (xmldoc == null || string.IsNullOrEmpty(xmldoc.InnerXml) || xmldoc.DocumentElement.SelectSingleNode(xpath) == null)
                return string.Empty;

            return xmldoc.DocumentElement.SelectSingleNode(xpath).Value;
        }

        /// <summary>
        /// DataSet을 XML 형태의 문자열 변환
        /// </summary>
        /// <param name="ds">데이터셋</param>
        /// <param name="RootElementName">루트XML노드명</param>
        /// <returns>XML</returns>
        public static string XmlDataSet(DataSet ds, string RootElementName)
        {
            if (TableCount(ds) > 0)
            {
                StringBuilder xmlsb = new StringBuilder(1024);
                xmlsb.AppendFormat("<{0}>", RootElementName);
                for (int i = 0; i < TableCount(ds); i++)
                {
                    for (int j = 0; j < TableRowCount(ds, i); j++)
                    {
                        xmlsb.AppendFormat("<{0}>", Table(ds, i).TableName);
                        for (int k = 0; k < Table(ds, i).Columns.Count; k++)
                        {
                            string CN = Table(ds, i).Columns[k].ColumnName;
                            xmlsb.AppendFormat("<{0}>", CN);
                            xmlsb.AppendFormat("<![CDATA[{0}]]>", StringColumnValue(ds, i, j, k));
                            xmlsb.AppendFormat("</{0}>", CN);
                        }
                        xmlsb.AppendFormat("</{0}>", Table(ds, i).TableName);
                    }
                }
                xmlsb.AppendFormat("</{0}>", RootElementName);
                ds.Dispose();
                return xmlsb.ToString();
            }
            else
            {
                ds.Dispose();
                return string.Format("<{0}></{0}>", RootElementName);
            }
        }

        /// <summary>
        /// XML을 DataSet으로 변환
        /// </summary>
        /// <param name="XmlString">XML 문자열</param>
        /// <param name="ds">DataTable 스키마 설정이 완료된 DataSet, 반환 시 결과값으로 받을 DataSet</param>
        /// <returns>DataSet (XML 데이터가 담겨져 반환)</returns>
        public static DataSet DataSetXml(string XmlString, DataSet ds)
        {
            try
            {
                StringReader xmlSR = new StringReader(XmlString);
                ds.ReadXml(xmlSR, XmlReadMode.IgnoreSchema);
            }
            catch
            {
            }
            return ds;
        }

        public static bool isEqualTables(DataTable table1, DataTable table2)
        {
            DataTable dt;

            if (table1 == null && table2 == null)
                return true;
            else if ((table1 == null && table2 != null) || table1 != null && table2 == null)
                return false;

            dt = getDifferentRecords(table1, table2);

            if (dt.Rows.Count == 0)
                return true;
            else
                return false;
        }
        
        public static DataTable getDifferentRecords(DataTable FirstDataTable, DataTable SecondDataTable)
        {
            //Create Empty Table     
            DataTable ResultDataTable = new DataTable("ResultDataTable");

            if (FirstDataTable == null && SecondDataTable == null)
                return ResultDataTable;
            else if (FirstDataTable != null && SecondDataTable == null)
                return SecondDataTable;
            else if (FirstDataTable == null && SecondDataTable != null)
                return FirstDataTable;

            using (DataSet ds = new DataSet())
            {
                ds.Tables.AddRange(new DataTable[] { FirstDataTable.Copy(), SecondDataTable.Copy() });
                
                DataColumn[] firstColumns = new DataColumn[ds.Tables[0].Columns.Count];
                for (int i = 0; i < firstColumns.Length; i++)
                {
                    firstColumns[i] = ds.Tables[0].Columns[i];
                }

                DataColumn[] secondColumns = new DataColumn[ds.Tables[1].Columns.Count];
                for (int i = 0; i < secondColumns.Length; i++)
                {
                    secondColumns[i] = ds.Tables[1].Columns[i];
                }
                
                DataRelation r1 = new DataRelation(string.Empty, firstColumns, secondColumns, false);
                ds.Relations.Add(r1);

                DataRelation r2 = new DataRelation(string.Empty, secondColumns, firstColumns, false);
                ds.Relations.Add(r2);
                
                for (int i = 0; i < FirstDataTable.Columns.Count; i++)
                {
                    ResultDataTable.Columns.Add(FirstDataTable.Columns[i].ColumnName, FirstDataTable.Columns[i].DataType);
                }
                
                ResultDataTable.BeginLoadData();
                foreach (DataRow parentrow in ds.Tables[0].Rows)
                {
                    DataRow[] childrows = parentrow.GetChildRows(r1);
                    if (childrows == null || childrows.Length == 0)
                        ResultDataTable.LoadDataRow(parentrow.ItemArray, true);
                }
                
                foreach (DataRow parentrow in ds.Tables[1].Rows)
                {
                    DataRow[] childrows = parentrow.GetChildRows(r2);
                    if (childrows == null || childrows.Length == 0)
                        ResultDataTable.LoadDataRow(parentrow.ItemArray, true);
                }
                ResultDataTable.EndLoadData();
            }

            return ResultDataTable;
        }
    }
}
