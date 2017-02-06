using System.Collections.ObjectModel;

namespace Chioy.Communication.Networking.Client.DB.Models
{
    public class ReportSaveModel : ModelBase
    {
        private ObservableCollection<FileFormatModel> _childrenRule = new ObservableCollection<FileFormatModel>();
        private string _dirAddress;
        private bool _isReportChecked = true;
        private bool _isImageDataChecked;
        private string _fileFormat;
        private string _ftpAdresse;
        private string _ftpPassword;
        private string _ftpUser;
        private string _imageExt = "JPG";
        private bool _isCreateChildDir;
        private string _reportSaveType = "无";

        public string ReportSaveType
        {
            get { return _reportSaveType; }
            set
            {
                if (_reportSaveType != value)
                {
                    _reportSaveType = value;
                    RaisePropertyChanged("ReportSaveType");
                }
            }
        }

        public string ImageExt
        {
            get { return _imageExt; }
            set
            {
                if (_imageExt != value)
                {
                    _imageExt = value;
                    RaisePropertyChanged("ImageExt");
                }
            }
        }

        public string DirAddress
        {
            get { return _dirAddress; }
            set
            {
                if (_dirAddress != value)
                {
                    _dirAddress = value;
                    RaisePropertyChanged("DirAddress");
                }
            }
        }

        public bool IsReportChecked
        {
            get { return _isReportChecked; }
            set
            {
                if (_isReportChecked != value)
                {
                    _isReportChecked = value;
                    RaisePropertyChanged("IsReportChecked");
                }
            }
        }

        public bool IsImageDataChecked
        {
            get { return _isImageDataChecked; }
            set
            {
                if (_isImageDataChecked != value)
                {
                    _isImageDataChecked = value;
                    RaisePropertyChanged("IsImageDataChecked");
                }
            }
        }

        public string FtpAdresse
        {
            get { return _ftpAdresse; }
            set
            {
                if (_ftpAdresse != value)
                {
                    _ftpAdresse = value;
                    RaisePropertyChanged("FtpAdresse");
                }
            }
        }

        public string FtpUser
        {
            get { return _ftpUser; }
            set
            {
                if (_ftpUser != value)
                {
                    _ftpUser = value;
                    RaisePropertyChanged("FtpUser");
                }
            }
        }

        public string FtpPassword
        {
            get { return _ftpPassword; }
            set
            {
                if (_ftpPassword != value)
                {
                    _ftpPassword = value;
                    RaisePropertyChanged("FtpPassword");
                }
            }
        }

        public string FileFormat
        {
            get { return _fileFormat; }
            set
            {
                if (_fileFormat != value)
                {
                    _fileFormat = value;
                    RaisePropertyChanged("FileFomat");
                }
            }
        }


        public bool IsCreateChildDir
        {
            get { return _isCreateChildDir; }
            set
            {
                if (_isCreateChildDir != value)
                {
                    _isCreateChildDir = value;
                    RaisePropertyChanged("IsCreateChildDir");
                }
            }
        }

        public ObservableCollection<FileFormatModel> ChildrenRule
        {
            get { return _childrenRule; }
            set
            {
                if (_childrenRule != value)
                {
                    _childrenRule = value;
                    RaisePropertyChanged("ChildrenRule");
                }
            }
        }
    }
}