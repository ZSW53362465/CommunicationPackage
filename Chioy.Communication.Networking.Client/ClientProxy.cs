using Chioy.Communication.Networking.Client.Client;
using Chioy.Communication.Networking.Common;
using Chioy.Communication.Networking.Models;
using Chioy.Communication.Networking.Models.DTO;
using Chioy.Communication.Networking.Models.ReportMetadata;
using System;
using System.Diagnostics;

namespace Chioy.Communication.Networking.Client
{
    public enum CommunicationType
    {
        Wcf,
        Http
    }
   

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

        public ClientProxy(BaseClient<T> client)
        {
            _client = client;
        }

        public void ConfigClient(ProductType type, Protocol protocol)
        {

            switch (protocol)
            {
                case Protocol.WebService:
                    _client = new WebServiceClient<T>();
                    break;
                case Protocol.Ftp:
                    break;
                case Protocol.Http:
                    _client = new HttpClient<T>();
                    break;
                case Protocol.DB:
                    _client = new DBClient<T>();
                    break;
                case Protocol.Wcftcp:
                    _client = new TcpClient<T>();
                    break;
                default:
                    break;
            }
            if (_client != null)
            {
                _client.ConfigClient(type, protocol);
            }
            else
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
                    if (dbClient?.Config != null)
                    {
                        if (dbClient.Config.ReportSaveModel.ReportSaveType == "无" && dbClient.Config.DataCallBackModel.CallbackType == "无")
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
            return _client.GetPatient(patientId);
        }
        public KRResponse SendExamResult(ExamResultMetadata<T> result)
        {
            return _client.PostExamResult(result);
        }

        public KRResponse SendOperator(Operator_DTO op)
        {
            return _client.PostOperator(op);
        }

        public ClientProxy()
        {
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

        protected virtual void ReleaseManager() { }

        private void ThrowException(string method, string description, string message)
        {
            Trace.TraceError(string.Format("[{0}]:{1}   {2}"), method, description, message);
            ExceptionEvent?.Invoke(new KRException(method, description, message));
        }

        public void Dispose()
        {
            ReleaseManager();
        }
    }
}
