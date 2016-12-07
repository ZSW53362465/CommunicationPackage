using Chioy.Communication.Networking.Common;
using Chioy.Communication.Networking.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chioy.Communication.Networking.Client
{
    public enum CommunicationType
    {
        WCF,
        Http
    }

    public abstract class ClientManager : IDisposable
    {
        public event EventHandler<DataEventArgs> CommunicationEvent;

        public event KRExceptionEventHandler ExceptionEvent;

        protected void HandleCommunicationEvent(object sender, string args)
        {
            CommunicationEvent?.Invoke(sender, new DataEventArgs(args));
        }

        protected void HandleExceptionEvent(KRException ex)
        {
            ExceptionEvent?.Invoke(ex);
        }

        protected abstract void ReleaseManager();

        private void ThrowException(string method, string description, string message)
        {
            Trace.TraceError(string.Format("[{0}]:{1}   {2}"), method, description, message);
            ExceptionEvent?.Invoke(new KRException(method, description, message));
        }

        public void Dispose()
        {
            ReleaseManager();
        }
    }
}
