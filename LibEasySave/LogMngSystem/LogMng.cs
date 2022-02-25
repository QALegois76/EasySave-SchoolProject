using LibEasySave.AppInfo;
using LibEasySave.Model.LogMng.Interface;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace LibEasySave
{
    public class LogMng
    {
        private static LogMng _instance;
        private static readonly object _lockobj = new object();

        private const string FILE_NAME_DAILY_LOG = "EasySave_DailyLog.txt";
        private const string FILE_NAME_STATE_LOG = "EasySave_StateLog.txt";
        private string PATH_DAILY_LOG;
        private string PATH_STATE_LOG;


        private List<IDailyLog> _dailyLogs = new List<IDailyLog>();

        private List<IStateLog> _statesLog = new List<IStateLog>();

        public event ProgressJobEventHandler OnProgressChanged;

        // private constructor
        private LogMng()
        {
            //PATH_DAILY_LOG = Path.Combine( Environment.GetEnvironmentVariable(ENV_VAR_NAME),FILE_NAME_DAILY_LOG);
            PATH_DAILY_LOG = Path.Combine(DataModel.Instance.LogInfo.DailyLogPath, FILE_NAME_DAILY_LOG);
            //PATH_STATE_LOG =Path.Combine( Environment.GetEnvironmentVariable(ENV_VAR_NAME), FILE_NAME_STATE_LOG);
            PATH_STATE_LOG =Path.Combine(DataModel.Instance.LogInfo.DailyLogPath, FILE_NAME_STATE_LOG);

            DataModel.Instance.LogInfo.PropertyChanged -= LogInfo_PropertyChanged;
            DataModel.Instance.LogInfo.PropertyChanged += LogInfo_PropertyChanged;
        }



        public static LogMng Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_lockobj)
                    {
                        if (_instance == null)
                        {
                            _instance = new LogMng();
                        }
                    }
                }
                return _instance;
            }
        }


        #region MyRegion

        public void AddStateLog(Guid guid , string name)
        {
            if (GetStateLog(guid)!= null)
            {
                Debug.Fail("error guid already existing");
                return;
            }
            _statesLog.Add(new StateLog(name,guid, false));
        }

        public void RemoveStateLog(Guid guid)
        {
            for (int i = 0; i < _statesLog.Count; i++)
            {
                if (_statesLog[i].Guid == guid)
                {
                    _statesLog.RemoveAt(i);
                    return;
                }    
            }
        }

        public void RenameJob(Guid guid , string name)
        {
            var log = GetStateLog(guid);
            log = new StateLog(name, log.Guid);
        }

        public void SetActivStateLog(Guid guid, int totalNFiles , long totalSize, string srcFile , string destFile)
        {
            int idx = -1;
            for (int i = 0; i < _statesLog.Count; i++)
            {
                if (_statesLog[i].Guid == guid)
                {
                    idx = i;
                    break;
                }    
            }

            if (idx < 0)
                return;

            _statesLog[idx] = new ActivStateLog(_statesLog[idx].JobName , _statesLog[idx].Guid, totalNFiles, totalSize,srcFile,destFile);

            (_statesLog[idx] as IActivStateLog).ProgressChanged -= ActivStateLog_ProgressChanged;
            (_statesLog[idx] as IActivStateLog).ProgressChanged += ActivStateLog_ProgressChanged;
        }

        public IStateLog GetStateLog(Guid guid)
        {
            for(int i = 0; i< _statesLog.Count; i++)
            {
                if (i < _statesLog.Count)
                    if (_statesLog[i].Guid == guid)
                        return _statesLog[i];
            }

            return null;
        }




        // event from progress Job
        private void ActivStateLog_ProgressChanged(object sender, EventArgs e)
        {
            if (!(sender is IActivStateLog))
            {
                Debug.Fail("error sender message");
                return;
            }

            IActivStateLog temp = sender as IActivStateLog;
            OnProgressChanged?.Invoke(this, new ProgressJobEventArgs( temp));

            if (temp.IsFinished)
                sender = new StateLog(temp);

            SaveStateLog();

        }
       
        // event from dataModel
        private void LogInfo_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            PATH_DAILY_LOG = Path.Combine(DataModel.Instance.LogInfo.DailyLogPath, FILE_NAME_DAILY_LOG);
            PATH_STATE_LOG = Path.Combine(DataModel.Instance.LogInfo.DailyLogPath, FILE_NAME_STATE_LOG);
        }




        public void SaveStateLog()
        {
            FileSaverStrategy.Save(_statesLog, PATH_STATE_LOG, true, ESavingFormat.JSON);
        }

        #endregion


        #region Daily Log

        public void AddDailyLog(string jobName, string srcPath, string destPath, long size, long timeSaving, bool isCrypted = false)
        {
            _dailyLogs.Add(new DailyLog(jobName, srcPath, destPath, size, timeSaving,isCrypted));
        }

        public void SaveDailyLog()
        {
            FileSaverStrategy.Save(_dailyLogs, PATH_DAILY_LOG, true, ESavingFormat.JSON);
        }

        #endregion


        public void Exit()
        {
            SaveDailyLog();
            SaveStateLog();
        }


    }

    public delegate void ProgressJobEventHandler(object sender, ProgressJobEventArgs jobEventArgs);
    public class ProgressJobEventArgs : EventArgs
    {
        private IActivStateLog _activStateLog;



        public Guid Guid => _activStateLog.Guid;
        public double SizeToDo => _activStateLog.TotalSizeFiles;
        public double SizeDone => _activStateLog.Progress.SizeFilesLeft;
        public double FilesToDo => _activStateLog.TotalNbFiles;
        public double FilesDone => _activStateLog.Progress.NbFilesLeft;


        public ProgressJobEventArgs(IActivStateLog activStateLog)
        {
            _activStateLog = activStateLog;
        }
    }


   
}
