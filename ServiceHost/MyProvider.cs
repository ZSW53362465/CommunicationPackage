using Chioy.Communication.Networking.Models.DTO;
using Chioy.Communication.Networking.Service.Provider;
using System;
using System.Collections.Generic;

namespace ServiceHost
{
    public class MyProvider : IBMDDataProvider
    {
        public List<Patient_DTO> RequestAllPatients()
        {
            return new List<Patient_DTO>();
        }

        public Patient_DTO RequestPatientByPatientID(string patientId)
        {
            throw new NotImplementedException();
        }

        public List<BMD_Measure_Result_DTO> Request_BMD_Measure_All_Result()
        {
            throw new NotImplementedException();
        }

        public BMD_Measure_Result_DTO Request_BMD_Measure_Result(string checkId)
        {
            throw new NotImplementedException();
        }

        public List<BMD_Measure_Result_DTO> Request_BMD_Measure_ResultByPatientID(string patientId)
        {
            throw new NotImplementedException();
        }

        public List<PatientCheck_DTO> Request_BMD_Summary_All_Result()
        {
            throw new NotImplementedException();
        }

        public PatientCheck_DTO Request_BMD_Summary_ResultByCheckId(string checkId)
        {
            throw new NotImplementedException();
        }

        public List<PatientCheck_DTO> Request_BMD_Summary_ResultByPatientID(string patientId)
        {
            throw new NotImplementedException();
        }
    }
}
