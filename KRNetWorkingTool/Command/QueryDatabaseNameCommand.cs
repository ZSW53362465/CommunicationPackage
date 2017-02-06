using System;
using System.Collections.ObjectModel;
using System.Data;
using System.Windows;
using Chioy.Communication.Networking.Client.DB.DBHelper;
using Chioy.Communication.Networking.Client.DB.Models;
using Chioy.Communication.Networking.Common;

using KRNetWorkingTool.ViewModel;

namespace KRNetWorkingTool.Command
{
    public class QueryDatabaseNameCommand : CommandBase
    {
        public QueryDatabaseNameCommand(NetWorkingViewModel p_networkingVM)
            : base(p_networkingVM)
        {
        }

        public override void Execute(object parameter)
        {
            try
            {
                DatabaseConfigModel dbConfig = NetworkingViewModel.DatabaseConfigModel;

                DatabaseEnum databaseEnum = DataBaseSoft.TransDatabaseSoft(dbConfig.DatabaseSoft, dbConfig.IsAdvancedSetting);

                IDatabaseHelper db = DatabaseHelper.Open(databaseEnum, dbConfig.ConnectionString);
                ;

                string sql = null;
                if (dbConfig.DatabaseSoft == DatabaseSoft.SQLServer)
                {
                    sql = "select name from sys.databases";
                }
                else if (dbConfig.DatabaseSoft == DatabaseSoft.MySql)
                {
                    sql = " SHOW DATABASES ";
                }
                else if (dbConfig.DatabaseSoft == DatabaseSoft.PostgreSQL)
                {
                    sql = "SELECT datname FROM pg_database";
                }
                if (sql == null)
                {
                    return;
                }

                DataTable table = db.ExecuteQuery(sql);

                if (table == null)
                {
                    return;
                }

                var list = new ObservableCollection<string>();

                EnumerableRowCollection<string> en = null;
                if (table.Columns.Contains("datname"))
                {
                    en = table.AsEnumerable().Select(r => r["datname"].ToString());
                } 
                else if (table.Columns.Contains("name"))
                {
                    en = table.AsEnumerable().Select(r => r["name"].ToString());
                }
                else
                {
                    en = table.AsEnumerable().Select(r => r["Database"].ToString());
                }

                foreach (string str in en)
                {
                    if (string.IsNullOrEmpty(str))
                    {
                        continue;
                    }

                    list.Add(str);
                }

                NetworkingViewModel.DatabaseList = list;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}