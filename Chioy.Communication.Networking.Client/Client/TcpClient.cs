using Chioy.Communication.Networking.Client.TCP;
using Chioy.Communication.Networking.Common;
using Chioy.Communication.Networking.Interface;
using Chioy.Communication.Networking.Models.DTO;
using Chioy.Communication.Networking.Models.ReportMetadata;

namespace Chioy.Communication.Networking.Client.Client
{
    public class TcpClient<T> : BaseClient<T> where T : BaseCheckResult
    {
        private TCPClientManager<T> mgr;

        public event OnEventReceivedEventHandler MsgReceiveEvent;

        public override void ConfigClient(ProductType type, Protocol protocol)
        {
            base.ConfigClient(type, protocol);
            mgr = new TCPClientManager<T>();
            mgr.InitializeManager(type, Address.BaseAddress, Address.Port.ToString());
            mgr.MsgReceiveEvent += Mgr_MsgReceiveEvent;
        }

        private void Mgr_MsgReceiveEvent(ArgumentBase<string> arg)
        {
            MsgReceiveEvent?.Invoke(arg);
        }

        public override Patient_DTO GetPatient(string patientId)
        {
            return mgr.GetPatient(patientId);
        }

        public override KRResponse PostExamResult(ExamResultMetadata<T> result)
        {
            return mgr.PostExamResult(result);
        }
    }
}