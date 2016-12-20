using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chioy.Communication.Networking.Client.Helper
{
    public class WebServiceClient
    {
        WebServiceProxy _proxy = null;
        public WebServiceClient(string wsdlUrl )
        {
            _proxy = new WebServiceProxy(wsdlUrl);
        }

        public WebServiceClient(string wsdlUrl,string wsdlName)
        {
            _proxy = new WebServiceProxy(wsdlUrl, wsdlName);
        }

        public object ExecuteQuery(string methodName,object[] param)
        {
            try
            {
                return _proxy.ExecuteQuery(methodName, param);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void ExecuteNoQuery(string methodName, object[] param)
        {
            try
            {
                _proxy.ExecuteNoQuery(methodName, param);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
}
