using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Input;

namespace LibEasySave
{
    public class SaveJobFileJobCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;

        IJobMng _model;

        public SaveJobFileJobCommand(IJobMng model)
        {
            _model = model;
        }

        public bool CanExecute(object parameter)
        {
            if (!(parameter is string))
                return false;

            if (!Directory.Exists(Path.GetDirectoryName(parameter.ToString())))
                return false;

            return true;
        }

        public void Execute(object parameter)
        {
            if (!CanExecute(parameter))
                return;


            List<IJob> jobs = new List<IJob>();
            foreach (var item in _model.BaseJober)
            {
                jobs.Add(item.Value.Job);
            }

            FileSaverStrategy.Save(jobs, parameter.ToString(), true, ESavingFormat.JSON);
        }
    }


}
