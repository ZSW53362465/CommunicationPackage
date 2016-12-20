using Chioy.Communication.Networking.Client.FTP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chioy.Communication.Networking.Client.Client
{
    public class FtpClient : BaseClient
    {
        public EventHandler<UploadFileCompletedEventLibArgs> UploadFileCompleted;
        public EventHandler<UploadProgressChangedLibArgs> UploadProgressChanged;
        public EventHandler<DownloadFileCompletedEventLibArgs> DownloadFileCompleted;
        public EventHandler<DownloadProgressChangedLibArgs> DownloadProgressChanged;

        FtpHelper _helper = new FtpHelper();
        public FtpClient()
        {
            _protocol = Protocol.FTP;
            _helper.UploadFileCompleted += _helper_UploadFileCompleted;
            _helper.UploadProgressChanged += _helper_UploadProgressChanged;
            _helper.DownloadFileCompleted += _helper_DownloadFileCompleted;
            _helper.DownloadProgressChanged += _helper_DownloadProgressChanged;
        }
        public override void ConfigClient()
        {
            base.ConfigClient();
            _helper.Host = Address.FTPAddress;
            _helper.UserName = Address.FTPUserName;
            _helper.Password = Address.FTPPassword;
        }

        public void UploadFile(string filename, string remoteFileName)
        {
            if (_helper != null)
            {
                _helper.Upload(Address.FTPLocalDir, filename, Address.FTPRemoteDir, remoteFileName);
            }
        }

        public void UploadAsync(string filename, string remoteFileName, string localDir = null)
        {
            if (_helper != null)
            {
                _helper.UploadAsync(Address.FTPLocalDir, filename, Address.FTPRemoteDir, remoteFileName);
            }
        }

        public void DownLoadFile(string filename, string remoteFileName)
        {
            if (_helper != null)
            {
                _helper.Download(Address.FTPLocalDir, filename, Address.FTPRemoteDir, remoteFileName);
            }
        }

        public void DownloadAsync(string filename, string remoteFileName)
        {
            if (_helper != null)
            {
                _helper.DownloadAsync(Address.FTPLocalDir, filename, Address.FTPRemoteDir, remoteFileName);
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
