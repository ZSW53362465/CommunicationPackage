using Chioy.Communication.Networking.Interface;
using Chioy.Communication.Networking.Models;
using Chioy.Communication.Networking.Models.DTO;
using Chioy.Communication.Networking.Service.ProductService;
using Chioy.Communication.Networking.Service.Provider;
using System;
using System.Collections.Generic;
using System.ServiceModel;

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
            List<Employee> list = new List<Employee>();
            return list;
        }
        #endregion
        public Patient_DTO RequestPatientByPatientID(string patientId)
        {
            return Provider.RequestPatientByPatientID(patientId);
        }

        public IEnumerable<Patient_DTO> RequestAllPatients()
        {
            return Provider.RequestAllPatients();
        }

        public IEnumerable<BMD_Measure_Result_DTO> Request_BMD_Measure_ResultByPatientID(string patientId)
        {
            return Provider.Request_BMD_Measure_ResultByPatientID(patientId);
        }

        public BMD_Measure_Result_DTO Request_BMD_Measure_Result(string checkId)
        {
            return Provider.Request_BMD_Measure_Result(checkId);
        }

        public IEnumerable<BMD_Measure_Result_DTO> Request_BMD_Measure_All_Result()
        {
            return Provider.Request_BMD_Measure_All_Result();
        }

        public IEnumerable<PatientCheck_DTO> Request_BMD_Summary_ResultByPatientID(string patientId)
        {
            return Provider.Request_BMD_Summary_ResultByPatientID(patientId);
        }

        public IEnumerable<PatientCheck_DTO> Request_BMD_Summary_All_Result()
        {
            return Provider.Request_BMD_Summary_All_Result();
        }

        public PatientCheck_DTO Request_BMD_Summary_ResultByCheckId(string checkId)
        {
            return Provider.Request_BMD_Summary_ResultByCheckId(checkId);
        }

    }
}
