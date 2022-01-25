using System;
using System.Windows.Input;

namespace LibEasySave
{
    public class RemoveJobCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;

        private IModelViewJob _modelView;

        public RemoveJobCommand(IModelViewJob modelView)
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

            return true;
        }

        public void Execute(object parameter)
        {
            if (!CanExecute(parameter))
                return;

            _modelView.Jobs.Remove(parameter.ToString());
        }
    }



}
