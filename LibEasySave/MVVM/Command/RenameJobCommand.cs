using System;
using System.Windows.Input;

namespace LibEasySave
{
    public class RenameJobCommand : ICommand
    {

        public event EventHandler CanExecuteChanged;

        private IJobMng _model;

        public RenameJobCommand(IJobMng model)
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

            if (!_model.Jobs.ContainsKey(_model.EditingJobName))
                return false;

            if (_model.Jobs.ContainsKey(name))
                return false;

            return true;
        }

        public void Execute(object parameter)
        {
            if (!CanExecute(parameter))
                return;

            IJob job = _model.Jobs[_model.EditingJobName].Copy(false, parameter.ToString());
            _model.Jobs.Remove(_model.EditingJobName);
            _model.Jobs.Add(parameter.ToString(), job);
            _model.EditingJobName = parameter.ToString();

            LogMng.Instance.RenameJob(job.Guid, parameter.ToString());
        }
    }
}
