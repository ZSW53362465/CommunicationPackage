using Chioy.Communication.Networking.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chioy.Communication.Networking.Service
{
    public class ServiceManagerFactory
    {
        private volatile static ServiceManagerFactory _instance = null;

        private static readonly object lockHelper = new object();

        public static ServiceManagerFactory Instance()
        {
            if (_instance == null)
            {
                lock (lockHelper)
                {
                    if (_instance == null)
                    {
                        _instance = new ServiceManagerFactory();
                    }
                }
            }
            return _instance;
        }


        public BaseServiceMgr GetService(BindingType type)
        {
            BaseServiceMgr rtnService = null;
            switch (type)
            {
                case BindingType.TCP:
                    rtnService = new TcpServiceMgr();
                    break;
                case BindingType.HTTP:
                    rtnService = new HttpServiceMgr();
                    break;
                default:
                    break;
            }
            return rtnService;
        }
    }
}
