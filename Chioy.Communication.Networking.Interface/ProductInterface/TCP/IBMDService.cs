using Chioy.Communication.Networking.Models;
using Chioy.Communication.Networking.Models.DTO;
using System.Collections.Generic;
using System.ServiceModel;

namespace Chioy.Communication.Networking.Interface
{
    [ServiceContract(SessionMode = SessionMode.Required, CallbackContract = typeof(IKRDuplexCallback))]
    public interface IBMDService: IService
    {
        #region Test
        [OperationContract]
        UserInfo CreateNewUser(string name);

        [OperationContract]
        List<UserInfo> GetAllUsers();

        [OperationContract(IsOneWay = true)]
        void RemoveUser(string id);
        #endregion

        [OperationContract]
        Patient_DTO RequestPatientByPatientID(string patientId);
        [OperationContract]
        List<Patient_DTO> RequestAllPatients();
        [OperationContract]
        List<BMD_Measure_Result_DTO> Request_BMD_Measure_ResultByPatientID(string patientId);
        [OperationContract]
        BMD_Measure_Result_DTO Request_BMD_Measure_Result(string checkId);
        [OperationContract]
        List<BMD_Measure_Result_DTO> Request_BMD_Measure_All_Result();
        [OperationContract]
        List<PatientCheck_DTO> Request_BMD_Summary_ResultByPatientID(string patientId);
        [OperationContract]
        List<PatientCheck_DTO> Request_BMD_Summary_All_Result();
        [OperationContract]
        PatientCheck_DTO Request_BMD_Summary_ResultByCheckId(string checkId);
    }
}
