using LibEasySave.Model.LogMng.Interface;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace LibEasySave.MVVM_Job.Model
{
    public class JobInfo : ObservableObject, IJobInfo
    {
        private long _nFiles = 0;
        private long _nFolder = 0;
        private long _nFileCrypt = 0;
        private long _totalSize = 0;

        IJob _job;

        public string JobName => _job.Name;
        public string SrcFolderPath => _job.SourceFolder;
        public string DestFolderPath => _job.DestinationFolder;
        public string SavingMode => _job.SavingMode.ToString();

        public long NFiles { get => _nFiles; set { _nFiles = value; PropChanged(nameof(NFiles)); } }
        public long NFolders { get => _nFolder; set { _nFolder = value; PropChanged(nameof(NFolders)); } }
        public long NFileCrypt { get => _nFileCrypt; set { _nFileCrypt = value; PropChanged(nameof(NFileCrypt)); } }
        public long TotalSize { get => _totalSize; set { _totalSize = value; PropChanged(nameof(TotalSize)); } }

        public JobInfo(IJob job)
        {
            _job = job;
        }
    }

    public interface IJobInfo : INotifyPropertyChanged
    {
        string JobName { get; }
        string SrcFolderPath {get;}
        string DestFolderPath {get; }
        string SavingMode {get; } 

        long NFiles {get; } 
        long NFolders {get;} 
        long NFileCrypt {get;} 
        long TotalSize {get; }
        
    }
}
