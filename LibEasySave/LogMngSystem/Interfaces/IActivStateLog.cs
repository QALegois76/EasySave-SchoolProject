using System;

namespace LibEasySave.Model.LogMng.Interface
{
    public interface IActivStateLog : IStateLog
    {
        event EventHandler ProgressChanged;
        bool IsFinished { get; }
        int TotalNbFiles { get; }
        long TotalSizeFiles { get; }

        IProgressJob Progress { get; }
        
    }
}

