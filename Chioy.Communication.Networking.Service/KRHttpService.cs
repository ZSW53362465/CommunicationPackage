using Chioy.Communication.Networking.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Chioy.Communication.Networking.Models;
using System.ServiceModel;
using Chioy.Communication.Networking.Service.ProductService;

namespace Chioy.Communication.Networking.Service
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public class KRHttpService : KRService, IKRHttpService
    {
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

        public IEnumerable<Employee> GetAll()
        {
            throw new NotImplementedException();
        }

        public void Update(Employee employee)
        {
            throw new NotImplementedException();
        }
    }
}
