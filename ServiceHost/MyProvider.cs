using Chioy.Communication.Networking.Models.DTO;
using Chioy.Communication.Networking.Service.Provider;
using System;
using System.Collections.Generic;
using Chioy.Communication.Networking.Common;
using Chioy.Communication.Networking.Models.ReportMetadata;

namespace ServiceHost
{
    public class MyProvider : IBMDDataProvider
    {
        public KRResponse PostExamResult(ExamResultMetadata<BMDCheckResult> result)
        {
            if (result != null)
            {
                return new KRResponse() { Status = "SUCCESS", Msg = "" };
            }
            return null;
        }

        public Patient_DTO GetPatient(string patientId)
        {
            var patient = new Patient_DTO()
            {
                PatientID = patientId,
                Name = "张士威",
                Age = 27,
                Address = "河北廊坊",
                Birthday = DateTime.Now.AddYears(-30).ToShortDateString(),
                City = "廊坊",
                IdentityType = 0,
                CredentialName = "身份证",
                FirstName = "Zhang",
                Work = "IT",
                Gender = 1,
                Nation = "汉"
            };

            return patient;
        }
    }
}
