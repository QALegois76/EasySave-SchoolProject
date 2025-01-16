using System;
using System.Windows.Input;

namespace LibEasySave
{
    public class ExitJobCommandUI : ICommand
    {
        public event EventHandler CanExecuteChanged;

        public ExitJobCommandUI()
        {
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            if (!CanExecute(parameter))
                return;
            LogMng.Instance.Exit();
        }
    }



}
