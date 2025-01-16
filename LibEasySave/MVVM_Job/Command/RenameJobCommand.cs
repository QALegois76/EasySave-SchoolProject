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


            Guid name = (Guid)parameter;

            if (name == Guid.Empty)
            {
                _lastError = Translater.Instance.TranslatedText.ErrorParameterNull;
                return false;
            }

            /// Disable for UI : we name string to guid

            //if(name.ToUpper() == _modelView.ALL)
            //{
            //    _lastError = Translater.Instance.TranslatedText.ErrorNameNotAllowed;
            //    return false;
            //}

            if (!_model.BaseJober.ContainsKey(_model.EditingJob))
            {
                _lastError = Translater.Instance.TranslatedText.ErrorModelDontContainsEditingJob;
                return false;
            }

            if (_model.BaseJober.ContainsKey(name))
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

                _model.BaseJober[_model.EditingJob].Job.Name = parameter.ToString();
                //_model.Jobs.Remove(_model.EditingJobName);
                //_model.Jobs.Add(job.Guid, job);
                //_model.EditingJobName = parameter.ToString();
                //LogMng.Instance.RenameJob(job.Guid, parameter.ToString());

            }
        }

    }
}
