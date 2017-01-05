using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Chioy.Communication.Networking.Models.DTO;
using Chioy.Communication.Networking.Models.ReportMetadata;

namespace Chioy.Communication.Networking.Service.Provider
{
    public interface IDataProvider
    {
        Patient_DTO GetPatient(string patientId);
        KRResponse PostExamResult(ExamResultMetadata<BMDCheckResult> result);

    }
}
