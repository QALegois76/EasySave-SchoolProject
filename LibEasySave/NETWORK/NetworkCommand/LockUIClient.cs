using LibEasySave.AppInfo;
using System;
using System.Windows.Input;

namespace LibEasySave.Network
{
    public class LockUIClient : ICommand
    {
        public event EventHandler CanExecuteChanged;


        public bool CanExecute(object parameter)
        {
            if (!(parameter is bool))
                return false;

            return true;
        }

        public void Execute(object parameter)
        {
            if (!CanExecute(parameter))
                return;

            DataModel.Instance.IsClientLock = (bool)parameter;
            // fire event to lock UI
        }
    }
}


