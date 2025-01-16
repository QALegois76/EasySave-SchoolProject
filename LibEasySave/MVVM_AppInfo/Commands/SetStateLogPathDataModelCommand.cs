using System;
using System.Windows.Input;
using System.IO;

namespace LibEasySave.AppInfo
{
    public class SetStateLogPathDataModelCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;

        private IViewDataModel _model;

        public SetStateLogPathDataModelCommand(IViewDataModel model)
        {
            _model = model;
        }

        public bool CanExecute(object parameter)
        {
            if (string.IsNullOrWhiteSpace(parameter.ToString()))
                return false;

            if (!(Directory.Exists(parameter.ToString())))
                return false;


            return true;
        }

        public void Execute(object parameter)
        {
            if (!CanExecute(parameter))
                return;

            _model.DataModel.LogInfo.StateLogPath = parameter.ToString();
        }
    }



}
