using System;
using System.Windows.Input;
using Chioy.Communication.Networking.KRNetWorkingTool.ViewModel;

namespace Chioy.Communication.Networking.KRNetWorkingTool.Command
{
    public class CommandBase : ICommand
    {
        private NetWorkingViewModel _networkingViewModel;

        public CommandBase()
        {
        }

        public CommandBase(NetWorkingViewModel p_networkingVM)
        {
            NetworkingViewModel = p_networkingVM;
        }

        public NetWorkingViewModel NetworkingViewModel
        {
            get { return _networkingViewModel; }
            set
            {
                _networkingViewModel = value;

                if (CanExecuteChanged != null)
                {
                    CanExecuteChanged(this, new EventArgs());
                }
            }
        }

        #region ICommand Members

        public virtual bool CanExecute(object parameter)
        {
            return NetworkingViewModel != null;
        }

        public event EventHandler CanExecuteChanged;

        public virtual void Execute(object parameter)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}