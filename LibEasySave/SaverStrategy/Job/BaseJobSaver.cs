﻿using LibEasySave.AppInfo;
using LibEasySave.Model.LogMng.Interface;
using LibEasySave.MVVM_Job.Model;
using LibEasySave.TranslaterSystem;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LibEasySave
{
    /// <summary>
    /// BaseJobsaver is an abstact class.
    /// It permits to prioritize files, encrypt files because of lists extends of priorities files and files to encrypt
    /// It stops the work of the software when notepad runs
    /// It permits to make play, play and stop jobs
    /// It contains a progressJob
    /// It blocks a save when the file is a big file (filesize >= 200 Mo), and another big file is running.
    /// </summary>
    public abstract partial class BaseJobSaver
    {

        private string _lastError = null;
        protected IJob _job;
        protected IJobInfo _jobInfo;
        protected IProgressJob _progressJob;
        protected List<DataFile> _fileToSave = new List<DataFile>();
        protected List<DataFile> _fileToSaveEncrypt = new List<DataFile>();
        protected List<string> _folderToCreate = new List<string>();
        private static AutoResetEvent _bigFile = new AutoResetEvent(false);
        //private static ManualResetEvent _playBreak = new ManualResetEvent(false);
        private EState _currentState = EState.Stop;
        protected long _totalSize;
        protected const long MAX_SIZE = 1024 * 1024 * 256;

        public IJob Job => _job;
        public IJobInfo JobInfo => _jobInfo;
        // constructor
        public BaseJobSaver(IJob job)
        {
            this._job = job;
            this._jobInfo = new JobInfo(_job);
            _job.PropertyChanged -= Pob_PropertyChanged;
            _job.PropertyChanged += Pob_PropertyChanged;

            _totalSize = 0;
        }

        private void Pob_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(_job.Name))
                return;

            ClearAllList();
            SearchFile(_job.SourceFolder, _job.DestinationFolder,false);
            _folderToCreate = SortFolder(_folderToCreate);
        }

        public void Save(object obj)
        {
            CreateFolder(_folderToCreate);

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
            _currentState = EState.Play;
            CopyFiles();
        }
        
        public List<DataFile> SortList(List<DataFile> listToChange, List<String> list)
        {
            if (list == null || listToChange == null)
            {
                throw new Exception("list is null");
            }
            List<DataFile> dict = new List<DataFile>();
            List<DataFile> dict_rest = new List<DataFile>();

            for (int i = 0; i < list.Count; i++)
            {
                for (int j = 0; j < listToChange.Count; j++)
                {
                    String extend = new FileInfo(listToChange[j].SrcFile).Extension;
                    if (extend == list[i])
                    {
                        dict.Add(listToChange[j]);
 
                    }
                    else if (!list.Contains(extend)) //&& dict_rest.Contains(listToChange[j]))
                    {
                        dict_rest.Add(listToChange[j]);
                    }
                }
            }

            foreach (var item in dict_rest)
            {
                dict.Add(item);
            }

            return dict;
        }

        private List<string> SortFolder(List<string> listToSort)
        {
            if (listToSort == null)
                return null;

            if (listToSort.Count <= 1)
                return listToSort;
            List<string> inf = new List<string>();
            List<string> sup = new List<string>();
            List<string> pivot = new List<string>();

            bool infNeedSort = false;
            bool supNeedSort = false;

            pivot.Add(listToSort[0]);
            listToSort.RemoveAt(0);

            foreach (var str in listToSort)
            {
                if (str.Length == pivot[0].Length)
                {
                    pivot.Add(str);
                }
                else if(str.Length < pivot[0].Length)
                {
                    if (inf.Count == 0)
                        inf.Add(str);
                    else
                    {
                        if ((inf[0].Length != inf[inf.Count - 1].Length) && inf[inf.Count - 1].Length != str.Length)
                            infNeedSort = true;
                        inf.Add(str);

                    }

                }
                else
                {
                    if (sup.Count == 0)
                        sup.Add(str);
                    else
                    {
                        if ((sup[0].Length != sup[sup.Count - 1].Length) && sup[sup.Count - 1].Length != str.Length)
                            supNeedSort = true;
                        sup.Add(str);
                    }
                }
            }

            if (infNeedSort)
                inf = SortFolder(inf);

            inf.AddRange(pivot);

            if (supNeedSort)
                sup = SortFolder(sup);

            inf.AddRange(sup);

            return inf;
        }

        private void CreateFolder(List<string> folders)
        {
            foreach (string fold in folders)
            {
                if (!Directory.Exists(fold))
                    Directory.CreateDirectory(fold);
            }
        }


        private void ClearAllList()
        {
            _fileToSave.Clear();
            _fileToSaveEncrypt.Clear();
            _folderToCreate.Clear();
            _jobInfo.NFileCrypt = 0;
            _jobInfo.NFiles = 0;
            _jobInfo.NFolders = 0;
            _jobInfo.TotalSize = 0;

        }

        public bool IsBigFileRunnig(DataFile dataFile)
        {
            if (dataFile.SrcFile.Length < MAX_SIZE)
            {
                return false;
            }

            return true;
        }

        private void WaitPriorityFileRunning(DataFile data)
        {
            while (!(DataModel.Instance.AppInfo.PriorityExt.Contains(new FileInfo(data.SrcFile).Extension)) && DataModel.Instance.AppInfo.IsPriorityFileRunning())
            {
                Thread.Sleep(100);
            }

        }

        private void IncrementPriorityFile(DataFile data)
        {
            if (DataModel.Instance.AppInfo.PriorityExt.Contains(new FileInfo(data.SrcFile).Extension))
                DataModel.Instance.AppInfo.IncrementPriorityFile();
        }

        private void DecrementPriorityFile(DataFile data)
        {
            if (DataModel.Instance.AppInfo.PriorityExt.Contains(new FileInfo(data.SrcFile).Extension))
                DataModel.Instance.AppInfo.DecrementPriorityFile();
        }

        private bool IsSoftwareRunning()
        {
            foreach (Process p in Process.GetProcesses())
            {
                if (DataModel.Instance.AppInfo.ContainsJobApp( p.ProcessName ))
                {
                    return true;
                }
            }
            return false;
        }


        protected void CopyFiles()
        {
            Stopwatch watch = new Stopwatch();

            if (DataModel.Instance.AppInfo.PriorityExt.Count > 0)
                _fileToSave = SortList(_fileToSave, DataModel.Instance.AppInfo.PriorityExt);

            if (DataModel.Instance.AppInfo.PriorityExt.Count > 0)
                _fileToSaveEncrypt = SortList(_fileToSaveEncrypt, DataModel.Instance.AppInfo.PriorityExt);

            if (_fileToSave != null && _fileToSave.Count > 0)
            {
                foreach (DataFile item in _fileToSave)
                {
                    long timeSave = -1;
                    try
                    {
                        WaitPriorityFileRunning(item);
                        
                        if (!IsBigFileRunnig(item))
                        {
                            _bigFile.Set();

                        } else
                        {
                            _bigFile.WaitOne();
                        }
                        watch.Restart();
                        IncrementPriorityFile(item);

                        if (_currentState == EState.Stop)
                        {
                            break;
                        }
                        while (IsPaused())
                        {
                            //_lastError = Translater.Instance.TranslatedText.ErrorSoftwareIsRunning;
                            Thread.Sleep(100);
                        }
                        File.Copy(item.SrcFile, item.DestFile);

                        DecrementPriorityFile(item);

                        //if (!IsBigFileRunnig(item))
                        //    _bigFile.Reset();

                        watch.Stop();
                        timeSave = watch.ElapsedMilliseconds;
                    }
                    catch (Exception ex)
                    {
                        timeSave = -1;
                    }

                    _progressJob.UpdateProgress(item.SrcFile, item.DestFile, item.SizeFile);
                    LogMng.Instance.AddDailyLog(_job.Name, item.SrcFile, item.DestFile, item.SizeFile, timeSave);
                }
            }
            if (_fileToSaveEncrypt != null && _fileToSaveEncrypt.Count > 0)
            {
                foreach (DataFile item in _fileToSaveEncrypt)
                {
                    long timeSave = -1;
                    try
                    {
                        WaitPriorityFileRunning(item);
                        if (!IsBigFileRunnig(item))
                        {
                            _bigFile.Set();

                        }
                        else
                        {
                            _bigFile.WaitOne();
                        }
                        watch.Restart();
                        Random rand = new Random();
                        long n = (long)rand.Next(int.MaxValue);
                        n *= (long)rand.Next(int.MaxValue);
                        IncrementPriorityFile(item);
                        if (_currentState == EState.Stop)
                        {
                            break;
                        }
                        while (IsPaused())
                        {
                            //_lastError = Translater.Instance.TranslatedText.ErrorSoftwareIsRunning;
                            Thread.Sleep(100);
                        }
                        CrytBaseJobSaver.CryptoSoft(item.SrcFile, item.DestFile, n.ToString());
                        DecrementPriorityFile(item);
                        //if (IsBigFileRunnig(item))
                        //    _bigFile.Reset();
                        watch.Stop();
                        timeSave = watch.ElapsedMilliseconds;
                    }
                    catch (Exception ex)
                    {
                        timeSave = -1;
                    }

                    _progressJob.UpdateProgress(item.SrcFile, item.DestFile, item.SizeFile);
                    LogMng.Instance.AddDailyLog(_job.Name, item.SrcFile, item.DestFile, item.SizeFile, timeSave);
                }
                
            }
            _currentState = EState.Finish;
        }

        public void Pause()
        {
            this._currentState = EState.Pause;
        }

        public bool IsPaused()
        {
            return _currentState == EState.Pause || IsSoftwareRunning();
        }

        public void Play()
        {
            this._currentState = EState.Play;
        }

        public void Stop()
        {
            this._currentState = EState.Stop;
        }



 

        public EState State
        {
            get
            {
                return _currentState;
            }
        }




        // This method broke off the current thread;
        //public void BreakJob(EState state)
        //{
        //     Task.Run(() =>
        //    {
        //        switch (state)
        //        {
        //            case EState.Break:
        //                if (Thread.CurrentThread.ThreadState.ToString() == "Running")
        //                {
        //                    try
        //                    {
        //                        //_playBreak.WaitOne();
        //                        //_playBreak.Set();
        //                        Task.Delay(Timeout.Infinite).Wait();
        //                    }
        //                    catch (ThreadInterruptedException)
        //                    {
        //                        Console.WriteLine("Thread '{0}' awoken.",
        //                                          Thread.CurrentThread.Name);
        //                    }
        //                    catch (ThreadAbortException)
        //                    {
        //                        Console.WriteLine("Thread '{0}' aborted.",
        //                                          Thread.CurrentThread.Name);
        //                    }
        //                }
        //                break;

        //            case EState.Stop:
        //                if ((Thread.CurrentThread.ThreadState.ToString() == "Suspended") || (Thread.CurrentThread.ThreadState.ToString() == "Running"))
        //                {
        //                    try
        //                    {
        //                        Thread.ResetAbort();
        //                    }
        //                    catch (ThreadInterruptedException)
        //                    {
        //                        Console.WriteLine("Thread '{0}' awoken.",
        //                                          Thread.CurrentThread.Name);
        //                    }
        //                    catch (ThreadAbortException)
        //                    {
        //                        Console.WriteLine("Thread '{0}' aborted.",
        //                                          Thread.CurrentThread.Name);
        //                    }
        //                }
        //                break;
        //            case EState.Play:
        //                if (Thread.CurrentThread.ThreadState.ToString() == "Suspended")
        //                {
        //                    try
        //                    {
        //                        //_bigFile.WaitOne();
        //                        Thread.CurrentThread.Interrupt();
        //                    }
        //                    catch (ThreadInterruptedException)
        //                    {
        //                        Console.WriteLine("Thread '{0}' awoken.",
        //                                          Thread.CurrentThread.Name);
        //                    }
        //                    catch (ThreadAbortException)
        //                    {
        //                        Console.WriteLine("Thread '{0}' aborted.",
        //                                          Thread.CurrentThread.Name);
        //                    }
        //                }
        //                break;

        //            default:
        //                break;
        //        }

        //    }

        //}

        protected abstract void SearchFile(string path, string destinationPath, bool isCheck);
    }

    public static class CrytBaseJobSaver
    {
        private static ProcessStartInfo _ProcessStartInfo = new ProcessStartInfo("CrytoSoft.exe");

        public static void CryptoSoft(string srcFile, string destFile, string key)
        {
            Process process = new Process();
            _ProcessStartInfo.CreateNoWindow = true;
            process.StartInfo.Arguments = "\"" + srcFile + "\" \"" + destFile + "\" " + key + " -D";
            process.Start();

        }

    }

    public enum EState
    {
        Play,
        Pause,
        Stop,
        Finish
    }
}
