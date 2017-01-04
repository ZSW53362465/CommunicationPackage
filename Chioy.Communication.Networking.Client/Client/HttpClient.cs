using Chioy.Communication.Networking.Common;
using Chioy.Communication.Networking.Models.DTO;
using Chioy.Communication.Networking.Models.ReportMetadata;
using System;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Runtime;
using System.Windows.Media.Imaging;

namespace Chioy.Communication.Networking.Client.Client
{
    public class HttpClient<T> : BaseClient<T> where T : BaseCheckResult
    {
        HttpHelper _helper = null;

        public int Timeout { get; set; }
        public bool NeedUrlDecode { get; set; }
        public HttpClient()
        {
            _protocol = Protocol.Http;
            _helper = new HttpHelper();

        }

        public override Patient_DTO GetPatient(string patientId)
        {
            try
            {
                Trace.WriteLine($"开始获取病人{patientId}的信息");
                Patient_DTO patient = null;
                if (_helper == null) return null;
                Trace.WriteLine($"获取病人信息地址为{Address.GetPatientUrl}");
                var url = Address.GetPatientUrl +
                          $"?{ClientConstants.Type}={ClientConstants.KR_GET_PATIENT}&{ClientConstants.Content}={patientId}";
                _helper.NeedUrlDecode = NeedUrlDecode;
                var jsonStr = _helper.HttpGetData(url);
                Trace.WriteLine($"获取病人信息为{jsonStr}");
                patient = CommunicationHelper.DeserializeJsonToObj<Patient_DTO>(jsonStr);
                return patient;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public override KRResponse PostExamResult(ExamResultMetadata<T> result)
        {
            try
            {
                Trace.WriteLine(string.Format("开始发送检查结果，地址为{0}", Address.PostCheckResultUrl));
                _helper.Timeout = Timeout;
                _helper.NeedUrlDecode = NeedUrlDecode;
                string resultStr = _helper.HttpPostData(Address.PostCheckResultUrl, result, ClientConstants.KR_POST_RESULT);
                Trace.WriteLine(string.Format("开始发送检查结果结束，返回结果{0}", resultStr));
                return CommunicationHelper.DeserializeJsonToObj<KRResponse>(resultStr);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public TOut PostData<TIn, TOut>(string url, TIn obj)
        {
            try
            {
                _helper.NeedUrlDecode = NeedUrlDecode;
                var result = _helper.HttpPostData(url, obj);
                return CommunicationHelper.DeserializeJsonToObj<TOut>(result);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw ex;
            }
        }

        public string PostData<TIn>(string url, TIn obj)
        {
            try
            {
                _helper.NeedUrlDecode = NeedUrlDecode;
                return _helper.HttpPostData(url, obj);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string PostData(string url, NameValueCollection pCollection)
        {
            try
            {
                _helper.NeedUrlDecode = NeedUrlDecode;
                return _helper.HttpPostData(url, pCollection);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public TOut GetData<TOut>(string url)
        {
            try
            {
                _helper.NeedUrlDecode = NeedUrlDecode;
                return _helper.HttpGetData<TOut>(url);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public override KRResponse PostOperator(Operator_DTO op)
        {
            try
            {
                Trace.WriteLine(string.Format("开始发送操作人员信息，地址为{0}", Address.PostOperatorUrl));
                _helper.Timeout = Timeout;
                _helper.NeedUrlDecode = NeedUrlDecode;
                var resultStr = _helper.HttpPostData(Address.PostOperatorUrl, op, ClientConstants.KR_POST_OPERATOR);
                Trace.WriteLine(string.Format("开始发送操作人员信息结束，返回结果{0}", resultStr));
                return CommunicationHelper.DeserializeJsonToObj<KRResponse>(resultStr);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
    }
}
