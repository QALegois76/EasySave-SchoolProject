using LibEasySave.Model.LogMng.Interface;
using Newtonsoft.Json.Linq;
using System;
using System.Windows.Input;

namespace LibEasySave.Network
{
    public class UpdateProgressJobsNetworkCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;

        public UpdateProgressJobsNetworkCommand()
        {
        }

        public bool CanExecute(object parameter)
        {
            if (!(parameter is JToken))
                return false;

            var temp = (parameter as JToken).ToObject<ActivStateLog>();

            if (temp == null)
                return false;

            if (LogMng.Instance.GetStateLog(temp.Guid) == null)
                return false;

            return true;
        }

        public void Execute(object parameter)
        {
            if (!CanExecute(parameter))
                return;


            IActivStateLog activStateLogItem = (parameter as JToken).ToObject<ActivStateLog>();
            var temp = LogMng.Instance.GetStateLog(activStateLogItem.Guid);
            if (temp is IActivStateLog)
            {

                (temp as IActivStateLog).Progress.UpdateProgress(
                    activStateLogItem.Progress.PathCurrentSrcFile,
                    activStateLogItem.Progress.PathCurrentDestFile,
                    (temp as IActivStateLog).Progress.SizeFilesLeft - activStateLogItem.Progress.SizeFilesLeft);

            }
            else
            {
                LogMng.Instance.SetActivStateLog(
                    temp.Guid, activStateLogItem.TotalNbFiles,
                    activStateLogItem.TotalSizeFiles,
                    null, null);
                var activTemp = LogMng.Instance.GetStateLog(activStateLogItem.Guid) as IActivStateLog;
                
                activTemp.Progress.UpdateProgress(
                    activStateLogItem.Progress.PathCurrentSrcFile,
                    activStateLogItem.Progress.PathCurrentDestFile,
                    activTemp.Progress.SizeFilesLeft - activStateLogItem.Progress.SizeFilesLeft,
                    activTemp.Progress.NbFilesLeft - activStateLogItem.Progress.NbFilesLeft
                    );
                


            }




        }

    }
}




//        UpdateJobData,
//        RunJobs,
//        UpdateDataModel,
//        LockUIClient,