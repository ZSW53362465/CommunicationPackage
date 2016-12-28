using Chioy.Communication.Networking.Common;
using Chioy.Communication.Networking.Models.DTO;
using Chioy.Communication.Networking.Models.ReportMetadata;
using System;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Windows.Media.Imaging;

namespace Chioy.Communication.Networking.Client.Client
{
    public class HttpClient : BaseClient
    {
        HttpHelper _helper = null;
        public int Timeout { get; set; }
        public HttpClient()
        {
            _protocol = Protocol.Http;
            _helper = new HttpHelper();

        }
        public override void ConfigClient()
        {
            base.ConfigClient();
        }

        public override Patient_DTO GetPatient(string patientId)
        {
            try
            {
                Trace.WriteLine(string.Format("开始获取病人{0}的信息", patientId));
                Patient_DTO patient = null;
                if (_helper != null)
                {
                    NameValueCollection param = new NameValueCollection();
                    param.Add(ClientConstants.Type, ClientConstants.KR_GET_PATIENT);
                    param.Add(ClientConstants.Content, patientId);
                    Trace.WriteLine(string.Format("获取病人信息地址为{0}", Address.GetPatientUrl));
                    var jsonStr = _helper.HttpGetData(Address.GetPatientUrl, param);
                    Trace.WriteLine(string.Format("获取病人信息为{0}", jsonStr));
                    patient = CommunicationHelper.DeserializeJsonToObj<Patient_DTO>(jsonStr);
                }
                return patient;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public override KRResponse PostExamResult(ExamResultMetadata<BaseCheckResult> result, Func<ExamResultMetadata<BaseCheckResult>, RenderTargetBitmap> function = null)
        {
            try
            {
                Trace.WriteLine(string.Format("开始发送检查结果，地址为{0}", Address.PostCheckResultUrl));
                string resultStr = _helper.HttpPostData(Address.PostCheckResultUrl, result, Timeout, ClientConstants.KR_POST_RESULT);
                Trace.WriteLine(string.Format("开始发送检查结果结束，返回结果{0}", resultStr));
                return CommunicationHelper.DeserializeJsonToObj<KRResponse>(resultStr);
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
                string resultStr = _helper.HttpPostData(Address.PostOperatorUrl, op, Timeout, ClientConstants.KR_POST_OPERATOR);
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
