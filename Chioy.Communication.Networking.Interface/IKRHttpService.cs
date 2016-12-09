using Chioy.Communication.Networking.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace Chioy.Communication.Networking.Interface
{
    [ServiceContract(SessionMode = SessionMode.Allowed)]
    public interface IKRHttpService
    {
        [WebGet(UriTemplate = "all")]
        [Description("获取所有员工列表")]
        IEnumerable<Employee> GetAll();

        [WebGet(UriTemplate = "{id}", ResponseFormat = WebMessageFormat.Json)]
        [Description("获取指定ID的员工")]
        Employee Get(string id);

        [WebInvoke(UriTemplate = "/Create", Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        [Description("创建一个新的员工")]
        Employee Create(Employee employee);

        [WebInvoke(UriTemplate = "/", Method = "PUT")]
        [Description("修改现有员工信息")]
        void Update(Employee employee);

        [WebInvoke(UriTemplate = "{id}", Method = "DELETE")]
        [Description("删除指定ID的员工")]
        void Delete(string id);
    }
}
