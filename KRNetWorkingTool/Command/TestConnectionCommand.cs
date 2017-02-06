using System;
using System.Windows;
using Chioy.Communication.Networking.Client.DB.DBHelper;
using Chioy.Communication.Networking.Client.DB.Models;
using Chioy.Communication.Networking.Common;

using KRNetWorkingTool.ViewModel;

namespace KRNetWorkingTool.Command
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
                DatabaseEnum databaseEnum = DataBaseSoft.TransDatabaseSoft(dbConfig.DatabaseSoft, dbConfig.IsAdvancedSetting);
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