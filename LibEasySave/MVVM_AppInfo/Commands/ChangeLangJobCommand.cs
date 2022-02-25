using LibEasySave.AppInfo;
using LibEasySave.TranslaterSystem;
using System;
using System.Windows.Input;

namespace LibEasySave.AppInfo
{
    public class ChangeLangJobCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;

        private IViewDataModel _model;
        private UpdateDelegate _updateDelegate;

        public ChangeLangJobCommand(IViewDataModel model , UpdateDelegate updateDelegate)
        {
            _model = model;
            _updateDelegate = updateDelegate;
        }

        public bool CanExecute(object parameter)
        {
            if (!(parameter is ELangCode))
                return false;

            return true;
        }

        public void Execute(object parameter)
        {
            if (!CanExecute(parameter))
                return;
            _model.DataModel.AppInfo.ActivLang = (ELangCode)parameter;
            _updateDelegate.Invoke();
        }
    }



}
