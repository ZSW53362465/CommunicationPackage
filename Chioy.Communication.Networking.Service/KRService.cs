using Chioy.Communication.Networking.Interface;
using Chioy.Communication.Networking.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;

namespace Chioy.Communication.Networking.Service
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerSession)]
    public class KRService : IKRService
    {
        //IKRDuplexCallback callback = null;

        public KRService()
        {
            //callback = OperationContext.Current.GetCallbackChannel<IKRDuplexCallback>();
        }

        List<UserInfo> _userList = new List<UserInfo>();
        public UserInfo CreateNewUser(string name)
        {
            var newUser = new UserInfo() { Name = name, ID = Guid.NewGuid().ToString(), Age = 21, Sex = 1 };
            _userList.Add(newUser);
            //callback.CreateNewUserInClient();
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
