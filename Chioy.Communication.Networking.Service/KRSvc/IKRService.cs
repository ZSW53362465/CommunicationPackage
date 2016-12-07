using KRWcfLib.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace KRWcfLib.KRSvc
{
    [ServiceContract(SessionMode = SessionMode.Required, CallbackContract = typeof(IKRDuplexCallback))]
    public interface IKRService
    {
        [OperationContract]
        UserInfo CreateNewUser(string name);

        [OperationContract]
        List<UserInfo> GetAllUsers();

        [OperationContract(IsOneWay = true)]
        void RemoveUser(string id);
    }
}
