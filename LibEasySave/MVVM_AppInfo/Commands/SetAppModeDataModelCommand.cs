using System;
using System.Windows.Input;

namespace LibEasySave.AppInfo
{
    public class SetAppModeDataModelCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;

        private IAppInfo _model;
        private UpdateDelegate _updateDelgate;
        public SetAppModeDataModelCommand(IAppInfo model, UpdateDelegate updateDelgate)
        {
            _model = model;
            _updateDelgate = updateDelgate;
        }

        public bool CanExecute(object parameter)
        {
            if (!(parameter is EModeIHM))
                return false;

            return true;
        }

        public void Execute(object parameter)
        {
            if (!CanExecute(parameter))
                return;

            _model.ModeIHM = (EModeIHM)parameter;
            _updateDelgate.Invoke();
        }
    }



}
