using Chioy.Communication.Networking.Common;
using Chioy.Communication.Networking.Interface;
using Chioy.Communication.Networking.Models;
using System;
using System.Net;
using System.ServiceModel;
using Chioy.Communication.Networking.Interface.ProductInterface.TCP;
using static Chioy.Communication.Networking.Common.Constants;

namespace Chioy.Communication.Networking.Client
{
    public delegate void OnEventReceivedEventHandler(ArgumentBase<string> arg);
    public class WCFClientEventCallback : IEventCallback, IDisposable
    {
        #region Private Memeber

        private static int _maxErrCount = 5;

        public static int _heartbeatInterval = 1000 * 10;//10秒一次心跳检测

        IService _krProxy = null;
        IEventService _krEventproxy = null;

        private int _errCounter = 0;

        private string _address = string.Empty;

        private string _port = string.Empty;

        #endregion

        private volatile static WCFClientEventCallback _instance = null;

        private static readonly object lockHelper = new object();

        public event EventHandler<DataEventArgs> CommunicationEvent;

        public event KRExceptionEventHandler ExceptionEvent;

        public static WCFClientEventCallback Instance()
        {
            if (_instance == null)
            {
                lock (lockHelper)
                {
                    if (_instance == null)
                    {
                        _instance = new WCFClientEventCallback();
                    }
                }
            }
            return _instance;
        }
        #region Public

        public IService KRService { get { return _krProxy; } }

        public IEventService KRHeartService { get { return _krEventproxy; } }

        public bool Enabled { get; set; }

        private ProductType _type;

        public ProductType Type
        {
            get { return _type; }
        }

        /// <summary>
        /// Server 主动发送过来的消息，由子线程弹出，如果想更新UI，需先回到主线程
        /// </summary>
        public event OnEventReceivedEventHandler OnEventReceivedEvent;

        public void OnEventFired(ArgumentBase<string> arg)
        {
            OnEventReceivedEvent?.Invoke(arg);
        }

        public void RegisterServices(ProductType type, string baseAddress, string port, string name = null)
        {
            _type = type;
            _address = baseAddress;
            _port = port;
            ConnectServer(name);
        }

        public void UnRegistServices()
        {
            Close();
        }

        #endregion

        #region Private Method
        private string CurrentHostName
        {
            get { return Dns.GetHostName(); }
        }

        public WCFClientEventCallback()
        {
            Enabled = false;
        }

        private void ConnectServer(string name)
        {
            Subscribe(name);
            //task.Start();
            StartHeartJumpListen();
            //StartHeartJumpListen();
        }

        private void StartHeartJumpListen()
        {
            var timer = new System.Timers.Timer
            {
                Enabled = false,
                Interval = _heartbeatInterval
            };
            timer.Elapsed += (s, ie) =>
            {
                try
                {
                    timer.Enabled = false;
                    _krEventproxy.Ping();
                    _errCounter = 0;
                }
                catch (Exception ex)
                {
                    _errCounter++;
                    if (_errCounter >= _maxErrCount)
                    {
                        Restart();
                    }
                }
                finally
                {
                    timer.Enabled = true;
                }
            };
            timer.Start();
        }

        private void Restart()
        {
            Close();
            Subscribe();
        }

        private void Subscribe(string name = null)
        {
            Enabled = true;

            var eventSvcRemoteFactory = CreateEventServiceRemoteFactory();
            try
            {
                _krEventproxy = eventSvcRemoteFactory.CreateChannel();
                BuildKRService();

                 var comObj = _krEventproxy as ICommunicationObject;

                comObj.Faulted += (s, ie) =>
                {
                       //ExceptionEvent?.Invoke(s, "Faulted");
                   };
                comObj.Closed += (s, ie) =>
                {
                       //ExceptionEvent?.Invoke(s, "Closed");
                   };
                comObj.Closing += (s, ie) =>
                {
                    CommunicationEvent?.Invoke(s, new DataEventArgs("remote host is closed"));
                };

                _krEventproxy.Subscribe(CreateDefaultSubscribeArg(name));
            }
            catch (Exception ex)
            {
                ExceptionEvent?.Invoke(new KRException("Subscribe", "connection error", ex.Message));
            }

        }
        private void BuildKRService()
        {
            var serviceName = string.Empty;
            switch (_type)
            {
                case ProductType.BMD:
                    serviceName = ServiceName.BMDTcpService;
                    _krProxy = CreateProductService<IBMDTcpService>(serviceName);
                    break;
                case ProductType.KRTCD:
                    serviceName = ServiceName.TCDTcpService;
                    //_krProxy = CreateProductService<IKRTCDService>(serviceName);
                    break;
                default:
                    break;
            }
        }
        private DuplexChannelFactory<IEventService> CreateEventServiceRemoteFactory()
        {
            var binding = new NetTcpBinding() { MaxBufferPoolSize = 2147483647, MaxReceivedMessageSize = 2147483647 };
            binding.Security.Mode = SecurityMode.None;
            return new DuplexChannelFactory<IEventService>(
                new InstanceContext(this),
               binding,
                new EndpointAddress(string.Format("net.tcp://{0}:{1}/{2}", _address, _port, ServiceName.KREventService)));
        }

        private T CreateProductService<T>(string serviceName)
        {
            var binding = new NetTcpBinding() { MaxBufferPoolSize = 2147483647, MaxReceivedMessageSize = 2147483647 };
            binding.Security.Mode = SecurityMode.None;
            var factory = new DuplexChannelFactory<T>(
             new WCFClientCallbackManager(),
             binding,
             new EndpointAddress(string.Format("net.tcp://{0}:{1}/{2}", _address, _port, serviceName)));
            return factory.CreateChannel();
        }

        private SubscribeArg CreateDefaultSubscribeArg(string name = null)
        {
            return new SubscribeArg() { Code = KRCode.Subscribe, Alarms = null, Model = 0, Msg = "Subscribe", Username = string.IsNullOrEmpty(name) ? CurrentHostName : name };
        }
        private void Close()
        {
            if (_krEventproxy != null && _krProxy != null)
            {
                try
                {
                    var comObj = _krEventproxy as ICommunicationObject;
                    var krcomObj = _krProxy as ICommunicationObject;
                    krcomObj.Abort();
                    comObj.Abort();
                }
                catch { }
            }
        }

        public void Dispose()
        {
            Close();
            _krProxy = null;
            _krEventproxy = null;
        }


        #endregion
    }

    public class WCFClientCallbackManager : IKRDuplexCallback
    {
        public void CreateNewUserInClient()
        {
            //throw new NotImplementedException(); 
        }

        public void RemoveUserInClient()
        {
            //throw new NotImplementedException();
        }
    }
}
