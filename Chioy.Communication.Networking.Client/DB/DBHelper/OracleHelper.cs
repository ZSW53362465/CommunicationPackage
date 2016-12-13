using System;
using System.Data;
using System.Data.Common;
using System.Data.OracleClient;

namespace Chioy.Communication.Networking.Client.DB.DBHelper
{
    public class OracleHelper : IDatabaseHelper
    {
        private OracleConnection _oracleConnection;

        private OracleHelper(string p_connectionString)
        {
            Reset(p_connectionString);
        }

        #region IDatabaseHelper Members

        public void Reset(string p_connectionString)
        {
            try
            {
                if (_oracleConnection != null && _oracleConnection.State != ConnectionState.Closed)
                {
                    _oracleConnection.Close();
                }
                _oracleConnection = new OracleConnection(p_connectionString);
            }
            catch (Exception ex)
            {
                //KLog.Logger.Error("Reset出现异常！", ex);
                throw ex;
            }
        }

        public DataTable ExecuteQuery(string p_sql)
        {
            var cmd = new OracleCommand();
            cmd.Connection = _oracleConnection;
            cmd.CommandText = p_sql;

            var adapter = new OracleDataAdapter(cmd);

            var ds = new DataSet();
            adapter.Fill(ds);

            return ds.Tables[0];
        }

        public DataTable ExecuteQuery(string p_sql, params DbParameter[] p_parameters)
        {
            var cmd = new OracleCommand();
            cmd.Connection = _oracleConnection;
            cmd.CommandText = p_sql;

            foreach (DbParameter dbParam in p_parameters)
            {
                cmd.Parameters.Add(dbParam);
            }

            var adapter = new OracleDataAdapter(cmd);

            var ds = new DataSet();
            adapter.Fill(ds);

            return ds.Tables[0];
        }

        /// <summary>
        /// 测试连接
        /// </summary>
        /// <param name="p_connectionString"></param>
        /// <returns></returns>
        public bool TestConnection()
        {
            try
            {
                _oracleConnection.Open();

                _oracleConnection.Close();

                return true;
            }
            catch (Exception ex)
            {
                //KLog.Logger.Error("测试连接出现异常！", ex);
                throw ex;
            }
        }


        public int ExecuteNonQuery(string p_sql, params DbParameter[] p_parameters)
        {
            var cmd = new OracleCommand();
            cmd.Connection = _oracleConnection;
            cmd.CommandText = p_sql;

            foreach (DbParameter dbParam in p_parameters)
            {
                var sqlParam = new OracleParameter(dbParam.ParameterName, dbParam.Value);
                sqlParam.DbType = dbParam.DbType;
                cmd.Parameters.Add(sqlParam);
            }

            int result = -1;
            try
            {
                _oracleConnection.Open();

                result = cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                //KLog.Logger.Error("执行数据库脚本出现异常！", ex);
                throw ex;
            }
            finally
            {
                _oracleConnection.Close();
            }

            return result;
        }

        #endregion

        /// <summary>
        /// 打开数据库连接
        /// </summary>
        /// <param name="p_connectionString"></param>
        /// <returns></returns>
        public static OracleHelper Open(string p_connectionString)
        {
            return new OracleHelper(p_connectionString);
        }
    }
}