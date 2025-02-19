﻿using LibEasySave.Model.LogMng.Interface;
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
            _job.PropertyChanged += Job_PropertyChanged;
        }

        private void Job_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(_job.Name))
                PropChanged(nameof(JobName));

            if (e.PropertyName == nameof(_job.SourceFolder))
                PropChanged(nameof(SrcFolderPath));

            if (e.PropertyName == nameof(_job.DestinationFolder))
                PropChanged(nameof(DestFolderPath));
            
            if (e.PropertyName == nameof(_job.SavingMode))
                PropChanged(nameof(SavingMode));
        }
    }

    public interface IJobInfo : INotifyPropertyChanged
    {
        string JobName { get; }
        string SrcFolderPath { get;  }
        string DestFolderPath { get;  }
        string SavingMode { get; } 

        long NFiles { get; set; } 
        long NFolders { get; set; } 
        long NFileCrypt { get; set; } 
        long TotalSize { get; set; }
        
    }
}
