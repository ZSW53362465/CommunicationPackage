using Chioy.Communication.Networking.Common;
using System;

namespace Chioy.Communication.Networking.Client.DB.DBHelper
{
    public class DatabaseHelper
    {
        /// <summary>
        /// 打开数据库连接
        /// </summary>
        /// <param name="p_connectionString"></param>
        /// <returns></returns>
        public static IDatabaseHelper Open(DatabaseEnum p_soft, string p_connectionString)
        {
            IDatabaseHelper dbHelper = null;
            switch (p_soft)
            {
                case DatabaseEnum.SQLServer:
                    dbHelper = SqlServerHelper.Open(p_connectionString);
                    break;
                case DatabaseEnum.Oracle:
                    dbHelper = OracleHelper.Open(p_connectionString);
                    break;
                case DatabaseEnum.MySql:
                    dbHelper = MySQLHelper.Open(p_connectionString);
                    break;
                case DatabaseEnum.OleDb:
                    dbHelper = OleDbHelper.Open(p_connectionString);
                    break;
                case DatabaseEnum.PostgreSQL:
                    dbHelper = PostgreSQLHelper.Open(p_connectionString);
                    break;
            }
            return dbHelper;
        }

        /// <summary>
        /// 测试连接
        /// </summary>
        /// <param name="p_connectionString"></param>
        /// <returns></returns>
        public static bool TestConnection(DatabaseEnum p_soft, string p_connectionString)
        {
            try
            {
                IDatabaseHelper dbHelper = Open(p_soft, p_connectionString);
                dbHelper.TestConnection();
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return true;
        }
    }

}