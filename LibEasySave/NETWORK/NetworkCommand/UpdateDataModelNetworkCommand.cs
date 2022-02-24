using LibEasySave.AppInfo;
using Newtonsoft.Json.Linq;
using System;
using System.Windows.Input;

namespace LibEasySave.Network
{
    public class UpdateDataModelNetworkCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;

        public UpdateDataModelNetworkCommand()
        {

        }

        public bool CanExecute(object parameter)
        {
            if (!(parameter is JToken))
                return false;

            if ((parameter as JToken).ToObject<DataModel>() == null)
                return false;

            return true;
        }

        public void Execute(object parameter)
        {
            if (!CanExecute(parameter))
                return;

            (DataModel.Instance as DataModel).Copy((parameter as JToken).ToObject<DataModel>());

        }
    }
}




//        UpdateJobData,
//        RunJobs,
//        UpdateDataModel,
//        LockUIClient,