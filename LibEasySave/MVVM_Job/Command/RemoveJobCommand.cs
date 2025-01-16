using LibEasySave.AppInfo;
using LibEasySave.Network;
using LibEasySave.TranslaterSystem;
using System;
using System.Windows.Input;

namespace LibEasySave
{
    public class RemoveJobCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;

        private string _lastError = null;
        private IJobMng _model;
        private IModelViewJob _modelView;

        public RemoveJobCommand(IJobMng model, IModelViewJob modelView)
        {
            _model = model;
            _modelView = modelView;
        }

        public bool CanExecute(object parameter)
        {
            if (parameter.ToString() == _modelView.HELP)
                return true;

            Guid name = Guid.Empty;

            if (parameter == null)
            {
                _lastError = Translater.Instance.TranslatedText.ErrorParameterNull;
                return false;
            }
            
            if (!Guid.TryParse(parameter.ToString(), out name))
            {
                _lastError = Translater.Instance.TranslatedText.ErrorParameterWrongType;
                return false;
            }




            //if (string.IsNullOrEmpty(name))
            //{
            //    _lastError = Translater.Instance.TranslatedText.ErrorParameterNull;
            //    return false;
            //}

            if (!_model.BaseJober.ContainsKey(name))
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
                _modelView.FirePopMsgEventInfo(Translater.Instance.TranslatedText.RemoveTemplate);
            }
            else
            {
                Guid g = Guid.Parse(parameter.ToString());

                LogMng.Instance.RemoveStateLog(g);
                _model.BaseJober.Remove(g);
                NetworkMng.Instance.SendNetworkCommad(ENetorkCommand.RemoveJob,g);
                _modelView.FireRemovingEvent(g);
            }

        }
    }



}
