using LibEasySave.TranslaterSystem;
using System;
using System.Windows.Input;

namespace LibEasySave
{
    public class EditJobCommandUI : ICommand
    {
        public event EventHandler CanExecuteChanged;

        private string _lastError = null;

        private IModelViewJob _modelView = null;
        private IJobMng _model = null;

        public EditJobCommandUI(IJobMng model, IModelViewJob modelViewJob)
        {
            _model = model;
            _modelView = modelViewJob;
        }

        public bool CanExecute(object parameter)
        {
            if (!(parameter is string))
            {
                _lastError = Translater.Instance.TranslatedText.ErrorParameterWrongType;
                return false;
            }

            if (parameter.ToString() == _modelView.HELP)
                return true;

            string name = parameter.ToString();



            if (string.IsNullOrEmpty(name))
            {
                _lastError = Translater.Instance.TranslatedText.ErrorParameterNull;
                return false;
            }

            if (!_model.Jobs.ContainsKey(name))
            {
                _lastError = Translater.Instance.TranslatedText.ErrorNameDontExist;
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

            if (parameter.ToString() == _modelView.HELP)
            {
                _modelView.FirePopMsgEventInfo(Translater.Instance.TranslatedText.EditTemplate);
                _modelView.GetAllNameJobCommand.Execute(null);
            }
            else
            {

                _model.EditingJobName = parameter.ToString();
                _modelView.FireEditingEvent();
            }
        }
    }



}
