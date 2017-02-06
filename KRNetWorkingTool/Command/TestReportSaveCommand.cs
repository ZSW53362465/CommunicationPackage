using System;
using System.IO;
using System.Windows;
using Chioy.Communication.Networking.Client.DB.Models;
using Chioy.Communication.Networking.Client.FTP;
using KRNetWorkingTool.ViewModel;

namespace KRNetWorkingTool.Command
{
    public class TestReportSaveCommand : CommandBase
    {
        public TestReportSaveCommand(NetWorkingViewModel p_networkingVM)
            : base(p_networkingVM)
        {
        }

        public override void Execute(object parameter)
        {
            ReportSaveModel reprotConfig = NetworkingViewModel.ReportSaveModel;

            if (reprotConfig.ReportSaveType == "文件夹")
            {
                try
                {
                    string path = Path.Combine(reprotConfig.DirAddress, string.Format("test.{0}", reprotConfig.ImageExt));

                    var fs = new FileStream(path, FileMode.Create);
                    fs.Close();

                    File.Delete(path);
                }
                catch (IOException exception)
                {
                    //ioex.
                    MessageBox.Show(exception.Message);
                    return;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error");
                    return;
                }

                MessageBox.Show("文件地址测试成功");
            }
            else if (reprotConfig.ReportSaveType.ToUpper() == "FTP")
            {
                try
                {
                    string filePath = string.Format("Test{0}.{1}", DateTime.Now.ToString("HHmmss"),
                                                    reprotConfig.ImageExt);
                    var fs = new FileStream(filePath, FileMode.Create);
                    fs.Close();

                    var ftpHelper = new FtpHelper(reprotConfig.FtpAdresse, reprotConfig.FtpUser,
                        reprotConfig.FtpPassword, 21);
                    var currentDateTime = DateTime.Now.ToLongDateString();
                    ftpHelper.Upload(filePath, currentDateTime, filePath);

                    if (ftpHelper.Delete(currentDateTime+ "\\" + filePath, "DELE"))
                    {
                        ftpHelper.Delete(currentDateTime, "RMD");

                        MessageBox.Show("Ftp地址及用户测试通过！");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error");
                }
            }
        }
    }
}