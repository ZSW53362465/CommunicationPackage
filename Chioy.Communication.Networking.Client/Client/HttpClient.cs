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

        public override Patient_DTO GetPatient(string patientId, HttpItem pitem = null)
        {
            try
            {
                CommunicationHelper.RecordTrace("GetPatient", $"开始获取病人{patientId}的信息");
                Patient_DTO patient = null;
                if (_helper == null) return null;
                CommunicationHelper.RecordTrace("GetPatient", $"获取病人信息地址为{Address.GetPatientUrl}");
                string url = string.Empty;
                if (Address.GetPatientUrl.EndsWith("=") || Address.GetPatientUrl.EndsWith("/"))
                {
                    url = Address.GetPatientUrl + patientId;
                }
                else
                {
                    url = Address.GetPatientUrl + "/" + patientId;
                }
                _helper.NeedUrlDecode = NeedUrlDecode;
                patient = _helper.HttpGetData<Patient_DTO>(url, pitem);
                return patient;
            }
            catch (Exception ex)
            {
                throw new Exception("获取病人信息失败");
            }
        }

        public override KRResponse PostExamResult(ExamResultMetadata<T> checkResult, HttpItem pitem = null)
        {
            try
            {
                Trace.WriteLine(string.Format("开始发送检查结果，地址为{0}", Address.PostCheckResultUrl));
                if (!string.IsNullOrEmpty(Address.PostResultParamter))
                    checkResult.ParamterName = Address.PostResultParamter;
                if (pitem == null)
                {
                    pitem = new HttpItem
                    {
                        URL = Address.PostCheckResultUrl,
                        Method = "Post",
                        Postdata = !string.IsNullOrEmpty(Address.PostResultParamter)
                            ? string.Format("{0}={1}", Address.PostResultParamter,
                                CommunicationHelper.SerializeObjToJsonStr(checkResult))
                            : CommunicationHelper.SerializeObjToJsonStr(checkResult),
                        ContentType = "application/x-www-form-urlencoded",
                        PostEncoding = Encoding.UTF8

                    };
                }
                
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
