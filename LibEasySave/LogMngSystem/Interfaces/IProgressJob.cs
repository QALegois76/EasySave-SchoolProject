using System;

namespace LibEasySave.Model.LogMng.Interface
{
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
}

