using System;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;

namespace Chioy.Communication.Networking.Client.DB.DBHelper
{
    public class SqlServerHelper : IDatabaseHelper
    {
        private SqlConnection _sqlServerConnection;

        private SqlServerHelper(string p_connectionString)
        {
            Reset(p_connectionString);
        }

        #region IDatabaseHelper Members

        public void Reset(string p_connectionString)
        {
            try
            {
                if (_sqlServerConnection != null && _sqlServerConnection.State != ConnectionState.Closed)
                {
                    _sqlServerConnection.Close();
                }
                _sqlServerConnection = new SqlConnection(p_connectionString);
            }
            catch (Exception ex)
            {
                //KLog.Logger.Error("Reset出现异常！", ex);
                throw ex;
            }
        }

        public DataTable ExecuteQuery(string p_sql)
        {
            var cmd = new SqlCommand();
            cmd.Connection = _sqlServerConnection;
            cmd.CommandText = p_sql;

            var adapter = new SqlDataAdapter(cmd);

            var ds = new DataSet();
            adapter.Fill(ds);

            return ds.Tables[0];
        }

        public DataTable ExecuteQuery(string p_sql, params DbParameter[] p_parameters)
        {
            var cmd = new SqlCommand();
            cmd.Connection = _sqlServerConnection;
            cmd.CommandText = p_sql;

            foreach (DbParameter dbParam in p_parameters)
            {
                cmd.Parameters.Add(dbParam);
            }

            var adapter = new SqlDataAdapter(cmd);

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
                _sqlServerConnection.Open();

                _sqlServerConnection.Close();

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
            var cmd = new SqlCommand();
            cmd.Connection = _sqlServerConnection;
            cmd.CommandText = p_sql;

            foreach (DbParameter dbParam in p_parameters)
            {
                var sqlParam = new SqlParameter(dbParam.ParameterName, dbParam.Value);
                sqlParam.DbType = dbParam.DbType;
                //sqlParam.Size = dbParam.Size;
                cmd.Parameters.Add(sqlParam);
            }

            int result = -1;
            try
            {
                _sqlServerConnection.Open();

                result = cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                //KLog.Logger.Error("执行查询出现异常！", ex);
                throw ex;
            }
            finally
            {
                _sqlServerConnection.Close();
            }

            return result;
        }

        #endregion

        /// <summary>
        /// 打开数据库连接
        /// </summary>
        /// <param name="p_connectionString"></param>
        /// <returns></returns>
        public static SqlServerHelper Open(string p_connectionString)
        {
            return new SqlServerHelper(p_connectionString);
        }
    }
}