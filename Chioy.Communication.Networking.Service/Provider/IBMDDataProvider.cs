using Chioy.Communication.Networking.Models.DTO;
using System.Collections.Generic;

namespace Chioy.Communication.Networking.Service.Provider
{
    public interface IBMDDataProvider : IDataProvider
    {
        Patient_DTO RequestPatientByPatientID(string patientId);

        List<Patient_DTO> RequestAllPatients();

        List<BMD_Measure_Result_DTO> Request_BMD_Measure_ResultByPatientID(string patientId);

        BMD_Measure_Result_DTO Request_BMD_Measure_Result(string checkId);

        List<BMD_Measure_Result_DTO> Request_BMD_Measure_All_Result();

        List<PatientCheck_DTO> Request_BMD_Summary_ResultByPatientID(string patientId);

        List<PatientCheck_DTO> Request_BMD_Summary_All_Result();

        PatientCheck_DTO Request_BMD_Summary_ResultByCheckId(string checkId);
    }
}
