using System;
using System.Collections.Generic;
using System.Windows.Input;

namespace LibEasySave.AppInfo
{
    public class SetPriorityExtListDataModelCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;

        private IViewDataModel _model;

        public SetPriorityExtListDataModelCommand(IViewDataModel model)
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

            _model.DataModel.AppInfo.PriorityExt = (List<string>)parameter;
        }
    }



}
