using System;
using System.Windows.Input;

namespace LibEasySave
{
    public class SetSavingModeJobCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;

        private IModelViewJob _modelView;

        public SetSavingModeJobCommand(IModelViewJob modelView)
        {
            _modelView = modelView;
        }

        public bool CanExecute(object parameter)
        {
            if (!(parameter is ESavingMode))
                return false;

            if (!_modelView.Jobs.ContainsKey(_modelView.ActivName))
                return false;

            if (_modelView.Jobs[_modelView.ActivName] == null)
                return false;

            return true;
        }

        public void Execute(object parameter)
        {
            if (!CanExecute(parameter))
                return;

            _modelView.Jobs[_modelView.ActivName].SavingMode = (ESavingMode)parameter;
        }
    }



}
