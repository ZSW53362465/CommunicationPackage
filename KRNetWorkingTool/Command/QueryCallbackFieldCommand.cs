using System;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using KRBMDCommon;
using KRBMDCommon.NetWorking.DBHelper;
using KRBMDCommon.NetWorking.Model;
using Chioy.Communication.Networking.KRNetWorkingTool.ViewModel;

namespace Chioy.Communication.Networking.KRNetWorkingTool.Command
{
    public class QueryCallbackFieldCommand : CommandBase
    {
        public QueryCallbackFieldCommand(NetWorkingViewModel p_networkingVM)
            : base(p_networkingVM)
        {
        }

        public override void Execute(object parameter)
        {
            DatabaseConfigModel dbConfig = NetworkingViewModel.DatabaseConfigModel;
            DataCallBackModel dcModel = NetworkingViewModel.DataCallBackModel;

            DatabaseEnum databaseEnum = KRBMDUtility.TransDatabaseSoft(dbConfig.DatabaseSoft, dbConfig.IsAdvancedSetting);

            IDatabaseHelper db = DatabaseHelper.Open(databaseEnum, dbConfig.ConnectionString);
            ;

            string tableName = string.Empty;

            if (dcModel.CallbackType == "表")
            {
                tableName = dcModel.TargetTableName;
            }
            else if (dcModel.CallbackType == "存储过程")
            {
                tableName = dcModel.TargetProcName;
            }

            if (string.IsNullOrEmpty(tableName))
            {
                return;
            }

            string sql = string.Empty;
            if (dbConfig.DatabaseSoft == DatabaseSoft.SQLServer)
            {
                sql =
                    string.Format(
                        "SELECT A.NAME,B.NAME AS TYPE,A.LENGTH,A.ISNULLABLE FROM SYSCOLUMNS A,SYSTYPES B,SYSOBJECTS D WHERE A.XTYPE=B.XUSERTYPE AND A.ID=D.ID AND A.ID =OBJECT_ID('{0}')",
                        tableName);
            }
            else if (dbConfig.DatabaseSoft == DatabaseSoft.Oracle)
            {
                if (dcModel.CallbackType == "表")
                {
                    sql =
                        string.Format(
                            "SELECT A.COLUMN_NAME AS NAME,A.DATA_TYPE AS TYPE,A.DATA_LENGTH AS LENGTH,A.NULLABLE AS ISNULLABLE FROM USER_TAB_COLS A WHERE UPPER(A.TABLE_NAME) = '{0}'",
                            tableName.ToUpper());
                }
                else
                {
                    sql =
                        string.Format(
                            "SELECT A.ARGUMENT_NAME AS NAME,A.DATA_TYPE AS TYPE,NVL(A.DATA_LENGTH,0) AS LENGTH, 0 AS ISNULLABLE FROM USER_ARGUMENTS A WHERE UPPER(A.OBJECT_NAME)='{0}' ORDER BY SEQUENCE",
                            tableName.ToUpper());
                }
            }
            else if (dbConfig.DatabaseSoft == DatabaseSoft.PostgreSQL)
            {
                sql =
                    string.Format(
                        "select a.column_name as Name, a.udt_name as TYPE,a.character_maximum_length as LENGTH, a.is_nullable as ISNULLABLE from information_schema.columns a where table_schema='public' and table_name='{0}'",
                        tableName);
            }
            else
            {
                if (dcModel.CallbackType == "表")
                {
                    sql =
                        string.Format(
                            "SELECT A.COLUMN_NAME AS NAME, A.DATA_TYPE TYPE,A.COLUMN_TYPE AS LENGTH,A.IS_NULLABLE AS ISNULLABLE FROM INFORMATION_SCHEMA.COLUMNS A WHERE A.TABLE_SCHEMA = '{0}' AND A.TABLE_NAME = '{1}'",
                            dbConfig.Database, tableName);
                }
                else
                {
                    sql =
                        string.Format(
                            "SELECT DB, TYPE, SPECIFIC_NAME, PARAM_LIST, RETURNS FROM MYSQL.PROC A WHERE UPPER(A.SPECIFIC_NAME) = '{0}'",
                            tableName.ToUpper());
                }
            }

            DataTable table = db.ExecuteQuery(sql);

            dcModel.CallbackTabelMap.Clear();

            if (!(dbConfig.DatabaseSoft == DatabaseSoft.MySql && dcModel.CallbackType == "存储过程"))
            {
                var regex = new Regex(@"^\d*$");

                foreach (DataRow row in table.AsEnumerable())
                {
                    string name = row["NAME"].ToString();

                    if (string.IsNullOrEmpty(name))
                    {
                        continue;
                    }

                    int size = -1;
                    string length = row["LENGTH"].ToString();

                    Match match = regex.Match(length);

                    if (match.Success && !string.IsNullOrEmpty(match.Value))
                    {
                        size = Convert.ToInt32(match.Value);
                    }

                    var tfmm = new TableFieldMapModel();

                    tfmm.LocalField = row["NAME"].ToString();
                    tfmm.Type = row["TYPE"].ToString();
                    tfmm.TargetField = "";
                    tfmm.Size = size;

                    dcModel.CallbackTabelMap.Add(tfmm);
                }
            }
            else
            {
                DataRow dataRow = table.AsEnumerable().FirstOrDefault();

                if (dataRow != null)
                {
                    var parambytes = dataRow["PARAM_LIST"] as byte[];

                    var ue = new ASCIIEncoding();

                    string paramList = ue.GetString(parambytes);

                    if (!string.IsNullOrEmpty(paramList))
                    {
                        string[] paraml = paramList.Split(new string[1] {","}, StringSplitOptions.RemoveEmptyEntries);

                        foreach (string param in paraml)
                        {
                            string value = param.Trim();
                            if (value.StartsWith("OUT"))
                            {
                                continue;
                            }

                            string[] values = value.Split(new[] {" "}, StringSplitOptions.RemoveEmptyEntries);
                            string name = string.Empty;
                            string type = string.Empty;

                            if (value.StartsWith("IN"))
                            {
                                name = values[1];
                                type = values[2];
                            }
                            else
                            {
                                name = values[0];
                                type = values[1];
                            }

                            var tfmm = new TableFieldMapModel();

                            tfmm.LocalField = name;
                            tfmm.Type = type;
                            tfmm.TargetField = "";
                            tfmm.Size = 0;

                            dcModel.CallbackTabelMap.Add(tfmm);
                        }
                    }
                }
            }

            //NetworkingViewModel.TargetFieldList = list;
        }
    }
}