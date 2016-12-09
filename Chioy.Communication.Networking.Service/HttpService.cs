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
        ServiceHost _httpService;
        public HttpService()
        {
            _type = BindingType.HTTP;
        }
        public override void ConfigService()
        {
            _httpService = ServiceFactory.CreateService<IKRHttpService, KRHttpService>(Address, _type, string.Empty);
        }
    }
}
