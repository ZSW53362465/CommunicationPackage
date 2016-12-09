using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chioy.Communication.Networking.Common
{
    public class ConfigSetting
    {
        public int WCFPort { get; private set; }
        public int HttpPort { get; private set; }
        public string BaseAddress { get; private set; }
        public ConfigSetting(int wcfPort,int httpPort,string baseAddress)
        {
            WCFPort = wcfPort;
            HttpPort = HttpPort;
            BaseAddress = baseAddress;
        }
    }
}
