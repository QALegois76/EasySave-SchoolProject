using Newtonsoft.Json.Linq;
using System;
using System.Windows.Input;

namespace LibEasySave.Network
{
    public class RunJobNetworkCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;

        private IJobMng _model;
        private IModelViewJob _modelView;

        public RunJobNetworkCommand(IJobMng model, IModelViewJob modelView)
        {
            _model = model;
            _modelView = modelView;
        }

        public bool CanExecute(object parameter)
        {
            if (!(parameter is JArray))
                return false;


            var temp = (parameter as JArray).ToObject<Job[]>();

            if (temp == null)
                return false;




            foreach (IJob job in temp)
            {
                if (!_model.Jobs.ContainsKey((parameter as IJob).Guid))
                    return false;
            }

            return true;
        }

        public void Execute(object parameter)
        {
            if (!CanExecute(parameter))
                return;

            var temp = (parameter as JArray).ToObject<Job[]>();


            foreach (IJob job in temp)
            {
                RunCommand runCommand = new RunCommand(_model, _modelView);
                runCommand.Execute(job);
            }

        }
    }
}




//        UpdateJobData,
//        RunJobs,
//        UpdateDataModel,
//        LockUIClient,