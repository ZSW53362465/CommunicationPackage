using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chioy.Communication.Networking.Service
{
    public class ServiceManager
    {
        private volatile static ServiceManager _instance = null;

        private static readonly object lockHelper = new object();

   

        public static ServiceManager Instance()
        {
            if (_instance == null)
            {
                lock (lockHelper)
                {
                    if (_instance == null)
                    {
                        _instance = new ServiceManager();
                    }
                }
            }
            return _instance;
        }


        public BaseService GetService(BindingType type)
        {
            BaseService rtnService = null;
            switch (type)
            {
                case BindingType.TCP:
                    rtnService = new TCPService();
                    break;
                case BindingType.HTTP:
                    rtnService = new HttpService();
                    break;
                case BindingType.All:
                    break;
                default:
                    break;
            }
            return rtnService;
        }
    }
}
