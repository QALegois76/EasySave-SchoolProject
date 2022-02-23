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
            if (!(parameter is Guid))
            {
                _lastError = Translater.Instance.TranslatedText.ErrorParameterWrongType;
                return false;
            }

            //if (parameter.ToString() == _modelView.HELP)
            //    return true;

            //Guid name = (Guid) parameter;

            ////if (string.IsNullOrEmpty(name))
            ////{
            ////    _lastError = Translater.Instance.TranslatedText.ErrorParameterNull;
            ////    return false;
            ////}

            //if (_model.Jobs.ContainsKey(name))
            //{
            //    _lastError = Translater.Instance.TranslatedText.ErrorNameExistAlready;
            //    return false;
            //}


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
                Guid? param = (Guid?)parameter;

                IJob job = _model.JOB_MODEL.Copy(_model.NextDefaultName,param);


                _model.Jobs.Add(job.Guid, job);
                LogMng.Instance.AddStateLog(job.Guid, job.Name);
                _modelView.FireAddingEvent(job.Guid);
            }
        }
    }


    public class OpenJobCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;

        private IJobMng _model;

        public OpenJobCommand(IJobMng model)
        {
            _model = model;
        }

        public bool CanExecute(object parameter)
        {
            if (!(parameter is string))
                return false;

            if (_model == null)
                return false;

            return true;
        }

        public void Execute(object parameter)
        {
            if (!CanExecute(parameter))
                return;


        }
    }


    public class SaveJobCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            throw new NotImplementedException();
        }

        public void Execute(object parameter)
        {
            throw new NotImplementedException();
        }
    }

    public class SaveJobFileInfo
    {
        public enum EJobFileSavingFormat
        {
            EasySaveCommand,
            JSON
        }

        private string _fileName;

        private EJobFileSavingFormat _savingFormat = EJobFileSavingFormat.JSON;

        public string FileName => _fileName;
        public EJobFileSavingFormat SavingFormat => _savingFormat;


        public SaveJobFileInfo(string fileName , EJobFileSavingFormat jobFileSavingFormat)
        {
            _fileName = fileName;
            _savingFormat = jobFileSavingFormat;
        }


    }


}
