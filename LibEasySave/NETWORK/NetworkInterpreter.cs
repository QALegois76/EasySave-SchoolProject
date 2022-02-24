using LibEasySave.AppInfo;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace LibEasySave.Network
{
    class NetworkInterpreter
    {
        private List<ENetorkCommand> AllowLockClientCommand = new List<ENetorkCommand>()
        { 
            ENetorkCommand.UpdateDataModel, 
            ENetorkCommand.UpdateJobList, 
            ENetorkCommand.UpdateJobProgress
        };

        private IModelViewJob _modelViewJob;
        private IViewDataModel _modelViewDataModel;

        public NetworkInterpreter(IModelViewJob modelViewJob, IViewDataModel modelViewDataModel)
        {
            this._modelViewJob = modelViewJob;
            this._modelViewDataModel = modelViewDataModel;
        }


        public void Interprete(NetworkInfo networkInfo)
        {
            if (DataModel.Instance.AppInfo.ModeIHM == EModeIHM.Server && DataModel.Instance.IsClientLock && !AllowLockClientCommand.Contains(networkInfo.Command))
                return;

            ICommand networkCommand = null;
            switch (networkInfo.Command)
            {
                case ENetorkCommand.AddJob:
                    networkCommand = new AddJobCommand(_modelViewJob.Model, _modelViewJob);
                    break;


                case ENetorkCommand.RemoveJob:
                    networkCommand = new RemoveJobCommand(_modelViewJob.Model, _modelViewJob);
                    break;


                case ENetorkCommand.UpdateJobData:
                    networkCommand = new UpdateDataJobNetworkCommand(_modelViewJob.Model, _modelViewJob);
                    break;


                case ENetorkCommand.UpdateJobList:
                    networkCommand = new UpdateJobListNetworkCommand(_modelViewJob.Model, _modelViewJob);
                    break;


                case ENetorkCommand.UpdateJobProgress:
                    networkCommand = new UpdateProgressJobsNetworkCommand();
                    break;


                case ENetorkCommand.RunJobs:
                    networkCommand = new RunJobNetworkCommand(_modelViewJob.Model, _modelViewJob);
                    break;


                case ENetorkCommand.UpdateDataModel:
                    networkCommand = new UpdateDataModelNetworkCommand();
                    break;


                case ENetorkCommand.LockUIClient:
                    networkCommand = new LockUIClient();
                    break;


                case ENetorkCommand.GetJobList:
                    networkCommand = new GetJobListNetworkCommand(_modelViewJob.Model);
                    break;


                case ENetorkCommand.GetDataModel:
                    networkCommand = new GetDataModelNetworkCommand();
                    break;


                default:
                case ENetorkCommand.Unknown:
                    return;
            }

            networkCommand?.Execute(networkInfo.Parameter);
        }


    }
}
