using Chioy.Communication.Networking.Common;
using Chioy.Communication.Networking.Models.DTO;
using Chioy.Communication.Networking.Models.ReportMetadata;
using System;
using System.Diagnostics;

namespace Chioy.Communication.Networking.Client.Client
{
    public class WebServiceClient : BaseClient
    {


        public WebServiceClient()
        {
            _protocol = Protocol.WebService;
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
                Trace.WriteLine(string.Format("获取病人信息地址为{0}", Address.GetPatientUrl));
                var proxy = new WebServiceProxy(Address.GetPatientUrl, ClientConstants.DoAction);
                string[] param = { ClientConstants.KR_GET_PATIENT, patientId };
                var jsonStr = (string)proxy.ExecuteQuery(ClientConstants.DoAction, param);
                Trace.WriteLine(string.Format("获取病人信息为{0}", jsonStr));
                return CommunicationHelper.DeserializeJsonToObj<Patient_DTO>(jsonStr);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public override KRResponse PostExamResult(ExamResultMetadata<BaseCheckResult> result)
        {
            try
            {
                var resultStr = CommunicationHelper.SerializeObjToJsonStr(result);
                string[] param = { ClientConstants.KR_POST_RESULT, resultStr };
                var proxy = new WebServiceProxy(Address.PostCheckResultUrl, ClientConstants.DoAction);
                Trace.WriteLine(string.Format("开始发送检查结果，地址为{0}", Address.PostCheckResultUrl));
                var response = (string)proxy.ExecuteQuery(ClientConstants.DoAction, param);
                Trace.WriteLine(string.Format("开始发送检查结果结束，返回结果{0}", resultStr));

                return CommunicationHelper.DeserializeJsonToObj<KRResponse>(response);
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
                var resultStr = CommunicationHelper.SerializeObjToJsonStr(op);
                string[] param = { ClientConstants.KR_POST_RESULT, resultStr };
                Trace.WriteLine(string.Format("开始发送操作人员信息，地址为{0}", Address.PostOperatorUrl));
                var proxy = new WebServiceProxy(Address.PostOperatorUrl, ClientConstants.DoAction);
                var response = (string)proxy.ExecuteQuery(ClientConstants.DoAction, param);
                Trace.WriteLine(string.Format("开始发送操作人员信息结束，返回结果{0}", response));
                return CommunicationHelper.DeserializeJsonToObj<KRResponse>(response);
            }
            catch (Exception ex)
            {
                throw ex;
            }
          
        }
    }
}
