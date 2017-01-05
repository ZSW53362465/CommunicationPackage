using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Chioy.Communication.Networking.Models.DTO;
using Chioy.Communication.Networking.Client.DB;
using Chioy.Communication.Networking.Client.DB.Models;
using Chioy.Communication.Networking.Common;
using Chioy.Communication.Networking.Client.DB.DBHelper;
using System.Data;
using System.Globalization;
using System.Diagnostics;
using Chioy.Communication.Networking.Models.ReportMetadata;
using System.Windows.Media.Imaging;

namespace Chioy.Communication.Networking.Client.Client
{
    public class DBClient<T> : BaseClient<T> where T : BaseCheckResult
    {
        KRNetworkingConfig _config = null;
        string _connStr = null;
        DatabaseConfigModel _dbConfig = null;
        IDatabaseHelper _dbHelper = null;
        private DataView _popView = new DataView();
        private string _finalDir = string.Empty;

        public DataView PopView
        {
            get
            {
                return _popView;
            }
            set
            {
                _popView = value;
            }
        }

        private bool _popOpen;

        public bool PopOpen
        {
            get
            {
                return _popOpen;
            }
            set
            {
                _popOpen = value;
            }
        }
        public KRNetworkingConfig Config { get { return _config; } }
        public DatabaseConfigModel DBConfig { get { return _dbConfig; } }

        public IDatabaseHelper DBHelper { get { return _dbHelper; } }

        public string FinalDir { get { return _finalDir; } }


        public DBClient()
        {
            _protocol = Protocol.DB;
        }
        public override void ConfigClient(ProductType type, Protocol protocol)
        {
            try
            {
                base.ConfigClient(type, protocol);
                _config = KRNetworkingConfig.Load();
                if (_config == null)
                {
                    ClientHelper.TraceException("DBClient.ConfigClient", "未能成功加载数据库联网方式联网配置文件", "_config为Null");
                }
                _dbConfig = _config.DatabaseConfigModel;
                if (_dbConfig == null)
                {
                    ClientHelper.TraceException("DBClient.ConfigClient", "未能成功加载数据库联网方式联网配置文件", "DatabaseConfigModel为Null");
                }
                _connStr = _dbConfig.ConnectionString;
                DatabaseEnum databaseEnum = DataBaseSoft.TransDatabaseSoft(_dbConfig.DatabaseSoft,
                                                                             _dbConfig.IsAdvancedSetting);
                _dbHelper = DatabaseHelper.Open(databaseEnum, _connStr);
            }
            catch (KRException ex)
            {
                throw new Exception("加载联网配置失败");
            }

        }

        public override Patient_DTO GetPatient(string patientId)
        {
            try
            {
                Patient_DTO patient = null;
                string sql = _config.PatientMapModel.GetPatientInfoSql(_dbConfig.DatabaseSoft);
                string targetCheck = _config.PatientMapModel.GetTargetCheckByCheckType(1);

                sql = string.Format(sql, patientId, targetCheck);
                Trace.WriteLine(string.Format("根据patientId:{0}去数据库取病人信息,Sql 语句为:{2}", patientId, sql));

                DataTable table = _dbHelper.ExecuteQuery(sql);

                if (table == null || table.Rows.Count == 0)
                {
                    Trace.WriteLine("没有查到病人信息");
                    return null;
                }

                DataRow dr = table.AsEnumerable().FirstOrDefault();
                if (table.Rows.Count == 1)
                {
                    PopOpen = false;
                    patient = BuildDTO(dr);
                }
                else if (table.Rows.Count > 1)
                {
                    PopView = table.DefaultView;
                    PopOpen = true;
                }
                return patient;
            }
            catch (Exception ex)
            {
                throw new Exception("获取病人信息失败");
            }

        }
        private Patient_DTO BuildDTO(DataRow p_dataRow)
        {
            Patient_DTO patient = null;
            if (p_dataRow == null)
            {
                return null;
            }
            try
            {
                DataColumnCollection rowColumns = p_dataRow.Table.Columns;

                for (int i = 0; i < rowColumns.Count; i++)
                {
                    DataColumn dc = rowColumns[i];

                    var obj = p_dataRow[dc.ColumnName];
                    if (obj is DBNull)
                    {
                        continue;
                    }
                    switch (dc.ColumnName.ToUpper())
                    {
                        case "PATIENTID":
                            patient.PatientID = obj.ToString();
                            break;
                        case "NAME":
                            patient.FullName = obj.ToString();
                            break;
                        case "GENDER":
                            string gender = obj.ToString();
                            switch (gender)
                            {
                                case "0":
                                case "男":
                                    patient.Gender = 0;
                                    break;
                                case "1":
                                case "女":
                                    patient.Gender = 1;
                                    break;
                            }
                            break;
                        case "BIRTHDAY":
                            patient.Birthday = obj.ToString();
                            break;
                        case "AGE":
                            patient.Age = int.Parse(obj.ToString());
                            break;
                        case "REQUESTDOCTOR":
                            patient.RequestDoctor = obj.ToString();
                            break;
                        case "REQUESTDEPOT":
                            patient.RequestDepartment = obj.ToString();
                            break;
                        case "REQUESTDATE":
                            patient.RequestDate = obj.ToString();
                            break;
                        case "EXAMDEPARTMENT":
                            patient.ExamDepartment = obj.ToString();
                            break;
                        case "EXAMDOCTOR":
                            patient.ExamDoctor = obj.ToString();
                            break;
                        case "DIAGNOSTICIANDOCTOR":
                            patient.DiagnosticianDoctor = obj.ToString();
                            break;
                    }
                }
                return patient;
            }
            catch (Exception ex)
            {
                ClientHelper.TraceException("DBClient.BuildDTO", "数据映射失败", "p_dataRow数据出错");
            }
            return null;
        }

        public override KRResponse PostExamResult(ExamResultMetadata<T> result)
        {
            var response = new KRResponse();
            var nh = new KRNetworkingHelper<T>(result);
            bool isSuccSaveReport = false;
            bool isSuccDataSave = false;
            try
            {
                switch (_config.ReportSaveModel.ReportSaveType)
                {
                    case "无":
                        isSuccSaveReport = isSuccDataSave = nh.SaveCallBackData();
                        break;
                    case "FTP":
                        isSuccSaveReport = nh.SaveReportByFtp(result.Bitmap, ref _finalDir);
                        isSuccDataSave = nh.SaveCallBackData();
                        break;
                    case "文件夹":
                        isSuccSaveReport = nh.SaveReportByDir(result.Bitmap, ref _finalDir);
                        isSuccDataSave = nh.SaveCallBackData();
                        break;
                    case "表":
                        isSuccSaveReport = isSuccDataSave = nh.SaveCallBackData(result.Bitmap);
                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("上传检查结果失败");
            }

            StringBuilder resultSb = new StringBuilder();
            resultSb.Append(isSuccSaveReport ? string.Empty : "报告单图片保存失败！");
            resultSb.Append(resultSb.ToString().Length == 0 ? string.Empty : "\n");
            resultSb.Append(isSuccDataSave ? string.Empty : "数据回写失败");
            var msg = resultSb.ToString();

            if (string.IsNullOrEmpty(msg))
            {
                response.Msg = msg = "联网数据保存成功！";
                response.Status = "SUCCESS";
            }
            else
            {
                response.Msg = msg;
                response.Status = "FAIL";
            }
            return response;
        }
    }
}
