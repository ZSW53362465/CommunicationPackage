using Chioy.Communication.Networking.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chioy.Communication.Networking.Service
{
    public class BaseService : IDisposable
    {
        protected IDataProvider _provider = null;



        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
