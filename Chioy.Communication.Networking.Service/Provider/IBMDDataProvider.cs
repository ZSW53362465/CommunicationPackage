using Chioy.Communication.Networking.Models.DTO;
using System.Collections.Generic;
using Chioy.Communication.Networking.Models.ReportMetadata;

namespace Chioy.Communication.Networking.Service.Provider
{
    public interface IBMDDataProvider :IDataProvider
    {
        KRResponse PostExamResult(ExamResultMetadata<BMDCheckResult> result);
    }
}
