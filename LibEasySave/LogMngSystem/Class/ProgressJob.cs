using LibEasySave.Model.LogMng.Interface;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace LibEasySave
{
    class ProgressJob : IProgressJob 
    {
        public event EventHandler ProgressChanged;

        private int _nbFilesLeft;
        private long _sizeFilesLeft;
        private string _srcPathFileCurrent;
        private string _destPathFileCurrent;


        public int NbFilesLeft => _nbFilesLeft;
        public long SizeFilesLeft => _sizeFilesLeft;
        public string PathCurrentSrcFile => _srcPathFileCurrent;
        public string PathCurrentDestFile => _destPathFileCurrent;

        // constructor
        public ProgressJob(int initFile , long initSize , string srcFile , string destFile)
        {
            _nbFilesLeft = initFile;
            _sizeFilesLeft = initSize;
            _srcPathFileCurrent = srcFile;
            _destPathFileCurrent = destFile;
        }



        public void UpdateProgress(string srcFile, string destFile, long sizeDone, int nbFilesDone = 1)
        {
            _srcPathFileCurrent = srcFile;
            _destPathFileCurrent = destFile;
            _sizeFilesLeft -= sizeDone;
            _nbFilesLeft -= nbFilesDone;
            ProgressChanged?.Invoke(this, EventArgs.Empty);
        }

        public IProgressJob Copy()
        {
            return new ProgressJob(_nbFilesLeft, _sizeFilesLeft, _srcPathFileCurrent, _destPathFileCurrent);
        }
    }
}
