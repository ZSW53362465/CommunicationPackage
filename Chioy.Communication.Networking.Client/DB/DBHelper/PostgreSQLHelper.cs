using Npgsql;
using System;
using System.Data;
using System.Data.Common;

namespace Chioy.Communication.Networking.Client.DB.DBHelper
{
    public class PostgreSQLHelper : IDatabaseHelper
    {
        private NpgsqlConnection _postgreSQLConnection;

        private PostgreSQLHelper(string p_connectionString)
        {
            Reset(p_connectionString);
        }

        #region IDatabaseHelper Members

        public void Reset(string p_connectionString)
        {
            try
            {
                if (_postgreSQLConnection != null && _postgreSQLConnection.State != ConnectionState.Closed)
                {
                    _postgreSQLConnection.Close();
                }
                _postgreSQLConnection = new NpgsqlConnection(p_connectionString);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable ExecuteQuery(string p_sql)
        {
            var cmd = new NpgsqlCommand();
            cmd.Connection = _postgreSQLConnection;
            cmd.CommandText = p_sql;

            var adapter = new NpgsqlDataAdapter(cmd);

            var ds = new DataSet();
            adapter.Fill(ds);

            return ds.Tables[0];
        }

        public DataTable ExecuteQuery(string p_sql, params DbParameter[] p_parameters)
        {
            var cmd = new NpgsqlCommand();
            cmd.Connection = _postgreSQLConnection;
            cmd.CommandText = p_sql;

            foreach (DbParameter dbParam in p_parameters)
            {
                cmd.Parameters.Add(dbParam);
            }

            var adapter = new NpgsqlDataAdapter(cmd);

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
                _postgreSQLConnection.Open();

                _postgreSQLConnection.Close();

                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public int ExecuteNonQuery(string p_sql, params DbParameter[] p_parameters)
        {
            var cmd = new NpgsqlCommand();
            cmd.Connection = _postgreSQLConnection;
            cmd.CommandText = p_sql;

            foreach (DbParameter dbParam in p_parameters)
            {
                var sqlParam = new NpgsqlParameter(dbParam.ParameterName, dbParam.Value);
                sqlParam.DbType = dbParam.DbType;
                //sqlParam.Size = dbParam.Size;
                cmd.Parameters.Add(sqlParam);
            }

            int result = -1;
            try
            {
                _postgreSQLConnection.Open();

                result = cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                _postgreSQLConnection.Close();
            }

            return result;
        }

        #endregion

        /// <summary>
        /// 打开数据库连接
        /// </summary>
        /// <param name="p_connectionString"></param>
        /// <returns></returns>
        public static PostgreSQLHelper Open(string p_connectionString)
        {
            return new PostgreSQLHelper(p_connectionString);
        }
    }
}
