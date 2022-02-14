﻿using LibEasySave.Model;
using LibEasySave.TranslaterSystem;
using System;
using System.Diagnostics;
using System.Windows.Input;

namespace LibEasySave
{
    public class RunCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;

        private string _lastError = null;
        private IJobMng _model;
        private IModelViewJob _modelView;

        public RunCommand(IJobMng model, IModelViewJob modelView)
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

            Guid name = (Guid)parameter;

            //if (string.IsNullOrEmpty(name))
            //{
            //    _lastError = Translater.Instance.TranslatedText.ErrorParameterNull;
            //    return false;
            //}

            //if (parameter.ToString().Trim().ToUpper() == _modelView.ALL)
            //    return true;

            if (!_model.Jobs.ContainsKey(name))
            {
                _lastError = Translater.Instance.TranslatedText.ErrorNameDontExist;
                return false;
            }

            if (_model.Jobs[name] == null)
            {
                _lastError = Translater.Instance.TranslatedText.ErrorEditingJobNull;
                return false;
            }

            if (IsSoftwareRunning())
            {
                _lastError = Translater.Instance.TranslatedText.ErrorSoftwareIsRunning;
                return false;
            }

            return true;
        }

        private bool IsSoftwareRunning()
        {
            foreach (Process p in Process.GetProcesses())
            {
                if (p.ProcessName == "calc")
                {
                    return true;
                }
            }
            return false;
        }

        public void Execute(object parameter)
        {
            if (!CanExecute(parameter))
            {
                _modelView.FirePopMsgEventError(Translater.Instance.TranslatedText.ErrorMsg + " : " + _lastError);
                return;
            }

            //if (parameter.ToString() == _modelView.HELP)
            //{
            //    _modelView.FirePopMsgEventInfo(Translater.Instance.TranslatedText.RunTemplate);
            //}
            //else if (parameter.ToString().Trim().ToUpper() == _modelView.ALL)
            //    _modelView.RunAllJobCommand.Execute(null);

            if (JobSaverStrategy.Save(_model.Jobs[(Guid)parameter]))
            {
                _modelView.FirePopMsgEventInfo(parameter.ToString() + " : " + Translater.Instance.TranslatedText.SucessMsg);
            }
            else
            {
                _modelView.FirePopMsgEventInfo(parameter.ToString() + " : " + Translater.Instance.TranslatedText.FailMsg);
            }


        }
    }



}
