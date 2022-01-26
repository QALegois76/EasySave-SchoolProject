using System;
using System.IO;
using System.Windows.Input;

namespace LibEasySave
{
    public class SetRepSrcJobCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;

        private IJobMng _model;

        public SetRepSrcJobCommand(IJobMng model)
        {
            _model = model;
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

            if (!_model.Jobs.ContainsKey(_model.EditingJobName))
                return false;

            if (_model.Jobs[_model.EditingJobName] == null)
                return false;

            return true;
        }

        public void Execute(object parameter)
        {
            if (!CanExecute(parameter))
                return;

            _model.Jobs[_model.EditingJobName].SourceFolder = parameter.ToString();
        }
    }



}
