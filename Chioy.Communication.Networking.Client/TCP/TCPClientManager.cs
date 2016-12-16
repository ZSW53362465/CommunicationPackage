﻿using Chioy.Communication.Networking.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Chioy.Communication.Networking.Models;
using Chioy.Communication.Networking.Common;

namespace Chioy.Communication.Networking.Client
{
    public class TCPClientManager : ClientManager
    {
        private IService _proxy = null;
        private IEventService _heartProxy = null;
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
            WCFClientEventCallback.Instance().CommunicationEvent += WCFClientManager_CommunicationEvent;
            WCFClientEventCallback.Instance().ExceptionEvent += WCFClientManager_ExceptionEvent;
            WCFClientEventCallback.Instance().RegisterServices(type, baseAddress, port);
            _proxy = WCFClientEventCallback.Instance().KRService;
            _heartProxy = WCFClientEventCallback.Instance().KRHeartService;
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

        public DateTime Ping()
        {
            return _heartProxy.Ping();
        }

        public UserInfo CreateNewUser(string name)
        {
            var proxy = _proxy as IBMDService;
            return proxy.CreateNewUser(name);
        }

        public List<UserInfo> GetAllUsers()
        {
            var proxy = _proxy as IBMDService;
            return proxy.GetAllUsers();
        }
    }
}