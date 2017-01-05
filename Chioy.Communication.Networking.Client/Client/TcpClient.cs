using Chioy.Communication.Networking.Client.TCP;
using Chioy.Communication.Networking.Common;
using Chioy.Communication.Networking.Interface;
using Chioy.Communication.Networking.Models.DTO;
using Chioy.Communication.Networking.Models.ReportMetadata;
using System;

namespace Chioy.Communication.Networking.Client.Client
{
    public class TcpClient<T> : BaseClient<T> where T : BaseCheckResult
    {
        private TCPClientManager<T> mgr;

        public event OnEventReceivedEventHandler MsgReceiveEvent;

        public override void ConfigClient(ProductType type, Protocol protocol)
        {
            try
            {
                base.ConfigClient(type, protocol);
                mgr = new TCPClientManager<T>();
                mgr.InitializeManager(type, Address.BaseAddress, Address.Port.ToString());
                mgr.MsgReceiveEvent += Mgr_MsgReceiveEvent;
            }
            catch (Exception ex)
            {
                throw new Exception("配置联网信息失败");
            }
         
        }

        private void Mgr_MsgReceiveEvent(ArgumentBase<string> arg)
        {
            MsgReceiveEvent?.Invoke(arg);
        }

        public override Patient_DTO GetPatient(string patientId)
        {
            try
            {
                return mgr.GetPatient(patientId);

            }
            catch (System.Exception ex)
            {
                throw new System.Exception ("获取病人信息失败");
            }
        }

        public override KRResponse PostExamResult(ExamResultMetadata<T> result)
        {
            try
            {
                return mgr.PostExamResult(result);

            }
            catch (System.Exception ex)
            {
                throw new System.Exception("上传检查结果失败");
            }
        }
    }
}