using System;
using System.Windows.Input;
using KRNetWorkingTool.ViewModel;

namespace KRNetWorkingTool.Command
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