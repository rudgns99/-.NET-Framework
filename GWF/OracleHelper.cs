using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Oracle.ManagedDataAccess.Client;
using System.Configuration;
using System.Data;

namespace GWF
{
    public class OracleHelper : BaseHelper, IDisposable
    {
        private OracleConnection conn = new OracleConnection();
        private const string ConnectionStringFormat = "Data Source=(DESCRIPTION="
              + "(ADDRESS_LIST=(ADDRESS=(PROTOCOL=TCP)(HOST={0})(PORT={4})))"
              + "(CONNECT_DATA=(SERVER=DEDICATED)(SERVICE_NAME={1})));"
              + "User Id={2};Password=\"{3}\"";

        public bool setConnection(string Host, string ServiceName, string UserName, string Password, string Port)
        {
            try
            {
                conn = new OracleConnection(string.Format(ConnectionString, Host, ServiceName, UserName, Password, Port));
                
                conn.Open();
            }
            catch (Exception ex)
            {
                base.ErrorMsg = ex.Message;
                return false;
            }

            return true;
        }

        public bool isOpen
        {
            get
            {
                return conn.State == System.Data.ConnectionState.Open;
            }
        }

        public ConnectionState Status
        {
            get
            {
                return conn.State;
            }
        }

        public DataSet ExecuteDataSet2(string CommandText)
        {
            base.Reset();
            DataSet retValue = new DataSet();
            OracleCommand cmd = new OracleCommand();
            cmd.CommandText = CommandText;
            cmd.CommandType = CommandType.Text;
            cmd.Connection = this.conn;
            OracleDataAdapter da = new OracleDataAdapter(cmd);

            if (isOpen)
            {
                da.Fill(retValue);
            }
            else
            {
                Result = false;
                ErrorCode = BaseConstants.ERROR_CODE.DBCONNECTIONERROR;
                ErrorMsg = BaseConstants.ERROR_MSG.DBCONNECTIONERROR;
            }

            return retValue;
        }

        public int ExecuteNonQuery2(string CommandText)
        {
            base.Reset();
            OracleCommand cmd = new OracleCommand();
            cmd.CommandText = CommandText;
            cmd.CommandType = CommandType.Text;
            cmd.Connection = this.conn;

            return cmd.ExecuteNonQuery();
        }

        public int ExecuteNonQuery2(string CommandText, string argu)
        {
            base.Reset();
            int result = -1;
            try
            {
                OracleCommand cmd = new OracleCommand();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = CommandText;
                cmd.Connection = this.conn;

                OracleParameter op = new OracleParameter(":xdata", OracleDbType.XmlType, ParameterDirection.Input);
                op.Size = argu.Length;
                op.Value = argu;

                cmd.Parameters.Add(op);

                result = cmd.ExecuteNonQuery();
                cmd.Parameters.Clear();
            }
            catch (Exception e)
            {
                Result = false;
                ErrorCode = "01";
                ErrorMsg = e.Message;
            }

            return result;
        }



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

        #endregion

        #region GetConnection()

        public static OracleConnection GetConnection()
        {
            return new OracleConnection(ConnectionString);
        }


        public static OracleConnection GetConnection(string connString)
        {
            return new OracleConnection(connString);

        }

        #endregion

        #region PrepareCommand()

        public static OracleCommand PrepareCommand(OracleConnection conn, OracleTransaction trx, string commandText, CommandType type, CustomParameterCollection pCollection, string[] paramKeys)
        {
            // OracleCommand 생성 및 명령문 설정
            OracleCommand command = conn.CreateCommand();
            command.CommandText = commandText;
            command.CommandType = type;

            // 트랙잭션이 지정된 경우 명령에 트랜잭션 지정
            if (trx != null)
            {
                command.Transaction = trx;
            }

            // 파라미터를 조합하여 명령에 추가한다.
            AttachParameters(command, type, pCollection, paramKeys);

            return command;
        }

        #endregion

        #region AttachParameters(), GetParameter()

        public static void AttachParameters(OracleCommand command, CommandType type, CustomParameterCollection pCollection, string[] paramKeys)
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

                            OracleParameter sqlParam = GetParameter(command, pCollection, paramName);
                            if (sqlParam != null)
                            {
                                command.Parameters.Add(sqlParam);
                            }
                        }
                    }
                }
                else if (type == CommandType.Text && paramKeys == null)
                {
                    for (int i = 0; i < pCollection.Count; i++)
                    {
                        CustomParameter cParam = pCollection[i] as CustomParameter;
                        if (cParam != null)
                        {
                            command.Parameters.Add(cParam.Name, cParam.GetValue());
                        }
                    }
                }
                else if (paramKeys != null)
                {
                    for (int i = 0; i < paramKeys.Length; i++)
                    {
                        string paramName = paramKeys[i];

                        OracleParameter sqlParam = GetParameter(command, pCollection, paramName);
                        if (sqlParam != null)
                        {
                            command.Parameters.Add(sqlParam);
                        }
                    }
                }
            }
        }

        public static OracleParameter GetParameter(OracleCommand command, CustomParameterCollection pCollection, string paramName)
        {

            OracleParameter sqlParam = null;

            for (int i = 0; i < pCollection.Count; i++)
            {
                if (pCollection[i].Name.Equals(paramName, StringComparison.OrdinalIgnoreCase))
                {
                    CustomParameter cParam = pCollection[i] as CustomParameter;
                    if (cParam != null)
                    {
                        sqlParam = command.CreateParameter();
                        sqlParam.OracleDbType = cParam.ParamOracleType;
                        sqlParam.Size = cParam.Size;
                        sqlParam.Direction = cParam.Direction;
                        sqlParam.ParameterName = cParam.Name;
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

        //public static OracleParameter GetParameter(OracleCommand command, CustomParameterCollection pCollection, string paramName)
        //{
        //    OracleParameter sqlParam = null;

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

        public static DataSet GetStoredProcedureReport(OracleConnection conn, string spName)
        {
            DataSet ds = null;

            if (conn != null && !String.IsNullOrEmpty(spName))
            {
                OracleCommand command = conn.CreateCommand();
                command.CommandText = "sp_help";
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new OracleParameter("@objname", spName));

                ds = new DataSet("StoredProcedure");
                OracleDataAdapter adpt = new OracleDataAdapter(command);
                adpt.Fill(ds);
            }

            return ds;
        }

        #endregion

        #region ExecuteDataSet()

        public static DataSet ExecuteDataSet(OracleConnection conn, OracleTransaction trx, string commandText, CommandType type, CustomParameterCollection pCollection, string[] paramKeys)
        {
            OracleCommand command = PrepareCommand(conn, trx, commandText, type, pCollection, paramKeys);

            DataSet ds = new DataSet();
            OracleDataAdapter adpt = new OracleDataAdapter(command);
            adpt.Fill(ds);

            return ds;
        }

        public static DataSet ExecuteDataSet(OracleConnection conn, string commandText, CommandType type, CustomParameterCollection pCollection, string[] paramKeys)
        {
            OracleCommand command = PrepareCommand(conn, null, commandText, type, pCollection, paramKeys);

            DataSet ds = new DataSet();
            OracleDataAdapter adpt = new OracleDataAdapter(command);
            string aa = command.CommandText;

            adpt.Fill(ds);

            return ds;
        }

        public static DataSet ExecuteDataSet(OracleConnection conn, string commandText, CommandType type, CustomParameterCollection pCollection)
        {
            return ExecuteDataSet(conn, commandText, type, pCollection, null);
        }

        public static DataSet ExecuteDataSet(OracleConnection conn, string commandText, CommandType type)
        {
            return ExecuteDataSet(conn, commandText, type, null, null);
        }

        public static DataSet ExecuteDataSet(OracleConnection conn, string commandText)
        {
            return ExecuteDataSet(conn, commandText, CommandType.StoredProcedure, null, null);
        }

        public static DataSet ExecuteDataSet(string commandText, CustomParameterCollection pCollection)
        {
            return ExecuteDataSet(GetConnection(), commandText, CommandType.Text, pCollection, null);
        }

        public static DataSet ExecuteDataSet(string commandText, CommandType type, CustomParameterCollection pCollection)
        {
            return ExecuteDataSet(GetConnection(), commandText, type, pCollection, null);
        }

        public static DataSet ExecuteDataSet(string commandText)
        {
            return ExecuteDataSet(GetConnection(), commandText, CommandType.Text, null, null);
        }
        
        #endregion

        #region ExecuteReader() - Not Released

        #endregion

        #region ExecuteNonQuery()

        public static int ExecuteNonQuery(OracleConnection conn, OracleTransaction trx, string commandText, CommandType type, CustomParameterCollection pCollection, string[] paramKeys)
        {
            int rowCount = 0;
            bool openConnect = false;

            OracleCommand command = PrepareCommand(conn, trx, commandText, type, pCollection, paramKeys);

            if (conn.State == ConnectionState.Closed)
            {
                conn.Open();
                openConnect = true;
            }

            rowCount = command.ExecuteNonQuery();

            if (openConnect)
            {
                conn.Close();
            }

            return rowCount;
        }

        public static int ExecuteNonQuery(OracleConnection conn, OracleTransaction trx, string commandText, CommandType type, CustomParameterCollection pCollection)
        {
            return ExecuteNonQuery(conn, trx, commandText, type, pCollection, null);
        }

        public static int ExecuteNonQuery(OracleConnection conn, OracleTransaction trx, string commandText, CommandType type)
        {
            return ExecuteNonQuery(conn, trx, commandText, type, null, null);
        }

        public static int ExecuteNonQuery(OracleConnection conn, OracleTransaction trx, string commandText)
        {
            return ExecuteNonQuery(conn, trx, commandText, CommandType.StoredProcedure, null, null);
        }

        public static int ExecuteNonQuery(OracleConnection conn, string commandText, CommandType type, CustomParameterCollection pCollection, string[] paramKeys)
        {
            return ExecuteNonQuery(conn, null, commandText, type, pCollection, paramKeys);
        }

        public static int ExecuteNonQuery(OracleConnection conn, string commandText, CommandType type, CustomParameterCollection pCollection)
        {
            return ExecuteNonQuery(conn, null, commandText, type, pCollection, null);
        }

        public static int ExecuteNonQuery(OracleConnection conn, string commandText, CommandType type)
        {
            return ExecuteNonQuery(conn, null, commandText, type, null, null);
        }

        public static int ExecuteNonQuery(OracleConnection conn, string commandText)
        {
            return ExecuteNonQuery(conn, null, commandText, CommandType.StoredProcedure, null, null);
        }

        public static int ExecuteNonQuery(string commandText, CommandType type, CustomParameterCollection pCollection)
        {
            return ExecuteNonQuery(GetConnection(), commandText, type, pCollection);
        }

        public static int ExecuteNonQuery(string commandText, CustomParameterCollection pCollection)
        {
            return ExecuteNonQuery(GetConnection(), commandText, CommandType.Text, pCollection);
        }

        public static int ExecuteNonQuery(string commandText)
        {
            return ExecuteNonQuery(GetConnection(), null, commandText, CommandType.Text, null, null);
        }

        #endregion

        #region ExecuteScalar()

        public static object ExecuteScalar(OracleConnection conn, string commandText, CommandType type, CustomParameterCollection pCollection, string[] paramKeys)
        {
            object oScalar = null;
            bool openConnect = false;

            OracleCommand command = PrepareCommand(conn, null, commandText, type, pCollection, paramKeys);

            if (conn.State == ConnectionState.Closed)
            {
                conn.Open();
                openConnect = true;
            }

            oScalar = command.ExecuteScalar();

            if (openConnect)
            {
                conn.Close();
            }

            return oScalar;
        }

        public static object ExecuteScalar(OracleConnection conn, string commandText, CommandType type, CustomParameterCollection pCollection)
        {
            return ExecuteScalar(conn, commandText, type, pCollection, null);
        }

        public static object ExecuteScalar(OracleConnection conn, string commandText, CommandType type)
        {
            return ExecuteScalar(conn, commandText, type, null, null);
        }

        public static object ExecuteScalar(OracleConnection conn, string commandText)
        {
            return ExecuteScalar(conn, commandText, CommandType.StoredProcedure, null, null);
        }

        public static object ExecuteScalar(string commandText)
        {
            return ExecuteScalar(GetConnection(), commandText, CommandType.Text, null, null);
        }

        public static object ExecuteScalar(string commandText, CustomParameterCollection pCollection)
        {
            return ExecuteScalar(GetConnection(), commandText, CommandType.Text, pCollection, null);
        }

        #endregion

        #region Dispose

        public void Close()
        {
            if (conn.State == System.Data.ConnectionState.Open)
            {
                conn.Close();
            }
        }

        public void Dispose()
        {
            Close();
            GC.SuppressFinalize(this);
        }

        #endregion
    }
}
