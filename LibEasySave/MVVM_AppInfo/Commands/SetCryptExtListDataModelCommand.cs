using System;
using System.Collections.Generic;
using System.Windows.Input;

namespace LibEasySave.AppInfo
{
    public class SetCryptExtListDataModelCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;

        private ICryptInfo _model;

        public SetCryptExtListDataModelCommand(ICryptInfo model)
        {
            _model = model;
        }

        public bool CanExecute(object parameter)
        {
            if (!(parameter is List<string>))
                return false;

            return true;
        }

        public void Execute(object parameter)
        {
            if (!CanExecute(parameter))
                return;

            _model.AllowEtx = (List<string>)parameter;

        }
    }



}
