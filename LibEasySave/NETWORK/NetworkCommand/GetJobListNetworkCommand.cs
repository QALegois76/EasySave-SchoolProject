using System;
using System.Collections.Generic;
using System.Windows.Input;

namespace LibEasySave.Network
{
    class GetJobListNetworkCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;

        IJobMng _model;

        public GetJobListNetworkCommand(IJobMng model)
        {
            _model = model;
        }

        public bool CanExecute(object parameter)
        {
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

            NetworkMng.Instance.SendNetworkCommad(ENetorkCommand.UpdateJobList, jobs);
        }
    }
}


