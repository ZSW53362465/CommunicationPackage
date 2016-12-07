using Chioy.Communication.Networking.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Chioy.Communication.Networking.Models;

namespace Chioy.Communication.Networking.Client
{
    public class WCFClientManager : ClientManager
    {
        private IKRService _proxy = null;
        public WCFClientManager()
        {

        }

        public  void DisconnectServer()
        {
            WCFClientEventCallback.Instance().UnRegistServices();
            WCFClientEventCallback.Instance().CommunicationEvent -= WCFClientManager_CommunicationEvent;
            WCFClientEventCallback.Instance().ExceptionEvent -= WCFClientManager_ExceptionEvent;
        }

        public  void InitializeManager()
        {
            WCFClientEventCallback.Instance().CommunicationEvent += WCFClientManager_CommunicationEvent;
            WCFClientEventCallback.Instance().ExceptionEvent += WCFClientManager_ExceptionEvent;
            WCFClientEventCallback.Instance().RegisterServices();
            _proxy = WCFClientEventCallback.Instance().KRService;
        }

        private void WCFClientManager_ExceptionEvent(Common.KRException ex)
        {
            HandleExceptionEvent(ex);
        }

        private void WCFClientManager_CommunicationEvent(object sender, DataEventArgs e)
        {
            HandleCommunicationEvent(sender, e.Data.ToString());
        }

        protected override void ReleaseManager()
        {
            WCFClientEventCallback.Instance().UnRegistServices();
            WCFClientEventCallback.Instance().CommunicationEvent -= WCFClientManager_CommunicationEvent;
            WCFClientEventCallback.Instance().ExceptionEvent -= WCFClientManager_ExceptionEvent;
            _proxy = null;
        }

        protected  UserInfo CreateNewUser(string name)
        {
            return _proxy.CreateNewUser(name);
        }

        protected  List<UserInfo> GetAllUsers()
        {
            return _proxy.GetAllUsers();
        }
    }
}
