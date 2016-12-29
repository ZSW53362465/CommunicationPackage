using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Data.Common;
using System.Diagnostics;
using System.Linq;

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

        public DataTable GetPatientInfoByProcedure(string productid)
        {
            //获取病人信息，假设存储过程名xinyueqi_id，输入参数@设备编号 productid
            //输出参数 @check_id @name @gender @age @height @weight
            MySqlCommand cmd = new MySqlCommand("TJPSB_GetMemInfo", _mySqlConnection);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(new MySqlParameter("@CheckDate", SqlDbType.VarChar));
            cmd.Parameters["@CheckDate"].Value = DateTime.Now.ToShortDateString();
            cmd.Parameters.Add(new MySqlParameter("@SBID", SqlDbType.VarChar));
            cmd.Parameters["@SBID"].Value = productid;
            cmd.Parameters.Add(new MySqlParameter("@NeedData", SqlDbType.VarChar));
            cmd.Parameters["@NeedData"].Value = "SG|TZ|";
            cmd.Parameters.Add(new MySqlParameter("@Qparam", SqlDbType.VarChar));
            cmd.Parameters["@Qparam"].Value = "";

            try
            {
                var adapter = new MySqlDataAdapter(cmd);

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
            MySqlCommand cmd = new MySqlCommand("TJPSB_UpdateResult", _mySqlConnection);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(new MySqlParameter("@CheckID", SqlDbType.VarChar));
            cmd.Parameters["@CheckID"].Value = check_id;
            cmd.Parameters.Add(new MySqlParameter("@SBID", SqlDbType.VarChar));
            cmd.Parameters["@SBID"].Value = productid;
            cmd.Parameters.Add(new MySqlParameter("@Result", SqlDbType.VarChar));
            cmd.Parameters["@Result"].Value = result;
            cmd.Parameters.Add(new MySqlParameter("@Qparam", SqlDbType.VarChar));
            cmd.Parameters["@Qparam"].Value = "";
            cmd.Parameters.Add(new MySqlParameter("@rtn", SqlDbType.Int));
            cmd.Parameters["@rtn"].Direction = ParameterDirection.ReturnValue;
            try
            {
                _mySqlConnection.Open();
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