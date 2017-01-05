using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chioy.Communication.Networking.Common
{
    public class Constants
    {
        public struct ServiceName
        {
            public const string BMDHttpService = "bmdhttpservice";
            public const string BMDTcpService = "BMDTcpService";
            public const string TCDHttpService = "tcdhttpservice";
            public const string TCDTcpService = "KRTCDService";

            //public const string KRHttpService = "KRHttpService";
            public const string KREventService = "EventService";
            public const string Employees = "employees";
        }
    }
}
