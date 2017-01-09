using Chioy.Communication.Networking.Common;
using Chioy.Communication.Networking.Interface;
using Chioy.Communication.Networking.Models;
using System;
using System.ServiceModel;
using System.Threading.Tasks;
using Chioy.Communication.Networking.Interface.ProductInterface.TCP;
using Chioy.Communication.Networking.Service.ProductService;
using Chioy.Communication.Networking.Service.ProductService.HTTP;
using Chioy.Communication.Networking.Service.ProductService.TCP;
using Chioy.Communication.Networking.Service.Provider;
using static Chioy.Communication.Networking.Common.Constants;

namespace Chioy.Communication.Networking.Service
{
    public class TcpServiceMgr : BaseServiceMgr
    {
        public event EventHandler<DataEventArgs> NewClientSubscribed;
        public event EventHandler<DataEventArgs> ClientLost;
        ServiceHost _krHost = null;
        ServiceHost _krEventHost = null;
        IEventService _eventService = null;

        //private volatile static TCPService _instance = null;

        private static readonly object lockHelper = new object();

        private string eventServiceAddress
        {
            get { return string.Format("net.tcp://{0}:{1}/{2}", configSetting.BaseAddress, configSetting.WCFPort, ServiceName.KREventService); }
        }
        public TcpServiceMgr() : base()
        {
            _type = BindingType.TCP;
        }

        public override void ConfigService(ProductType type)
        {
            try
            {
                base.ConfigService(type);
                BuildServiceHost().Open();
                BuildHeartJumpService().Open();
                _state = _krHost.State;
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
        private ServiceHost BuildHeartJumpService()
        {
            var servicePair = ServiceFactory.CreateService<IEventService, EventService>(eventServiceAddress, _type, string.Empty);
            _eventService = servicePair.Item1 as IEventService;
            return servicePair.Item2;
        }
        private ServiceHost BuildServiceHost()
        {
            Tuple<DataProviderAdpter, ServiceHost> servicePair = null;
            switch (_produceType)
            {
                case ProductType.BMD:
                    servicePair = ServiceFactory.CreateService<IBMDTcpService, BMDTcpService>(Address, _type, string.Empty);
                    break;
                case ProductType.KRTCD:
                    //servicePair = ServiceFactory.CreateService<IKRTCDService, KRTCDService>(Address, _type, string.Empty);
                    break;
                default:
                    break;
            }
            providerAdpter = servicePair.Item1;
            _krHost = servicePair.Item2;
            return servicePair.Item2;
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
            _state = _krHost.State;
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
