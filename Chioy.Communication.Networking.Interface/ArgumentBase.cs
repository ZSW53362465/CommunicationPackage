using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chioy.Communication.Networking.Interface
{
    public enum KRCode
    {
        Subscribe = 1,
        DataFromSvr,
    }
    [Serializable]
    public class ArgumentBase<T>
    {
        private KRCode code;
        private string msg;
        private T model;

        public KRCode Code
        {
            get
            {
                return code;
            }

            set
            {
                code = value;
            }
        }

        public string Msg
        {
            get
            {
                return msg;
            }

            set
            {
                msg = value;
            }
        }

        public T Model
        {
            get
            {
                return model;
            }

            set
            {
                model = value;
            }
        }
    }

    public class SubscribeArg : ArgumentBase<int>
    {
        public string Username { get; set; }
        public List<int> Alarms { get; set; }
        public SubscribeArg()
        {
            Alarms = new List<int>();
        }
    }
    public class SubscribeContext
    {
        public SubscribeArg Arg { get; set; }
        public IEventCallback Callback { get; set; }

        public int Port { get; set; }

        public string Address { get; set; }

    }
}
