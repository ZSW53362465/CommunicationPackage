using Chioy.Communication.Networking.Client;
using Chioy.Communication.Networking.Client.Client;
using Chioy.Communication.Networking.Client.Helper;
using Chioy.Communication.Networking.Common;
using Chioy.Communication.Networking.Models.DTO;
using Chioy.Communication.Networking.Models.ReportMetadata;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Text;
using System.Threading.Tasks;

namespace TestCallWebServiceClient
{
    class Program
    {
        static void Main(string[] args)
        {
            //ClientProxy<BMDCheckResult> proxy = new ClientProxy<BMDCheckResult>();
            //proxy.ConfigClient(ProductType.BMD, Protocol.WebService);

            ////Use CallUnknowWebService
            //var webSvcClient = proxy.ClientObj as WebServiceClient<BMDCheckResult>;
            //string[] param = { "20160101" };
            //var result = webSvcClient.CallUnknowWebService("http://localhost:51374/WebService1.asmx", "GetPatient", param);
            //var patient = CommunicationHelper.DeserializeJsonToObj<Patient_DTO>(result);

            //Use config
            //[NET_CONFIG]
            //BassAddress=localhost
            //Port = 51374
            //[BUSINESS]
            //GetPatientUrl=WebService1.asmx
            //PostCheckResultUrl = WebService1.asmx
            //var patient1 = proxy.GetPatient("20160101");

            //ExamResultMetadata<BMDCheckResult> bmdCheckResult = new ExamResultMetadata<BMDCheckResult>()
            //{
            //    Diagnosis = "everything is good",
            //    BrithDay = "19870808",
            //    Age = 30,
            //    CardID = "131002198702274615",
            //    CardType = CardType.IDCard,
            //    Name = "zhangshiwei",
            //    Result = new BMDCheckResult()
            //    {
            //        Position = "zuo",
            //        EOA = 22,
            //        Fracturerisk = 55,
            //        HP = 90,
            //        LimbSide = "123",
            //        PAB = 33,
            //        TValue = 33,
            //        ZValue = 89,
            //        RRF = 98,
            //        Percentage = 76,
            //        Physical = "444",
            //        SOS = 4233,
            //        STI = 34
            //    }

            //};
            ClientProxy<APIPWVCheckResult> proxy = new ClientProxy<APIPWVCheckResult>();
            //proxy.ConfigClient(Protocol.Http);
            ExamResultMetadata<APIPWVCheckResult> ABIPWV = new ExamResultMetadata<APIPWVCheckResult>()
            {
                Diagnosis = "everything is good",
                BrithDay = "19870808",
                Age = 30,
                CardID = "131002198702274615",
                CardType = CardType.IDCard,
                Name = "zhangshiwei",
                Result = new APIPWVCheckResult()
                {
                    ABIL = "3212",
                    ABIR = "343",
                    BAIL = "454",
                    BAIR = "53",
                    DBPLA = 123,
                    DBPLB = 5656,
                    DBPRA = 44,
                    DBPRB = 454,
                    MBPLA = 123,
                    MBPLB = 123,
                    MBPRA = 645,
                    MBPRB = 654,
                    PPLA = 64,
                    PPLB = 345,
                    PPRA = 435,
                    PPRB = 456,
                    PWVL = 123,
                    PWVR = 234,
                    SBPLA = 345,
                    SBPLB = 345,
                    SBPRA = 567,
                    SBPRB = 77
                }
            };

            string json = CommunicationHelper.SerializeObjToJsonStr(ABIPWV);
            var response = proxy.SendExamResult(ABIPWV);
            //string result1 = PostWebRequest("http://121.42.41.188:8080/PostCheckExam", "checkresult=" + json, Encoding.UTF8);

            var response1=CreateGetHttpResponse(string.Format("{0}{1}", "http://121.42.41.188:8080/AAS/PostCheckExam?checkresult=",
                json));

            Console.Read();
            //ExamResultMetadata<KRSGCheckResult> krsgCheckresult = new ExamResultMetadata<KRSGCheckResult>()
            //{
            //    Diagnosis = "everything is good",
            //    BrithDay = "19870808",
            //    Age = 30,
            //    CardID = "131002198702274615",
            //    CardType = CardType.IDCard,
            //    Name = "zhangshiwei",
            //    Result = new KRSGCheckResult()
            //    {
            //        BMI = 24.1f,
            //        Brithday = "1987/01/01",
            //        CurrentCount = 3,
            //        DeviceID = "2015354878512",
            //        Gender = 1,
            //        Height = 198,
            //        MeasureDateTime = "2016/05/09",
            //        Order = 4,
            //        PatientID = "201650548",
            //        PatientName = "小明",
            //        ScanCode = "65584",
            //        Status = 2,
            //        Temperature = 39.5f,
            //        TotalCount = 44,
            //        TransferIndex = 234,
            //        TransferTotalCount = 2,
            //        Weight = 59.5f

            //    }
            //};

            //string json = CommunicationHelper.SerializeObjToJsonStr(krsgCheckresult);
            //ExamResultMetadata<TCDDopplerCheckResult> dopplerCheckresult = new ExamResultMetadata<TCDDopplerCheckResult>()
            //{
            //    Diagnosis = "everything is good",
            //    BrithDay = "19870808",
            //    Age = 30,
            //    CardID = "131002198702274615",
            //    CardType = CardType.IDCard,
            //    Name = "zhangshiwei",
            //    Result = new TCDDopplerCheckResult()
            //    {
            //        Age = 20,
            //        BaseLine = 33,
            //        Depth = 4,
            //        Filter = 6,
            //        Gain = 6,
            //        Gender = 1,
            //        GraphUse = false,
            //        HR = 4,
            //        ImageFilePath = "",
            //        ListUse = false,
            //        Number = 5,
            //        PI = 55,
            //        PRF = 55,
            //        ProbeHz = 33,
            //        ProberDirection = 3,
            //        ProbeType = 3,
            //        RI = 5,
            //        SBI = 55,
            //        Scale = 5,
            //        SD = 3,
            //        StudyUID = "",
            //        UID = "",
            //        UseIndex = 3,
            //        Vs = 43,
            //        Vd = 34,
            //        Vm = 1


            //    }
            //};

            //string json = CommunicationHelper.SerializeObjToJsonStr(dopplerCheckresult);

            //var response = proxy.SendExamResult(bmdCheckResult);


            Console.Read();
        }

        public static string CreateGetHttpResponse(string url)
        {
            System.Net.HttpWebRequest request;
            // 创建一个HTTP请求
            request = (System.Net.HttpWebRequest)WebRequest.Create(url);
            request.Method="get";
            System.Net.HttpWebResponse response;
            response = (System.Net.HttpWebResponse)request.GetResponse();
            System.IO.StreamReader myreader = new System.IO.StreamReader(response.GetResponseStream(), Encoding.UTF8);
            string responseText = myreader.ReadToEnd();
            return responseText;
        }

        private static string PostWebRequest(string postUrl, string paramData, Encoding dataEncode)
        {
            string ret = string.Empty;
            try
            {
                byte[] byteArray = dataEncode.GetBytes(paramData); //转化
                HttpWebRequest webReq = (HttpWebRequest)WebRequest.Create(new Uri(postUrl));
                webReq.Method = "POST";
                webReq.ContentType = "application/text";

                webReq.ContentLength = byteArray.Length;
                Stream newStream = webReq.GetRequestStream();
                newStream.Write(byteArray, 0, byteArray.Length);//写入参数
                newStream.Close();
                HttpWebResponse response = (HttpWebResponse)webReq.GetResponse();
                StreamReader sr = new StreamReader(response.GetResponseStream(), Encoding.Default);
                ret = sr.ReadToEnd();
                sr.Close();
                response.Close();
                newStream.Close();
            }
            catch (Exception ex)
            {
            }
            return ret;
        }
    }
}
