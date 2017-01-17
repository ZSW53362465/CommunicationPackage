using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.ServiceModel;
using Chioy.Communication.Networking.Common;
using Chioy.Communication.Networking.Interface.ProductInterface.TCP;
using Chioy.Communication.Networking.Models;
using Chioy.Communication.Networking.Models.DTO;
using Chioy.Communication.Networking.Models.ReportMetadata;
using Chioy.Communication.Networking.Service.Provider;

namespace Chioy.Communication.Networking.Service.ProductService.TCP
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public class BMDTcpService : DataProviderAdpter, IBMDTcpService
    {
        private IBMDDataProvider Provider
        {
            get { return _provider as IBMDDataProvider; }
        }
        //IKRDuplexCallback callback = null;

        public BMDTcpService()
        {
            //callback = OperationContext.Current.GetCallbackChannel<IKRDuplexCallback>();
        }
        #region Test

        public List<UserInfo> _userList = new List<UserInfo>();
        public UserInfo CreateNewUser(string name)
        {
            var newUser = new UserInfo() { Name = name, ID = Guid.NewGuid().ToString(), Age = 21, Sex = 1 };
            _userList.Add(newUser);
            Trace.WriteLine("WCF:CreateNewUser: " + name);
            return newUser;
        }

        public List<UserInfo> GetAllUsers()
        {
            return _userList;
        }

        public void RemoveUser(string id)
        {
            _userList.Remove(_userList.FirstOrDefault(u => u.ID == id));
        }

        #endregion
        public Patient_DTO GetPatient(string patientId)
        {
            return Provider.GetPatient(patientId);
        }

        public KRResponse PostExamResult(string resultJson)
        {
            var result = CommunicationHelper.DeserializeJsonToObj<ExamResultMetadata<BMDCheckResult>>(resultJson);
            return Provider.PostExamResult(result);
        }
    }
}
