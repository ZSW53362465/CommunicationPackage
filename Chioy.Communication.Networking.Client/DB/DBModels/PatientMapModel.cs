using Chioy.Communication.Networking.Client.DB.DBHelper;
using System.Data;
using System.Linq;
using System.Text;

namespace Chioy.Communication.Networking.Client.DB.Models
{
    public class PatientMapModel : ModelBase
    {
        private CheckTypeMapListModel _checkTypeMapList;

        private TableMapModel _patientTableMap;


        private string _patientTableName;

        private string localConnectionString =
            "Data Source=192.168.0.6;Initial Catalog=VesselWorkstation;Persist Security Info=True;User ID=sa;Password=k0f1b2";

        public PatientMapModel(string p_connectionString)
        {
            string connStr = localConnectionString;
            if (!string.IsNullOrEmpty(p_connectionString))
            {
                connStr = p_connectionString;
            }
            PatientTableMap = TableMapModel.CreateEmptyPatientTableMap();


            IDatabaseHelper db = DatabaseHelper.Open(DataBaseSoft.TransDatabaseSoft(DatabaseSoft.SQLServer, false), connStr);
            
            string sql = "select *,'' as Value from kr_check_type where DeleteFlag = 0";
            DataTable dt = db.ExecuteQuery(sql);
            CheckTypeMapList = new CheckTypeMapListModel(dt);
        }

        public PatientMapModel()
        {
        }

        /// <summary>
        /// 检查类型映射
        /// </summary>
        public CheckTypeMapListModel CheckTypeMapList
        {
            get { return _checkTypeMapList; }
            set
            {
                if (_checkTypeMapList != value)
                {
                    _checkTypeMapList = value;
                    RaisePropertyChanged("CheckTypeMapListModel");
                }
            }
        }

        /// <summary>
        /// 病案字段映射
        /// </summary>
        public TableMapModel PatientTableMap
        {
            get { return _patientTableMap; }
            set
            {
                if (_patientTableMap != value)
                {
                    _patientTableMap = value;
                    RaisePropertyChanged("PatientTableMap");
                }
            }
        }

        /// <summary>
        /// 目标病案表名
        /// </summary>
        public string PatientTableName
        {
            get { return _patientTableName; }
            set
            {
                if (_patientTableName != value)
                {
                    _patientTableName = value;
                    RaisePropertyChanged("PatientTableName");
                }
            }
        }

        public string GetTestPatientInfoSql(DatabaseSoft databaseSoft)
        {
            var sb = new StringBuilder();

            foreach (TableFieldMapModel field in PatientTableMap)
            {
                if (string.IsNullOrEmpty(field.TargetField))
                {
                    continue;
                }

                if (!field.TargetField.StartsWith("[") || !field.TargetField.EndsWith("]"))
                {
                    return null;
                }
                string target = field.TargetField.Replace("[", "");
                target = target.Replace("]", "");
                if (sb.Length > 0)
                {
                    sb.Append(",");
                }

                //sb.Append(field.TargetField);
                if (databaseSoft == DatabaseSoft.PostgreSQL)
                {
                    sb.AppendFormat("\"{0}\" as {1}", target, field.LocalField);
                }
                else
                {
                    sb.AppendFormat("{0} as {1}", target, field.LocalField);
                }
            }

            string sql = string.Format("select {0} from {1}", sb, PatientTableName);
            if (databaseSoft == DatabaseSoft.PostgreSQL)
            {
                sql = string.Format("select {0} from \"{1}\"", sb, PatientTableName);
            }
            return sql;
        }

        public string GetPatientInfoSql(DatabaseSoft databaseSoft)
        {
            var sb = new StringBuilder();
            var sbWhere = new StringBuilder();

            int index = 0;

            foreach (TableFieldMapModel field in PatientTableMap)
            {
                if (string.IsNullOrEmpty(field.TargetField))
                {
                    continue;
                }

                if (sb.Length > 0)
                {
                    sb.Append(",");
                }

                string targetField = field.TargetField.Trim();

                if (targetField.StartsWith("[") && targetField.EndsWith("]"))
                {
                    string tempField = targetField.Replace("[", string.Empty).Replace("]", string.Empty);
                    string localField = field.LocalField.ToUpper();
                    if (databaseSoft == DatabaseSoft.PostgreSQL)
                    {
                        tempField = string.Format("\"{0}\"", tempField);
                        localField = string.Format("\"{0}\"", localField);
                    }
                    if (field.LocalField.ToUpper() == "GENDER")
                    {
                        sb.AppendFormat(
                            "(case {0} when '男' then 0  when '女' then 1 when '1' then 1 else 0 end) as {1} ", tempField,
                            localField);
                    }
                    else
                    {
                        sb.AppendFormat("{0} as {1}", tempField, localField);
                    }

                    if (field.IsWhere)
                    {
                        if (sbWhere.Length > 0)
                        {
                            sbWhere.Append(" AND ");
                        }

                        if (field.LocalField.ToUpper() == "PATIENTID")
                        {
                            sbWhere.AppendFormat(" {0} Like '{1}%'", tempField, "{" + index++ + "}");
                        }
                        else if (field.LocalField.ToUpper() == "CHECKTYPE")
                        {
                            sbWhere.AppendFormat(" {0} = '{1}'", tempField, "{" + index++ + "}");
                        }
                        else
                        {
                            sbWhere.AppendFormat(" {0} = {1}", tempField, "{" + index++ + "}");
                        }
                    }
                }
                else if ((field.TargetField.StartsWith("<") && field.TargetField.EndsWith(">")))
                {
                }
                else
                {
                    sb.AppendFormat("'{0}' as {1}", field.TargetField, field.LocalField.ToUpper());
                }
            }

            string sql = string.Format("select {0} from {1} where {2}", sb, PatientTableName, sbWhere);
            if (databaseSoft == DatabaseSoft.PostgreSQL)
            {
                sql = string.Format("select {0} from \"{1}\" where {2}", sb, PatientTableName, sbWhere);
            }
            bool isExist =
                PatientTableMap.Any(
                    i => !string.IsNullOrEmpty(i.TargetField) && i.LocalField.ToUpper().Contains("PATIENTID"));

            if (isExist)
            {
                if (databaseSoft == DatabaseSoft.PostgreSQL)
                {
                    sql += string.Format(" order by \"{0}\" desc", "PATIENTID");
                }
                else
                {
                    sql += " order by PATIENTID desc";
                }
            }

            return sql;
        }

        public string GetTargetCheckByCheckType(int p_checkType)
        {
            CheckTypeMapModel check = CheckTypeMapList.FirstOrDefault(r => r.ID == p_checkType);
        
            if (check != null)
            {
                return check.TargetCheckType;
            }
        
            return "-1";
        }
    }
}