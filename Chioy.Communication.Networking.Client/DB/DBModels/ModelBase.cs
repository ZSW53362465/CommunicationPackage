using System.ComponentModel;

namespace Chioy.Communication.Networking.Client.DB.Models
{
    public class ModelBase : INotifyPropertyChanged
    {
        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        public void RaisePropertyChanged(string p_propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(p_propertyName));
            }
        }

        public void RaisePropertiesChanged(params string[] p_propertyNames)
        {
            if (PropertyChanged != null)
            {
                foreach (string propertyName in p_propertyNames)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
                }
            }
        }

        #endregion
    }
}