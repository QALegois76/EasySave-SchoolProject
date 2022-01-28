using LibEasySave.TranslaterSystem;
using System;
using System.Windows.Input;

namespace LibEasySave
{
    public class UnknownJobCommand: ICommand
    {
        public event EventHandler CanExecuteChanged;

        private string _lastError = null;
        private IModelViewJob _modelView;

        public UnknownJobCommand(IModelViewJob modelView)
        {
            _modelView = modelView;
        }

        public bool CanExecute(object parameter)
        {
            if (!(parameter is bool))
            {
                _lastError = Translater.Instance.TranslatedText.ErrorParameterWrongType;
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

            _lastError = ((bool)parameter) ? Translater.Instance.TranslatedText.CommandUnknow : Translater.Instance.TranslatedText.ErrorCommandNotAvailable;

            _modelView.FirePopMsgEventError(Translater.Instance.TranslatedText.ErrorMsg + " : " + _lastError);


        }
    }



}
