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
        WCF,
        Http
    }
    public enum Protocol
    {
        WebService,
        FTP,
        Http,
        DB,
        WCFTCP
    }

    public abstract class ClientProxy : IDisposable
    {
        BaseClient _client;

        public void IntManager(Protocol protocol)
        {

        }

        public Patient_DTO GetPatient(string patientId)
        {
            return _client.GetPatient(patientId);
        }
        public KRResponse SendExamResult(ExamResultMetadata<BaseCheckResult> result)
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

        protected abstract void ReleaseManager();

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
