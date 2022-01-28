using LibEasySave.Model.LogMng.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace LibEasySave.Model
{
    public abstract class LogBaseSaver
    {
        public LogBaseSaver()
        {
        }

        public abstract string GetSavedLogText(ILog log);

        public abstract string GetSavedLogText(IDailyLog dailyLog);

        public abstract string GetSavedStateText(IStateLog state);

    }
}
