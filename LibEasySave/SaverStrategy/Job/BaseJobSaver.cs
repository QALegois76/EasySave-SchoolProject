using LibEasySave.Model.LogMng.Interface;
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
        protected IProgressJob _progressJob;
        protected  List<DataFile> _fileToSave = new List<DataFile>();
        protected long _totalSize;
        protected const long MAX_SIZE = 1024*1024*256;

        // constructor
        public BaseJobSaver(IJob job)
        {
            this._job = job;
            _totalSize = 0;
        }

        public void Save()
        {
            SearchFile(_job.SourceFolder, _job.DestinationFolder);

            if (_fileToSave.Count == 0)
            {
                return;
            }

            LogMng.Instance.SetActivStateLog(_job.Guid, _fileToSave.Count, _totalSize, _fileToSave[0].SrcFile, _fileToSave[0].DestFile);
            this._progressJob = (LogMng.Instance.GetStateLog(_job.Guid) as IActivStateLog)?.Progress;

            if (this._progressJob == null)
            {
                return;
                throw new Exception("activStateJob or ProgressJob is null");
            }

            CopyFile();
        }

        protected void CopyFile()
        {
            Stopwatch watch = new Stopwatch();
            foreach (DataFile item in _fileToSave)
            {
                long timeSave = -1;
                try
                {
                    watch.Restart();
                    File.Copy(item.SrcFile, item.DestFile);
                    watch.Stop();
                    timeSave = watch.ElapsedMilliseconds;
                }
                catch (Exception ex)
                {
                    timeSave = -1;
                }

                _progressJob.UpdateProgress(item.SrcFile, item.DestFile, item.SizeFile);
                LogMng.Instance.AddDailyLog(_job.Name, item.SrcFile, item.DestFile,item.SizeFile,timeSave);
            }
        }

        protected abstract void SearchFile(string path, string destinationPath);
    }
}
