using System;
using System.Data;
using System.Data.Common;
using System.Data.Sql;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Reflection;
using System.Threading.Tasks;

namespace GWF
{
    public class SqlHelper
    {
        #region ConnectionString

        private static string _connString;

        /// <summary>
        /// DB연결 문자열
        /// </summary>
        public static string ConnectionString
        {
            get
            {
                if (String.IsNullOrEmpty(_connString))
                {
                    _connString = GetConnectionString();
                }

                return _connString;
            }
        }

        #endregion

        #region GetConnectionString()

        /// <summary>
        /// web.config에서 기본DB연결 문자열을 반환한다.
        /// </summary>
        /// <returns></returns>
        public static string GetConnectionString()
        {
            string connStrName = ConfigurationManager.AppSettings["ConnectionStringName"];

            if (!String.IsNullOrEmpty(connStrName))
            {
                string connString = ConfigurationManager.ConnectionStrings[connStrName].ConnectionString;

                if (!String.IsNullOrEmpty(connString))
                {
                    return connString;
                }
            }

            throw new ApplicationException("ConnectionString's not found.");
        }

        public static string GetConnectionString(string connStrName)
        {
            string connString = ConfigurationManager.ConnectionStrings[connStrName].ConnectionString;

            if (!String.IsNullOrEmpty(connString))
            {
                return connString;
            }

            throw new ApplicationException("ConnectionString's not found.");
        }

        #endregion

        #region GetConnection()

        public static SqlConnection GetConnection()
        {
            return new SqlConnection(ConnectionString);
        }

        public static SqlConnection GetConnectionByKey(string connStrName)
        {
            return new SqlConnection(GetConnectionString(connStrName));
        }

        #endregion

        #region PrepareCommand()

        public static SqlCommand PrepareCommand(SqlConnection conn, SqlTransaction trx, string commandText, CommandType type, ParameterCollection pCollection, string[] paramKeys)
        {
            // SqlCommand 생성 및 명령문 설정
            SqlCommand command = conn.CreateCommand();
            command.CommandText = commandText;
            command.CommandType = type;
            command.CommandTimeout = 0;

            // 트랙잭션이 지정된 경우 명령에 트랜잭션 지정
            if (trx != null)
            {
                if (trx.Connection == null) throw new ArgumentException("The transaction was rollbacked or commited, please provide an open transaction.", "transaction");
                command.Transaction = trx;
            }

            // 파라미터를 조합하여 명령에 추가한다.
            AttachParameters(command, type, pCollection, paramKeys);

            return command;
        }

        public async static Task<SqlCommand> PrepareCommandAsync(SqlConnection conn, SqlTransaction trx, string commandText, CommandType type, ParameterCollection pCollection, string[] paramKeys)
        {
            // SqlCommand 생성 및 명령문 설정
            SqlCommand command = conn.CreateCommand();
            command.CommandText = commandText;
            command.CommandType = type;
            command.CommandTimeout = 0;

            if (conn.State != ConnectionState.Open)
            {
                await conn.OpenAsync().ConfigureAwait(false);
            }

            // 트랙잭션이 지정된 경우 명령에 트랜잭션 지정
            if (trx != null)
            {
                if (trx.Connection == null) throw new ArgumentException("The transaction was rollbacked or commited, please provide an open transaction.", "transaction");
                command.Transaction = trx;
            }

            // 파라미터를 조합하여 명령에 추가한다.
            AttachParameters(command, type, pCollection, paramKeys);

            return command;
        }

        #endregion

        #region AttachParameters(), GetParameter()

        public static void AttachParameters(SqlCommand command, CommandType type, ParameterCollection pCollection, string[] paramKeys)
        {
            if (pCollection != null)
            {
                if (type == CommandType.StoredProcedure && paramKeys == null)
                {
                    DataSet ds = GetStoredProcedureReport(command.Connection, command.CommandText);

                    if (ds != null && ds.Tables.Count >= 2 && ds.Tables[1].Rows.Count > 0)
                    {
                        DataTable dt = ds.Tables[1];

                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            string paramName = (string)dt.Rows[i]["Parameter_Name"];

                            SqlParameter sqlParam = GetParameter(command, pCollection, paramName);
                            if (sqlParam != null)
                            {
                                command.Parameters.Add(sqlParam);
                            }
                        }
                    }
                }
                else if (type == CommandType.Text && paramKeys == null)
                {
                    CustomParameter cParam = new CustomParameter();
                    for (int i = 0; i < pCollection.Count; i++)
                    {
                        cParam = pCollection[i] as CustomParameter;
                        SqlParameter sqlParam = GetParameter(command, pCollection, cParam.Name);
                        if (sqlParam != null)
                        {
                            command.Parameters.Add(sqlParam);
                        }
                    }
                }
                else if (paramKeys != null)
                {
                    for (int i = 0; i < paramKeys.Length; i++)
                    {
                        string paramName = paramKeys[i];

                        SqlParameter sqlParam = GetParameter(command, pCollection, paramName);
                        if (sqlParam != null)
                        {
                            command.Parameters.Add(sqlParam);
                        }
                    }
                }
            }
        }

        public static SqlParameter GetParameter(SqlCommand command, ParameterCollection pCollection, string paramName)
        {
            SqlParameter sqlParam = null;

            for (int i = 0; i < pCollection.Count; i++)
            {
                if (pCollection[i].Name.Equals(paramName, StringComparison.OrdinalIgnoreCase))
                {
                    CustomParameter cParam = pCollection[i] as CustomParameter;
                    if (cParam != null)
                    {
                        sqlParam = command.CreateParameter();
                        
                        if (cParam.Direction == ParameterDirection.Output)
                            sqlParam.Size = cParam.Size;

                        sqlParam.Direction = cParam.Direction;
                        sqlParam.ParameterName = cParam.Name;

                        if (cParam.GetValue() == null)
                            sqlParam.Value = DBNull.Value;
                        else
                            sqlParam.Value = cParam.GetValue();
                    }
                    else
                    {
                        throw new ApplicationException("Please use CustomParameter. Other's not allowed yet!");
                    }

                    break;
                }
            }

            return sqlParam;
        }

        #region GetParameter(Not working...)

        //public static SqlParameter GetParameter(SqlCommand command, ParameterCollection pCollection, string paramName)
        //{
        //    SqlParameter sqlParam = null;

        //    if (pCollection[paramName] != null)
        //    {
        //        Parameter p = pCollection[paramName];

        //        object oParameterValue = null;
        //        if (p is ParameterEx)
        //        {
        //            oParameterValue = ((ParameterEx)p).DataValue;
        //        }
        //        else if (p is ControlParameterEx)
        //        {
        //            ControlParameterEx cep = (ControlParameterEx)p;
        //            oParameterValue = cep.Control.GetType().GetProperty(cep.PropertyName).GetValue(cep.Control, null);
        //        }
        //        else if (p is ControlParameter)
        //        {
        //            ControlParameter cp = (ControlParameter)p;

        //            Control ctrl;
        //            AdminPage mfinder = HttpContext.Current.Handler as AdminPage;
        //            BlogPage pfinder = HttpContext.Current.Handler as BlogPage;
        //            if (mfinder != null)
        //            {
        //                ctrl = mfinder.FindControl(cp.ControlID);
        //                oParameterValue = ctrl.GetType().GetProperty(cp.PropertyName).GetValue(ctrl, null);
        //            }
        //            else if (pfinder != null)
        //            {
        //                ctrl = pfinder.Master.FindControl(cp.ControlID);
        //                oParameterValue = ctrl.GetType().GetProperty(cp.PropertyName).GetValue(ctrl, null);
        //            }
        //        }
        //        else if (p is FormParameter)
        //        {
        //            oParameterValue = HttpContext.Current.Request.Form[p.Name];
        //        }
        //        else if (p is CookieParameter)
        //        {
        //            oParameterValue = HttpContext.Current.Request.Cookies[p.Name].Value;
        //        }
        //        else if (p is SessionParameter)
        //        {
        //            oParameterValue = HttpContext.Current.Session[p.Name];
        //        }
        //        else if (p is QueryStringParameter)
        //        {
        //            oParameterValue = HttpContext.Current.Request.QueryString[p.Name];
        //        }

        //        sqlParam = command.CreateParameter();
        //        //sqlParam.DbType = p.Type;
        //        //sqlParam.Size = p.Size;
        //        sqlParam.Direction = p.Direction;
        //        sqlParam.ParameterName = p.Name;
        //        sqlParam.Value = oParameterValue;
        //    }

        //    return sqlParam;
        //}

        #endregion

        #endregion

        #region GetStoredProcedureReport()

        public static DataSet GetStoredProcedureReport(SqlConnection conn, string spName)
        {
            DataSet ds = null;

            if (conn != null && !String.IsNullOrEmpty(spName))
            {
                SqlCommand command = conn.CreateCommand();
                command.CommandText = "sp_help";
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("@objname", spName));

                ds = new DataSet("StoredProcedure");
                SqlDataAdapter adpt = new SqlDataAdapter(command);
                adpt.Fill(ds);
            }

            return ds;
        }

        #endregion

        #region ExecuteDataSet()

        public static DataSet ExecuteDataSet(SqlConnection conn, SqlTransaction trx, string commandText, CommandType type, ParameterCollection pCollection, string[] paramKeys)
        {
            DataSet ds = new DataSet();
            
            SqlCommand command = PrepareCommand(conn, trx, commandText, type, pCollection, paramKeys);
            SqlDataAdapter adpt = new SqlDataAdapter(command);
            adpt.Fill(ds);

            return ds;
        }

        public static DataSet ExecuteDataSet(SqlConnection conn, string commandText, CommandType type, ref ParameterCollection pCollection, string[] paramKeys)
        {
            DataSet ds = new DataSet();

            try
            {
                SqlCommand command = PrepareCommand(conn, null, commandText, type, pCollection, paramKeys);
                SqlDataAdapter adpt = new SqlDataAdapter(command);
                adpt.Fill(ds);

                foreach (SqlParameter sp in command.Parameters)
                {
                    if (sp.Direction == ParameterDirection.InputOutput || sp.Direction == ParameterDirection.Output)
                    {
                        pCollection[sp.ParameterName].DefaultValue = sp.Value.ToString();
                    }
                }
            }
            finally
            {
                conn.Close();
            }

            return ds;
        }

        public static DataSet ExecuteDataSet(SqlConnection conn, string commandText, CommandType type, ParameterCollection pCollection, string[] paramKeys)
        {
            DataSet ds = new DataSet();

            try
            {
                SqlCommand command = PrepareCommand(conn, null, commandText, type, pCollection, paramKeys);
                SqlDataAdapter adpt = new SqlDataAdapter(command);
                adpt.Fill(ds);
            }
            finally
            {
                conn.Close();
            }

            return ds;
        }

        public static DataSet ExecuteDataSet(SqlConnection conn, SqlTransaction trx, string commandText, CommandType type, ParameterCollection pCollection)
        {
            return ExecuteDataSet(conn, trx, commandText, type, pCollection, null);
        }

        public static DataSet ExecuteDataSet(string connString, string commandText, CommandType type, ParameterCollection pCollection)
        {
            using (SqlConnection conn = GetConnectionByKey(connString)) {
                return ExecuteDataSet(conn, commandText, type, pCollection, null);
            }
        }

        public static DataSet ExecuteDataSet(SqlConnection conn, string commandText, CommandType type, ParameterCollection pCollection)
        {
            return ExecuteDataSet(conn, commandText, type, pCollection, null);
        }

        public static DataSet ExecuteDataSet(SqlConnection conn, string commandText, CommandType type)
        {
            return ExecuteDataSet(conn, commandText, type, null, null);
        }

        public static DataSet ExecuteDataSet(SqlConnection conn, string commandText)
        {
            return ExecuteDataSet(conn, commandText, CommandType.StoredProcedure, null, null);
        }

        public static DataSet ExecuteDataSet(string commandText, ParameterCollection pCollection)
        {
            using (SqlConnection conn = GetConnection())
            {
                return ExecuteDataSet(conn, commandText, CommandType.Text, pCollection, null);
            }
        }

        public static DataSet ExecuteDataSet(string commandText, CommandType type, ParameterCollection pCollection)
        {
            using (SqlConnection conn = GetConnection())
            {
                return ExecuteDataSet(conn, commandText, type, pCollection, null);
            }
        }

        public static DataSet ExecuteDataSet(string commandText, CommandType type, ref ParameterCollection pCollection)
        {
            using (SqlConnection conn = GetConnection())
            {
                return ExecuteDataSet(conn, commandText, type, ref pCollection, null);
            }
        }

        public static DataSet ExecuteDataSet(string commandText)
        {
            using (SqlConnection conn = GetConnection())
            {
                return ExecuteDataSet(conn, commandText, CommandType.Text, null, null);
            }
        }
        #endregion

        #region ExecuteDataSetAsync()

        public async static Task<DataSet> ExecuteDataSetAsync(SqlConnection conn, SqlTransaction trx, string commandText, CommandType type, ParameterCollection pCollection, string[] paramKeys)
        {
            DataSet ds = new DataSet();
            SqlCommand command = await PrepareCommandAsync(conn, trx, commandText, type, pCollection, paramKeys);
            SqlDataAdapter adpt = new SqlDataAdapter(command);
            adpt.Fill(ds);

            return ds;
        }

        public async static Task<DataSet> ExecuteDataSetAsync(SqlConnection conn, SqlTransaction trx, string commandText, ParameterCollection pCollection)
        {
            return await ExecuteDataSetAsync(conn, trx, commandText, CommandType.Text, pCollection, null);
        }

        public async static Task<DataSet> ExecuteDataSetAsync(SqlConnection conn, string commandText, CommandType type, ParameterCollection pCollection, string[] paramKeys)
        {
            return await ExecuteDataSetAsync(conn, null, commandText, type, pCollection, paramKeys);
        }

        public async static Task<DataSet> ExecuteDataSetAsync(SqlConnection conn, string commandText, CommandType type, ParameterCollection pCollection)
        {
            return await ExecuteDataSetAsync(conn, null, commandText, type, pCollection, null);
        }

        public async static Task<DataSet> ExecuteDataSetAsync(SqlConnection conn, string commandText, CommandType type)
        {
            return await ExecuteDataSetAsync(conn, null, commandText, type, null, null);
        }

        public async static Task<DataSet> ExecuteDataSetAsync(string commandText, CommandType type, ParameterCollection pCollection)
        {
            using (SqlConnection conn = GetConnection())
            {
                return await ExecuteDataSetAsync(conn, null, commandText, type, pCollection, null);
            }
        }

        public async static Task<DataSet> ExecuteDataSetAsync(string commandText, ParameterCollection pCollection)
        {
            using (SqlConnection conn = GetConnection())
            {
                return await ExecuteDataSetAsync(conn, null, commandText, CommandType.Text, pCollection, null);
            }
        }
        public async static Task<DataSet> ExecuteDataSetAsync(string commandText)
        {
            using (SqlConnection conn = GetConnection())
            {
                return await ExecuteDataSetAsync(conn, null, commandText, CommandType.Text, null, null);
            }
        }
        #endregion

        #region ExecuteReader() - Not Released

        #endregion

        #region ExecuteNonQuery()

        private static SqlCommand ExecuteNonQuery(SqlConnection conn, SqlTransaction trx, string commandText, CommandType type, ParameterCollection pCollection, string[] paramKeys, out int RowCount)
        {
            SqlCommand command =  PrepareCommand(conn, trx, commandText, type, pCollection, paramKeys);

            if (conn.State == ConnectionState.Closed)
            {
                conn.Open();
            }

            RowCount = command.ExecuteNonQuery();
            return command;
        }

        public static int ExecuteNonQuery(SqlConnection conn, SqlTransaction trx, string commandText, CommandType type, ParameterCollection pCollection, string[] paramKeys)
        {
            int rowCount = 0;

            ExecuteNonQuery(conn, trx, commandText, type, pCollection, paramKeys, out rowCount);

            return rowCount;
        }

        public static int ExecuteNonQuery(SqlConnection conn, SqlTransaction trx, string commandText, CommandType type, ref ParameterCollection pCollection, string[] paramKeys)
        {
            int rowCount = 0; 

            SqlCommand command = ExecuteNonQuery(conn, trx, commandText, type, pCollection, paramKeys, out rowCount);

            foreach (SqlParameter sp in command.Parameters)
            {
                if (sp.Direction == ParameterDirection.InputOutput || sp.Direction == ParameterDirection.Output)
                {
                    pCollection[sp.ParameterName].DefaultValue = sp.Value.ToString();
                }
            }

            return rowCount;
        }

        public static int ExecuteNonQuery(SqlConnection conn, SqlTransaction trx, string commandText, CommandType type, ref ParameterCollection pCollection)
        {
            return ExecuteNonQuery(conn, trx, commandText, type, ref pCollection, null);
        }

        public static int ExecuteNonQuery(SqlConnection conn, SqlTransaction trx, string commandText, CommandType type, ParameterCollection pCollection)
        {
            return ExecuteNonQuery(conn, trx, commandText, type, pCollection, null);
        }

        public static int ExecuteNonQuery(SqlConnection conn, SqlTransaction trx, string commandText, CommandType type)
        {
            return ExecuteNonQuery(conn, trx, commandText, type, null, null);
        }

        public static int ExecuteNonQuery(SqlConnection conn, SqlTransaction trx, string commandText)
        {
            return ExecuteNonQuery(conn, trx, commandText, CommandType.StoredProcedure, null, null);
        }

        public static int ExecuteNonQuery(SqlConnection conn, string commandText, CommandType type, ParameterCollection pCollection, string[] paramKeys)
        {
            return ExecuteNonQuery(conn, null, commandText, type, pCollection, paramKeys);
        }

        public static int ExecuteNonQuery(SqlConnection conn, string commandText, CommandType type, ParameterCollection pCollection)
        {
            return ExecuteNonQuery(conn, null, commandText, type, pCollection, null);
        }
        public static int ExecuteNonQuery(string connString, string commandText, CommandType type, ParameterCollection pCollection)
        {
            using (SqlConnection conn = GetConnectionByKey(connString))
            {
                return ExecuteNonQuery(conn, null, commandText, type, pCollection, null);
            }
        }
        public static int ExecuteNonQuery(SqlConnection conn, string commandText, CommandType type, ref ParameterCollection pCollection)
        {
            return ExecuteNonQuery(conn, null, commandText, type, ref pCollection, null);
        }

        public static int ExecuteNonQuery(SqlConnection conn, string commandText, CommandType type)
        {
            return ExecuteNonQuery(conn, null, commandText, type, null, null);
        }

        public static int ExecuteNonQuery(SqlConnection conn, string commandText)
        {
            return ExecuteNonQuery(conn, null, commandText, CommandType.StoredProcedure, null, null);
        }

        public static int ExecuteNonQuery(string commandText, CommandType type, ParameterCollection pCollection)
        {
            using (SqlConnection conn = GetConnection())
            {
                return ExecuteNonQuery(conn, commandText, type, pCollection);
            }
        }

        public static int ExecuteNonQuery(string commandText, CommandType type, ref ParameterCollection pCollection)
        {
            using (SqlConnection conn = GetConnection())
            {
                return ExecuteNonQuery(conn, commandText, type, ref pCollection);
            }
        }

        public static int ExecuteNonQuery(string commandText, ParameterCollection pCollection)
        {
            using (SqlConnection conn = GetConnection())
            {
                return ExecuteNonQuery(conn, commandText, CommandType.Text, pCollection);
            }
        }

        public static int ExecuteNonQuery(string commandText)
        {
            using (SqlConnection conn = GetConnection())
            {
                return ExecuteNonQuery(conn, null, commandText, CommandType.Text, null, null);
            }
        }

        #endregion

        #region ExecuteNonQueryAsync()

        private static async Task<int> ExecuteNonQueryAsync(SqlConnection conn, SqlTransaction trx, string commandText, CommandType type, ParameterCollection pCollection, string[] paramKeys)
        {
            SqlCommand command = await PrepareCommandAsync(conn, trx, commandText, type, pCollection, paramKeys).ConfigureAwait(false);

            if (conn.State == ConnectionState.Closed)
            {
                conn.Open();
            }

            var retVal = await command.ExecuteNonQueryAsync().ConfigureAwait(false);

            return retVal;
        }

        public static async Task<int> ExecuteNonQueryAsync(SqlConnection conn, SqlTransaction trx, string commandText, ParameterCollection pCollection)
        {
            return await ExecuteNonQueryAsync(conn, trx, commandText, CommandType.Text, pCollection);
        }
        public static async Task<int> ExecuteNonQueryAsync(SqlConnection conn, SqlTransaction trx, string commandText, CommandType type, ParameterCollection pCollection)
        {
            return await ExecuteNonQueryAsync(conn, trx, commandText, type, pCollection, null);
        }

        public static async Task<int> ExecuteNonQueryAsync(string commandText, ParameterCollection pCollection)
        {
            using (SqlConnection conn = GetConnection())
            {
                return await ExecuteNonQueryAsync(conn, commandText, CommandType.Text, pCollection);
            }
        }

        public static async Task<int> ExecuteNonQueryAsync(string commandText, CommandType type)
        {
            using (SqlConnection conn = GetConnection())
            {
                return await ExecuteNonQueryAsync(conn, commandText, type, null);
            }
        }

        public static async Task<int> ExecuteNonQueryAsync(SqlConnection conn, string commandText, CommandType type, ParameterCollection pCollection)
        {
            return await ExecuteNonQueryAsync(conn, null, commandText, type, pCollection, null);
        }

        public static async Task<int> ExecuteNonQueryAsync(string commandText)
        {
            using (SqlConnection conn = GetConnection())
            {
                return await ExecuteNonQueryAsync(conn, null, commandText, CommandType.Text, null, null);
            }
        }

        #endregion

        #region ExecuteScalar()


        private static SqlCommand ExecuteScalar(SqlConnection conn, SqlTransaction trx, string commandText, CommandType type, ParameterCollection pCollection, string[] paramKeys, out object oScalar)
        {
            SqlCommand command = PrepareCommand(conn, trx, commandText, type, pCollection, paramKeys);

            if (conn.State == ConnectionState.Closed)
            {
                conn.Open();
            }

            oScalar = command.ExecuteScalar();

            return command;
        }

        public static object ExecuteScalar(SqlConnection conn, SqlTransaction trx, string commandText, CommandType type, ParameterCollection pCollection, string[] paramKeys)
        {
            object oScalar = null;

            SqlCommand command = ExecuteScalar(conn, trx, commandText, type, pCollection, paramKeys, out oScalar);

            return oScalar;
        }


        public static object ExecuteScalar(SqlConnection conn, SqlTransaction trx, string commandText, CommandType type, ref ParameterCollection pCollection, string[] paramKeys)
        {
            object oScalar = null;

            SqlCommand command = ExecuteScalar(conn, trx, commandText, type, pCollection, paramKeys, out oScalar);
            
            foreach (SqlParameter sp in command.Parameters)
            {
                if (sp.Direction == ParameterDirection.InputOutput || sp.Direction == ParameterDirection.Output)
                {
                    pCollection[sp.ParameterName].DefaultValue = sp.Value.ToString();
                }
            }

            return oScalar;
        }

        public static object ExecuteScalar(SqlConnection conn, string commandText, CommandType type, ParameterCollection pCollection)
        {
            return ExecuteScalar(conn, null, commandText, type, pCollection, null);
        }
        public static object ExecuteScalar(SqlConnection conn, SqlTransaction trx, string commandText, CommandType type, ParameterCollection pCollection)
        {
            return ExecuteScalar(conn, trx, commandText, type, pCollection, null);
        }

        public static object ExecuteScalar(string connString, string commandText, CommandType type, ParameterCollection pCollection)
        {
            using (SqlConnection conn = GetConnectionByKey(connString))
            {
                return ExecuteScalar(conn, null, commandText, type, pCollection, null);
            }
        }

        public static object ExecuteScalar(SqlConnection conn, string commandText, CommandType type)
        {
            return ExecuteScalar(conn, null, commandText, type, null, null);
        }

        public static object ExecuteScalar(SqlConnection conn, string commandText)
        {
            return ExecuteScalar(conn, null, commandText, CommandType.StoredProcedure, null, null);
        }

        public static object ExecuteScalar(SqlConnection conn, SqlTransaction trx, string commandText)
        {
            return ExecuteScalar(conn, trx, commandText, CommandType.StoredProcedure, null, null);
        }

        public static object ExecuteScalar(string commandText)
        {
            using (SqlConnection conn = GetConnection())
            {
                return ExecuteScalar(conn, null, commandText, CommandType.Text, null, null);
            }
        }

        public static object ExecuteScalar(string commandText, SqlTransaction trx)
        {
            using (SqlConnection conn = GetConnection())
            {
                return ExecuteScalar(conn, trx, commandText, CommandType.Text, null, null);
            }
        }

        public static object ExecuteScalar(string commandText, ParameterCollection pCollection)
        {
            using (SqlConnection conn = GetConnection())
            {
                return ExecuteScalar(conn, null, commandText, CommandType.Text, pCollection, null);
            }
        }

        
        public static object ExecuteScalar(string commandText, SqlTransaction trx, ParameterCollection pCollection)
        {
            using (SqlConnection conn = GetConnection())
            {
                return ExecuteScalar(conn, trx, commandText, CommandType.Text, pCollection, null);
            }
        }

        public static object ExecuteScalar(string commandText, CommandType type, ParameterCollection pCollection)
        {
            using (SqlConnection conn = GetConnection())
            {
                return ExecuteScalar(conn, null, commandText, type, pCollection, null);
            }
        }

        public static object ExecuteScalar(string commandText, CommandType type, ref ParameterCollection pCollection)
        {
            using (SqlConnection conn = GetConnection())
            {
                return ExecuteScalar(conn, null, commandText, type, ref pCollection, null);
            }
        }

        #endregion
        
        #region ExecuteScalarAsync()

        private async static Task<object> ExecuteScalarAsync(SqlConnection conn, SqlTransaction trx, string commandText, CommandType type, ParameterCollection pCollection, string[] paramKeys)
        {
            SqlCommand command = await PrepareCommandAsync(conn, trx, commandText, type, pCollection, paramKeys).ConfigureAwait(false);

            if (conn.State == ConnectionState.Closed)
            {
                conn.Open();
            }

            var retVal = await command.ExecuteScalarAsync().ConfigureAwait(false);
            return retVal;
        }
        public static async Task<object> ExecuteScalarAsync(SqlConnection conn, SqlTransaction trx, string commandText, ParameterCollection pCollection)
        {
            return await ExecuteScalarAsync(conn, trx, commandText, CommandType.Text, pCollection, null);
        }

        public static async Task<object> ExecuteScalarAsync(string commandText, CommandType type, ParameterCollection pCollection)
        {
            using (SqlConnection conn = GetConnection())
            {
                return await ExecuteScalarAsync(conn, null, commandText, type, pCollection, null);
            }
        }

        public static async Task<object> ExecuteScalarAsync(string commandText, ParameterCollection pCollection)
        {
            using (SqlConnection conn = GetConnection())
            {
                return await ExecuteScalarAsync(conn, null, commandText, CommandType.Text, pCollection, null);
            }
        }

        #endregion
    }
}
