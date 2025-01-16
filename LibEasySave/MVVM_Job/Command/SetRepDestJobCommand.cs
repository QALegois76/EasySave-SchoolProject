﻿using LibEasySave.TranslaterSystem;
using System;
using System.IO;
using System.Windows.Input;

namespace LibEasySave
{
    public class SetRepDestJobCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;

        private string _lastError = null;
        private IJobMng _model;
        private IModelViewJob _modelView;


        public SetRepDestJobCommand(IJobMng model, IModelViewJob modelView)
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

            string rep = parameter.ToString();

            if (string.IsNullOrEmpty(rep))
            {
                _lastError = Translater.Instance.TranslatedText.ErrorParameterWrongType;
                return false;
            }

            if (!Directory.Exists(rep))
            {
                _lastError = Translater.Instance.TranslatedText.ErrorFolderDontExist;
                return false;
            }

            if (!_model.BaseJober.ContainsKey(_model.EditingJob))
            {
                _lastError = Translater.Instance.TranslatedText.ErrorModelDontContainsEditingJob;
                return false;
            }

            if (_model.BaseJober[_model.EditingJob] == null)
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

            if (parameter.ToString() == _modelView.HELP)
            {
                _modelView.FirePopMsgEventInfo(Translater.Instance.TranslatedText.SetRepDestTemplate);
            }
            else
                _model.BaseJober[_model.EditingJob].Job.DestinationFolder = parameter.ToString();
        }
    }



}
