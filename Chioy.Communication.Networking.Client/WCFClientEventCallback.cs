using Chioy.Communication.Networking.Common;
using Chioy.Communication.Networking.Interface;
using Chioy.Communication.Networking.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.ServiceModel;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Chioy.Communication.Networking.Client
{
    public delegate void OnEventReceivedEventHandler(ArgumentBase<string> arg);
    public class WCFClientEventCallback : IEventCallback, IDisposable
    {
        #region Private Memeber

        private const string KR_Config = "KRWcfConfig";

        private const string KR_EventServiceConfig = "EventServiceConfig";

        private static int _maxErrCount = 5;

        public static int _heartbeatInterval = 1000 * 10;//10秒一次心跳检测

        IKRService _krProxy = null;
        IEventService _krEventproxy = null;

        private int _errCounter = 0;

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

        public IKRService KRService { get { return this._krProxy; } }

        public bool Enabled { get; set; }

        /// <summary>
        /// Server 主动发送过来的消息，由子线程弹出，如果想更新UI，需先回到主线程
        /// </summary>
        public event OnEventReceivedEventHandler OnEventReceivedEvent;

        public void OnEventFired(ArgumentBase<string> arg)
        {
            OnEventReceivedEvent?.Invoke(arg);
        }

        public void RegisterServices(string name = null)
        {
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

        private  void ConnectServer(string name)
        {
            var task = Subscribe(name);
            //task.Start();
            task.ContinueWith(StartHeartJumpListen);
            //StartHeartJumpListen();
        }

        private void StartHeartJumpListen(Task task)
        {
            var timer = new System.Timers.Timer();
            timer.Enabled = false;
            timer.Interval = _heartbeatInterval;
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

        private  Task Subscribe(string name = null)
        {
            return  Task.Factory.StartNew(() =>
            {

                Enabled = true;

                var eventSvcRemoteFactory = CreateEventServiceRemoteFactory();
                var krSvcRemoteFactory = CreateKRServiceRemoteFactory();
                try
                {
                    _krEventproxy = eventSvcRemoteFactory.CreateChannel();
                    _krProxy = krSvcRemoteFactory.CreateChannel();

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
            });
        }
        private DuplexChannelFactory<IEventService> CreateEventServiceRemoteFactory()
        {
            return new DuplexChannelFactory<IEventService>(
                new InstanceContext(this),
                new NetTcpBinding() ,
                new EndpointAddress("net.tcp://127.0.0.1:7777/EventService"));
        }

        private DuplexChannelFactory<IKRService> CreateKRServiceRemoteFactory()
        {
            return new DuplexChannelFactory<IKRService>(
              new WCFClientCallbackManager(),
              new NetTcpBinding() { MaxBufferSize = 2147483647, MaxReceivedMessageSize = 2147483647 },
              new EndpointAddress("net.tcp://127.0.0.1:8888/KRService"));
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
