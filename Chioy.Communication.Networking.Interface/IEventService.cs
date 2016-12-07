using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Chioy.Communication.Networking.Interface
{
    [ServiceContract(SessionMode = SessionMode.Required, CallbackContract = typeof(IEventCallback))]
    public interface IEventService
    {
        [OperationContract(IsOneWay = true)]
        void Subscribe(SubscribeArg a);
        [OperationContract(IsOneWay = true)]
        void Unsubscribe(ArgumentBase<string> a);

        [OperationContract]
        DateTime Ping();
    }

    public interface IEventCallback
    {
        [OperationContract(IsOneWay = true)]
        void OnEventFired(ArgumentBase<string> a);
    }
}
