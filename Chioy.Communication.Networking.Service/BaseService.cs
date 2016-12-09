using Chioy.Communication.Networking.Common;
using Chioy.Communication.Networking.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chioy.Communication.Networking.Service
{
    public class BaseService : IDisposable
    {

        protected struct ServiceName
        {
            public const string KRService = "KRService";
            public const string KRHttpService = "KRHttpService";
            public const string KREventService = "EventService";
        }
        protected BindingType _type;

        protected IDataProvider _provider = null;

        public event KRExceptionEventHandler ExceptionEvent;

        protected static ConfigSetting configSetting = null;

        protected string Address
        {
            get
            {
                string address = string.Empty;

                switch (_type)
                {
                    case BindingType.TCP:
                        address = string.Format("net.tcp://{0}:{1}/{2}", configSetting.BaseAddress, configSetting.WCFPort, ServiceName.KRService);
                        break;
                    case BindingType.HTTP:
                        address = string.Format("http://{0}:{1}/{2}", configSetting.BaseAddress, configSetting.WCFPort, ServiceName.KRHttpService);
                        break;
                    case BindingType.All:
                        break;
                    default:
                        break;
                }
                return address;
            }
        }

        public BaseService()
        {
            configSetting = CommunicationHelper.GetConfigSetting();
        }

        public virtual void ConfigService()
        {

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
