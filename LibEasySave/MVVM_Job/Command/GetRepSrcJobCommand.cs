﻿using LibEasySave.TranslaterSystem;
using System;
using System.Windows.Input;

namespace LibEasySave
{
    public class GetRepSrcJobCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;

        private string _lastError = null;
        private IModelViewJob _modelView;
        private IJobMng _model;


        // constructor
        public GetRepSrcJobCommand( IJobMng model, IModelViewJob viewModel)
        {
            _modelView = viewModel;
            _model = model;
        }

        public bool CanExecute(object parameter)
        {
            //if (!(parameter is Guid))
            //{
            //    _lastError = Translater.Instance.TranslatedText.ErrorParameterWrongType;
            //    return false;
            //}

            //if (string.IsNullOrEmpty(_model.EditingJob))
            //{
            //    _lastError = Translater.Instance.TranslatedText.ErrorParameterNull;
            //    return false;
            //}

            if (parameter.ToString() == _modelView.HELP)
                return true;

            if (!_model.BaseJober.ContainsKey(_modelView.EditingJob))
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
                _modelView.FirePopMsgEventInfo(Translater.Instance.TranslatedText.GetRepSrcTemplate);
            }
            else 
                _modelView.FirePopMsgEventInfo(_model.BaseJober[_modelView.EditingJob].Job.SourceFolder);
        }
    }



}
