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
    }
}

