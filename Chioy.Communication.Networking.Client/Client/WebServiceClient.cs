using Chioy.Communication.Networking.Common;
using Chioy.Communication.Networking.Models.DTO;
using Chioy.Communication.Networking.Models.ReportMetadata;
using System;
using System.Diagnostics;
using System.Windows.Media.Imaging;

namespace Chioy.Communication.Networking.Client.Client
{
    public class WebServiceClient<T> : BaseClient<T> where T : BaseCheckResult
    {
        public WebServiceClient()
        {
            _protocol = Protocol.WebService;
        }

        public override Patient_DTO GetPatient(string patientId)
        {
            try
            {
                Trace.WriteLine(string.Format("开始获取病人{0}的信息", patientId));
                Trace.WriteLine(string.Format("获取病人信息地址为{0}", Config.HttpConfigModel.PatientUrl));
                var proxy = new WebServiceProxy(Config.HttpConfigModel.PatientUrl, Config.HttpConfigModel.WebServiceName);
                string[] param = { ClientConstants.KR_GET_PATIENT, patientId };
                var jsonStr = (string)proxy.ExecuteQuery(Config.HttpConfigModel.WebServiceName, param);
                Trace.WriteLine(string.Format("获取病人信息为{0}", jsonStr));
                return CommunicationHelper.DeserializeJsonToObj<Patient_DTO>(jsonStr);
            }
            catch (KRException ex)
            {
                throw new Exception("获取病人信息失败" + ex.Msg);
            }
        }
        public override KRResponse PostExamResult(ExamResultMetadata<T> result)
        {
            try
            {
                var resultStr = CommunicationHelper.SerializeObjToJsonStr(result);
                string[] param = { ClientConstants.KR_POST_RESULT, resultStr };
                var proxy = new WebServiceProxy(Config.HttpConfigModel.PostCheckResultUrl, Config.HttpConfigModel.WebServiceName);
                Trace.WriteLine(string.Format("开始发送检查结果，地址为{0}", Config.HttpConfigModel.PostCheckResultUrl));
                var response = (string)proxy.ExecuteQuery(Config.HttpConfigModel.WebServiceName, param);
                Trace.WriteLine(string.Format("开始发送检查结果结束，返回结果{0}", resultStr));

                return CommunicationHelper.DeserializeJsonToObj<KRResponse>(response);
            }
            catch (KRException ex)
            {
                throw new Exception("上传检查结果失败 " + ex.Msg);
            }

        }
        public override KRResponse PostOperator(Operator_DTO op)
        {
            try
            {
                return null;
                //var resultStr = CommunicationHelper.SerializeObjToJsonStr(op);
                //string[] param = { ClientConstants.KR_POST_RESULT, resultStr };
                //Trace.WriteLine(string.Format("开始发送操作人员信息，地址为{0}", Address.PostOperatorUrl));
                //var proxy = new WebServiceProxy(Address.PostOperatorUrl, ClientConstants.DoAction);
                //var response = (string)proxy.ExecuteQuery(ClientConstants.DoAction, param);
                //Trace.WriteLine(string.Format("开始发送操作人员信息结束，返回结果{0}", response));
                //return CommunicationHelper.DeserializeJsonToObj<KRResponse>(response);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string CallUnknowWebService(string url, string method, string[] param)
        {
            try
            {
                var proxy = new WebServiceProxy(url, method);
                return (string)proxy.ExecuteQuery(method, param);
            }
            catch (Exception ex)
            {
                throw new Exception("调用WebService" + method + "失败" + ex.Message);
            }
        }
    }
}
