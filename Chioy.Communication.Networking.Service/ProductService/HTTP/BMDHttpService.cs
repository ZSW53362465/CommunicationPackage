using Chioy.Communication.Networking.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Chioy.Communication.Networking.Models;
using System.ServiceModel;
using Chioy.Communication.Networking.Service.ProductService;
using Chioy.Communication.Networking.Models.ProductModel;
using Chioy.Communication.Networking.Service.Provider;

namespace Chioy.Communication.Networking.Service
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public class BMDHttpService : KRService, IBMDHttpService
    {
        private IBMDDataProvider Provider
        {
            get { return _provider as IBMDDataProvider; }
        }
        #region Test
        public Employee Create(Employee employee)
        {
            return new Employee() { Id = "99", Name = "CC" };
        }

        public void Delete(string id)
        {
            throw new NotImplementedException();
        }

        public Employee Get(string id, string test)
        {
            return new Employee() { Id = "99", Name = "CC" };
        }
        public void Update(Employee employee)
        {
            throw new NotImplementedException();
        }
        public IEnumerable<Employee> GetAll()
        {
            throw new NotImplementedException();
        }
        #endregion
        public Transfer_Patient RequestPatientByPatientID(string patientId)
        {
            return Provider.RequestPatientByPatientID(patientId);
        }

        public List<Transfer_Patient> RequestAllPatients()
        {
            return Provider.RequestAllPatients();
        }

        public List<Transfer_BMD_Measure_Result> Request_BMD_Measure_ResultByPatientID(string patientId)
        {
            return Provider.Request_BMD_Measure_ResultByPatientID(patientId);
        }

        public Transfer_BMD_Measure_Result Request_BMD_Measure_Result(string checkId)
        {
            return Provider.Request_BMD_Measure_Result(checkId);
        }

        public List<Transfer_BMD_Measure_Result> Request_BMD_Measure_All_Result()
        {
            return Provider.Request_BMD_Measure_All_Result();
        }

        public List<Transfer_PatientCheck> Request_BMD_Summary_ResultByPatientID(string patientId)
        {
            return Provider.Request_BMD_Summary_ResultByPatientID(patientId);
        }

        public List<Transfer_PatientCheck> Request_BMD_Summary_All_Result()
        {
            return Provider.Request_BMD_Summary_All_Result();
        }

        public Transfer_PatientCheck Request_BMD_Summary_ResultByCheckId(string checkId)
        {
            return Provider.Request_BMD_Summary_ResultByCheckId(checkId);
        }

    }
}
