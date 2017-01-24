using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Chioy.Communication.Networking.Client.DB.Models;

namespace Chioy.Communication.Networking.Client.DB.DBModels
{
    public class HttpConfigModel : ModelBase
    {
        private string _baseAddress;
        private string _port;
        private string _patientUrl;
        private string _postCheckResultUrl;
        private string _tokenUrl;
        private string _webServiceName;
        private bool _isUseToken;
        private bool _isUsePort;
        private string _resultParamterName;

        public string BaseAddress
        {
            get
            {
                return _baseAddress;
            }

            set
            {
                if (_baseAddress != value)
                {
                    _baseAddress = value;
                    RaisePropertyChanged("BaseAddress");
                }
            }
        }

        public string Port
        {
            get
            {
                return _port;
            }

            set
            {
                if (_port != value)
                {
                    _port = value;
                    RaisePropertyChanged("Port");
                }
            }
        }

        public string PatientUrl
        {
            get
            {
                return _patientUrl;
            }

            set
            {
                if (_patientUrl != value)
                {
                    _patientUrl = value;
                    RaisePropertyChanged("PatientUrl");
                }
            }
        }
        public string TokenUrl
        {
            get
            {
                return _tokenUrl;
            }

            set
            {
                if (_tokenUrl != value)
                {
                    _tokenUrl = value;
                    RaisePropertyChanged("TokenUrl");
                }
            }
        }

        public string PostCheckResultUrl
        {
            get
            {
                return _postCheckResultUrl;
            }

            set
            {
                if (_postCheckResultUrl != value)
                {
                    _postCheckResultUrl = value;
                    RaisePropertyChanged("PostCheckResultUrl");
                }
            }
        }

        public string WebServiceName
        {
            get
            {
                return _webServiceName;
            }

            set
            {
                if (_webServiceName != value)
                {
                    _webServiceName = value;
                    RaisePropertyChanged("WebServiceName");
                }
            }
        }

        public bool IsUseToken
        {
            get
            {
                return _isUseToken;
            }

            set
            {
                if (_isUseToken != value)
                {
                    _isUseToken = value;
                    RaisePropertyChanged("IsUseToken");
                }
            }
        }

        public bool IsUsePort
        {
            get
            {
                return _isUsePort;
            }

            set
            {
                if (_isUsePort != value)
                {
                    _isUsePort = value;
                    RaisePropertyChanged("IsUsePort");
                }
            }
        }

        public string ResultParamter
        {
            get { return _resultParamterName; }
            set
            {
                if (_resultParamterName != value)
                {
                    _resultParamterName = value;
                    RaisePropertyChanged("ResultParamter");
                }
            }
        }
    }
}
