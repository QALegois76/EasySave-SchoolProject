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
            if (parameter == null)
                return false;

            if (string.IsNullOrEmpty(parameter.ToString()))
                return false;



            Guid g = Guid.Empty;

            if (!Guid.TryParse(parameter.ToString(), out g))
                return false;

            if (g == Guid.Empty)
                return false;

            if (!_model.BaseJober.ContainsKey(g))
                return false;

            return true;
        }

        public void Execute(object parameter)
        {
            if (!CanExecute(parameter))
                return;

            Guid g = Guid.Parse(parameter.ToString());

            RunCommand runCommand = new RunCommand(_model, _modelView);
            runCommand.Execute(g);
            

        }
    }
}




//        UpdateJobData,
//        RunJobs,
//        UpdateDataModel,
//        LockUIClient,