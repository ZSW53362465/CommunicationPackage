using Chioy.Communication.Networking.Common;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace Chioy.Communication.Networking.Client.Client
{
    public class ClientHelper
    {
        public static void TraceException(string method,string description,string message)
        {
            Trace.WriteLine("KRNetError:"+ method+ " " + description +" "+ message);

            throw new KRException(method, description, message);
        }
    }
}
