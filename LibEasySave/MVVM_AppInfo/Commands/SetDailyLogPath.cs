using System;
using System.Windows.Input;
using System.Reflection;
using System.IO;

namespace LibEasySave.AppInfo
{
    public class SetDailyLogPathDataModelCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;

        private IViewDataModel _model;

        public SetDailyLogPathDataModelCommand(IViewDataModel model)
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

            _model.DataModel.LogInfo.DailyLogPath = parameter.ToString();
        }
    }   



}
