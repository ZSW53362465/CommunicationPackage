using KRWcfLib.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace KRWcfLib.KRSvc
{
    //暂时用不到回调方式的双工，但是例子暂留
    public interface IKRDuplexCallback
    {
        [OperationContract(IsOneWay = true)]
        void CreateNewUserInClient();

        [OperationContract(IsOneWay = true)]
        void RemoveUserInClient();
    }
}
