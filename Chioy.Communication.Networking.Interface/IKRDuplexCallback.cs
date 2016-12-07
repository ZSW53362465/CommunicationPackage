using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Chioy.Communication.Networking.Interface
{
    public interface IKRDuplexCallback
    {
        [OperationContract(IsOneWay = true)]
        void CreateNewUserInClient();

        [OperationContract(IsOneWay = true)]
        void RemoveUserInClient();
    }
}
