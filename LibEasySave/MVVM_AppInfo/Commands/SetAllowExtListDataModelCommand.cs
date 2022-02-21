using System;
using System.Collections.Generic;
using System.Windows.Input;

namespace LibEasySave.AppInfo
{
    public class SetAllowExtListDataModelCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;

        private IAppInfo _model;

        public SetAllowExtListDataModelCommand(IAppInfo model)
        {
            _model = model;
        }

        public bool CanExecute(object parameter)
        {
            if (!(parameter is string[]))
                return false;

            return true;
        }

        public void Execute(object parameter)
        {
            if (!CanExecute(parameter))
                return;

            _model.AllowExt = (List<string>)parameter;
        }
    }



}
