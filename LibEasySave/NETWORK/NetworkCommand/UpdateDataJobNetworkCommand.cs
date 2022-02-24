using Newtonsoft.Json.Linq;
using System;
using System.Windows.Input;

namespace LibEasySave.Network
{
    public class UpdateDataJobNetworkCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;

        private IJobMng _model;
        private IModelViewJob _modelView;


        public UpdateDataJobNetworkCommand(IJobMng model, IModelViewJob viewModel)
        {
            _model = model;
            _modelView = viewModel;
        }


        public bool CanExecute(object parameter)
        {
            if (parameter is IJob)
                return true;

            if (!(parameter is JToken))
                return false;


            var temp =(parameter as JToken).ToObject<Job>();

            if (temp == null)
                return false;

            if (!_model.BaseJober.ContainsKey(temp.Guid))
                return false;

            return true;
        }

        public void Execute(object parameter)
        {
            if (!CanExecute(parameter))
                return;

            IJob job;
            if (parameter is IJob)
                job = (IJob)parameter;
            else
                job = (parameter as JToken).ToObject<Job>();



            _model.BaseJober[job.Guid].Job.IsEncrypt = job.IsEncrypt;
            _model.BaseJober[job.Guid].Job.Name = job.Name;
            _model.BaseJober[job.Guid].Job.SourceFolder = job.SourceFolder;
            _model.BaseJober[job.Guid].Job.DestinationFolder = job.DestinationFolder;
            _model.BaseJober[job.Guid].Job.SavingMode = job.SavingMode;

            return;

            _model.EditingJob = job.Guid;

            RenameJobCommand renameJobCommand = new RenameJobCommand(_model, _modelView);
            renameJobCommand.Execute(job.Name);

            SetRepDestJobCommand setRepDestJob = new SetRepDestJobCommand(_model, _modelView);
            setRepDestJob.Execute(job.DestinationFolder);

            SetRepSrcJobCommand setRepSrcJob = new SetRepSrcJobCommand(_model, _modelView);
            setRepSrcJob.Execute(job.SourceFolder);

            SetSavingModeJobCommand setSavingMode = new SetSavingModeJobCommand(_model, _modelView);
            setSavingMode.Execute(job.SavingMode);

            _model.BaseJober[job.Guid].Job.IsEncrypt = job.IsEncrypt;

        }
    }
}




//        UpdateJobData,
//        RunJobs,
//        UpdateDataModel,
//        LockUIClient,