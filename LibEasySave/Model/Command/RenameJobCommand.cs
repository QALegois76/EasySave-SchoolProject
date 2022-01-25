using System;
using System.Windows.Input;

namespace LibEasySave
{
    public class RenameJobCommand : ICommand
    {

        public event EventHandler CanExecuteChanged;

        private IModelViewJob _modelView;


        public RenameJobCommand(IModelViewJob modelView)
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

            if (!_modelView.Jobs.ContainsKey(_modelView.ActivName))
                return false;

            if (_modelView.Jobs.ContainsKey(name))
                return false;

            return true;
        }

        public void Execute(object parameter)
        {
            if (!CanExecute(parameter))
                return;

            IJob job = _modelView.Jobs[_modelView.ActivName].Copy(parameter.ToString());
            _modelView.Jobs.Remove(_modelView.ActivName);
            _modelView.Jobs.Add(parameter.ToString(), job);
            _modelView.ActivName = parameter.ToString();
        }



    }



}
