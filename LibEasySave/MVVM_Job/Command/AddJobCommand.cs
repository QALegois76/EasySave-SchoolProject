using LibEasySave.AppInfo;
using LibEasySave.Network;
using LibEasySave.TranslaterSystem;
using System;
using System.Collections.Generic;
using System.Windows.Input;

namespace LibEasySave
{
    public class AddJobCommand: ICommand
    {
        public event EventHandler CanExecuteChanged;


        private string _lastError = null;

        private IJobMng _model;
        private IModelViewJob _modelView;



        public AddJobCommand(IJobMng model, IModelViewJob modelView)
        {
            _model = model;
            _modelView = modelView;
        }

        public bool CanExecute(object parameter)
        {

            if (!(parameter == null || parameter is Guid))
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
            if (parameter?.ToString() == _modelView.HELP)
            {
                _modelView.FirePopMsgEventInfo(Translater.Instance.TranslatedText.AddTemplate);
            }
            else
            {
                Job j = new Job("");
                IJob job = j.Copy(_model.NextDefaultName,(Guid?)parameter);

                _model.BaseJober.Add(job.Guid, JobSaverFactory.CreateInstance(job));
                LogMng.Instance.AddStateLog(job.Guid, job.Name);
                NetworkMng.Instance.SendNetworkCommad(ENetorkCommand.AddJob, parameter);
                _modelView.FireAddingEvent(job.Guid);
            }
        }
    }


}
