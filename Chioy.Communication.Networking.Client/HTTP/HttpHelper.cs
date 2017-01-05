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

        public int Timeout { get; set; }

        public bool NeedUrlDecode { get; set; }


        public string HttpPostData<T>(string url, T obj, string type)
        {
            try
            {
                var jsonStr = CommunicationHelper.SerializeObjToJsonStr<T>(obj);

                var client = new KRWebClient { Timeout = Timeout };
                NameValueCollection param = new NameValueCollection();
                //client.Headers.Add("Content-Type", "application/json");
                param.Add("type", type);
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
                ClientHelper.TraceException("HttpPostData:Post", "Post数据失败", ex.Message);
                return string.Empty;
            }

            //return HttpUtility.UrlDecode(retString);
        }

        public string HttpPostData<T>(string url, T obj)
        {
            try
            {
                //var jsonStr = CommunicationHelper.SerializeObjToJsonStr<T>(obj);
                //var client = new KRWebClient { Timeout = Timeout };
                //var strContent = DecodeResponseStr(client.UploadString(url, jsonStr));
                //Trace.WriteLine("HttpPostData返回数据:" + strContent);
                //return strContent;
                var jsonStr = CommunicationHelper.SerializeObjToJsonStr<T>(obj);
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "Post";
                request.ContentType = "application/json";
                request.ContentLength = Encoding.UTF8.GetByteCount(jsonStr);
                Stream myRequestStream = request.GetRequestStream();
                StreamWriter myStreamWriter = new StreamWriter(myRequestStream);
                myStreamWriter.Write(jsonStr);
                myStreamWriter.Close();

                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                Stream myResponseStream = response.GetResponseStream();
                StreamReader myStreamReader = new StreamReader(myResponseStream);
                var retString = myStreamReader.ReadToEnd();
                myStreamReader.Close();
                myResponseStream.Close();
                return retString;

                //var jsonStr = CommunicationHelper.SerializeObjToJsonStr<T>(obj);
                //var jsonStr = "asdfasf";
                //Encoding myEncode = Encoding.GetEncoding("UTF-8");
                //byte[] postBytes = Encoding.UTF8.GetBytes(jsonStr);

                //HttpWebRequest req = (HttpWebRequest)HttpWebRequest.Create(url);
                //req.Method = "POST";
                //req.ContentType = "application/x-www-form-urlencoded";
                //req.ContentLength = postBytes.Length;

                //using (Stream reqStream = req.GetRequestStream())
                //{
                //    reqStream.Write(postBytes, 0, postBytes.Length);
                //}
                //using (WebResponse res = req.GetResponse())
                //{
                //    using (StreamReader sr = new StreamReader(res.GetResponseStream(), myEncode))
                //    {
                //        string strResult = sr.ReadToEnd();
                //        return strResult;
                //    }
                //}

            }
            catch (Exception ex)
            {
                ClientHelper.TraceException("HttpPostData:Post", "Post数据失败", ex.Message);
                return string.Empty;
            }
        }

        public string HttpPostData(string url, NameValueCollection param, Encoding encoder = null)
        {
            try
            {
                encoder = encoder ?? Encoding.UTF8;
                var client = new KRWebClient { Timeout = Timeout };
                //client.Headers.Add("Content-Type", "application/x-www-form-urlencoded");
                var content = client.UploadValues(url, param);
                var strContent = DecodeResponseStr(encoder.GetString(content));
                Trace.WriteLine("HttpPostData返回数据:" + strContent);
                return strContent;
            }
            catch (Exception ex)
            {
                ClientHelper.TraceException("HttpPostData:Post", "Post数据失败", ex.Message);
                return string.Empty;
            }
        }

        public string HttpGetData(string url, Encoding encoder = null)
        {
            encoder = encoder ?? Encoding.UTF8;
            try
            {
                var client = new KRWebClient();
                //client.Headers.Add("Content-Type", "application/x-www-form-urlencoded");
                client.Timeout = Timeout;
                client.Encoding = encoder;
                var strContent = DecodeResponseStr(client.DownloadString(url));
                Trace.WriteLine("HttpGetData返回数据:" + strContent);
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


        public TOut HttpGetData<TOut>(string url, Encoding encoder = null)
        {
            encoder = encoder ?? Encoding.UTF8;
            try
            {
                var client = new KRWebClient();
                //client.Headers.Add("Content-Type", "application/x-www-form-urlencoded");
                client.Timeout = Timeout;
                var strContent = DecodeResponseStr(client.DownloadString(url));
                Trace.WriteLine("HttpGetData返回数据:" + strContent);
                return CommunicationHelper.DeserializeJsonToObj<TOut>(strContent);

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
