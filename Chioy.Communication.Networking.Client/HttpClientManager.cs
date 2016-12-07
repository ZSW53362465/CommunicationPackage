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

namespace Chioy.Communication.Networking.Client
{
    public class HttpClientManager : ClientManager
    {
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

        public string HttpPostData<T>(string url, T obj)
        {
            string retString = string.Empty;
            try
            {
                var jsonStr = CommunicationHelper.SerializeToJsonStr<T>(obj);

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "Post";
                request.ContentType = "application/json";
                request.ContentLength = _encoding.GetByteCount(jsonStr);
                Stream myRequestStream = request.GetRequestStream();
                StreamWriter myStreamWriter = new StreamWriter(myRequestStream);
                myStreamWriter.Write(jsonStr);
                myStreamWriter.Close();

                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                Stream myResponseStream = response.GetResponseStream();
                StreamReader myStreamReader = new StreamReader(myResponseStream);
                retString = myStreamReader.ReadToEnd();
                myStreamReader.Close();
                myResponseStream.Close();
            }
            catch (Exception ex)
            {
                HandleExceptionEvent(new KRException("HttpPostData", "http connection error", ex.Message));
            }

            return HttpUtility.UrlDecode(retString);
        }

        public string HttpGetData(string url)
        {
            string retString = string.Empty;

            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "GET";
                request.ContentType = "Content-Type = application/json";
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                Stream myResponseStream = response.GetResponseStream();
                StreamReader myStreamReader = new StreamReader(myResponseStream, _encoding);
                retString = myStreamReader.ReadToEnd();
                myStreamReader.Close();
                myResponseStream.Close();
            }
            catch (Exception ex)
            {
                HandleExceptionEvent(new KRException("HttpGetData", "http connection error", ex.Message));
            }
       
            return retString;
        }

        protected override void ReleaseManager()
        {

        }
    }
}
