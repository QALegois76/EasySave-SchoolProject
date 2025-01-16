using LibEasySave.Model.LogMng.Interface;
using Newtonsoft.Json;
using System;

namespace LibEasySave
{
    [Serializable]
    public class StateLog :ObservableObject, IStateLog
    {
        // protected
        [JsonProperty]
        protected string _jobName;
        [JsonProperty]
        protected EJobState _jobState = EJobState.JobWaiting;
        [JsonProperty]
        protected Guid _guid;
        [JsonProperty]
        protected DateTime _time;


        // public
        [JsonIgnore]
        public string JobName => _jobName;
        [JsonIgnore]
        public EJobState JobState => _jobState;
        [JsonIgnore]
        public Guid Guid => _guid;
        [JsonIgnore]
        public DateTime Time => _time;

        public StateLog()
        {

        }


        // constructor
        public StateLog(string jobName ,Guid guid, bool isDone = false)
        {
            _jobName = jobName;
            _guid = guid;
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
