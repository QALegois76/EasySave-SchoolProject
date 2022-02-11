using LibEasySave.TranslaterSystem;
using System;
using System.Windows.Input;

namespace LibEasySave
{
    public class RunAllJobUI : ICommand
    {
        public event EventHandler CanExecuteChanged;

        private string _lastError = null;
        private IJobMng _model;
        private IModelViewJob _modelView;

        public RunAllJobUI(IJobMng model , IModelViewJob modelView)
        {
            _model = model;
            _modelView = modelView;
        }

        public bool CanExecute(object parameter)
        {
            if (_model.Jobs.Count==0)
            {
                _lastError = Translater.Instance.TranslatedText.ErrorNoJobDeclared;
                return false;
            }

            return true;
        }

        public void Execute(object parameter)
        {
            if (!CanExecute(parameter))
            {
                _modelView.FirePopMsgEventError(Translater.Instance.TranslatedText.ErrorMsg + " : " + _lastError);
                return;
            }

            foreach (var item in _model.Jobs)
            {
                _modelView.RunJobCommand.Execute(item.Key);
            }
            
        }
    }



}
