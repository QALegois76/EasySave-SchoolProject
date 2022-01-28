using LibEasySave.TranslaterSystem;
using System;
using System.Windows.Input;

namespace LibEasySave
{
    public class RenameJobCommand : ICommand
    {

        public event EventHandler CanExecuteChanged;

        private string _lastError = null;

        private IJobMng _model;
        private IModelViewJob _modelView;

        public RenameJobCommand(IJobMng model, IModelViewJob modelView)
        {
            _model = model;
            _modelView = modelView;
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

            if(name.ToUpper() == _modelView.ALL)
            {
                _lastError = Translater.Instance.TranslatedText.ErrorNameNotAllowed;
                return false;
            }

            if (!_model.Jobs.ContainsKey(_model.EditingJobName))
            {
                _lastError = Translater.Instance.TranslatedText.ErrorModelDontContainsEditingJob;
                return false;
            }

            if (_model.Jobs.ContainsKey(name))
            {
                _lastError = Translater.Instance.TranslatedText.ErrorNameExistAlready;
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
                _modelView.FirePopMsgEventInfo(Translater.Instance.TranslatedText.RenameTemplate);
            }
            else
            {

                IJob job = _model.Jobs[_model.EditingJobName].Copy(false, parameter.ToString());
                _model.Jobs.Remove(_model.EditingJobName);
                _model.Jobs.Add(parameter.ToString(), job);
                _model.EditingJobName = parameter.ToString();
                LogMng.Instance.RenameJob(job.Guid, parameter.ToString());

            }
        }

    }
}
