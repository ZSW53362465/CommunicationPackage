using Chioy.Communication.Networking.Client.HTTP;
using Chioy.Communication.Networking.Common;
using Chioy.Communication.Networking.Models.DTO;
using Chioy.Communication.Networking.Models.ReportMetadata;
using System;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Runtime;
using System.Text;
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

        #region override
        public override Patient_DTO GetPatient(string patientId)
        {
            try
            {
                var patientUrl = Config.HttpConfigModel.PatientUrl;
                CommunicationHelper.RecordTrace("GetPatient", $"开始获取病人{patientId}的信息");
                Patient_DTO patient = null;
                if (_helper == null) return null;
                CommunicationHelper.RecordTrace("GetPatient", $"获取病人信息地址为{patientUrl}");
                string url = string.Empty;

                if (patientUrl.EndsWith("=") || patientUrl.EndsWith("/"))
                {
                    url = patientUrl + patientId;
                }
                else
                {
                    url = patientUrl + "/" + patientId;
                }

                var finalUrl = AddToken(url);

                _helper.NeedUrlDecode = NeedUrlDecode;
                patient = _helper.HttpGetData<Patient_DTO>(finalUrl);
                return patient;
            }
            catch (KRException ex)
            {
                throw new Exception(ex.Msg);
            }
        }

        public override KRResponse PostExamResult(ExamResultMetadata<T> checkResult)
        {
            try
            {
                var postResultUrl = Config.HttpConfigModel.PostCheckResultUrl;
                var parameterName = Config.HttpConfigModel.ResultParameterName;
                Trace.WriteLine(string.Format("开始发送检查结果，地址为{0}", postResultUrl));
                if (!string.IsNullOrEmpty(parameterName))
                    checkResult.ParamterName = parameterName;

                var pitem = new HttpItem();

                BuildHttpItem(checkResult, pitem, postResultUrl, parameterName);

                var result = PostData(pitem);

                KRResponse response = new KRResponse();
                if (result.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    response.Msg = "联网数据上传成功";
                    response.Status = "SUCCESS";
                }
                else
                {
                    response.Msg = "联网数据上传失败";
                    response.Status = "FAIL";
                }

                Trace.WriteLine(string.Format("开始发送检查结果结束，返回结果{0}", response.Status));
                return response;
            }
            catch (Exception ex)
            {
                throw new Exception("上传检查结果失败");
            }
        }

        #endregion

        #region private
        private string AddToken(string url)
        {
            if (!Config.HttpConfigModel.IsUseToken) return url;

            var finalUrl = string.Empty;

            try
            {
                var token = GetData<string>(Config.HttpConfigModel.TokenUrl);

                if (!string.IsNullOrEmpty(token))
                {
                    finalUrl = string.Format("{0}&token={1}", url, token);
                }
            }
            catch (Exception ex)
            {
                throw new KRException("AddToken", "获取Token失败", ex.Message);
            }

            return finalUrl;
        }
        private void BuildHttpItem(ExamResultMetadata<T> checkResult, HttpItem pitem, string postResultUrl, string parameterName)
        {
            pitem.URL = postResultUrl;
            if (string.IsNullOrEmpty(parameterName))
            {
                pitem.Postdata = CommunicationHelper.SerializeObjToJsonStr(checkResult);
                pitem.ContentType = "application/json";
            }
            else
            {
                pitem.Postdata = AddToken(string.Format("{0}={1}", parameterName,
                    CommunicationHelper.SerializeObjToJsonStr(checkResult)));
                pitem.ContentType = "application/x-www-form-urlencoded";
                pitem.PostEncoding = Encoding.UTF8;
            }
        }
        #endregion

        #region public
        public TOut PostData<TIn, TOut>(string url, TIn obj, HttpItem pitem = null)
        {
            try
            {
                return _helper.HttpPostData<TIn, TOut>(url, obj, pitem);
            }
            catch (Exception ex)
            {
                throw new Exception("上传数据失败");
            }
        }
        public string PostData<TIn>(string url, TIn obj, HttpItem pitem = null)
        {
            try
            {
                return _helper.HttpPostData(url, obj, pitem);
            }
            catch (Exception ex)
            {
                throw new Exception("上传数据失败");
            }
        }
        public TOut GetData<TOut>(string url, HttpItem pitem = null)
        {
            try
            {
                return _helper.HttpGetData<TOut>(url, pitem);
            }
            catch (Exception ex)
            {
                throw new Exception("获取数据失败", ex);
            }
        }
        public string GetData(string url, HttpItem pitem = null)
        {
            try
            {
                return _helper.HttpGetData(url, pitem);
            }
            catch (Exception ex)
            {
                throw new Exception("获取数据失败", ex);
            }
        }
        public HttpResult GetData(HttpItem pitem)
        {
            try
            {
                return _helper.HttpGetData(pitem);
            }
            catch (Exception ex)
            {
                throw new Exception("获取数据失败", ex);
            }
        }
        public HttpResult PostData(HttpItem pitem, string postData)
        {
            try
            {
                return _helper.HttpPostData(pitem, postData);
            }
            catch (Exception ex)
            {
                throw new Exception("上传数据失败");
            }
        }
        public HttpResult PostData(HttpItem pitem)
        {
            try
            {
                return _helper.HttpPostData(pitem);
            }
            catch (Exception ex)
            {
                throw new Exception("上传数据失败");
            }
        }
        #endregion

        //public override KRResponse PostOperator(Operator_DTO op)
        //{
        //    try
        //    {
        //        Trace.WriteLine(string.Format("开始发送操作人员信息，地址为{0}", Address.PostOperatorUrl));
        //        _helper.Timeout = Timeout;
        //        _helper.NeedUrlDecode = NeedUrlDecode;
        //        var resultStr = _helper.HttpPostData(Address.PostOperatorUrl, op, ClientConstants.KR_POST_OPERATOR);
        //        Trace.WriteLine(string.Format("开始发送操作人员信息结束，返回结果{0}", resultStr));
        //        return CommunicationHelper.DeserializeJsonToObj<KRResponse>(resultStr);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }

        //}
    }
}
