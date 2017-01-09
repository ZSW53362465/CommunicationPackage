namespace Chioy.Communication.Networking.Service.Provider
{
    public class DataProviderAdpter
    {
        protected IDataProvider _provider = null;
        public void RegisterProvider(IDataProvider provider)
        {
            _provider = provider;
        }
    }
}
