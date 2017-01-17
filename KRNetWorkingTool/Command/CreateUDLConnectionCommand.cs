using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Windows;
using Chioy.Communication.Networking.Common;
using KRNetWorkingTool.ViewModel;

namespace KRNetWorkingTool.Command
{
    public class CreateUDLConnectionCommand : CommandBase
    {
        public CreateUDLConnectionCommand(NetWorkingViewModel p_networkingVM)
            : base(p_networkingVM)
        {
        }

        public override void Execute(object parameter)
        {
            string path = AppDomain.CurrentDomain.SetupInformation.ApplicationBase;

            string filePath = Path.Combine(path, "test.udl");

            using (var fs = new FileStream(filePath, FileMode.OpenOrCreate))
            {
            }

            Process process = Process.Start(filePath);

            process.WaitForExit();

            try
            {
                var key = new StringBuilder();

                CommunicationHelper.GetPrivateProfileString("oledb", null, null, key, 16, filePath);

                int size = 4*255;
                var sb = new StringBuilder(size);
                IniHelper.GetPrivateProfileString("oledb", key.ToString(), null, sb, size, filePath);

                NetworkingViewModel.DatabaseConfigModel.AdvancedConnectionString = key + " = " + sb;

                if (sb.Length == 0)
                {
                    MessageBox.Show("UDL 配置不正确！请重新设置！");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}