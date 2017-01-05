using Chioy.Communication.Networking.Client.FTP;
using Chioy.Communication.Networking.Models.ReportMetadata;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Chioy.Communication.Networking.Common;

namespace Chioy.Communication.Networking.Client.Client
{
    public class FtpClient<T> : BaseClient<T> where T : BaseCheckResult
    {
        public EventHandler<UploadFileCompletedEventLibArgs> UploadFileCompleted;
        public EventHandler<UploadProgressChangedLibArgs> UploadProgressChanged;
        public EventHandler<DownloadFileCompletedEventLibArgs> DownloadFileCompleted;
        public EventHandler<DownloadProgressChangedLibArgs> DownloadProgressChanged;

        FtpHelper _helper = new FtpHelper();
        public FtpClient()
        {
            _protocol = Protocol.Ftp;
            _helper.UploadFileCompleted += _helper_UploadFileCompleted;
            _helper.UploadProgressChanged += _helper_UploadProgressChanged;
            _helper.DownloadFileCompleted += _helper_DownloadFileCompleted;
            _helper.DownloadProgressChanged += _helper_DownloadProgressChanged;
        }
        public override void ConfigClient(ProductType type, Protocol protocol)
        {
            try
            {
                base.ConfigClient(type, protocol);
                _helper.Host = Address.FTPAddress;
                _helper.UserName = Address.FTPUserName;
                _helper.Password = Address.FTPPassword;
            }
            catch (KRException ex)
            {
                throw new Exception("加载联网配置失败");
            }
        }

        public void UploadFile(string filename, string remoteFileName)
        {
            try
            {
                if (_helper != null)
                {
                    _helper.Upload(Address.FTPLocalDir, filename, Address.FTPRemoteDir, remoteFileName);
                }
            }
            catch (Exception ex)
            {

                throw new Exception("上传文件出错");
            }

        }

        public void UploadAsync(string filename, string remoteFileName, string localDir = null)
        {
            try
            {
                if (_helper != null)
                {
                    _helper.UploadAsync(Address.FTPLocalDir, filename, Address.FTPRemoteDir, remoteFileName);
                }
            }
            catch (Exception)
            {

                throw new Exception("上传文件出错");
            }

        }

        public void DownLoadFile(string filename, string remoteFileName)
        {
            try
            {
                if (_helper != null)
                {
                    _helper.Download(Address.FTPLocalDir, filename, Address.FTPRemoteDir, remoteFileName);
                }
            }
            catch (Exception)
            {

                throw new Exception("下载文件出错");
            }
        }

        public void DownloadAsync(string filename, string remoteFileName)
        {
            try
            {
                if (_helper != null)
                {
                    _helper.DownloadAsync(Address.FTPLocalDir, filename, Address.FTPRemoteDir, remoteFileName);
                }
            }
            catch (Exception)
            {

                throw new Exception("下载文件出错");
            }
        }

        private void _helper_DownloadProgressChanged(object sender, DownloadProgressChangedLibArgs e)
        {
            DownloadProgressChanged?.Invoke(sender, e);
        }

        private void _helper_DownloadFileCompleted(object sender, DownloadFileCompletedEventLibArgs e)
        {
            DownloadFileCompleted?.Invoke(sender, e);
        }

        private void _helper_UploadProgressChanged(object sender, UploadProgressChangedLibArgs e)
        {
            UploadProgressChanged?.Invoke(sender, e);
        }

        private void _helper_UploadFileCompleted(object sender, UploadFileCompletedEventLibArgs e)
        {
            UploadFileCompleted?.Invoke(sender, e);
        }
    }
}
