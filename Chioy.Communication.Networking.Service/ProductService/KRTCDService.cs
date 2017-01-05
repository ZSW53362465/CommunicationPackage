using Chioy.Communication.Networking.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using Chioy.Communication.Networking.Models.DTO;

namespace Chioy.Communication.Networking.Service.ProductService
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public class KRTCDService : KRService, IKRTCDService
    {
        public Patient_DTO GetPatient(string patientId)
        {
            throw new NotImplementedException();
        }

        public KRResponse PostExamResult(string resultJson)
        {
            throw new NotImplementedException();
        }
    }
}
