using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using Chioy.Communication.Networking.Client.HTTP;
using Chioy.Communication.Networking.Common;
using Chioy.Communication.Networking.Models.DTO;
using Chioy.Communication.Networking.Models.ReportMetadata;

namespace Chioy.Communication.Networking.Client.Client
{
    public class BaseClient<T> : IDisposable where T : BaseCheckResult
    {
        private string _configPath = string.Format("{0}\\{1}\\NetworkConfig.ini", Path.GetFullPath(".."), "conf");
        //private const string KRNetworkingConfig = @"..\Conf\KRNetworkingConfig.xml";

        public const string Section_NetConfig = "NET_CONFIG";
        public const string Key_BassAddress = "BassAddress";
        public const string Key_Port = "Port";
        public const string Section_BusinessConfig = "BUSINESS";
        public const string Key_GetPatientUrl = "GetPatientUrl";

        public const string Key_GetCheckResultUrl = "GetCheckResult";
        public const string Key_PostCheckResultUrl = "PostCheckResultUrl";
        public const string Key_PostResultParamName = "PostParameter";
        public const string Key_PostOperatorUrl = "PostOperatorUrl";

        public const string Section_FtpConfig = "FTP_CONFIG";
        public const string Key_FtpAddress = "Address";
        public const string Key_FtpUserName = "UserName";
        public const string Key_FtpPwd = "Pwd";
        public const string Key_FtpRemoteDir = "RemoteDir";
        public const string Key_FtpLocalDir = "LocalDir";

        protected Protocol _protocol;
        protected ProductType _productType;
        //protected AddressInfo Address = null;
        protected BindingType _bindingType;
        public static KRNetworkingConfig Config
        {
            get { return KRNetworkingConfig.Config; }
        }

        public BaseClient()
        {
            ConfigClient();
        }

        public Protocol Protocol
        {
            get { return _protocol; }
        }

        public static void SetupConfig()
        {
            KRNetworkingConfig.Load();
        }

        protected virtual void ConfigClient()
        {
            _productType = GetTypeByT();
            Trace.WriteLine("开始读取联网配置信息");



            //Address = new AddressInfo(_protocol);
            //var baseAddressSb = new StringBuilder(32);
            //var ftpAddressSb = new StringBuilder(100);
            //var ftpUserNameSb = new StringBuilder(32);
            //var ftpUserPwdSb = new StringBuilder(32);
            //var ftpRemoteDirSb = new StringBuilder(100);
            //var ftpLocalDirSb = new StringBuilder(100);
            //var getPatientUrlSb = new StringBuilder(100);
            //var getCheckResultUrlSb = new StringBuilder(100);
            //var postCheckResultUrlSb = new StringBuilder(100);
            //var postOperatorUrlSb = new StringBuilder(100);
            //var postResultParameterNameSb = new StringBuilder(50);
            //CommunicationHelper.GetPrivateProfileString(Section_NetConfig, Key_BassAddress, "", baseAddressSb, 32, _configPath);
            //CommunicationHelper.GetPrivateProfileString(Section_NetConfig, Key_BassAddress, "", baseAddressSb, 32, _configPath);
            //CommunicationHelper.GetPrivateProfileString(Section_FtpConfig, Key_FtpAddress, "", ftpAddressSb, 100, _configPath);
            //CommunicationHelper.GetPrivateProfileString(Section_FtpConfig, Key_FtpUserName, "", ftpUserNameSb, 32, _configPath);
            //CommunicationHelper.GetPrivateProfileString(Section_FtpConfig, Key_FtpPwd, "", ftpUserPwdSb, 32, _configPath);
            //CommunicationHelper.GetPrivateProfileString(Section_FtpConfig, Key_FtpRemoteDir, "", ftpRemoteDirSb, 100, _configPath);
            //CommunicationHelper.GetPrivateProfileString(Section_FtpConfig, Key_FtpLocalDir, "", ftpLocalDirSb, 100, _configPath);
            //CommunicationHelper.GetPrivateProfileString(Section_BusinessConfig, Key_GetPatientUrl, "", getPatientUrlSb, 100, _configPath);
            //CommunicationHelper.GetPrivateProfileString(Section_BusinessConfig, Key_GetCheckResultUrl, "", getCheckResultUrlSb, 100, _configPath);
            //CommunicationHelper.GetPrivateProfileString(Section_BusinessConfig, Key_PostCheckResultUrl, "", postCheckResultUrlSb, 100, _configPath);
            //CommunicationHelper.GetPrivateProfileString(Section_BusinessConfig, Key_PostOperatorUrl, "", postOperatorUrlSb, 100, _configPath);
            //CommunicationHelper.GetPrivateProfileString(Section_BusinessConfig, Key_PostResultParamName, "",
            // postResultParameterNameSb, 50, _configPath);


            //Address.Port = CommunicationHelper.GetPrivateProfileInt(Section_NetConfig, Key_Port, -1, _configPath);
            //Trace.WriteLine("联网配置信息读取结束");
            //if (File.Exists(KRNetworkingConfig))
            //{
            //    Trace.WriteLine("存在默认联网配置文件，读取FTP配置信息");
            //    var doc = new XmlDocument();
            //    doc.Load(KRNetworkingConfig);
            //    var ftpAddressNode = doc.SelectSingleNode("KRNetworkingConfig/ReportSaveModel/FtpAdresse");
            //    if (ftpAddressNode != null)
            //        Address.FTPAddress = ftpAddressNode.InnerText;
            //    var ftpUser = doc.SelectSingleNode("KRNetworkingConfig/ReportSaveModel/FtpUser");
            //    if (ftpUser != null)
            //        Address.FTPUserName = ftpUser.InnerText;
            //    var ftpPwd = doc.SelectSingleNode("KRNetworkingConfig/ReportSaveModel/FtpPassword");
            //    if (ftpPwd != null)
            //        Address.FTPPassword = ftpPwd.InnerText;


            //    if (string.IsNullOrEmpty(Address.FTPUserName))
            //    {
            //        Address.FTPUserName = ftpUserNameSb.ToString();
            //    }
            //    if (string.IsNullOrEmpty(Address.FTPAddress))
            //    {
            //        Address.FTPAddress = ftpAddressSb.ToString();
            //    }
            //    if (string.IsNullOrEmpty(Address.FTPPassword))
            //    {
            //        Address.FTPPassword = ftpUserPwdSb.ToString();
            //    }
            //    Address.FTPRemoteDir = ftpRemoteDirSb.ToString().Trim();
            //    Address.FTPLocalDir = ftpLocalDirSb.ToString().Trim();
            //    Trace.WriteLine(string.Format("FTP配置信息:ftp address:{0}, ftp username{1}, ftp password{2}", Address.FTPAddress, Address.FTPUserName, Address.FTPPassword));
            //}

            //Address.BaseAddress = baseAddressSb.ToString().Trim();
            //Address.Route_Patient = getPatientUrlSb.ToString().Trim();
            //Address.Route_Get_CheckResult = getCheckResultUrlSb.ToString().Trim();
            //Address.Route_Post_CheckResult = postCheckResultUrlSb.ToString().Trim();
            //Address.Route_Operator = postOperatorUrlSb.ToString().Trim();
            //Address.PostResultParamter = postResultParameterNameSb.ToString();

            ////if (!IsValidIp())
            ////{
            ////    throw new KRException("BaseClient.ConfigClient", "配置文件出错", _configPath + "文件中的 BassAddress格式不对，格式应为192.168.0.1,当前错误会导致联网出现问题");
            ////}
            //if (!IsValidPort())
            //{
            //    throw new KRException("BaseClient.ConfigClient", "配置文件出错", _configPath + "文件中的 Port格式不对，格式应为大于0小于65535的整数，当前错误会导致联网出现问题");
            //}
            //Address.ConfigBusinessAddress();
            //Trace.WriteLine(string.Format("配置信息:GetPatientUrl:{0}, PostExamResult:{1}", Address.GetPatientUrl, Address.GetCheckResultUrl));
        }



        public virtual Patient_DTO GetPatient(string patientId)
        { return new Patient_DTO(); }

        public virtual KRResponse PostExamResult(ExamResultMetadata<T> result) { return null; }

        public virtual KRResponse PostOperator(Operator_DTO op) { return null; }


        public virtual void RefreshNewConfig(AddressInfo info)
        {
            //Address = (AddressInfo)info.Clone();
            //Address.ConfigBusinessAddress();
        }

        public void Dispose()
        {

        }
        //private bool IsValidIp()
        //{
        //    if (string.IsNullOrEmpty(Address.BaseAddress))
        //    {
        //        return true;
        //    }
        //    return Address.BaseAddress.ToLower() == "localhost" || Regex.IsMatch(Address.BaseAddress, @"^(d{1,2}|1dd|2[0-4]d|25[0-5]).(d{1,2}|1dd|2[0-4]d|25[0-5]).(d{1,2}|1dd|2[0-4]d|25[0-5]).(d{1,2}|1dd|2[0-4]d|25[0-5])$");
        //}

        //private bool IsValidPort()
        //{
        //    return Address.Port < 65535 && Address.Port > 0;
        //}

        private ProductType GetTypeByT()
        {
            ProductType type = ProductType.BMD;
            if (typeof(T) == typeof(BMDCheckResult))
            {
                type = ProductType.BMD;
            }
            else if (typeof(T) == typeof(APIPWVCheckResult))
            {
                type = ProductType.VBP9;
            }
            return type;
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
        public string PostResultParamter { get; set; }

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
                case Protocol.Ftp:
                    break;
                case Protocol.Http:
                    address = string.Format("http://{0}:{1}/{2}", BaseAddress, Port.ToString(), serviceType);
                    break;
                case Protocol.DB:
                    break;
                case Protocol.Wcftcp:
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
                Route_Post_CheckResult = Route_Post_CheckResult,
                PostResultParamter = PostResultParamter
            };
        }
    }
}
