using LibEasySave.TranslaterSystem;
using System;
using System.Windows.Input;

namespace LibEasySave
{
    public class SetSavingModeJobCommandUI : ICommand
    {
        public event EventHandler CanExecuteChanged;

        private string _lastError = null;
        private IJobMng _model;
        private IModelViewJob _modelView;

        public SetSavingModeJobCommandUI(IJobMng model, IModelViewJob modelView)
        {
            _model = model;
            _modelView = modelView;
        }

        public bool CanExecute(object parameter)
        {
            if (parameter.ToString() == _modelView.HELP)
                return true;

            ESavingMode mode;
            if (!Enum.TryParse(parameter.ToString().Trim().ToUpper(),out mode))
            {
                _lastError = Translater.Instance.TranslatedText.ErrorParameterWrongType;
                return false;
            }


            if (string.IsNullOrEmpty(_model.EditingJobName))
            {
                _lastError = Translater.Instance.TranslatedText.ErrorEditingJobNameNull;
                return false;
            }

            if (!_model.Jobs.ContainsKey(_model.EditingJobName))
            {
                _lastError = Translater.Instance.TranslatedText.ErrorModelDontContainsEditingJob;
                return false;
            }

            if (_model.Jobs[_model.EditingJobName] == null)
            {
                _lastError = Translater.Instance.TranslatedText.ErrorEditingJobNull;
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

            if (parameter.ToString()== _modelView.HELP)
            {
                _modelView.FirePopMsgEventInfo(Translater.Instance.TranslatedText.SetRepSavingModeTemplate);
                foreach (ESavingMode eSavingMode in (ESavingMode[])Enum.GetValues(typeof(ESavingMode)))
                {
                    _modelView.FirePopMsgEventInfo(Translater.Instance.GetTextInfo(eSavingMode));
                }
            }
            else
                _model.Jobs[_model.EditingJobName].SavingMode = (ESavingMode)Enum.Parse(typeof(ESavingMode), parameter.ToString().Trim().ToUpper());
        }
    }



}
