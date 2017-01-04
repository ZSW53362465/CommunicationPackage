using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace Chioy.Communication.Networking.Client.HTTP
{
    public class KRWebClient : WebClient
    {
        private int _timeOut = 10;

        /// <summary>
        /// 过期时间
        /// </summary>
        public int Timeout
        {
            get
            {
                return _timeOut;
            }
            set
            {
                if (value <= 0)
                    _timeOut = 10;
                _timeOut = value;
            }
        }

        /// <summary>
        /// 重写GetWebRequest,添加WebRequest对象超时时间
        /// </summary>
        /// <param name="address"></param>
        /// <returns></returns>
        protected override WebRequest GetWebRequest(Uri address)
        {

            HttpWebRequest request = (HttpWebRequest)base.GetWebRequest(address);
            if (Timeout > 0)
            {
                request.Timeout = 1000 * Timeout;
                request.ReadWriteTimeout = 1000 * Timeout;
            }

            return request;
        }
    }
}
