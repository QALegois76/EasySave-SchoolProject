using System;
using System.ComponentModel;

namespace LibEasySave.Model.LogMng.Interface
{
    public interface IProgressJob
    {
        public event EventHandler ProgressChanged;

        int NbFilesLeft { get; }
        long SizeFilesLeft { get;  }
        string PathCurrentSrcFile { get; }
        string PathCurrentDestFile { get;}

        void UpdateProgress(string srcFile , string destFile, long sizeDone,int nbFilesDone = 1);

        IProgressJob Copy();
    }
}

