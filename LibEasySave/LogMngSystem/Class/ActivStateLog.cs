using LibEasySave.Model.LogMng.Interface;
using System;

namespace LibEasySave
{
    public class ActivStateLog : StateLog, IActivStateLog
    {
        // event
        public event EventHandler ProgressChanged;


        // private
        protected int _totalNbFiles;
        protected long _totalSizeFiles;
        protected IProgressJob _progressJob;


        // accessor
        public bool IsFinished => (_progressJob.NbFilesLeft <=0);
        public int TotalNbFiles =>_totalNbFiles;
        public long TotalSizeFiles =>_totalSizeFiles;
        public IProgressJob Progress => _progressJob;



        // constructor
        public ActivStateLog(IActivStateLog copy) : base(copy)
        {
            _jobState = EJobState.JobRunnig;
            _totalNbFiles = copy.TotalNbFiles;
            _totalSizeFiles = copy.TotalSizeFiles;
            _progressJob = copy.Progress.Copy();

            _progressJob.ProgressChanged -= ProgressJob_ProgressChanged;
            _progressJob.ProgressChanged += ProgressJob_ProgressChanged;
        }

        // constructor
        public ActivStateLog(string jobName, int nbTotalFiles , long sizeTotalFiles, string srcFile, string destFile) : base(jobName, false)
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
            _jobState = (IsFinished) ? EJobState.JobDone : EJobState.JobRunnig;
            ProgressChanged?.Invoke(this, EventArgs.Empty);
        }


    }
}
