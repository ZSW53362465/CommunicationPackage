using Chioy.Communication.Networking.Models;
using Chioy.Communication.Networking.Models.ProductModel;
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
        Transfer_Patient RequestPatientByPatientID(string patientId);
        [OperationContract]
        List<Transfer_Patient> RequestAllPatients();
        [OperationContract]
        List<Transfer_BMD_Measure_Result> Request_BMD_Measure_ResultByPatientID(string patientId);
        [OperationContract]
        Transfer_BMD_Measure_Result Request_BMD_Measure_Result(string checkId);
        [OperationContract]
        List<Transfer_BMD_Measure_Result> Request_BMD_Measure_All_Result();
        [OperationContract]
        List<Transfer_PatientCheck> Request_BMD_Summary_ResultByPatientID(string patientId);
        [OperationContract]
        List<Transfer_PatientCheck> Request_BMD_Summary_All_Result();
        [OperationContract]
        Transfer_PatientCheck Request_BMD_Summary_ResultByCheckId(string checkId);
    }
}
