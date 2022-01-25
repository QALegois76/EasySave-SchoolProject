using System;
using System.IO;
using System.Windows.Input;

namespace LibEasySave
{
    public class SetRepDestJobCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;

        private IModelViewJob _modelView;

        public SetRepDestJobCommand(IModelViewJob modelView)
        {
            _modelView = modelView;
        }

        public bool CanExecute(object parameter)
        {
            if (!(parameter is string))
                return false;

            string rep = parameter.ToString();

            if (string.IsNullOrEmpty(rep))
                return false;

            if (!Directory.Exists(rep))
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

            _modelView.Jobs[_modelView.ActivName].DestinationFolder = parameter.ToString();
        }
    }



}
