using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Data.Common;

namespace Chioy.Communication.Networking.Client.DB.DBHelper
{
    public class MySQLHelper : IDatabaseHelper
    {
        private MySqlConnection _mySqlConnection;

        private MySQLHelper(string p_connectionString)
        {
            Reset(p_connectionString);
        }

        #region IDatabaseHelper Members

        public void Reset(string p_connectionString)
        {
            try
            {
                if (_mySqlConnection != null && _mySqlConnection.State != ConnectionState.Closed)
                {
                    _mySqlConnection.Close();
                }
                _mySqlConnection = new MySqlConnection(p_connectionString+";Charset=utf8;");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable ExecuteQuery(string p_sql)
        {
            var cmd = new MySqlCommand();
            cmd.Connection = _mySqlConnection;
            cmd.CommandText = p_sql;

            var adapter = new MySqlDataAdapter(cmd);

            var ds = new DataSet();
            adapter.Fill(ds);

            return ds.Tables[0];
        }

        public DataTable ExecuteQuery(string p_sql, params DbParameter[] p_parameters)
        {
            var cmd = new MySqlCommand();
            cmd.Connection = _mySqlConnection;
            cmd.CommandText = p_sql;

            foreach (DbParameter dbParam in p_parameters)
            {
                cmd.Parameters.Add(dbParam);
            }

            var adapter = new MySqlDataAdapter(cmd);

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
                _mySqlConnection.Open();

                _mySqlConnection.Close();

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
            var cmd = new MySqlCommand();
            cmd.Connection = _mySqlConnection;
            cmd.CommandText = p_sql;

            foreach (DbParameter dbParam in p_parameters)
            {
                cmd.Parameters.Add(dbParam);
            }

            int result = -1;
            try
            {
                _mySqlConnection.Open();

                result = cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                //KLog.Logger.Error("执行数据库脚本出现异常！", ex);
                throw ex;
            }
            finally
            {
                _mySqlConnection.Close();
            }

            return result;
        }

        #endregion

        /// <summary>
        /// 打开数据库连接
        /// </summary>
        /// <param name="p_connectionString"></param>
        /// <returns></returns>
        public static MySQLHelper Open(string p_connectionString)
        {
            return new MySQLHelper(p_connectionString);
        }
    }
}