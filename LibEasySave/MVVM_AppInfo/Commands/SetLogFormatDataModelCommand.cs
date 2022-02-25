using System;
using System.Windows.Input;

namespace LibEasySave.AppInfo
{
    public class SetLogFormatDataModelCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;

        private IViewDataModel _model;
        private UpdateDelegate _updateDelegate;

        public SetLogFormatDataModelCommand(IViewDataModel model, UpdateDelegate updateDelegate)
        {
            _model = model;
            _updateDelegate = updateDelegate;
        }

        public bool CanExecute(object parameter)
        {
            if (!(parameter is ESavingFormat))
                return false;

            return true;
        }

        public void Execute(object parameter)
        {
            if (!CanExecute(parameter))
                return;

            _model.DataModel.LogInfo.SavingFormat = (ESavingFormat)parameter;
            _updateDelegate.Invoke();
        }
    }



}
