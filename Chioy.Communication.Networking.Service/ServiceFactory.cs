using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.Text;
using System.Xml;

namespace Chioy.Communication.Networking.Service
{
    public enum BindingType
    {
        TCP,
        HTTP,
    }
    public class ServiceFactory
    {

        public static Tuple<KRService, ServiceHost> CreateService<IT, T>(string url, string baseUrl) where T : IT
        {
            return CreateService<IT, T>(url, BindingType.TCP, baseUrl);
        }
        public static Tuple<KRService, ServiceHost> CreateService<IT, T>(string url, BindingType binding, string baseUrl) where T : IT 
        {
            if (string.IsNullOrEmpty(url)) throw new NotSupportedException("This url is not Null or Empty!");
            object service = Activator.CreateInstance(typeof(T));
            ServiceHost host = new ServiceHost(service);
            EndpointAddress address = new EndpointAddress(url);
            Binding bing = CreateBinding(binding);
            var endPoint = host.AddServiceEndpoint(typeof(IT), bing, url);
            if (binding == BindingType.HTTP)
            {
                endPoint.Behaviors.Add(new WebHttpBehavior() { HelpEnabled = true });
            }
            return new Tuple<KRService, ServiceHost>(service as KRService, host);
        }

        #region 创建传输协议
        /// <summary>
        /// 创建传输协议
        /// </summary>
        /// <param name="binding">传输协议名称</param>
        /// <returns></returns>
        private static Binding CreateBinding(BindingType binding)
        {
            Binding bindinginstance = null;
            switch (binding)
            {
                case BindingType.TCP:
                    NetTcpBinding tcpBinding = new NetTcpBinding
                    {
                        MaxReceivedMessageSize = 2147483647,
                        MaxBufferPoolSize = 2147483647,
                        Security = {Mode = SecurityMode.None}
                    };
                    bindinginstance = tcpBinding;
                    break;
                case BindingType.HTTP:
                    WebHttpBinding httpBinding = new WebHttpBinding
                    {
                        MaxBufferSize = 2147483647,
                        MaxReceivedMessageSize = 2147483647,
                        ReaderQuotas = new XmlDictionaryReaderQuotas()
                        {
                            MaxArrayLength = 2147483647,
                            MaxBytesPerRead = 2147483647,
                            MaxDepth = 2147483647,
                            MaxNameTableCharCount = 2147483647,
                            MaxStringContentLength = 2147483647
                        }
                    };
                    bindinginstance = httpBinding;
                    break;
                default:
                    break;
            }
            return bindinginstance;
        }
        #endregion
    }
}
