using Chioy.Communication.Networking.Interface;
using Chioy.Communication.Networking.Models;
using Chioy.Communication.Networking.Service.Provider;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.ServiceModel;

namespace Chioy.Communication.Networking.Service.ProductService
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public class BMDService : KRService, IBMDService
    {
        private IBMDDataProvider Provider
        {
            get { return _provider as IBMDDataProvider; }
        }
        //IKRDuplexCallback callback = null;

        public BMDService()
        {
            //callback = OperationContext.Current.GetCallbackChannel<IKRDuplexCallback>();
        }

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
    }
}
