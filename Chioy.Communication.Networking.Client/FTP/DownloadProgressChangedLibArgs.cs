using Chioy.Communication.Networking.Client.FTP.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chioy.Communication.Networking.Client.FTP
{
    /// <summary>
    /// Provides data for DownloadFileChanged event
    /// </summary>
    public class DownloadProgressChangedLibArgs : EventArgs
    {
        /// <summary>
        /// Gets bytes downloaded
        /// </summary>
        public long BytesReceived { get; private set; }

        /// <summary>
        /// Gets total bytes downloaded
        /// </summary>
        public long TotalBytesReceived { get; private set; }

        /// <summary>
        /// Gets procent of download 
        /// </summary>
        public int Procent { get; private set; }

        /// <summary>
        /// Provides data for DownloadFileChanged event
        /// </summary>
        /// <param name="bytesReceived">Bytes downloaded</param>
        /// <param name="totalBytesReceived">Total bytes downloaded</param>
        public DownloadProgressChangedLibArgs( long bytesReceived, long totalBytesReceived )
        {
            BytesReceived = bytesReceived;
            TotalBytesReceived = totalBytesReceived;
            Procent = Procent_.Get( bytesReceived, totalBytesReceived );
        }//constructor
        
    }//class
}//namespace
