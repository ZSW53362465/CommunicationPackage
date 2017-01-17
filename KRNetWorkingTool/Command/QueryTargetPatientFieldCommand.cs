using System.Collections.ObjectModel;
using System.Data;
using KRBMDCommon;
using KRBMDCommon.NetWorking.DBHelper;
using KRBMDCommon.NetWorking.Model;
using Chioy.Communication.Networking.KRNetWorkingTool.ViewModel;

namespace Chioy.Communication.Networking.KRNetWorkingTool.Command
{
    public class QueryTargetPatientFieldCommand : CommandBase
    {
        public QueryTargetPatientFieldCommand(NetWorkingViewModel p_networkingVM)
            : base(p_networkingVM)
        {
        }

        public override void Execute(object parameter)
        {
            DatabaseConfigModel dbConfig = NetworkingViewModel.DatabaseConfigModel;
            DatabaseEnum databaseEnum = KRBMDUtility.TransDatabaseSoft(dbConfig.DatabaseSoft, dbConfig.IsAdvancedSetting);
            IDatabaseHelper db = DatabaseHelper.Open(databaseEnum, dbConfig.ConnectionString);
            if (string.IsNullOrEmpty(dbConfig.User) || string.IsNullOrEmpty(dbConfig.Password))
            {
                return;
            }
            string sql = null;

            string tableName = NetworkingViewModel.PatientMapModel.PatientTableName;

            if (string.IsNullOrEmpty(tableName))
            {
                return;
            }
            if (dbConfig.DatabaseSoft == DatabaseSoft.SQLServer)
            {
                sql =
                    string.Format(
                        "select a.name as ColumnName,b.name as type,a.length,a.isnullable from syscolumns a,systypes b,sysobjects d where a.xtype=b.xusertype and a.id=d.id and a.id =object_id('{0}')",
                        tableName);
            }
            else if (dbConfig.DatabaseSoft == DatabaseSoft.Oracle)
            {
                sql =
                    string.Format(
                        "SELECT A.TABLE_NAME   as TableName,A.COLUMN_NAME  as ColumnName,A.DATA_TYPE    as type,A.DATA_LENGTH  as length  FROM USER_TAB_COLS A  where A.TABLE_NAME  = '{0}'",
                        tableName.ToUpper());
            }
            else if (dbConfig.DatabaseSoft == DatabaseSoft.PostgreSQL)
            {
                sql =
                    string.Format(
                        "select a.column_name as ColumnName, a.udt_name as type,a.character_maximum_length as length, a.is_nullable as isnullable from information_schema.columns a where table_name='{0}'",
                        tableName);
            }
            else
            {
                sql =
                    string.Format(
                        "Select A.COLUMN_NAME as ColumnName, A.DATA_TYPE TYPE,A.COLUMN_TYPE as LENGTH,A.IS_NULLABLE as ISNULLABLE FROM INFORMATION_SCHEMA.COLUMNS A Where A.table_schema = '{0}' AND A.table_name = '{1}'",
                        dbConfig.Database, tableName);
            }

            DataTable table = db.ExecuteQuery(sql);

            var list = new ObservableCollection<string>();

            foreach (DataRow row in table.AsEnumerable())
            {
                string name = row["ColumnName"].ToString();

                if (string.IsNullOrEmpty(name))
                {
                    continue;
                }

                list.Add(string.Format("[{0}]", name));
            }

            NetworkingViewModel.TargetFieldList = list;
        }
    }
}