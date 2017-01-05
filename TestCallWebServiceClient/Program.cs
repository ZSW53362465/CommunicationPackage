using Chioy.Communication.Networking.Client;
using Chioy.Communication.Networking.Client.Client;
using Chioy.Communication.Networking.Client.Helper;
using Chioy.Communication.Networking.Common;
using Chioy.Communication.Networking.Models.DTO;
using Chioy.Communication.Networking.Models.ReportMetadata;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestCallWebServiceClient
{
    class Program
    {
        static void Main(string[] args)
        {
            ClientProxy<BMDCheckResult> proxy = new ClientProxy<BMDCheckResult>();
            proxy.ConfigClient(ProductType.BMD, Protocol.WebService);

            //Use CallUnknowWebService
            var webSvcClient = proxy.ClientObj as WebServiceClient<BMDCheckResult>;
            string[] param = { "20160101" };
            var result = webSvcClient.CallUnknowWebService("http://localhost:51374/WebService1.asmx", "GetPatient", param);
            var patient = CommunicationHelper.DeserializeJsonToObj<Patient_DTO>(result);

            //Use config
            //[NET_CONFIG]
            //BassAddress=localhost
            //Port = 51374
            //[BUSINESS]
            //GetPatientUrl=WebService1.asmx
            //PostCheckResultUrl = WebService1.asmx
            var patient1 = proxy.GetPatient("20160101");

            ExamResultMetadata<BMDCheckResult> bmdCheckResult = new ExamResultMetadata<BMDCheckResult>()
            {
                Diagnosis = "everything is good",
                BrithDay = "19870808",
                Age = 30,
                CardID = "131002198702274615",
                CardType = CardType.IDCard,
                Name = "zhangshiwei",
                Result = new BMDCheckResult()
                {
                    Position = "zuo",
                    EOA = 22,
                    Fracturerisk = 55,
                    HP = 90,
                    LimbSide = "123",
                    PAB = 33,
                    TValue = 33,
                    ZValue = 89,
                    RRF = 98,
                    Percentage = 76,
                    Physical = "444",
                    SOS = 4233,
                    STI = 34
                }

            };
            var response = proxy.SendExamResult(bmdCheckResult);


            Console.Read();
        }
    }
}
