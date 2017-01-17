using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using KRBMDCommon;
using KRBMDCommon.NetWorking;
using KRBMDCommon.NetWorking.Model;
using Chioy.Communication.Networking.KRNetWorkingTool.Command;

namespace Chioy.Communication.Networking.KRNetWorkingTool.ViewModel
{
    public class NetWorkingViewModel : INotifyPropertyChanged
    {
        #region 目标数据库参数

        private ICommand _createConnectionCommand;
        private DatabaseConfigModel _databaseConfigModel;
        private ObservableCollection<string> _databaseList;
        private ICommand _queryDatabaseNameCommand;


        private ICommand _testConnectionCommand;

        public DatabaseConfigModel DatabaseConfigModel
        {
            get { return _databaseConfigModel; }
            set
            {
                if (_databaseConfigModel != value)
                {
                    _databaseConfigModel = value;
                    RaisePropertyChanged("DatabaseConfigModel");
                }
            }
        }

        /// <summary>
        /// 测试Command
        /// </summary>
        public ICommand TestConnectionCommand
        {
            get { return _testConnectionCommand; }
            set
            {
                if (_testConnectionCommand != value)
                {
                    _testConnectionCommand = value;
                    RaisePropertyChanged("TestConnectionCommand");
                }
            }
        }

        /// <summary>
        /// 数据库名称列表
        /// </summary>
        public ObservableCollection<string> DatabaseList
        {
            get { return _databaseList; }
            set
            {
                if (_databaseList != value)
                {
                    _databaseList = value;
                    RaisePropertyChanged("DatabaseList");
                }
            }
        }

        /// <summary>
        /// 查找数据库名
        /// </summary>
        public ICommand QueryDatabaseNameCommand
        {
            get { return _queryDatabaseNameCommand; }
            set
            {
                if (_queryDatabaseNameCommand != value)
                {
                    _queryDatabaseNameCommand = value;
                    RaisePropertyChanged("QueryDatabaseNameCommand");
                }
            }
        }


        /// <summary>
        /// 新建连接
        /// </summary>
        public ICommand CreateConnectionCommand
        {
            get { return _createConnectionCommand; }
            set
            {
                if (_createConnectionCommand != value)
                {
                    _createConnectionCommand = value;
                    RaisePropertyChanged("CreateConnectionCommand");
                }
            }
        }

        #endregion

        #region 病案数据参数

        private PatientMapModel _patientMapModel;
        private ICommand _queryTargetPatientFieldCommand;

        private ObservableCollection<string> _targetFieldList;
        private ICommand _testPatientFieldSqlCommand;

        public PatientMapModel PatientMapModel
        {
            get { return _patientMapModel; }
            set
            {
                if (_patientMapModel != value)
                {
                    _patientMapModel = value;
                    RaisePropertyChanged("PatientMapModel");
                }
            }
        }

        /// <summary>
        /// 目标病案数据字段列表
        /// </summary>
        public ObservableCollection<string> TargetFieldList
        {
            get { return _targetFieldList; }
            set
            {
                if (_targetFieldList != value)
                {
                    _targetFieldList = value;
                    RaisePropertyChanged("TargetFieldList");
                }
            }
        }


        /// <summary>
        /// 查询目标病案字段
        /// </summary>
        public ICommand QueryTargetPatientFieldCommand
        {
            get { return _queryTargetPatientFieldCommand; }
            set
            {
                if (_queryTargetPatientFieldCommand != value)
                {
                    _queryTargetPatientFieldCommand = value;
                    RaisePropertyChanged("QueryTargetPatientFieldCommand");
                }
            }
        }

        /// <summary>
        /// 测试设置字段是否存在
        /// </summary>
        public ICommand TestPatientFieldSqlCommand
        {
            get { return _testPatientFieldSqlCommand; }
            set
            {
                if (_testPatientFieldSqlCommand != value)
                {
                    _testPatientFieldSqlCommand = value;
                    RaisePropertyChanged("TestPatientFieldSqlCommand");
                }
            }
        }

        #endregion

        #region 报告单保存

        private ReportSaveModel _reportSaveModel;

        private ICommand _testReportSaveCommand;

        public ReportSaveModel ReportSaveModel
        {
            get { return _reportSaveModel; }
            set
            {
                if (_reportSaveModel != value)
                {
                    _reportSaveModel = value;
                    RaisePropertyChanged("ReportSaveModel");
                }
            }
        }

        /// <summary>
        /// 测试设置字段是否存在
        /// </summary>
        public ICommand TestReportSaveCommand
        {
            get { return _testReportSaveCommand; }
            set
            {
                if (_testReportSaveCommand != value)
                {
                    _testReportSaveCommand = value;
                    RaisePropertyChanged("TestReportSaveCommand");
                }
            }
        }

        #endregion

        #region 数据回写

        private ObservableCollection<string> _callbackFieldList;
        private DataCallBackModel _dataCallBackModel;

        private ICommand _queryCallbackFieldCommand;

        public DataCallBackModel DataCallBackModel
        {
            get { return _dataCallBackModel; }
            set
            {
                if (_dataCallBackModel != value)
                {
                    _dataCallBackModel = value;
                    RaisePropertyChanged("DataCallBackModel");
                }
            }
        }

        /// <summary>
        /// 测试设置字段是否存在
        /// </summary>
        public ICommand QueryCallbackFieldCommand
        {
            get { return _queryCallbackFieldCommand; }
            set
            {
                if (_queryCallbackFieldCommand != value)
                {
                    _queryCallbackFieldCommand = value;
                    RaisePropertyChanged("QueryCallbackFieldCommand");
                }
            }
        }

        public ObservableCollection<string> CallbackFieldList
        {
            get { return _callbackFieldList; }
            set
            {
                if (_callbackFieldList != value)
                {
                    _callbackFieldList = value;
                    RaisePropertyChanged("CallbackFieldList");
                }
            }
        }

        #endregion

        #region 构造函数

        public NetWorkingViewModel()
        {
            //var dt = DatabaseHelper.ExecuteQuery("select *,'' as Value from kr_check_type where DeleteFlag = 0");
            //CheckTypeMapList = new CheckTypeMapListModel(dt);

            KRNetworkingConfig config = KRNetworkingConfig.Load();

            DatabaseConfigModel = config.DatabaseConfigModel != null
                                      ? config.DatabaseConfigModel
                                      : new DatabaseConfigModel();
            PatientMapModel = config.PatientMapModel != null
                                  ? config.PatientMapModel
                                  : new PatientMapModel(ConnectionString.Value);
            ReportSaveModel = config.ReportSaveModel != null ? config.ReportSaveModel : new ReportSaveModel();
            DataCallBackModel = config.DataCallBackModel != null ? config.DataCallBackModel : new DataCallBackModel();
            
            _callbackFieldList = new ObservableCollection<string>();
            
            foreach (TableFieldMapModel field in PatientMapModel.PatientTableMap)
            {
                _callbackFieldList.Add(string.Format("[{0}]", field.LocalField));
            }
        
            _callbackFieldList.Add("[CheckDate]");
            _callbackFieldList.Add("[CheckResult]");
            _callbackFieldList.Add("[Diagnosis]");
            _callbackFieldList.Add("<ImagePath>");
            _callbackFieldList.Add("<Image>");
            _callbackFieldList.Add("[Position]");
            _callbackFieldList.Add("[LimbSide]");
            _callbackFieldList.Add("[SOS]");
            _callbackFieldList.Add("[T]");
            _callbackFieldList.Add("[Z]");
            _callbackFieldList.Add("[Percent]");
            _callbackFieldList.Add("[RRF]");
            _callbackFieldList.Add("[EOA]");
            _callbackFieldList.Add("[PAB]");
            _callbackFieldList.Add("[HP]");
            _callbackFieldList.Add("[STI]");
            _callbackFieldList.Add("[Parameters]");
            
            
            TestReportSaveCommand = new TestReportSaveCommand(this);
            TestPatientFieldSqlCommand = new TestPatientFieldSQLCommand(this);
            TestConnectionCommand = new TestConnectionCommand(this);
            CreateConnectionCommand = new CreateUDLConnectionCommand(this);
            QueryDatabaseNameCommand = new QueryDatabaseNameCommand(this);
            QueryTargetPatientFieldCommand = new QueryTargetPatientFieldCommand(this);
            
            QueryCallbackFieldCommand = new QueryCallbackFieldCommand(this);
        }

        #endregion

        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        public void RaisePropertyChanged(string p_propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(p_propertyName));
            }
        }

        public void RaisePropertiesChanged(params string[] p_propertyNames)
        {
            if (PropertyChanged != null)
            {
                foreach (string propertyName in p_propertyNames)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
                }
            }
        }

        #endregion
    }
}