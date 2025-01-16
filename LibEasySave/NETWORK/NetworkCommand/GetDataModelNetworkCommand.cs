using LibEasySave.AppInfo;
using System;
using System.Windows.Input;

namespace LibEasySave.Network
{
    class GetDataModelNetworkCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            if (!CanExecute(parameter))
                return;

            NetworkMng.Instance.SendNetworkCommad(ENetorkCommand.UpdateDataModel, DataModel.Instance);
        }
    }
}


