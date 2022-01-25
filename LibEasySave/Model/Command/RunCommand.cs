using System;
using System.Windows.Input;

namespace LibEasySave
{
    public class RunCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;

        private IModelViewJob _modelView;

        public RunCommand(IModelViewJob modelView)
        {
            _modelView = modelView;
        }

        public bool CanExecute(object parameter)
        {
            if (!(parameter is string))
                return false;

            string name = parameter.ToString();

            if (string.IsNullOrEmpty(name))
                return false;

            if (!_modelView.Jobs.ContainsKey(name))
                return false;

            if (_modelView.Jobs[name] == null)
                return false;

            return true;
        }

        public void Execute(object parameter)
        {
            if (!CanExecute(parameter))
                return;

            // JobSaverStrategy
        }
    }



}
