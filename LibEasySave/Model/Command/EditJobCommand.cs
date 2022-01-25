using System;
using System.Windows.Input;

namespace LibEasySave
{
    public class EditJobCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;

        private IModelViewJob _modelViewJob = null;

        public EditJobCommand(IModelViewJob modelViewJob)
        {
            _modelViewJob = modelViewJob;
        }

        public bool CanExecute(object parameter)
        {
            if (!(parameter is string))
                return false;

            string name = parameter.ToString();

            if (string.IsNullOrEmpty(name))
                return false;

            if (!_modelViewJob.Jobs.ContainsKey(name))
                return false;

            return true;
        }

        public void Execute(object parameter)
        {
            if (!CanExecute(parameter))
                return;

            _modelViewJob.ActivName = parameter.ToString();
        }
    }



}
