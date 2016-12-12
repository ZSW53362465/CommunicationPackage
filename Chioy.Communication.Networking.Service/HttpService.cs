using Chioy.Communication.Networking.Common;
using Chioy.Communication.Networking.Interface;
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

            var servicePair = ServiceFactory.CreateService<IKRHttpService, KRHttpService>(Address, _type, string.Empty);
            currentService = servicePair.Item1;
            _httpServiceHost = servicePair.Item2;
            _httpServiceHost.Open();
        }
    }
}
