using System;
using System.Collections.Generic;
using System.Text;

namespace LibEasySave.Model.LogMng.Interface
{
    interface IStateLog
    {
        public string JobName
        {
            get { return JobName; }
            set { JobName = value; }
        }
        public DateTime Time
        {
            get { return Time; }
            set { Time = value; }
        }
        enum EJobState
        {
            isRunning,
            isDone,
            notStarted
        }
    }
}

