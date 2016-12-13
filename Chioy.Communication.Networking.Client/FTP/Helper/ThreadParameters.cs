using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chioy.Communication.Networking.Client.FTP.Helper
{
    internal class ThreadParameters
    {
        internal string LocalDirectory { get; set; }
        internal string LocalFilename { get; set; }
        internal string RemoteDirectory { get; set; }
        internal string RemoteFilename { get; set; }

        internal ThreadParameters(string localDirectory, string localFileName, string remoteDirectory, string remoteFileName)
        {
            LocalDirectory = localDirectory;
            LocalFilename = localFileName;
            RemoteDirectory = remoteDirectory;
            RemoteFilename = remoteFileName;
        }//constructor
    }//calss
}
