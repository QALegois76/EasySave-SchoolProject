using LibEasySave.TranslaterSystem;
using System;
using System.Collections.Generic;
using System.Windows.Input;

namespace LibEasySave
{
    public class AddJobCommandUI: ICommand
    {
        public event EventHandler CanExecuteChanged;


        private string _lastError = null;

        private IJobMng _model;
        private IModelViewJob _modelView;



        public AddJobCommandUI(IJobMng model, IModelViewJob modelView)
        {
            _model = model;
            _modelView = modelView;
        }

        public bool CanExecute(object parameter)
        {
            if (!(parameter is Guid))
            {
                _lastError = Translater.Instance.TranslatedText.ErrorParameterWrongType;
                return false;
            }


            Guid name = (Guid)parameter;

            if (name == Guid.Empty)
            {
                _lastError = Translater.Instance.TranslatedText.ErrorParameterNull;
                return false;
            }

            if (_model.Jobs.ContainsKey(name))
            {
                _lastError = Translater.Instance.TranslatedText.ErrorNameExistAlready;
                return false;
            }

            if (_model.Jobs.Count >= _model.MAX_JOB)
            {
                _lastError = Translater.Instance.TranslatedText.ErrorMaxJob;
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
                _modelView.FirePopMsgEventInfo(Translater.Instance.TranslatedText.AddTemplate);
            }
            else
            {
                _model.Jobs.Add(parameter, _model.JOB_MODEL.Copy(true, parameter.ToString()));
                LogMng.Instance.AddStateLog(_model.Jobs[parameter.ToString()].Guid, parameter.ToString());
            }
        }
    } 


}
