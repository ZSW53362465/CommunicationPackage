using Chioy.Communication.Networking.Common;
using Chioy.Communication.Networking.Interface;
using Chioy.Communication.Networking.Service.ProductService;
using Chioy.Communication.Networking.Service.Provider;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using static Chioy.Communication.Networking.Common.Constants;

namespace Chioy.Communication.Networking.Service
{
    public class BaseServiceMgr : IDisposable
    {

        protected BindingType _type;

        protected CommunicationState _state;

        public event KRExceptionEventHandler ExceptionEvent;

        protected static ConfigSetting configSetting = null;

        public ProductType _produceType { get; set; }

        public CommunicationState ServiceState { get { return _state; } }

        protected DataProviderAdpter providerAdpter;

        private string _address = string.Empty;

        protected string Address
        {
            get
            {
                return _address;
            }
        }


        public BaseServiceMgr()
        {
            configSetting = CommunicationHelper.GetConfigSetting();
        }

        public virtual void ConfigService(ProductType type)
        {
            _produceType = type;
            BuildAddress();
        }

        public void RegisterProvider(IDataProvider provider)
        {
            providerAdpter.RegisterProvider(provider);
        }

        private void BuildAddress()
        {
            string addressHeader = string.Empty;
            string serviceName = string.Empty;
            string baseAddress = configSetting.BaseAddress;
            string port = string.Empty;

            switch (_type)
            {
                case BindingType.TCP:
                    addressHeader = "net.tcp";
                    switch (_produceType)
                    {
                        case ProductType.BMD:
                            serviceName = ServiceName.BMDTcpService;
                            break;
                        case ProductType.KRTCD:
                            serviceName = ServiceName.TCDTcpService;
                            break;
                        default:
                            break;
                    }
                    port = configSetting.WCFPort.ToString();
                    break;
                case BindingType.HTTP:
                    addressHeader = "http";
                    switch (_produceType)
                    {
                        case ProductType.BMD:
                            //serviceName = ServiceName.Employees;
                            serviceName = ServiceName.BMDHttpService;
                            break;
                        case ProductType.KRTCD:
                            //serviceName = ServiceName.KRTCDHttpService;
                            break;
                        default:
                            break;
                    }
                    port = configSetting.HttpPort.ToString();
                    break;
                default:
                    _address = string.Format("net.tcp://{0}:{1}/{2}", configSetting.BaseAddress, configSetting.WCFPort, serviceName);
                    break;
            }
            _address = string.Format("{0}://{1}:{2}/{3}", addressHeader, baseAddress, port, serviceName);
        }

        public virtual void DisposeService()
        { }

        protected void HandleExceptionEvent(KRException ex)
        {
            ExceptionEvent?.Invoke(ex);
        }
        public void Dispose()
        {

        }
    }
}
