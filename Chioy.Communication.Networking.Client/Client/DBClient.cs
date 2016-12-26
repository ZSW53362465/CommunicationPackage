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

namespace Chioy.Communication.Networking.Client.Client
{
    public class DBClient : BaseClient
    {
        KRNetworkingConfig _config = null;
        string _connStr = null;
        DatabaseConfigModel _dbConfig = null;
        private DataView _popView = new DataView();

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
        public DBClient()
        {
            _protocol = Protocol.DB;
        }
        public override void ConfigClient()
        {
            base.ConfigClient();
            _config = KRNetworkingConfig.Load();
            _dbConfig = _config.DatabaseConfigModel;
            _connStr = _dbConfig.ConnectionString;
        }

        public override Patient_DTO GetPatient(string patientId)
        {
            try
            {
                Patient_DTO patient = null;
                DatabaseEnum databaseEnum = DataBaseSoft.TransDatabaseSoft(_dbConfig.DatabaseSoft,
                                                                          _dbConfig.IsAdvancedSetting);
                IDatabaseHelper dbHelper = DatabaseHelper.Open(databaseEnum, _connStr);
                string sql = _config.PatientMapModel.GetPatientInfoSql(_dbConfig.DatabaseSoft);
                string targetCheck = _config.PatientMapModel.GetTargetCheckByCheckType(1);

                sql = string.Format(sql, patientId, targetCheck);
                Trace.WriteLine(string.Format("根据patientId:{0}去数据库取病人信息,Sql 语句为:{2}", patientId, sql));

                DataTable table = dbHelper.ExecuteQuery(sql);

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
                throw ex;
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
                throw ex;
            }
        }
    }
}
