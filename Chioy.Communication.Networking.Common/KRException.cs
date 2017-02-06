using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chioy.Communication.Networking.Common
{
    public delegate void KRExceptionEventHandler(KRException ex);

    public class KRException : Exception
    {
        public KRException(string method, string description,string message)
        {
            ExceptionMethodName = method;
            Description = description;
            Msg = string.Format("在方法{0}中出现错误:{1},错误原因：{2}", method, description, message);
        }

        public string ExceptionMethodName { get; set; }

        public string Description { get; set; }

        public string Msg { get; set; }

    }
}
