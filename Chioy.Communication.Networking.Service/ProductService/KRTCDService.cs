using Chioy.Communication.Networking.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;

namespace Chioy.Communication.Networking.Service.ProductService
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public class KRTCDService : KRService, IKRTCDService
    {
    }
}
