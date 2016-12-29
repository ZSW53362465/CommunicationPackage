﻿using Npgsql;
using System;
using System.Data;
using System.Data.Common;
using System.Diagnostics;
using System.Linq;

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
        ///  /// <summary>
        /// 打开数据库连接
        /// </summary>
        /// <param name="p_connectionString"></param>
        /// <returns></returns>
        public static PostgreSQLHelper Open(string p_connectionString)
        {
            return new PostgreSQLHelper(p_connectionString);
        }

        public DataTable GetPatientInfoByProcedure(string productid)
        {
            //获取病人信息，假设存储过程名xinyueqi_id，输入参数@设备编号 productid
            //输出参数 @check_id @name @gender @age @height @weight

            NpgsqlCommand cmd = new NpgsqlCommand("TJPSB_GetMemInfo", _postgreSQLConnection);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(new NpgsqlParameter("@CheckDate", SqlDbType.VarChar));
            cmd.Parameters["@CheckDate"].Value = DateTime.Now.ToShortDateString();
            cmd.Parameters.Add(new NpgsqlParameter("@SBID", SqlDbType.VarChar));
            cmd.Parameters["@SBID"].Value = productid;
            cmd.Parameters.Add(new NpgsqlParameter("@NeedData", SqlDbType.VarChar));
            cmd.Parameters["@NeedData"].Value = "SG|TZ|";
            cmd.Parameters.Add(new NpgsqlParameter("@Qparam", SqlDbType.VarChar));
            cmd.Parameters["@Qparam"].Value = "";

            try
            {
                var adapter = new NpgsqlDataAdapter(cmd);

                var ds = new DataSet();
                adapter.Fill(ds);
                DataRow dr = ds.Tables[0].AsEnumerable().FirstOrDefault();
                DataTable dataTable = DatabaseHelper.MakeCallBackTable();
                if (dr == null)
                {
                    return null;
                }
                else
                {
                    DataRow row = dataTable.NewRow();
                    row["PatientID"] = dr["CheckID"];
                    row["Name"] = dr["MemName"];
                    row["Gender"] = dr["MemSex"];
                    row["Age"] = dr["MemAge"];
                    row["Height"] = string.IsNullOrEmpty(dr["SG"].ToString()) ? 170 : (int)Math.Round(double.Parse(dr["SG"].ToString()), 0);
                    row["Weight"] = string.IsNullOrEmpty(dr["TZ"].ToString()) ? 60 : (int)Math.Round(double.Parse(dr["TZ"].ToString()), 0);
                    dataTable.Rows.Add(row);
                    return dataTable;
                }
            }
            catch (Exception exception)
            {
                Trace.WriteLine("GetPatientInfoByProcedure:调用存储过程失败" + exception.Message);
                throw new Exception("获取病人信息失败！", exception);
            }
        }

        public void UploadResultByProcedure(string productid, string check_id, string result)
        {
            NpgsqlCommand cmd = new NpgsqlCommand("TJPSB_UpdateResult", _postgreSQLConnection);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(new NpgsqlParameter("@CheckID", SqlDbType.VarChar));
            cmd.Parameters["@CheckID"].Value = check_id;
            cmd.Parameters.Add(new NpgsqlParameter("@SBID", SqlDbType.VarChar));
            cmd.Parameters["@SBID"].Value = productid;
            cmd.Parameters.Add(new NpgsqlParameter("@Result", SqlDbType.VarChar));
            cmd.Parameters["@Result"].Value = result;
            cmd.Parameters.Add(new NpgsqlParameter("@Qparam", SqlDbType.VarChar));
            cmd.Parameters["@Qparam"].Value = "";
            cmd.Parameters.Add(new NpgsqlParameter("@rtn", SqlDbType.Int));
            cmd.Parameters["@rtn"].Direction = ParameterDirection.ReturnValue;
            try
            {
                _postgreSQLConnection.Open();
                cmd.ExecuteNonQuery();
                if (cmd.Parameters["@rtn"].Value.ToString() != "0")
                {
                    Trace.WriteLine("UploadResultByProcedure:调用存储过程失败");
                }
            }
            catch (Exception exception)
            {
                Trace.WriteLine("UploadResultByProcedure:调用存储过程失败" + exception.Message);
                throw new Exception("调用存储过程失败，返回值不为0！", exception);
            }
        }
    }
}
