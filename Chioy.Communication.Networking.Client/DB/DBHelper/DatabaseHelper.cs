using Chioy.Communication.Networking.Common;
using System;
using System.Data;
using System.Diagnostics;
using System.Text;
using Chioy.Communication.Networking.Client.DB.Models;

namespace Chioy.Communication.Networking.Client.DB.DBHelper
{
    public class DatabaseHelper
    {
        public static string _localConnStr = string.Empty;
        private const string _connectionStringPath = "../Conf/Common.ini";
        public static string LocalConnStr
        {
            get
            {
                if (string.IsNullOrEmpty(_localConnStr))
                {
                    Trace.WriteLine("进来了");
                    string connectionStringTemplete ="Data Source={0};Initial Catalog={1};Persist Security Info=True;User ID={2};Password={3}";
                    var server = new StringBuilder(32);
                    var database = new StringBuilder(32);
                    var user = new StringBuilder(32);
                    var password = new StringBuilder(32);
                    CommunicationHelper.GetPrivateProfileString("DATABASE", "DOMAINNAME", null, server, 32, _connectionStringPath);
                    CommunicationHelper.GetPrivateProfileString("DATABASE", "DBNAME", null, database, 32, _connectionStringPath);
                    CommunicationHelper.GetPrivateProfileString("DATABASE", "USERNAME", null, user, 32, _connectionStringPath);
                    CommunicationHelper.GetPrivateProfileString("DATABASE", "PASSWORD", null, password, 32, _connectionStringPath);
                    _localConnStr = string.Format(connectionStringTemplete, server, database, user, password);
                    Trace.WriteLine("_localConnStr:"+_localConnStr);
                }
                return _localConnStr;
            }
        }

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
        public static DataTable MakeCallBackTable()
        {
            // Create a new DataTable titled 'Names.'
            var callBackTable = new DataTable("PatientInfoTable");

            // Add three column objects to the table.
            var idColumn = new DataColumn();
            idColumn.DataType = Type.GetType("System.String");
            idColumn.ColumnName = "PatientID";
            callBackTable.Columns.Add(idColumn);

            var nameColumn = new DataColumn();
            nameColumn.DataType = Type.GetType("System.String");
            nameColumn.ColumnName = "Name";
            callBackTable.Columns.Add(nameColumn);

            var genderColumn = new DataColumn();
            genderColumn.DataType = Type.GetType("System.String");
            genderColumn.ColumnName = "Gender";
            callBackTable.Columns.Add(genderColumn);

            var ageColumn = new DataColumn();
            ageColumn.DataType = Type.GetType("System.Int32");
            ageColumn.ColumnName = "Age";
            callBackTable.Columns.Add(ageColumn);

            var heightColumn = new DataColumn();
            heightColumn.DataType = Type.GetType("System.Int32");
            heightColumn.ColumnName = "Height";
            callBackTable.Columns.Add(heightColumn);

            var weightColumn = new DataColumn();
            weightColumn.DataType = Type.GetType("System.Int32");
            weightColumn.ColumnName = "Weight";
            callBackTable.Columns.Add(weightColumn);

            var keys = new DataColumn[1];
            keys[0] = idColumn;
            callBackTable.PrimaryKey = keys;

            // Return the new DataTable.
            return callBackTable;
        }
        public static DatabaseEnum TransDatabaseSoft(DatabaseSoft p_soft, bool p_isAdvancedSetting = false)
        {
            var e = DatabaseEnum.OleDb;

            if (!p_isAdvancedSetting)
            {
                switch (p_soft)
                {
                    case DatabaseSoft.SQLServer:
                        e = DatabaseEnum.SQLServer;
                        break;
                    case DatabaseSoft.Oracle:
                        e = DatabaseEnum.Oracle;
                        break;
                    case DatabaseSoft.MySql:
                        e = DatabaseEnum.MySql;
                        break;
                    case DatabaseSoft.PostgreSQL:
                        e = DatabaseEnum.PostgreSQL;
                        break;
                }
            }

            return e;
        }
    }

}