﻿using LibEasySave.TranslaterSystem;
using System;
using System.Windows.Input;

namespace LibEasySave
{
    public class GetRepSrcJobCommandUI : ICommand
    {
        public event EventHandler CanExecuteChanged;

        private string _lastError = null;
        private IModelViewJob _modelView;
        private IJobMng _model;


        // constructor
        public GetRepSrcJobCommandUI( IJobMng model, IModelViewJob viewModel)
        {
            _modelView = viewModel;
            _model = model;
        }

        public bool CanExecute(object parameter)
        {
            if (string.IsNullOrEmpty(_model.EditingJobName))
            {
                _lastError = Translater.Instance.TranslatedText.ErrorParameterNull;
                return false;
            }

            if (parameter.ToString() == _modelView.HELP)
                return true;

            if (!_model.Jobs.ContainsKey(_modelView.EditingJobName))
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

            if (parameter.ToString() == _modelView.HELP)
            {
                _modelView.FirePopMsgEventInfo(Translater.Instance.TranslatedText.GetRepSrcTemplate);
            }
            else 
                _modelView.FirePopMsgEventInfo(_model.Jobs[_modelView.EditingJobName].SourceFolder);
        }
    }



}
