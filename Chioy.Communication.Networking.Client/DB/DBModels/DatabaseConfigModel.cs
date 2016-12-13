using System.Collections.Generic;

namespace Chioy.Communication.Networking.Client.DB.Models
{
    public class DatabaseConfigModel : ModelBase
    {
        private static readonly string _connectionStringFormat =
            "Data Source={0};User ID={1};Password={2};Initial Catalog={3}";

        private static readonly string _connectionStringFormatWithOutDatabase =
            "Data Source={0};User ID={1};Password={2}";

        private static readonly string _mySqlConnectionStringFormat = "Server={0};User ID={1};Password={2};Database={3}";

        private static readonly string _mySqlConnectionStringFormatWithOutDatabase =
            "Server={0};User ID={1};Password={2}";

        private static string _connectionStringForPostSQL =
            "Server={0};Port=5432;User Id={1}; Password={2}; Database={3};" +
            "CommandTimeout=0;ConnectionLifeTime=0";
        private static string _connectionStringForPostSQLWithOutDatabase =
            "Server={0};Port=5432;User Id={1}; Password={2};" +
            "CommandTimeout=0;ConnectionLifeTime=0";
        private static Dictionary<DatabaseSoft, string> _providerDic = new Dictionary<DatabaseSoft, string>
                                                                           {
                                                                               {DatabaseSoft.SQLServer, "SQLOLEDB"},
                                                                               {DatabaseSoft.Oracle, "msdaora.1"},
                                                                               {DatabaseSoft.MySql, "MySQL Provider"},
                                                                           };

        private string _advancedConnectionString;
        private string _database;
        private DatabaseSoft _databaseSoft = DatabaseSoft.SQLServer;
        private bool _isAdvancedSetting;

        private bool _isSimpleSetting = true;
        private string _password;
        private string _server;
        private string _user;

        /// <summary>
        /// 是否简单设置
        /// </summary>
        public bool IsSimpleSetting
        {
            get { return _isSimpleSetting; }
            set
            {
                if (_isSimpleSetting != value)
                {
                    _isSimpleSetting = value;
                    RaisePropertyChanged("IsSimpleSetting");
                }
            }
        }

        /// <summary>
        /// 高级设置
        /// </summary>
        public bool IsAdvancedSetting
        {
            get { return _isAdvancedSetting; }
            set
            {
                if (_isAdvancedSetting != value)
                {
                    _isAdvancedSetting = value;
                    RaisePropertyChanged("IsAdvancedSetting");
                }
            }
        }

        public DatabaseSoft DatabaseSoft
        {
            get { return _databaseSoft; }
            set
            {
                if (_databaseSoft != value)
                {
                    _databaseSoft = value;
                    RaisePropertyChanged("DatabaseSoft");
                }
            }
        }

        /// <summary>
        /// 服务器
        /// </summary>
        public string Server
        {
            get { return _server; }
            set
            {
                if (_server != value)
                {
                    _server = value;
                    RaisePropertyChanged("Server");
                }
            }
        }

        /// <summary>
        /// 用户名
        /// </summary>
        public string User
        {
            get { return _user; }
            set
            {
                if (_user != value)
                {
                    _user = value;
                    RaisePropertyChanged("User");
                }
            }
        }

        /// <summary>
        /// 密码
        /// </summary>
        public string Password
        {
            get { return _password; }
            set
            {
                if (_password != value)
                {
                    _password = value;
                    RaisePropertyChanged("Password");
                }
            }
        }

        /// <summary>
        /// 数据库
        /// </summary>
        public string Database
        {
            get { return _database; }
            set
            {
                if (_database != value)
                {
                    _database = value;
                    RaisePropertyChanged("Database");
                }
            }
        }

        /// <summary>
        /// 连接字符串
        /// </summary>
        public string AdvancedConnectionString
        {
            get { return _advancedConnectionString; }
            set
            {
                if (_advancedConnectionString != value)
                {
                    _advancedConnectionString = value;
                    RaisePropertyChanged("AdvancedConnectionString");
                }
            }
        }

        /// <summary>
        /// 连接字符串
        /// </summary>
        public string ConnectionString
        {
            get { return GetConnectionSting(); }
        }

        /// <summary>
        /// 获取连接字符串
        /// </summary>
        /// <returns></returns>
        protected virtual string GetConnectionSting()
        {
            if (IsAdvancedSetting)
            {
                return AdvancedConnectionString;
            }

            if (IsSimpleSetting)
            {
                string result;
                if (DatabaseSoft != DatabaseSoft.MySql)
                {
                    if (DatabaseSoft == DatabaseSoft.PostgreSQL)
                    {
                        if (!string.IsNullOrEmpty(Database))
                        {
                            result = string.Format(_connectionStringForPostSQL, Server, User, Password, Database);
                        }
                        else
                        {
                            result = string.Format(_connectionStringForPostSQLWithOutDatabase, Server, User, Password);
                        }
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(Database))
                        {
                            result = string.Format(_connectionStringFormat, Server, User, Password, Database);
                        }
                        else
                        {
                            result = string.Format(_connectionStringFormatWithOutDatabase, Server, User, Password);
                        }
                    }
                }
                else
                {
                    if (DatabaseSoft == DatabaseSoft.PostgreSQL)
                    {
                        if (!string.IsNullOrEmpty(Database))
                        {
                            result = string.Format(_connectionStringForPostSQL, Server, User, Password, Database);
                        }
                        else
                        {
                            result = string.Format(_connectionStringForPostSQLWithOutDatabase, Server, User, Password);
                        }
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(Database))
                        {
                            result = string.Format(_mySqlConnectionStringFormat, Server, User, Password, Database);
                        }
                        else
                        {
                            result = string.Format(_mySqlConnectionStringFormatWithOutDatabase, Server, User, Password);
                        }
                    }
                }

                return result;
            }

            return string.Empty;
        }
    }
}