using System;
using System.Data;
using System.Windows;
using Chioy.Communication.Networking.Client.DB.DBHelper;
using Chioy.Communication.Networking.Client.DB.Models;
using Chioy.Communication.Networking.Common;
using KRNetWorkingTool.ViewModel;

namespace KRNetWorkingTool.Command
{
    public class TestPatientFieldSQLCommand : CommandBase
    {
        public TestPatientFieldSQLCommand(NetWorkingViewModel p_networkingVM)
            : base(p_networkingVM)
        {
        }

        public override void Execute(object parameter)
        {
            PatientMapModel pmm = NetworkingViewModel.PatientMapModel;



            try
            {
                DatabaseConfigModel dataConfig = NetworkingViewModel.DatabaseConfigModel;
                string sql = pmm.GetTestPatientInfoSql(dataConfig.DatabaseSoft);
                DatabaseEnum databaseEnum = DataBaseSoft.TransDatabaseSoft(dataConfig.DatabaseSoft,
                                                                     dataConfig.IsAdvancedSetting);
                IDatabaseHelper dbHelper = DatabaseHelper.Open(databaseEnum, dataConfig.ConnectionString);
                DataTable table = dbHelper.ExecuteQuery(sql);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }

            MessageBox.Show("目标表与字段测试成功！");
        }
    }
}