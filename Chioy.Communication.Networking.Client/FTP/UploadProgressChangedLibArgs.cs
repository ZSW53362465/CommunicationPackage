using Chioy.Communication.Networking.Client.FTP.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chioy.Communication.Networking.Client.FTP
{

    /// <summary>
    /// Provides data for UploadFileChanged event
    /// </summary>
    public class UploadProgressChangedLibArgs : EventArgs
    {
        /// <summary>
        /// Gets number of bytes send by upload process
        /// </summary>
        public long BytesSent { get; private set; }

        /// <summary>
        /// Gets  total number of bytes send by upload process
        /// </summary>
        public long TotalBytesToSend { get; private set; }

        /// <summary>
        /// Gets procent of upload 
        /// </summary>
        public int Procent { get; private set; }

        /// <summary>
        /// Defines upload state
        /// </summary>
        public TransmissionState TransmissionState { get; private set; }

        /// <summary>
        ///Provides data for UploadFileChanged event 
        /// </summary>
        /// <param name="bytesSend">Bytes uploaded</param>
        /// <param name="totalBytesToSend">Total bytes uploaded</param>
        public UploadProgressChangedLibArgs( long bytesSend, long totalBytesToSend )
        {
            TransmissionState = TransmissionState.Uploading;
            BytesSent = bytesSend;
            TotalBytesToSend = totalBytesToSend;
            Procent = Procent_.Get( bytesSend, totalBytesToSend );
        }//constructor

        /// <summary>
        /// Provides data for UploadFileChanged event
        /// </summary>
        /// <param name="transmissionState">TransmissionState e.g. Uploading, CreatingDir..</param>
        public UploadProgressChangedLibArgs( TransmissionState transmissionState )
        {
            TransmissionState = transmissionState;
            BytesSent = 0;
            TotalBytesToSend = 0;
            Procent = 0;
        }//constructor

    }//class
}//namespace
