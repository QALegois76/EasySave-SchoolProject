using System;
using System.Collections.Generic;
using System.Windows.Input;

namespace LibEasySave
{
    public class AddJobCommand: ICommand
    {
        IModelViewJob _modelView;

        public event EventHandler CanExecuteChanged;

        public AddJobCommand(IModelViewJob modelView)
        {
            _modelView = modelView;
        }

        public bool CanExecute(object parameter)
        {
            if (!(parameter is string))
                return false;

            return true;

        }

        public void Execute(object parameter)
        {
            if (!CanExecute(parameter))
                return;

            if (_modelView.Jobs.ContainsKey(parameter.ToString()))
                return;

            _modelView.Jobs.Add(parameter.ToString(), new Job(parameter.ToString()));
        }
    }



}
