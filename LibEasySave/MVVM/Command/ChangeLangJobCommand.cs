using LibEasySave.TranslaterSystem;
using System;
using System.Windows.Input;

namespace LibEasySave
{
    public class ChangeLangJobCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;

        private string _lastError = null;
        private IModelViewJob _modelView;

        public ChangeLangJobCommand(IModelViewJob viewModel)
        {
            _modelView = viewModel;
        }

        public bool CanExecute(object parameter)
        {
            if (parameter.ToString() == _modelView.HELP)
                return true;

            ELangCode mode;
            if (!Enum.TryParse(parameter.ToString().Trim().ToUpper(), out mode))
            {
                _lastError = Translater.Instance.TranslatedText.ErrorParameterWrongType;
                return false;
            }


            return true;
        }

        public void Execute(object parameter)
        {
            if(!CanExecute(parameter))
            {
                _modelView.FirePopMsgEventError(Translater.Instance.TranslatedText.ErrorMsg + " : " + _lastError);
                return;
            }

            if (parameter.ToString() == _modelView.HELP)
            {
                _modelView.FirePopMsgEventInfo(Translater.Instance.TranslatedText.ChangeLangTemplate);
                foreach (var item in (ELangCode[])Enum.GetValues(typeof(ELangCode)))
                {
                    _modelView.FirePopMsgEventInfo(Translater.Instance.GetTextInfo(item));
                }
            }
            else
                Translater.Instance.SetActivLang((ELangCode)Enum.Parse(typeof(ELangCode), parameter.ToString().Trim().ToUpper()));
        }
    }



}
