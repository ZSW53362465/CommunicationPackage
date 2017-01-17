using System;
using System.Windows;
using KRBMDCommon;
using KRBMDCommon.NetWorking.DBHelper;
using KRBMDCommon.NetWorking.Model;
using Chioy.Communication.Networking.KRNetWorkingTool.ViewModel;

namespace Chioy.Communication.Networking.KRNetWorkingTool.Command
{
    public class TestConnectionCommand : CommandBase
    {
        public TestConnectionCommand(NetWorkingViewModel p_networkingVM)
            : base(p_networkingVM)
        {
            //NetworkingViewModel = p_networkingVM;
        }

        public override bool CanExecute(object parameter)
        {
            return NetworkingViewModel != null;
        }

        public override void Execute(object parameter)
        {
            DatabaseConfigModel dbConfig = NetworkingViewModel.DatabaseConfigModel;
            bool isSuccess = false;
            try
            {
                DatabaseEnum databaseEnum = KRBMDUtility.TransDatabaseSoft(dbConfig.DatabaseSoft, dbConfig.IsAdvancedSetting);
                isSuccess = DatabaseHelper.TestConnection(databaseEnum, dbConfig.ConnectionString);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "错误", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (isSuccess)
            {
                MessageBox.Show("连接测试成功！");
            }
        }
    }
}