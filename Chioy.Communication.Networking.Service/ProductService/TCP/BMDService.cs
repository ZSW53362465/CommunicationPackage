using Chioy.Communication.Networking.Interface;
using Chioy.Communication.Networking.Models;
using Chioy.Communication.Networking.Service.Provider;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.ServiceModel;
using Chioy.Communication.Networking.Models.DTO;

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

        public Patient_DTO RequestPatientByPatientID(string patientId)
        {
            return Provider.RequestPatientByPatientID(patientId);
        }

        public List<Patient_DTO> RequestAllPatients()
        {
            return Provider.RequestAllPatients();
        }

        public List<BMD_Measure_Result_DTO> Request_BMD_Measure_ResultByPatientID(string patientId)
        {
            return Provider.Request_BMD_Measure_ResultByPatientID(patientId);
        }

        public BMD_Measure_Result_DTO Request_BMD_Measure_Result(string checkId)
        {
            return Provider.Request_BMD_Measure_Result(checkId);
        }

        public List<BMD_Measure_Result_DTO> Request_BMD_Measure_All_Result()
        {
            return Provider.Request_BMD_Measure_All_Result();
        }

        public List<PatientCheck_DTO> Request_BMD_Summary_ResultByPatientID(string patientId)
        {
            return Provider.Request_BMD_Summary_ResultByPatientID(patientId);
        }

        public List<PatientCheck_DTO> Request_BMD_Summary_All_Result()
        {
            return Provider.Request_BMD_Summary_All_Result();
        }

        public PatientCheck_DTO Request_BMD_Summary_ResultByCheckId(string checkId)
        {
            return Provider.Request_BMD_Summary_ResultByCheckId(checkId);
        }
    }
}
