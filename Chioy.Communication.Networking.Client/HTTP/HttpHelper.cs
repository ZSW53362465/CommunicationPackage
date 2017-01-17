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
using Chioy.Communication.Networking.Client.Client;

namespace Chioy.Communication.Networking.Client
{
    public class HttpHelper
    {
        public const string ERROR_FLAG = "KRNetError:";
        private HttpEngine _engine;
        public HttpHelper()
        {
            _engine = new HttpEngine();
        }

        public int Timeout { get; set; }

        public bool NeedUrlDecode { get; set; }

        public string HttpPostData(string url, object paramter, HttpItem pitem = null)
        {
            try
            {
                if (paramter == null)
                    CommunicationHelper.TraceException("HttppostData", "null parameter", "parameter obj is null");

                string contentType = "application/json";

                if (paramter is string || paramter.GetType().IsValueType)
                {
                    contentType = "application/x-www-form-urlencoded";
                }

                var postStr = CommunicationHelper.SerializeObjToJsonStr(paramter);
                if (pitem == null)
                {
                    pitem = new HttpItem()
                    {
                        Method = "Post",
                        Postdata = postStr,
                        ContentType = contentType,
                        URL = url
                    };
                }
                else
                {
                    pitem.Method = "Post";
                    pitem.Postdata = postStr;
                    pitem.ContentType = contentType;
                    pitem.URL = url;
                }

                var result = _engine.GetHtml(pitem);
                CommunicationHelper.RecordTrace("HttpPostData", "返回结果:" + result.Html);
                return result.Html;

                //var client = new KRWebClient { Timeout = Timeout };
                //NameValueCollection param = new NameValueCollection();
                ////client.Headers.Add("Content-Type", "application/json");
                //param.Add("type", type);
                //param.Add("content", jsonStr);
                //var content = client.UploadValues(url, param);
                //var strContent = Encoding.UTF8.GetString(content);
                //return strContent;
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
                ClientHelper.TraceException("HttpPostData:Post", "Post数据失败", ex.Message);
                return string.Empty;
            }

            //return HttpUtility.UrlDecode(retString);
        }

        public TOut HttpPostData<TIn, TOut>(string url, TIn objIn, HttpItem pitem = null)
        {

            var resultStr = HttpPostData(url, objIn, pitem);
          
            return CommunicationHelper.DeserializeJsonToObj<TOut>(resultStr);
        }

        public string HttpGetData(string url, HttpItem pitem = null)
        {
            try
            {
                var httpItem = pitem ?? new HttpItem() { URL = url };

                var result = _engine.GetHtml(httpItem);
                var strContent = result.Html;
                CommunicationHelper.RecordTrace("HttpGetData", "返回的数据" + result.Html);
                return strContent;
                //string retString = string.Empty;
                //HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                //request.Method = "GET";
                //request.ContentType = "Content-Type = application/json";
                //HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                //Stream myResponseStream = response.GetResponseStream();
                //StreamReader myStreamReader = new StreamReader(myResponseStream, encoder);
                //retString = myStreamReader.ReadToEnd();
                //myStreamReader.Close();
                //myResponseStream.Close();
                //return DecodeResponseStr(retString);
            }
            catch (Exception ex)
            {
                ClientHelper.TraceException("HttpPostData:Get", "Get数据失败", ex.Message);
                return string.Empty;
            }
        }

        public TOut HttpGetData<TOut>(string url, HttpItem pitem = null)
        {
            //encoder = encoder ?? Encoding.UTF8;
            try
            {
                var resultStr = HttpGetData(url, pitem);
                return CommunicationHelper.DeserializeJsonToObj<TOut>(resultStr);

                //var client = new KRWebClient();
                ////client.Headers.Add("Content-Type", "application/x-www-form-urlencoded");
                //client.Timeout = Timeout;
                //var strContent = DecodeResponseStr(client.DownloadString(url));
                //Trace.WriteLine("HttpGetData返回数据:" + strContent);
                //return CommunicationHelper.DeserializeJsonToObj<TOut>(strContent);

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
                ClientHelper.TraceException("HttpPostData:Get", "Get数据失败", ex.Message);
                return default(TOut);
            }
        }

        /// <summary>
        /// Post方式上传数据
        /// </summary>
        /// <param name="item">Post请求配置信息</param>
        /// <param name="postData">Post请求发送的数据</param>
        /// <returns></returns>
        public HttpResult HttpPostData(HttpItem item,string postData)
        {
            try
            {
                item.Method = "Post";
                item.Postdata = postData;
                return _engine.GetHtml(item);
            }
            catch (Exception ex)
            {
                ClientHelper.TraceException("HttpPostData:Post", "上传数据失败", ex.Message);
                return new HttpResult() {StatusCode = HttpStatusCode.BadRequest};
            }
        }
        /// <summary>
        /// Get方式请求数据
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public HttpResult HttpGetData(HttpItem item)
        {
            try
            {
                return _engine.GetHtml(item);
            }
            catch (Exception ex)
            {
                ClientHelper.TraceException("HttpPostData:Get", "Get数据失败", ex.Message);
                return new HttpResult() { StatusCode = HttpStatusCode.BadRequest };
            }

        }


        private string DecodeResponseStr(string str)
        {
            if (NeedUrlDecode)
            {
                return HttpUtility.UrlDecode(str);
            }
            return str;
        }
    }
}
