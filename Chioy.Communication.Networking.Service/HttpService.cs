using Chioy.Communication.Networking.Common;
using Chioy.Communication.Networking.Interface;
using Chioy.Communication.Networking.Service.ProductService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;

namespace Chioy.Communication.Networking.Service
{
    public class HttpService : BaseService
    {
        ServiceHost _httpServiceHost;
        public HttpService()
        {
            _type = BindingType.HTTP;
        }
        public override void ConfigService(ProductType type)
        {
            base.ConfigService(type);
            Tuple<KRService, ServiceHost> servicePair = null;
            switch (type)
            {
                case ProductType.BMD:
                    servicePair = ServiceFactory.CreateService<IBMDHttpService, BMDHttpService>(Address, _type, string.Empty);
                    break;
                case ProductType.KRTCD:
                    servicePair = ServiceFactory.CreateService<IKRTCDService, KRTCDService>(Address, _type, string.Empty);
                    break;
                default:
                    break;
            }
            if (servicePair != null)
            {
                currentService = servicePair.Item1;
                _httpServiceHost = servicePair.Item2;
                _httpServiceHost.Open();
            }
        }
    }
}
