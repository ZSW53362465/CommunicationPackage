using Chioy.Communication.Networking.Common;
using Chioy.Communication.Networking.Models.DTO;
using Chioy.Communication.Networking.Models.ReportMetadata;
using System;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace Chioy.Communication.Networking.Client
{
    public class BaseClient : IDisposable
    {
        private string _configPath = string.Format("{0}\\{1}\\NetworkConfig.ini", Path.GetFullPath(".."), "conf");


        public const string Section_NetConfig = "NET_CONFIG";
        public const string Key_BassAddress = "BassAddress";
        public const string Key_Port = "Port";
        public const string Section_BusinessConfig = "BUSINESS";
        public const string Key_GetPatientUrl = "GetPatientUrl";
        public const string Key_GetCheckResultUrl = "GetCheckResult";
        public const string Key_PostCheckResultUrl = "PostCheckResultUrl";
        public const string Key_PostOperatorUrl = "PostOperatorUrl";

        public const string Section_FtpConfig = "FTP_CONFIG";
        public const string Key_FtpAddress = "Address";
        public const string Key_FtpUserName = "UserName";
        public const string Key_FtpPwd = "Pwd";
        public const string Key_FtpRemoteDir = "RemoteDir";
        public const string Key_FtpLocalDir = "LocalDir";

        protected Protocol _protocol;

        protected AddressInfo Address = null;
        public virtual void ConfigClient()
        {
            Address = new AddressInfo(_protocol);
            var baseAddressSB = new StringBuilder(32);
            var ftpAddressSB = new StringBuilder(100);
            var ftpUserNameSB = new StringBuilder(32);
            var ftpUserPwdSB = new StringBuilder(32);
            var ftpRemoteDirSB = new StringBuilder(100);
            var ftpLocalDirSB = new StringBuilder(100);
            var GetPatientUrlSB = new StringBuilder(100);
            var GetCheckResultUrlSB = new StringBuilder(100);
            var PostCheckResultUrlSB = new StringBuilder(100);
            var PostOperatorUrlSB = new StringBuilder(100);

            CommunicationHelper.GetPrivateProfileString(Section_NetConfig, Key_BassAddress, "", baseAddressSB, 32, _configPath);
            CommunicationHelper.GetPrivateProfileString(Section_NetConfig, Key_BassAddress, "", baseAddressSB, 32, _configPath);
            CommunicationHelper.GetPrivateProfileString(Section_FtpConfig, Key_FtpAddress, "", ftpAddressSB, 100, _configPath);
            CommunicationHelper.GetPrivateProfileString(Section_FtpConfig, Key_FtpUserName, "", ftpUserNameSB, 32, _configPath);
            CommunicationHelper.GetPrivateProfileString(Section_FtpConfig, Key_FtpPwd, "", ftpUserPwdSB, 32, _configPath);
            CommunicationHelper.GetPrivateProfileString(Section_FtpConfig, Key_FtpRemoteDir, "", ftpRemoteDirSB, 100, _configPath);
            CommunicationHelper.GetPrivateProfileString(Section_FtpConfig, Key_FtpLocalDir, "", ftpLocalDirSB, 100, _configPath);
            CommunicationHelper.GetPrivateProfileString(Section_BusinessConfig, Key_GetPatientUrl, "", GetPatientUrlSB, 100, _configPath);
            CommunicationHelper.GetPrivateProfileString(Section_BusinessConfig, Key_GetCheckResultUrl, "", GetCheckResultUrlSB, 100, _configPath);
            CommunicationHelper.GetPrivateProfileString(Section_BusinessConfig, Key_PostCheckResultUrl, "", PostCheckResultUrlSB, 100, _configPath);
            CommunicationHelper.GetPrivateProfileString(Section_BusinessConfig, Key_PostOperatorUrl, "", PostOperatorUrlSB, 100, _configPath);

            Address.Port = CommunicationHelper.GetPrivateProfileInt(Section_NetConfig, Key_Port, 9999, _configPath);
            Address.BaseAddress = baseAddressSB.ToString().Trim();
            Address.FTPAddress = ftpAddressSB.ToString().Trim();
            Address.FTPUserName = ftpUserNameSB.ToString().Trim();
            Address.FTPPassword = ftpUserPwdSB.ToString().Trim();
            Address.FTPRemoteDir = ftpRemoteDirSB.ToString().Trim();
            Address.FTPLocalDir = ftpLocalDirSB.ToString().Trim();
            Address.Route_Patient = GetPatientUrlSB.ToString().Trim();
            Address.Route_Get_CheckResult = GetCheckResultUrlSB.ToString().Trim();
            Address.Route_Post_CheckResult = PostCheckResultUrlSB.ToString().Trim();
            Address.Route_Operator = PostOperatorUrlSB.ToString().Trim();

            if (IsValidIp())
            {
                throw new KRException("BaseClient.ConfigClient", "配置文件出错", _configPath + "文件中的 BassAddress格式不对，格式应为192.168.0.1");
            }
            if (IsValidPort())
            {
                throw new KRException("BaseClient.ConfigClient", "配置文件出错", _configPath + "文件中的 Port格式不对，格式应为大于0小于65535的整数");
            }
            Address.ConfigBusinessAddress();
        }

        public virtual Patient_DTO GetPatient(string patientId) { return new Patient_DTO(); }

        public virtual KRResponse PostExamResult(ExamResultMetadata<BaseCheckResult> result) { return null; }

        public virtual KRResponse PostOperator(Operator_DTO op) { return null; }


        public virtual void RefreshNewConfig(AddressInfo info)
        {
            Address = (AddressInfo)info.Clone();
            Address.ConfigBusinessAddress();
        }

        public void Dispose()
        {

        }
        private bool IsValidIp()
        {
            return Regex.IsMatch(Address.BaseAddress, @"^(d{1,2}|1dd|2[0-4]d|25[0-5]).(d{1,2}|1dd|2[0-4]d|25[0-5]).(d{1,2}|1dd|2[0-4]d|25[0-5]).(d{1,2}|1dd|2[0-4]d|25[0-5])$");
        }

        private bool IsValidPort()
        {
            if (Address.Port >= 65535 || Address.Port <= 0)
            {
                return false;
            }
            return true;
        }
    }
    public class AddressInfo : ICloneable
    {
        Protocol _protocol;
        public AddressInfo(Protocol protocol)
        {
            _protocol = protocol;
        }
        public string BaseAddress { get; set; }

        public int Port { get; set; }

        public string FTPAddress { get; set; }
        public string FTPUserName { get; set; }
        public string FTPPassword { get; set; }
        public string FTPRemoteDir { get; set; }
        public string FTPLocalDir { get; set; }
        public string Route_Patient { get; set; }
        public string Route_Operator { get; set; }
        public string Route_Get_CheckResult { get; internal set; }
        public string Route_Post_CheckResult { get; internal set; }

        public string GetPatientUrl { get; set; }

        public string GetCheckResultUrl { get; set; }

        public string PostCheckResultUrl { get; set; }

        public string PostOperatorUrl { get; set; }


        public void ConfigBusinessAddress()
        {
            GetPatientUrl = BuildAddress(Route_Patient);
            GetCheckResultUrl = BuildAddress(Route_Get_CheckResult);
            PostCheckResultUrl = BuildAddress(Route_Post_CheckResult);
            PostOperatorUrl = BuildAddress(Route_Operator);
        }


        private string BuildAddress(string serviceType)
        {
            string address = string.Empty;
            switch (_protocol)
            {
                case Protocol.WebService:
                    address = string.Format("http://{0}:{1}/{2}?wsdl", BaseAddress, Port.ToString(), serviceType);
                    break;
                case Protocol.FTP:
                    break;
                case Protocol.Http:
                    address = string.Format("http://{0}:{1}/{2}", BaseAddress, Port.ToString(), serviceType);
                    break;
                case Protocol.DB:
                    break;
                case Protocol.WCFTCP:
                    address = string.Format("net.tcp://{0}:{1}/{2}", BaseAddress, Port.ToString(), serviceType);
                    break;
                default:
                    break;
            }
            return address;
        }

        public override string ToString()
        {
            return string.Format("{0}:{1}", BaseAddress, Port.ToString());
        }

        public object Clone()
        {
            return new AddressInfo(_protocol)
            {
                BaseAddress = BaseAddress,
                Route_Patient = Route_Patient,
                GetCheckResultUrl = GetCheckResultUrl,
                PostCheckResultUrl = PostCheckResultUrl,
                Route_Operator = Route_Operator,
                FTPAddress = FTPAddress,
                FTPLocalDir = FTPLocalDir,
                FTPPassword = FTPPassword,
                FTPRemoteDir = FTPRemoteDir,
                FTPUserName = FTPUserName,
                Port = Port,
                GetPatientUrl = GetPatientUrl,
                PostOperatorUrl = PostOperatorUrl,
                Route_Get_CheckResult = Route_Get_CheckResult,
                Route_Post_CheckResult = Route_Post_CheckResult
            };
        }
    }
}
