using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chioy.Communication.Networking.Client.FTP.Helper

{
    /// <summary>
    /// Defines transmissionState
    /// </summary>
    public enum TransmissionState
    {
        /// <summary>
        /// Transmission finnished successful
        /// </summary>
        Success,
        /// <summary>
        /// Transmission failed
        /// </summary>
        Failed,
        /// <summary>
        /// You try to resume upload, but file on ftp-server is bigger as your localfile
        /// </summary>
        LocalFileBiggerAsRemoteFile,
        /// <summary>
        /// DotNetFtpLibrary proofs if directory exits
        /// </summary>
        ProofingDirExits,
        /// <summary>
        /// DotNetFtpLibrary is creating (sub)directories
        /// </summary>
        CreatingDir,
        /// <summary>
        /// DotNetFtpLibrary uploads a file to ftp-server
        /// </summary>
        Uploading
    }//enum
}//namespace
