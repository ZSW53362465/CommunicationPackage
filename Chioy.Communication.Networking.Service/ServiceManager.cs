using Chioy.Communication.Networking.Common;
using Chioy.Communication.Networking.Interface;
using Chioy.Communication.Networking.Models;
using System;
using System.ServiceModel;
using System.Threading.Tasks;

namespace Chioy.Communication.Networking.Service
{
    public class ServiceManager
    {

        public event EventHandler<DataEventArgs> NewClientSubscribed;
        public event EventHandler<DataEventArgs> ClientLost;
        public event KRExceptionEventHandler ExceptionEvent;
        ServiceHost _krHost = null;
        ServiceHost _krEventHost = null;

        private volatile static ServiceManager _instance = null;

        private static readonly object lockHelper = new object();

        private ServiceManager() { }
        public static ServiceManager Instance()
        {
            if (_instance == null)
            {
                lock (lockHelper)
                {
                    if (_instance == null)
                        _instance = new ServiceManager();
                }
            }
            return _instance;
        }

        public void StartKRSvc()
        {
            try
            {
                var tcpBinding = new NetTcpBinding();
                tcpBinding.MaxBufferSize = 2147483647;
                tcpBinding.MaxReceivedMessageSize = 2147483647;

                _krHost = new ServiceHost(typeof(KRService), new Uri("http://127.0.0.1:9999/"));
                _krHost.AddServiceEndpoint(typeof(IKRService), tcpBinding, "net.tcp://127.0.0.1:8888/KRService");
                _krHost.Open();

                _krEventHost = new ServiceHost(typeof(EventService));
                _krEventHost.AddServiceEndpoint(typeof(IEventService), new NetTcpBinding(), "net.tcp://127.0.0.1:7777/EventService");
                _krEventHost.Open();

                EventService.NewClientSubscribedEvent += EventService_NewClientSubscribedEvent;
                EventService.ClientLostEvent += EventService_ClientLostEvent;
            }
            catch (ArgumentNullException ex)
            {
                ExceptionEvent?.Invoke(new KRException("StartKRSvc", "ServerType is null", ex.Message));
            }
            catch (TimeoutException ex)
            {
                ExceptionEvent?.Invoke(new KRException("StartKRSvc", "Connection is timeout", ex.Message));
            }

        }

        private void EventService_ClientLostEvent(SubscribeArg arg)
        {
            ClientLost?.Invoke(this, new DataEventArgs(arg.Username) );
        }

        private void EventService_NewClientSubscribedEvent(SubscribeArg arg)
        {
            NewClientSubscribed?.Invoke(this, new DataEventArgs(arg.Username) );
        }

        public void StopService()
        {
            Task.Factory.StartNew(() =>
            {
                if (_krHost.State == CommunicationState.Opened)
                {
                    _krHost.Close();
                }
                if (_krEventHost.State == CommunicationState.Opened)
                {
                    _krEventHost.Close();
                }
            });

            EventService.NewClientSubscribedEvent -= EventService_NewClientSubscribedEvent;
            EventService.ClientLostEvent -= EventService_ClientLostEvent;
        }

        public void SendDataToClient(ArgumentBase<string> arg)
        {
            EventService.BroadcastEvent(arg);
        }

        public void SendDataToSpecClient(string username, ArgumentBase<string> arg)
        {
            EventService.NotifySpecClient(username, arg);
        }

    }
}
