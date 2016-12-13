using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using Chioy.Communication.Networking.Client.FTP.Helper;

namespace Chioy.Communication.Networking.Client.FTP
{
    /// <summary>
    /// Provides data for DownloadFileCompleted event
    /// </summary>
    public class DownloadFileCompletedEventLibArgs : EventArgs
    {
        /// <summary>
        /// Get total bytes downloaded
        /// </summary>
        public long TotalBytesReceived { get; private set; }
        /// <summary>
        /// Get TransmissionState of download
        /// </summary>
        public TransmissionState TransmissionState { get; private set; }
        /// <summary>
        /// Get Webexception of download
        /// </summary>
        public WebException WebException { get; private set; }
        /// <summary>
        /// Get Exception of download
        /// </summary>
        public Exception Exception { get; private set; }
    
        /// <summary>
        /// Provides data for DownloadFileCompleted event
        /// </summary>
        /// <param name="totalBytesReceived">Total bytes downloaded</param>
        /// <param name="transmissionState">TransmissionState</param>
        public DownloadFileCompletedEventLibArgs( long totalBytesReceived, TransmissionState transmissionState )
        {
            TotalBytesReceived = totalBytesReceived;
            TransmissionState = transmissionState;
            WebException = null;
            Exception = null;
        }//constructor

        /// <summary>
        /// Provides data for DownloadFileChanged event
        /// </summary>
        /// <param name="totalBytesReceived">TotalBytes downloaded</param>
        /// <param name="transmissionState">TransmissionState</param>
        /// <param name="webException">Webexception | Webexception = NULL in case of success</param>
        public DownloadFileCompletedEventLibArgs( long totalBytesReceived, TransmissionState transmissionState, WebException webException )
        {
            TotalBytesReceived = totalBytesReceived;
            TransmissionState = transmissionState;
            WebException = webException;
            Exception = null;
        }//constructor

        /// <summary>
        /// Provides data for DownloadFileChanged event
        /// </summary>
        /// <param name="totalBytesReceived">TotalBytes downloaded</param>
        /// <param name="transmissionState">TransmissionState</param>
        /// <param name="exception">Exception | Exception = NULL in case of success</param>
        public DownloadFileCompletedEventLibArgs( long totalBytesReceived, TransmissionState transmissionState, Exception exception )
        {
            TotalBytesReceived = totalBytesReceived;
            TransmissionState = transmissionState;
            WebException = null;
            Exception = exception;
        }//constructor

    }//class
}//namespace
