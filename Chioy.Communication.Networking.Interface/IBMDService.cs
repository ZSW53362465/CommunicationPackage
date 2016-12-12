using Chioy.Communication.Networking.Models;
using System.Collections.Generic;
using System.ServiceModel;

namespace Chioy.Communication.Networking.Interface
{
    [ServiceContract(SessionMode = SessionMode.Required, CallbackContract = typeof(IKRDuplexCallback))]
    public interface IBMDService: IService
    {
        [OperationContract]
        UserInfo CreateNewUser(string name);

        [OperationContract]
        List<UserInfo> GetAllUsers();

        [OperationContract(IsOneWay = true)]
        void RemoveUser(string id);
    }
}
