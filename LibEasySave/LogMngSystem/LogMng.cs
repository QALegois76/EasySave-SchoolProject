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
        #region singleton declaration
        private static LogMng _instance;
        private static readonly object _lockobj = new object();

        // private constructor
        private LogMng()
        {
            PATH_DAILY_LOG = Path.Combine( Environment.GetEnvironmentVariable(ENV_VAR_NAME),FILE_NAME_DAILY_LOG);
            PATH_STATE_LOG =Path.Combine( Environment.GetEnvironmentVariable(ENV_VAR_NAME), FILE_NAME_STATE_LOG);
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

        #endregion

        private const string ENV_VAR_NAME = "TEMP";
        private const string FILE_NAME_DAILY_LOG = "EasySave_DailyLog.txt";
        private const string FILE_NAME_STATE_LOG = "EasySave_StateLog.txt";
        private readonly string PATH_DAILY_LOG;
        private readonly string PATH_STATE_LOG;

        private List<IDailyLog> _dailyLogs = new List<IDailyLog>();

        private Dictionary<Guid, IStateLog> _statesLog = new Dictionary<Guid, IStateLog>();


        #region MyRegion

        public void AddStateLog(Guid guid , string name)
        {
            if (_statesLog.ContainsKey(guid))
            {
                Debug.Fail("error guid already existing");
                return;
            }

            _statesLog.Add(guid, new StateLog(name, false));
        }

        public void RemoveStateLog(Guid guid)
        {
            if (!_statesLog.ContainsKey(guid))
            {
                Debug.Fail("error guid don't existe");
                return;
            }

            _statesLog.Remove(guid);
        }

        public void RenameJob(Guid guid , string name)
        {
            if (!_statesLog.ContainsKey(guid))
            {
                Debug.Fail("error guid don't existe");
                return;
            }

            _statesLog.Remove(guid);
        }

        public void SetActivStateLog(Guid guid, int totalNFiles , long totalSize, string srcFile , string destFile)
        {
            if (!_statesLog.ContainsKey(guid))
            {
                Debug.Fail("error guid don't existe");
                return;
            }

            _statesLog[guid] = new ActivStateLog(_statesLog[guid].JobName, totalNFiles, totalSize,srcFile,destFile);

            (_statesLog[guid] as IActivStateLog).ProgressChanged -= ActivStateLog_ProgressChanged;
            (_statesLog[guid] as IActivStateLog).ProgressChanged += ActivStateLog_ProgressChanged;
        }

        public IStateLog GetStateLog(Guid guid)
        {
            if (!_statesLog.ContainsKey(guid))
            {
                Debug.Fail("guid not foun't");
                throw new Exception("guid not found");
            }

            return _statesLog[guid];
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

            if (temp.IsFinished)
                sender = new StateLog(temp);

            FileSaverStrategy.Save(_statesLog.Values, PATH_STATE_LOG,true, ESavingFormat.JSON);
        }

        #endregion



        #region Daily Log

        public void AddDailyLog(string jobName, string srcPath, string destPath, long size, long timeSaving)
        {
            _dailyLogs.Add(new DailyLog(jobName, srcPath, destPath, size, timeSaving));
        }

        #endregion



        public void Exit()
        {
            FileSaverStrategy.Save(_dailyLogs, PATH_DAILY_LOG, false,ESavingFormat.JSON);
        }


    }
}
