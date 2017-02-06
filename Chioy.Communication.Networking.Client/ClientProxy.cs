using Chioy.Communication.Networking.Client.Client;
using Chioy.Communication.Networking.Common;
using Chioy.Communication.Networking.Models;
using Chioy.Communication.Networking.Models.DTO;
using Chioy.Communication.Networking.Models.ReportMetadata;
using System;
using System.Diagnostics;
using Chioy.Communication.Networking.Client.HTTP;

namespace Chioy.Communication.Networking.Client
{
    public class ClientProxy<T> : IDisposable where T : BaseCheckResult
    {
        BaseClient<T> _client;
        public DBClient<T> DataBaseClientObj
        {
            get { return _client as DBClient<T>; }
        }

        public BaseClient<T> ClientObj
        {
            get { return _client; }
        }

        public ClientProxy()
        {
            BaseClient<T>.SetupConfig();
            ConfigClient();
        }

        private void ConfigClient()
        {
            switch (BaseClient<T>.Config.NetType)
            {
                case "WebService":
                    _client = new WebServiceClient<T>();
                    break;
                case "Http":
                case "WCF-Http":
                    _client = new HttpClient<T>();
                    break;
                case "DB":
                    _client = new DBClient<T>();
                    break;
                case "WCF-Tcp":
                    _client = new TcpClient<T>();
                    break;
                default:
                    break;
            }
            if (_client == null)
            {
                throw new ArgumentNullException("BaseClient", "请在ClientProxy初始化的时候传入相对应的非空Client对象");

            }
        }

        public bool EnableNetWorking
        {
            get
            {
                if (_client?.Protocol == Protocol.DB)
                {
                    var dbClient = _client as DBClient<T>;
                    if (dbClient?.NetworkConfig != null)
                    {
                        if (dbClient.NetworkConfig.ReportSaveModel.ReportSaveType == "无" && dbClient.NetworkConfig.DataCallBackModel.CallbackType == "无")
                        {
                            return false;
                        }
                    }
                }
                return true;
            }
        }

        public Patient_DTO GetPatient(string patientId)
        {
            try
            {
                return _client.GetPatient(patientId);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public KRResponse SendExamResult(ExamResultMetadata<T> result)
        {
            try
            {
                return _client.PostExamResult(result);
            }
            catch (Exception ex)
            {
                
                throw ex;
            }
        }

        public KRResponse SendOperator(Operator_DTO op)
        {
            return _client.PostOperator(op);
        }


        public event EventHandler<DataEventArgs> CommunicationEvent;

        public event KRExceptionEventHandler ExceptionEvent;

        protected void HandleCommunicationEvent(object sender, string args)
        {
            CommunicationEvent?.Invoke(sender, new DataEventArgs(args));
        }

        protected void HandleExceptionEvent(KRException ex)
        {
            ExceptionEvent?.Invoke(ex);
        }

        private void ThrowException(string method, string description, string message)
        {
            Trace.TraceError(string.Format("[{0}]:{1}   {2}"), method, description, message);
            ExceptionEvent?.Invoke(new KRException(method, description, message));
        }

        public void Dispose()
        {
            _client.Dispose();
        }
    }
}
