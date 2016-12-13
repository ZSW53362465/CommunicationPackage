using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using Chioy.Communication.Networking.Client.FTP.Helper;

namespace Chioy.Communication.Networking.Client.FTP
{
    /// <summary>
    /// Provides data for UploadFileCompleted event
    /// </summary>
    public class UploadFileCompletedEventLibArgs : EventArgs
    {
        /// <summary>
        /// Gets totalBytes uploaded
        /// </summary>
        public long TotalBytesSend { get; private set; }
        /// <summary>
        /// Gets TransmissionState, e.g. Uploading, CreatingDir..
        /// </summary>
        public TransmissionState TransmissionState { get; set; }
        /// <summary>
        /// Webexception, in case of success Webexception = NULL
        /// </summary>
        public WebException WebException { get; private set; }
        /// <summary>
        /// Exception, in case of success Exception = NULL
        /// </summary>
        public Exception Exception { get; private set; }
    
        /// <summary>
        /// Provides data for UploadFileCompleted event
        /// </summary>
        /// <param name="totalBytesSend">Total bytes uploaded</param>
        /// <param name="transmissionState">State of transmission</param>
        public UploadFileCompletedEventLibArgs( long totalBytesSend, TransmissionState transmissionState )
        {
            TotalBytesSend = totalBytesSend;
            TransmissionState = transmissionState;
            WebException = null;
            Exception = null;
        }//constructor

        /// <summary>
        /// Provides data for UploadFileCompleted event
        /// </summary>
        /// <param name="totalBytesSend">Total bytes uploaded</param>
        /// <param name="transmissionState">State of transmission</param>
        /// <param name="webException">Upload failed => Webexception describes error | Upload succeded => Webexception = NULL</param>
        public UploadFileCompletedEventLibArgs( long totalBytesSend, TransmissionState transmissionState, WebException webException )
        {
            TotalBytesSend = totalBytesSend;
            TransmissionState = transmissionState;
            WebException = webException;
            Exception = null;
        }//constructor

        /// <summary>
        /// Provides data for UploadFileCompleted event
        /// </summary>
        /// <param name="totalBytesSend"></param>
        /// <param name="transmissionState"></param>
        /// <param name="exception">Upload failed => exception describes error</param>
        public UploadFileCompletedEventLibArgs( long totalBytesSend, TransmissionState transmissionState, Exception exception )
        {
            TotalBytesSend = totalBytesSend;
            TransmissionState = transmissionState;
            WebException = null;
            Exception = exception;
        }//constructor

    }//class
}//namespace
