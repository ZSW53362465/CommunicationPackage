using System;
using System.Collections.Generic;
using Chioy.Communication.Networking.Common;
using Chioy.Communication.Networking.Interface;
using Chioy.Communication.Networking.Interface.ProductInterface.TCP;
using Chioy.Communication.Networking.Models;
using Chioy.Communication.Networking.Models.DTO;
using Chioy.Communication.Networking.Models.ReportMetadata;

namespace Chioy.Communication.Networking.Client.TCP
{
    public class TCPClientManager<T> where T : BaseCheckResult
    {
        private IService _proxy = null;
        private IEventService _heartProxy = null;
        private ProductType _type;
        public event OnEventReceivedEventHandler MsgReceiveEvent;
        public TCPClientManager()
        {
        }

        public void DisconnectServer()
        {
            WCFClientEventCallback.Instance().UnRegistServices();
            WCFClientEventCallback.Instance().CommunicationEvent -= WCFClientManager_CommunicationEvent;
            WCFClientEventCallback.Instance().ExceptionEvent -= WCFClientManager_ExceptionEvent;
        }

        public void InitializeManager(ProductType type, string baseAddress, string port)
        {
            _type = type;
            WCFClientEventCallback.Instance().CommunicationEvent += WCFClientManager_CommunicationEvent;
            WCFClientEventCallback.Instance().ExceptionEvent += WCFClientManager_ExceptionEvent;
            WCFClientEventCallback.Instance().OnEventReceivedEvent += TCPClientManager_OnEventReceivedEvent;
            WCFClientEventCallback.Instance().RegisterServices(type, baseAddress, port);
            _proxy = WCFClientEventCallback.Instance().KRService;
            _heartProxy = WCFClientEventCallback.Instance().KRHeartService;
        }

        private void TCPClientManager_OnEventReceivedEvent(ArgumentBase<string> arg)
        {
            MsgReceiveEvent?.Invoke(arg);
        }

        private void WCFClientManager_ExceptionEvent(Common.KRException ex)
        {
            //HandleExceptionEvent(ex);
        }

        private void WCFClientManager_CommunicationEvent(object sender, DataEventArgs e)
        {
            //HandleCommunicationEvent(sender, e.Data.ToString());
        }

        //protected override void ReleaseManager()
        //{
        //    WCFClientEventCallback.Instance().UnRegistServices();
        //    WCFClientEventCallback.Instance().CommunicationEvent -= WCFClientManager_CommunicationEvent;
        //    WCFClientEventCallback.Instance().ExceptionEvent -= WCFClientManager_ExceptionEvent;
        //    _proxy = null;
        //}

        public DateTime Ping()
        {
            return _heartProxy.Ping();
        }

        public UserInfo CreateNewUser(string name)
        {
            //var proxy = _proxy as IBMDService;
            //return _proxy.CreateNewUser(name);
            return null;
        }

        public List<UserInfo> GetAllUsers()
        {
            //var proxy = _proxy as IBMDService;
            //return _proxy.GetAllUsers();
            return null;
        }

        public Patient_DTO GetPatient(string patientId)
        {
            switch (this._type)
            {
                case ProductType.BMD:
                    var bmdTcpService = _proxy as IBMDTcpService;
                    if (bmdTcpService != null) return bmdTcpService.GetPatient(patientId);
                    break;
                case ProductType.KRTCD:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            return null;
        }

        public KRResponse PostExamResult(ExamResultMetadata<T> result)
        {
            switch (this._type)
            {
                case ProductType.BMD:
                    var bmdTcpService = _proxy as IBMDTcpService;
                    var jsonStr = CommunicationHelper.SerializeObjToJsonStr<ExamResultMetadata<T>>(result);
                    if (bmdTcpService != null) return bmdTcpService.PostExamResult(jsonStr);
                    break;
                case ProductType.KRTCD:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            return new KRResponse() { Status = "FAIL", Msg = "" };
        }
    }
}
