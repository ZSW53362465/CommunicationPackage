using MySql.Data.MySqlClient;
using Npgsql;
using NpgsqlTypes;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.OracleClient;
using System.Data.SqlClient;
using System.Text;

namespace Chioy.Communication.Networking.Client.DB.Models
{
    public class DataCallBackModel : ModelBase
    {
        private TableMapModel _callBackTableMap = new TableMapModel();
        private string _callbackType = "无";

        private string _targetProcName;

        private string _targetTableName;

        private string _targetTableUpdateType = "插入";

        public TableMapModel CallbackTabelMap
        {
            get { return _callBackTableMap; }
            set
            {
                if (_callBackTableMap != value)
                {
                    _callBackTableMap = value;
                    RaisePropertyChanged("CallbackTabelMap");
                }
            }
        }

        public string CallbackType
        {
            get { return _callbackType; }
            set
            {
                if (_callbackType != value)
                {
                    _callbackType = value;
                    RaisePropertyChanged("CallbackType");
                }
            }
        }

        public string TargetProcName
        {
            get { return _targetProcName; }
            set
            {
                if (_targetProcName != value)
                {
                    _targetProcName = value;
                    RaisePropertyChanged("TargetProcName");
                }
            }
        }

        public string TargetTableName
        {
            get { return _targetTableName; }
            set
            {
                if (_targetTableName != value)
                {
                    _targetTableName = value;
                    RaisePropertyChanged("TargetTableName");
                }
            }
        }

        public string TargetTableUpdateType
        {
            get { return _targetTableUpdateType; }
            set
            {
                if (_targetTableUpdateType != value)
                {
                    _targetTableUpdateType = value;
                    RaisePropertyChanged("TargetTableUpdateType");
                }
            }
        }

        public CallBackSqlAndParam GetSqlStringByParam(DatabaseSoft p_soft)
        {
            var cbsap = new CallBackSqlAndParam();
            cbsap.Parameters = new List<DbParameterAndKey>();

            foreach (TableFieldMapModel item in CallbackTabelMap)
            {
                if (string.IsNullOrEmpty(item.LocalField))
                {
                    continue;
                }

                if (string.IsNullOrEmpty(item.TargetField) && CallbackType != "存储过程")
                {
                    continue;
                }

                //string pn = item.TargetField;
                //pn = pn.Replace("[", "").Replace("]", "");

                var pk = new DbParameterAndKey();


                pk.Key = item.TargetField;
                ;
                pk.IsWhere = item.IsWhere;

                DbParameter p = null;

                switch (p_soft)
                {
                    case DatabaseSoft.SQLServer:
                        var sqlParam = new SqlParameter();
                        sqlParam.SqlDbType = GetSqlServerType(item.Type);
                        p = sqlParam;
                        break;
                    case DatabaseSoft.Oracle:
                        var oracleParam = new OracleParameter();
                        oracleParam.OracleType = GetOracleType(item.Type);
                        p = oracleParam;
                        break;
                    case DatabaseSoft.MySql:
                        var mySqlParam = new MySqlParameter();
                        mySqlParam.MySqlDbType = GetMySqlType(item.Type);
                        p = mySqlParam;
                        break;
                    case DatabaseSoft.PostgreSQL:
                        var npSqlParameter = new NpgsqlParameter();
                        npSqlParameter.NpgsqlDbType = GetPostgreSQLType(item.Type);
                        p = npSqlParameter;
                        break;
                }

                p.ParameterName = item.LocalField;
                if (item.Size > 0)
                {
                    p.Size = item.Size;
                }

                pk.Parameter = p;

                cbsap.Parameters.Add(pk);
            }

            string sign = "@";
            if (p_soft == DatabaseSoft.Oracle)
            {
                sign = ":";
            }

            if (CallbackType == "表")
            {
                if (TargetTableUpdateType == "更新")
                {
                    //sb.AppendFormat(" Update {0}", this.TargetTableName);
                    var sb = new StringBuilder();
                    var whereSb = new StringBuilder();

                    foreach (DbParameterAndKey p in cbsap.Parameters)
                    {
                        string key = p.Key.Trim();
                        if (string.IsNullOrEmpty(key))
                        {
                            continue;
                        }

                        if (p.IsWhere)
                        {
                            if (whereSb.Length > 0)
                            {
                                whereSb.Append(" and ");
                            }
                            if (p_soft == DatabaseSoft.PostgreSQL)
                            {
                                whereSb.AppendFormat("\"{0}\"={1}{0}", p.Parameter.ParameterName, sign);
                            }
                            else
                            {
                                whereSb.AppendFormat("{0}={1}{0}", p.Parameter.ParameterName, sign);
                            }
                        }
                        else
                        {
                            if (sb.Length > 0)
                            {
                                sb.Append(" , ");
                            }

                            if (key.ToUpper().StartsWith("@TARGET"))
                            {
                                sb.AppendFormat("{0}={0}{1}", p.Parameter.ParameterName, key.Substring(7));
                                continue;
                            }
                            else
                            {
                                if (p_soft == DatabaseSoft.PostgreSQL)
                                {
                                    sb.AppendFormat("\"{0}\"={1}{0}", p.Parameter.ParameterName, sign);
                                }
                                else
                                {
                                    sb.AppendFormat("{0}={1}{0}", p.Parameter.ParameterName, sign);
                                }
                            }
                        }
                    }

                    if (p_soft == DatabaseSoft.PostgreSQL)
                    {
                        cbsap.UpdateSql = string.Format(" update \"{0}\" Set {1} where {2}", TargetTableName, sb, whereSb);
                    }
                    else
                    {
                        cbsap.UpdateSql = string.Format(" update {0} Set {1} where {2}", TargetTableName, sb, whereSb);
                    }
                }


                //sb.AppendFormat(" Insert into {0}", this.TargetTableName);

                var field = new StringBuilder();
                var sbParams = new StringBuilder();
                foreach (DbParameterAndKey p in cbsap.Parameters)
                {
                    if (string.IsNullOrEmpty(p.Key))
                    {
                        continue;
                    }

                    string pn = p.Parameter.ParameterName;
                    string tmpfield = pn;
                    if (p_soft == DatabaseSoft.PostgreSQL)
                    {
                        tmpfield = string.Format("\"{0}\"", tmpfield);
                    }
                    if (string.IsNullOrEmpty(pn))
                    {
                        continue;
                    }

                    if (p.Key.Trim().ToUpper().StartsWith("@TARGET"))
                    {
                        //sb.AppendFormat("{0}={0}{1}", p.Parameter.ParameterName, key.Substring(6));
                        string tempKey = p.Key.Trim().Substring(7).Trim();
                        if (tempKey.Length > 1)
                        {
                            p.Key = tempKey.Substring(1).Trim();
                        }
                        else
                        {
                            p.Key = string.Empty;
                        }
                    }


                    if (field.Length > 0)
                    {
                        field.Append(',');
                        sbParams.Append(',');
                    }
                    field.Append(tmpfield);
                    //if (p_soft != DatabaseSoft.MySql)
                    //{
                    //    sbParams.AppendFormat("?");
                    //}
                    //else
                    //{
                    sbParams.AppendFormat("{0}{1}", sign, pn);
                    //}
                }

                cbsap.InsertSql = string.Format("Insert into {0} ({1}) values ({2})", TargetTableName, field, sbParams);
                if (p_soft == DatabaseSoft.PostgreSQL)
                {
                    cbsap.InsertSql = string.Format("Insert into \"{0}\" ({1}) values ({2})", TargetTableName, field, sbParams);
                }
            }
            else if (CallbackType == "存储过程")
            {
                var sb = new StringBuilder();
                foreach (DbParameterAndKey p in cbsap.Parameters)
                {
                    if (sb.Length > 0)
                    {
                        sb.Append(',');
                    }

                    if (p_soft != DatabaseSoft.SQLServer)
                    {
                        sb.Append(sign);
                    }

                    sb.AppendFormat("{0}", p.Parameter.ParameterName);
                }

                string exec = " EXEC {0} {1} ";
                if (p_soft != DatabaseSoft.SQLServer)
                {
                    exec = " Call {0} ({1})";
                }

                cbsap.InsertSql = string.Format(exec, TargetProcName, sb);
            }

            // = sql;

            return cbsap;
        }

        private DbType GetDBType(string p_type)
        {
            var dbtype = DbType.String;

            bool isSucc = Enum.TryParse(p_type, out dbtype);


            if (isSucc)
            {
                return dbtype;
            }

            switch (p_type.ToUpper())
            {
                case "CHAR":
                case "VARCHAR":
                case "NVARCHAR":
                    dbtype = DbType.String;
                    break;
                case "INT":
                    dbtype = DbType.Int32;
                    break;
                case "NUMBER":
                    dbtype = DbType.Double;
                    break;
                case "FLOAT":
                    dbtype = DbType.Decimal;
                    break;
                case "DATATIME":
                    dbtype = DbType.DateTime;
                    break;
                case "DATATIME2":
                    dbtype = DbType.DateTime;
                    break;
                case "BLOB":
                    dbtype = DbType.Binary;
                    break;
            }

            return dbtype;
        }

        private SqlDbType GetSqlServerType(string p_type)
        {
            var dbtype = SqlDbType.VarChar;

            bool isSucc = Enum.TryParse(p_type, true, out dbtype);


            if (isSucc)
            {
                return dbtype;
            }

            switch (p_type.ToUpper())
            {
                case "CHAR":
                case "VARCHAR":
                case "VARCHAR2":
                case "NVARCHAR":
                    dbtype = SqlDbType.VarChar;
                    break;
                case "INT":
                    dbtype = SqlDbType.Int;
                    break;
                case "NUMBER":
                case "FLOAT":
                    dbtype = SqlDbType.Float;
                    break;
                case "DATATIME":
                case "DATATIME2":
                    dbtype = SqlDbType.DateTime2;
                    break;
                case "BLOB":
                case "VARBINARY":
                    dbtype = SqlDbType.VarBinary;
                    break;
                default:
                    dbtype = SqlDbType.VarChar;
                    break;
            }

            return dbtype;
        }

        private OracleType GetOracleType(string p_type)
        {
            var dbtype = OracleType.VarChar;

            bool isSucc = Enum.TryParse(p_type, true, out dbtype);

            if (isSucc)
            {
                return dbtype;
            }

            switch (p_type.ToUpper())
            {
                case "CHAR":
                case "VARCHAR":
                case "VARCHAR2":
                case "NVARCHAR":
                    dbtype = OracleType.VarChar;
                    break;
                case "INT":
                    dbtype = OracleType.Int32;
                    break;
                case "NUMBER":
                case "FLOAT":
                    dbtype = OracleType.Double;
                    break;
                case "DATE":
                case "DATETIME":
                case "DATETIME2":
                    dbtype = OracleType.DateTime;
                    break;
                case "BLOB":
                    dbtype = OracleType.Blob;
                    break;
                default:
                    dbtype = OracleType.VarChar;
                    break;
            }

            return dbtype;
        }

        private MySqlDbType GetMySqlType(string p_type)
        {
            var dbtype = MySqlDbType.VarChar;

            bool isSucc = Enum.TryParse(p_type.ToUpper(), true, out dbtype);

            if (isSucc)
            {
                return dbtype;
            }

            switch (p_type.ToUpper())
            {
                case "CHAR":
                case "VARCHAR":
                case "VARCHAR2":
                case "NVARCHAR":
                    dbtype = MySqlDbType.VarChar;
                    break;
                case "INT":
                    dbtype = MySqlDbType.Int32;
                    break;
                case "NUMBER":
                case "FLOAT":
                    dbtype = MySqlDbType.Float;
                    break;
                case "DATATIME":
                case "DATATIME2":
                    dbtype = MySqlDbType.DateTime;
                    break;
                case "BLOB":
                case "VARBINARY":
                    dbtype = MySqlDbType.VarBinary;
                    break;
                default:
                    dbtype = MySqlDbType.VarChar;
                    break;
            }

            return dbtype;
        }
        private NpgsqlDbType GetPostgreSQLType(string p_type)
        {
            var dbtype = NpgsqlDbType.Varchar;

            bool isSucc = Enum.TryParse(p_type, true, out dbtype);


            if (isSucc)
            {
                return dbtype;
            }

            switch (p_type.ToUpper())
            {
                case "FLOAT8":
                    dbtype = NpgsqlDbType.Double;
                    break;
                case "INT4":
                    dbtype = NpgsqlDbType.Integer;
                    break;
                case "_BYTEA":
                case "BYTEA":
                    dbtype = NpgsqlDbType.Bytea;
                    break;
            }

            return dbtype;
        }
    }

    public class CallBackSqlAndParam
    {
        public List<DbParameterAndKey> Parameters { get; set; }

        public string InsertSql { get; internal set; }

        public string UpdateSql { get; internal set; }
    }

    public class DbParameterAndKey
    {
        public bool IsWhere { get; set; }

        public string Key { get; set; }

        public DbParameter Parameter { get; set; }

        public bool IsFixValue { get; set; }
    }

    //public class StringToEnum
    //{
    //    public bool TryParse<TEnum>(string p)
    //    {
    //        var tType = typeof(TEnum);

    //        var fieldsInfo = tType.GetFields();

    //        foreach (var field in fieldsInfo)
    //        {
    //            field.Name
    //        }
    //    }
    //}
}