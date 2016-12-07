using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chioy.Communication.Networking.Models
{
    public class DataEventArgs : EventArgs
    {
        public DataEventArgs(string data)
        {
            Data = data;
        }
        public object Data { get; set; }
    }

}
