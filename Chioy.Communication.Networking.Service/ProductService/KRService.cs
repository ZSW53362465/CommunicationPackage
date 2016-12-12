using Chioy.Communication.Networking.Interface;
using Chioy.Communication.Networking.Service.Provider;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;

namespace Chioy.Communication.Networking.Service.ProductService
{
    public class KRService
    {
        protected IDataProvider _provider = null;
        public void RegisterProvider(IDataProvider provider)
        {
            _provider = provider;
        }
    }
}
