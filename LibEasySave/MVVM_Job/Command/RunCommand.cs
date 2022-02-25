using LibEasySave.AppInfo;
using LibEasySave.Model;
using LibEasySave.Network;
using LibEasySave.TranslaterSystem;
using System;
using System.Diagnostics;
using System.Threading;
using System.Windows.Input;

namespace LibEasySave
{
    /// <summary>
    /// RunCommand execute a command on an specific item (BaseJobsaver) of JobMng because of guid Which permit to find the good job.
    /// We use here multi-threading ThreadPool for executing jobs simultaneously and callback to impose threadPool to wait when resources are not free
    /// </summary>
    public class RunCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;

        /// <summary>
        /// Last error to meet
        /// </summary>
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
            if (parameter == null)
                return false;

            Guid name = Guid.Empty;
            if ((!(parameter is Guid) || !Guid.TryParse(parameter.ToString(), out name)))
            {
                _lastError = Translater.Instance.TranslatedText.ErrorParameterWrongType;
                return false;
            }

            if (name == Guid.Empty)
                return false;
            //if (parameter.ToString() == _modelView.HELP)
            //    return true;


            //if (string.IsNullOrEmpty(name))
            //{
            //    _lastError = Translater.Instance.TranslatedText.ErrorParameterNull;
            //    return false;
            //}

            //if (parameter.ToString().Trim().ToUpper() == _modelView.ALL)
            //    return true;

            if (!_model.BaseJober.ContainsKey(name))
            {
                _lastError = Translater.Instance.TranslatedText.ErrorNameDontExist;
                return false;
            }

            if (_model.BaseJober[name] == null)
            {
                _lastError = Translater.Instance.TranslatedText.ErrorEditingJobNull;
                return false;
            }

            return true;
        }

        //private bool IsSoftwareRunning()
        //{
        //    foreach (Process p in Process.GetProcesses())
        //    {
        //        if (p.ProcessName == "calc")
        //        {
        //            return true;
        //        }
        //    }
        //    return false;
        //}

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

            Guid g = Guid.Parse(parameter.ToString());

            if (_model.BaseJober[(Guid)parameter] != null)
            {
                if (DataModel.Instance.AppInfo.ModeIHM == EModeIHM.Server)
                {
                    NetworkMng.Instance.SendNetworkCommad(ENetorkCommand.RunJobs, g);
                }
                else
                {
                    WaitCallback callback = new WaitCallback(_model.BaseJober[g].Save);
                    ThreadPool.QueueUserWorkItem(callback);
                    LogMng.Instance.SaveDailyLog();
                    _modelView.FirePopMsgEventInfo(parameter.ToString() + " : " + Translater.Instance.TranslatedText.SucessMsg);
                }
            }
            else
            {
                _modelView.FirePopMsgEventInfo(parameter.ToString() + " : " + Translater.Instance.TranslatedText.FailMsg);
            }


        }
    }



}
