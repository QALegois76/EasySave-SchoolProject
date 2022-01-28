using LibEasySave.Model.LogMng.Interface;
using System;
using System.Text.Json.Serialization;

namespace LibEasySave
{
    public class StateLog : IStateLog
    {
        // protected
        protected string _jobName;
        protected EJobState _jobState = EJobState.JobWaiting;
        protected DateTime _time;


        // public
        public string JobName => _jobName;
        public EJobState JobState => _jobState;
        public DateTime Time => _time;



        // constructor
        public StateLog(string jobName , bool isDone = false)
        {
            _jobName = jobName;
            _time = DateTime.Now;
            _jobState = (isDone) ? EJobState.JobDone : EJobState.JobWaiting;
        }


        /// <summary>
        /// used for doned job
        /// </summary>
        /// <param name="copy"></param>
        public StateLog(IStateLog copy)
        {
            _jobState = EJobState.JobDone;
            _jobName = copy.JobName;
            _time = copy.Time;

        }
    }
}
