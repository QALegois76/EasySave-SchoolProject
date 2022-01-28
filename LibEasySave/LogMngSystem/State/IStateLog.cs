using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace LibEasySave.Model.LogMng.Interface
{
    public interface IStateLog
    {
        string JobName { get; }
<<<<<<< HEAD
        DateTime Time { get; }

        EJobState JobState { get; }
    }

    public interface IActivStateLog : IStateLog
    {
        event EventHandler ProgressChanged;
        bool IsFinished { get; }
        int TotalNbFiles { get; }
        long TotalSizeFiles { get; }

        IProgressJob Progress { get; }
        
    }

    public interface IProgressJob
    {
        event EventHandler ProgressChanged;

        int NbFilesLeft { get; }
        long SizeFilesLeft { get;  }
        string PathCurrentSrcFile { get; }
        string PathCurrentDestFile { get;}

        void UpdateProgress(string srcFile , string destFile, long sizeDone,int nbFilesDone = 1);

        IProgressJob Copy();
    }



    public enum EJobState
    {
        JobDone,
        JobRunnig,
        JobWaiting,
=======

        string PathFileSource { get; }

        string PathFileDestination { get; }

        string SizeFile { get; }

        DateTime Time { get; }

        long EllapsedTime { get; set; }

        public EJobState EjobStatemethod { get; set; }

    }

    [Serializable]
    public class StateLog : IStateLog
    {

        [JsonProperty]
        protected DateTime _time;
        [JsonProperty]
        protected string _jobName = null;
        [JsonProperty]
        protected string _pathFileSrc = null;
        [JsonProperty]
        protected string _pathFileDest = null;
        [JsonProperty]
        protected long _ellapsedTime;
        [JsonProperty]
        protected string _sizeFile = null;
        [JsonProperty]
        protected EJobState _eJobState;


        public StateLog(string jobName, string pathFileSrc, string pathFileDest, string sizeFile, EJobState eJobState = EJobState.isRunning, long ellapsedTime = 0)
        {
            _time = DateTime.Now;
            _jobName = jobName;
            _pathFileSrc = pathFileSrc;
            _pathFileDest = pathFileDest;
            _sizeFile = sizeFile;
            _ellapsedTime = ellapsedTime;
            _eJobState = eJobState;
        }


        [JsonIgnore]
        public DateTime Time
        {
            get => _time;
            set => _time = value;
        }


        [JsonIgnore]
        public long EllapsedTime
        {
            get => _ellapsedTime; set => _ellapsedTime = value;
        }

        [JsonIgnore]
        public EJobState EjobStatemethod
        {
            get => _eJobState; set => _eJobState = value;
        }

        [JsonIgnore]
        public string JobName => _jobName;

        [JsonIgnore]
        public string PathFileSource => _pathFileSrc;

        [JsonIgnore]
        public string PathFileDestination => _pathFileDest;

        [JsonIgnore]
        public string SizeFile { get => _sizeFile; set => _sizeFile = value}
    }

    public enum EJobState
    {
        isRunning,
        isDone,
        notStarted
>>>>>>> LogStrategy
    }
}

