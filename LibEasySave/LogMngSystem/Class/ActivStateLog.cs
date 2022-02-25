using LibEasySave.AppInfo;
using LibEasySave.Model.LogMng.Interface;
using LibEasySave.Network;
using Newtonsoft.Json;
using System;

namespace LibEasySave
{
    [Serializable]
    public class ActivStateLog : StateLog, IActivStateLog
    {
        // event
        public event EventHandler ProgressChanged;
        //{ add => _progressJob.ProgressChanged += value; remove => _progressJob.ProgressChanged -= value; }


        // private
        [JsonProperty]
        protected int _totalNbFiles;
        [JsonProperty]
        protected long _totalSizeFiles;
        [JsonProperty]
        protected ProgressJob _progressJob;


        // accessor
        [JsonIgnore]
        public bool IsFinished => (_progressJob.NbFilesLeft <=0);
        [JsonIgnore]
        public int TotalNbFiles =>_totalNbFiles;
        [JsonIgnore]
        public long TotalSizeFiles =>_totalSizeFiles;
        [JsonIgnore]
        public IProgressJob Progress => _progressJob;



        // constructor
        public ActivStateLog(IActivStateLog copy) : base(copy)
        {
            _jobState = EJobState.JobRunnig;
            _totalNbFiles = copy.TotalNbFiles;
            _totalSizeFiles = copy.TotalSizeFiles;
            _guid = copy.Guid;
            _progressJob = copy.Progress.Copy() as ProgressJob;
        }

        public ActivStateLog()
        {

        }

        // constructor
        public ActivStateLog(string jobName, Guid guid, int nbTotalFiles , long sizeTotalFiles, string srcFile, string destFile) : base(jobName, guid, false)
        {
            _totalNbFiles = nbTotalFiles;
            _totalSizeFiles = sizeTotalFiles;
            _progressJob = new ProgressJob(_totalNbFiles, _totalSizeFiles, srcFile, destFile);

            _progressJob.ProgressChanged -= ProgressJob_ProgressChanged;
            _progressJob.ProgressChanged += ProgressJob_ProgressChanged;

            _jobState = EJobState.JobRunnig;
        }


        // from progress job event
        private void ProgressJob_ProgressChanged(object sender, EventArgs e)
        {
            ProgressChanged?.Invoke(this, e);
            if (DataModel.Instance.AppInfo.ModeIHM == EModeIHM.Client)
                NetworkMng.Instance.SendNetworkCommad(ENetorkCommand.UpdateJobProgress, this);
            _jobState = (IsFinished) ? EJobState.JobDone : EJobState.JobRunnig;
        }


    }
}
