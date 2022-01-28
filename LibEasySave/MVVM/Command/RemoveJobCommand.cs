using System;
using System.Windows.Input;

namespace LibEasySave
{
    public class RemoveJobCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;

        private IJobMng _model;

        public RemoveJobCommand(IJobMng model)
        {
            _model = model;
        }

        public bool CanExecute(object parameter)
        {
            if (!(parameter is string))
                return false;

            string name = parameter.ToString();

            if (string.IsNullOrEmpty(name))
                return false;

            if (!_model.Jobs.ContainsKey(name))
                return false;

            return true;
        }

        public void Execute(object parameter)
        {
            if (!CanExecute(parameter))
                return;

            _model.Jobs.Remove(parameter.ToString());
            LogMng.Instance.RemoveStateLog(_model.Jobs[parameter.ToString()].Guid);

        }
    }



}
