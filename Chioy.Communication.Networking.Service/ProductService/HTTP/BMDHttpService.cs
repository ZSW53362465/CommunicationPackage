using System;
using System.Collections.Generic;
using System.ServiceModel;
using Chioy.Communication.Networking.Interface;
using Chioy.Communication.Networking.Interface.ProductInterface.HTTP;
using Chioy.Communication.Networking.Models;
using Chioy.Communication.Networking.Models.DTO;
using Chioy.Communication.Networking.Models.ReportMetadata;
using Chioy.Communication.Networking.Service.Provider;

namespace Chioy.Communication.Networking.Service.ProductService.HTTP
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public class BMDHttpService : DataProviderAdpter, IBMDHttpService
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
        
        public Patient_DTO GetPatient(string patientId)
        {
            return Provider.GetPatient(patientId);
        }

        public KRResponse PostExamResult(ExamResultMetadata<BMDCheckResult> result)
        {
            return Provider.PostExamResult(result);
        }

    }
}
