using LibEasySave.Model.LogMng.Interface;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace LibEasySave
{
    [Serializable]
    public class ProgressJob : IProgressJob 
    {
        public event EventHandler ProgressChanged;

        [JsonProperty]
        private int _nbFilesLeft;
        [JsonProperty]
        private long _sizeFilesLeft;
        [JsonProperty]
        private string _srcPathFileCurrent;
        [JsonProperty]
        private string _destPathFileCurrent;

        [JsonIgnore]
        public int NbFilesLeft => _nbFilesLeft;
        [JsonIgnore]
        public long SizeFilesLeft => _sizeFilesLeft;
        [JsonIgnore]
        public string PathCurrentSrcFile => _srcPathFileCurrent;
        [JsonIgnore]
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
