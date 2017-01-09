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
                Name = "路飞",
                Age = 27,
                Address = "河北廊坊",
                Birthday = DateTime.Now.AddYears(-30).ToShortDateString(),
                City = "廊坊",
                IdentityType = 0,
                CredentialName = "身份证",
                FirstName = "Zhang",
                Work = "IT",
                Gender = 1,
                Nation = "汉",
                CustomerFields = new List<CustomerFields>() { new CustomerFields() { PatientID = patientId, DataType = "string", FieldName = "别名", FieldValue = "海贼王" } }
            };

            return patient;
        }
    }
}
