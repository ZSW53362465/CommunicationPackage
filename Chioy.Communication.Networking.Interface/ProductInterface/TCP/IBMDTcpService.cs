using System.ServiceModel;
using Chioy.Communication.Networking.Models.DTO;

namespace Chioy.Communication.Networking.Interface.ProductInterface.TCP
{
    [ServiceContract(SessionMode = SessionMode.Required, CallbackContract = typeof(IKRDuplexCallback))]
    public interface IBMDTcpService:IService
    {
        [OperationContract]
        Patient_DTO GetPatient(string patientId);

        [OperationContract]
        KRResponse PostExamResult(string resultJson);
    }
}
