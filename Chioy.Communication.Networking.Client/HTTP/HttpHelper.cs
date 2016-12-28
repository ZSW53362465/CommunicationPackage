using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Chioy.Communication.Networking.Models;
using Chioy.Communication.Networking.Common;
using System.Net;
using System.IO;
using System.Web;
using System.Diagnostics;
using System.Collections.Specialized;
using Chioy.Communication.Networking.Client.HTTP;

namespace Chioy.Communication.Networking.Client
{
    public class HttpHelper
    {
        public const string ERROR_FLAG = "KRNetError:";
        private Encoding _encoding = Encoding.UTF8;

        public Encoding Encoding
        {
            get
            {
                return _encoding;
            }

            set
            {
                _encoding = value;
            }
        }

        public string HttpPostData<T>(string url, T obj, int timeout = 0, string type = null)
        {
            string retString = string.Empty;
            try
            {
                var jsonStr = CommunicationHelper.SerializeObjToJsonStr<T>(obj);

                KRWebClient client = new KRWebClient();
                client.Timeout = timeout;
                NameValueCollection param = new NameValueCollection();
                client.Headers.Add("Content-Type", "application/json");
                if (!string.IsNullOrEmpty(type))
                {
                    param.Add("type", type);
                }
                param.Add("content", jsonStr);
                var content = client.UploadValues(url, param);
                var strContent = Encoding.UTF8.GetString(content);
                Trace.WriteLine("HttpPostData返回数据:" + strContent);
                return strContent;
                //HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                //request.Method = "Post";
                //request.ContentType = "application/json";
                //request.ContentLength = _encoding.GetByteCount(jsonStr);
                //Stream myRequestStream = request.GetRequestStream();
                //StreamWriter myStreamWriter = new StreamWriter(myRequestStream);
                //myStreamWriter.Write(jsonStr);
                //myStreamWriter.Close();

                //HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                //Stream myResponseStream = response.GetResponseStream();
                //StreamReader myStreamReader = new StreamReader(myResponseStream);
                //retString = myStreamReader.ReadToEnd();
                //myStreamReader.Close();
                //myResponseStream.Close();
            }
            catch (Exception ex)
            {
                Trace.WriteLine(ERROR_FLAG + "HttpPostData:Post " + url + "失败" + ex.Message);
                throw new KRException("HttpPostData", "Post 请求失败", ex.Message);
            }

            //return HttpUtility.UrlDecode(retString);
        }

        public string HttpGetData(string url, NameValueCollection param, int timeout = 0)
        {
            string retString = string.Empty;

            try
            {
                KRWebClient client = new KRWebClient();
                client.Headers.Add("Content-Type", "application/x-www-form-urlencoded");
                client.Timeout = timeout;
                var content = client.UploadValues(url, param);
                var strContent = Encoding.UTF8.GetString(content);
                Trace.WriteLine("HttpGetData返回数据:" + strContent);
                return strContent;

                //HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                //request.Method = "GET";
                //request.ContentType = "Content-Type = application/json";
                //HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                //Stream myResponseStream = response.GetResponseStream();
                //StreamReader myStreamReader = new StreamReader(myResponseStream, _encoding);
                //retString = myStreamReader.ReadToEnd();
                //myStreamReader.Close();
                //myResponseStream.Close();
            }
            catch (Exception ex)
            {
                Trace.WriteLine(ERROR_FLAG + "HttpGetData:Get " + url + "失败" + ex.Message);
                throw new KRException("HttpGetData", "Get 请求失败", ex.Message);
            }

            return retString;
        }
    }
}
