using System.Collections.Generic;
using System.ComponentModel;
using System.ServiceModel;
using System.ServiceModel.Web;
using Chioy.Communication.Networking.Models;
using Chioy.Communication.Networking.Models.DTO;
using Chioy.Communication.Networking.Models.ReportMetadata;

namespace Chioy.Communication.Networking.Interface.ProductInterface.HTTP
{
    [ServiceContract(SessionMode = SessionMode.Allowed)]
    public interface IBMDHttpService
    {
        #region Test
        [WebGet(UriTemplate = "all", ResponseFormat = WebMessageFormat.Json)]
        [Description("获取所有员工列表-测试方法")]
        IEnumerable<Employee> GetAll();

        [WebGet(UriTemplate = "id={id}&test={test}", ResponseFormat = WebMessageFormat.Json)]
        [Description("获取指定ID的员工-测试方法")]
        Employee Get(string id,string test);

        [WebInvoke(UriTemplate = "/Create", Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        [Description("创建一个新的员工-测试方法")]
        Employee Create(Employee employee);

        [WebInvoke(UriTemplate = "/", Method = "PUT")]
        [Description("修改现有员工信息-测试方法")]
        void Update(Employee employee);

        [WebInvoke(UriTemplate = "{id}", Method = "DELETE")]
        [Description("删除指定ID的员工-测试方法")]
        void Delete(string id);
        #endregion


        [WebGet(UriTemplate = "patient/{patientid}", ResponseFormat = WebMessageFormat.Json)]
        [Description("根据patientId获取这个病人的信息")]
        Patient_DTO GetPatient(string patientId);


        [WebInvoke(UriTemplate = "/postresult", Method = "POST",ResponseFormat = WebMessageFormat.Json)]
        [Description("创建一个新的员工-测试方法")]
        KRResponse PostExamResult(ExamResultMetadata<BMDCheckResult> result);


        //[WebGet(UriTemplate = "patient/{patientid}",ResponseFormat = WebMessageFormat.Json)]
        //[Description("根据patientId获取这个病人的信息")]
        //Patient_DTO RequestPatientByPatientID(string patientId);

        //[WebGet(UriTemplate = "patient/all", ResponseFormat = WebMessageFormat.Json)]
        //[Description("获取所有检查过骨密度病人信息")]
        //IEnumerable<Patient_DTO> RequestAllPatients();

        //[WebGet(UriTemplate = "measure/{patientid}", ResponseFormat = WebMessageFormat.Json)]
        //[Description("根据patientId获取这个病人的所有检查结果")]
        //IEnumerable<BMD_Measure_Result_DTO> Request_BMD_Measure_ResultByPatientID(string patientId);

        //[WebGet(UriTemplate = "measure/check/{checkid}", ResponseFormat = WebMessageFormat.Json)]
        //[Description("根据checkid获取一个检查结果")]
        //BMD_Measure_Result_DTO Request_BMD_Measure_Result(string checkId);

        //[WebGet(UriTemplate = "measure/all", ResponseFormat = WebMessageFormat.Json)]
        //[Description("获取所有骨密度检查结果")]
        //IEnumerable<BMD_Measure_Result_DTO> Request_BMD_Measure_All_Result();

        //[WebGet(UriTemplate = "summary/{patientid}", ResponseFormat = WebMessageFormat.Json)]
        //[Description("根据patientId获取这个病人的所有信息，包括检查结果")]
        //IEnumerable<PatientCheck_DTO> Request_BMD_Summary_ResultByPatientID(string patientId);

        //[WebGet(UriTemplate = "summary/all", ResponseFormat = WebMessageFormat.Json)]
        //[Description("获取骨密度仪器中跟病人有关的所有信息")]
        //IEnumerable<PatientCheck_DTO> Request_BMD_Summary_All_Result();

        //[WebGet(UriTemplate = "summary/check/{checkid}", ResponseFormat = WebMessageFormat.Json)]
        //[Description("根据checkid获取当前这个检查的所有信息")]
        //PatientCheck_DTO Request_BMD_Summary_ResultByCheckId(string checkId);
    }
}
