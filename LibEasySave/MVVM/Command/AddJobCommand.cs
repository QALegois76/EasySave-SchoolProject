using System;
using System.Collections.Generic;
using System.Windows.Input;

namespace LibEasySave
{
    public class AddJobCommand: ICommand
    {
        IJobMng _model;

        public event EventHandler CanExecuteChanged;

        public AddJobCommand(IJobMng model)
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

            if (_model.Jobs.ContainsKey(name))
                return false;

            if (_model.Jobs.Count >= _model.MAX_JOB)
                return false;

            return true;

        }

        public void Execute(object parameter)
        {
            if (!CanExecute(parameter))
                return;

            _model.Jobs.Add(parameter.ToString(), _model.JOB_MODEL.Copy(parameter.ToString()));
        }
    }



}
