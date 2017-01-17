using Chioy.Communication.Networking.Client.DB.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chioy.Communication.Networking.Client.DB.DBModels
{
    public class WcfConfigModel:ModelBase
    {
        private string _baseAddress;
        private string _port;

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
    }
}
