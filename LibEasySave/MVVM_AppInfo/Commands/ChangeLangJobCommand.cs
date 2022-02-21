using LibEasySave.AppInfo;
using LibEasySave.TranslaterSystem;
using System;
using System.Windows.Input;

namespace LibEasySave.AppInfo
{
    public class ChangeLangJobCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;

        private IAppInfo _model;
        private UpdateDelegate _updateDelegate;

        public ChangeLangJobCommand(IAppInfo model , UpdateDelegate updateDelegate)
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
            _model.ActivLang = (ELangCode)parameter;
            _updateDelegate.Invoke();
        }
    }



}
