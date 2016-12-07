using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KRWcfLib.EventSvc
{
    [Serializable]
    public class ArgumentBase<T> 
    {
        private int code;
        private string msg;
        private T model;

        public int Code
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
        public String Username { get; set; }
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

    }
}
