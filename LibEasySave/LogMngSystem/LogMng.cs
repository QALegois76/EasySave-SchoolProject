using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using LibEasySave.Model.LogMng.Interface;

namespace LibEasySave
{
    public class LogMng
    {
        #region singleton declaration
        private static LogMng instance;
        private static readonly object lockobj = new object();

        private LogMng()
        {

        }


        public static LogMng Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (lockobj)
                    {
                        if (instance == null)
                        {
                            instance = new LogMng();
                        }
                    }
                }
                return instance;
            }
        }

        #endregion

        private List<IDailyLog> _dailyLogs = new List<IDailyLog>();

        private Dictionary<Guid, IStateLog> _statesLog = new Dictionary<Guid, IStateLog>();

        public bool AddStateLog(Guid guid , string name)
        {
            if (_statesLog.ContainsKey(guid))
            {
                Debug.Fail("error guid already existing");
                return false;
            }

            _statesLog.Add(guid, new StateLog(name, false));
            return true;
        }

        public bool RemoveStateLog(Guid guid)
        {
            if (!_statesLog.ContainsKey(guid))
            {
                Debug.Fail("error guid don't existe");
                return false;
            }

            _statesLog.Remove(guid);
            return true;
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
        

        private void ActivStateLog_ProgressChanged(object sender, EventArgs e)
        {
            if (!(sender is IActivStateLog))
            {
                Debug.Fail("error sender message");
                return;
            }

            IActivStateLog temp = sender as IActivStateLog;

            if (temp.IsFinished)
            {
                sender = new StateLog(temp);
                // send to save writter
            }
            else
            {
                // send to save writter
            }

        }

        public void AddDailyLog(string name,string srcPath , string destPath , long size  , int timeSaving )
        {
            _dailyLogs.Add(new DailyLog(name, srcPath, destPath, size, timeSaving));
        }


        public void Exit()
        {
            // to job to save file
        }

 
    }
}
