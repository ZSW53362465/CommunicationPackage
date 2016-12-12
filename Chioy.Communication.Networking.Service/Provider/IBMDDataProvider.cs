using Chioy.Communication.Networking.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chioy.Communication.Networking.Service.Provider
{
    public interface IBMDDataProvider : IDataProvider
    {
        string TestValue();

    }
}
