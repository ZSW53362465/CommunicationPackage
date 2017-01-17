using Chioy.Communication.Networking.Common;
using Chioy.Communication.Networking.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using Chioy.Communication.Networking.Interface.ProductInterface.HTTP;
using Chioy.Communication.Networking.Service.ProductService.HTTP;
using Chioy.Communication.Networking.Service.Provider;

namespace Chioy.Communication.Networking.Service
{
    public class HttpServiceMgr : BaseServiceMgr
    {
        ServiceHost _httpServiceHost;
        public HttpServiceMgr()
        {
            _type = BindingType.HTTP;
        }
        public override void ConfigService(ProductType type)
        {
            base.ConfigService(type);
            Tuple<DataProviderAdpter, ServiceHost> servicePair = null;
            switch (type)
            {
                case ProductType.BMD:
                    servicePair = ServiceFactory.CreateService<IBMDHttpService, BMDHttpService>(Address, _type, string.Empty);
                    break;
                case ProductType.KRTCD:
                    //servicePair = ServiceFactory.CreateService<IKRTCDService, KRTCDService>(Address, _type, string.Empty);
                    break;
                default:
                    break;
            }
            if (servicePair != null)
            {
                providerAdpter = servicePair.Item1;
                _httpServiceHost = servicePair.Item2;
                _httpServiceHost.Open();
            }
        }
    }
}
