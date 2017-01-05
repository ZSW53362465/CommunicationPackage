using Chioy.Communication.Networking.Common;
using Chioy.Communication.Networking.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using Chioy.Communication.Networking.Service.ProductService.HTTP;

namespace Chioy.Communication.Networking.Service
{
    public class HttpServiceMgr : BaseService
    {
        ServiceHost _httpServiceHost;
        public HttpServiceMgr()
        {
            _type = BindingType.HTTP;
        }
        public override void ConfigService(ProductType type)
        {
            base.ConfigService(type);
            Tuple<KRService, ServiceHost> servicePair = ServiceFactory.CreateService<IBMDHttpService, BMDHttpService>(Address, _type, string.Empty);
            if (servicePair != null)
            {
                currentService = servicePair.Item1;
                _httpServiceHost = servicePair.Item2;
                _httpServiceHost.Open();
            }
        }
    }
}
