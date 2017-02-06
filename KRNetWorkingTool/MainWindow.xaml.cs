using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Chioy.Communication.Networking.Client;
using Chioy.Communication.Networking.Client.DB;

using KRNetWorkingTool.ViewModel;
using MessageBox = System.Windows.MessageBox;

namespace KRNetWorkingTool
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        private void OnFinished(object sender, RoutedEventArgs e)
        {
            var vm = Resources["NetworkingViewModel"] as NetWorkingViewModel;

            if (vm == null)
            {
                return;
            }
            object s = _patientGrid.FindName("_cboField");


            var config = new KRNetworkingConfig();
            config.NetType = vm.NetworkingConfig.NetType;
            config.DatabaseConfigModel = vm.DatabaseConfigModel;
            config.PatientMapModel = vm.PatientMapModel;
            config.ReportSaveModel = vm.ReportSaveModel;
            config.DataCallBackModel = vm.DataCallBackModel;
            config.HttpConfigModel = vm.HttpConfigModel;
            config.WcfConfigModel = vm.WcfConfigModel;
            config.Save();
        }

        private void Wizard_OnHelp(object sender, RoutedEventArgs e)
        {
            try
            {
                string startup = System.Windows.Forms.Application.StartupPath;
                var info = new DirectoryInfo(startup);
                string path = info.Parent.FullName;
                string root = info.Root.Name.Substring(0,2);
                string bat = root + "\r\n" + path + "\\Conf\\NetWorkHelp.docx";
                File.WriteAllText("openhelp.bat", bat, Encoding.Default);
                ProcessStartInfo StartInfo = new ProcessStartInfo("openhelp.bat");
                StartInfo.UseShellExecute = false;
                StartInfo.CreateNoWindow = true;
                Process p = new Process();
                p.StartInfo = StartInfo;
                
                p.Start();//启动程序   
                p.WaitForExit();//等待程序执行完退出进程
                p.Close();

            }
            catch (Exception exception)
            {
                MessageBox.Show("获取帮助失败！ " + exception.Message);
            }
            
        }

        private void TextBox_MouseDown(object sender, MouseButtonEventArgs e)
        {
          
        }

        private void txt_ForlderPath_GotFocus(object sender, RoutedEventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            if (fbd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                txt_ForlderPath.Text = fbd.SelectedPath;
            }
        }
    }
}
