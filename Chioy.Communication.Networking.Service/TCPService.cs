using Chioy.Communication.Networking.Common;
using Chioy.Communication.Networking.Interface;
using Chioy.Communication.Networking.Models;
using System;
using System.ServiceModel;
using System.Threading.Tasks;

namespace Chioy.Communication.Networking.Service
{
    public class TCPService : BaseService
    {
        public event EventHandler<DataEventArgs> NewClientSubscribed;
        public event EventHandler<DataEventArgs> ClientLost;
        ServiceHost _krHost = null;
        ServiceHost _krEventHost = null;
        EventService _eventService;

        //private volatile static TCPService _instance = null;

        private static readonly object lockHelper = new object();

        private string EventServiceAddress
        {
            get { return string.Format("net.tcp://{0}:{1}/{2}", configSetting.BaseAddress, configSetting.WCFPort, ServiceName.KREventService); }
        }
        public TCPService() : base()
        {
            _type = BindingType.TCP;
        }

        public override void ConfigService()
        {
            try
            {
                _krHost = ServiceFactory.CreateService<IKRService, KRService>(Address, _type, string.Empty);
                _krHost.Open();
                _krEventHost = ServiceFactory.CreateService<IEventService, EventService>(EventServiceAddress, _type, string.Empty);
                _krEventHost.Open();
                EventService.NewClientSubscribedEvent += EventService_NewClientSubscribedEvent;
                EventService.ClientLostEvent += EventService_ClientLostEvent;

            }
            catch (ArgumentNullException ex)
            {
                HandleExceptionEvent(new KRException("ConfigService", "ServerType is null", ex.Message));
            }
            catch (TimeoutException ex)
            {
                HandleExceptionEvent(new KRException("ConfigService", "Connection is timeout", ex.Message));
            }
        }

        private void EventService_ClientLostEvent(SubscribeArg arg)
        {
            ClientLost?.Invoke(this, new DataEventArgs(arg.Username));
        }

        private void EventService_NewClientSubscribedEvent(SubscribeArg arg)
        {
            NewClientSubscribed?.Invoke(this, new DataEventArgs(arg.Username));
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
