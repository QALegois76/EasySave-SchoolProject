using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace LibEasySave
{
    public abstract partial class BaseJobSaver 
    {
        protected IJob _job;
        protected  List<DataFile> _fileToSave = new List<DataFile>();

        // constructor
        public BaseJobSaver(IJob job)
        {
            this._job = job; 
        }

        public void Save()
        {
            SearchFile(_job.SourceFolder, _job.DestinationFolder);
            CopyFile();
        }

        protected void CopyFile()
        {
            Stopwatch watch = new Stopwatch();
            foreach (DataFile item in _fileToSave)
            {
                watch.Start();
                File.Copy(item.SrcFile, item.DestFile);
                watch.Stop();
            }
        }

        protected abstract void SearchFile(string path, string destinationPath);
    }
}
