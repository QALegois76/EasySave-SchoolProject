using LibEasySave.AppInfo;
using LibEasySave.Model.LogMng.Interface;
using LibEasySave.TranslaterSystem;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading;

namespace LibEasySave
{
    public abstract partial class BaseJobSaver
    {

        private string _lastError = null;
        protected IJob _job;
        protected IProgressJob _progressJob;
        protected List<DataFile> _fileToSave = new List<DataFile>();
        protected List<DataFile> _fileToSaveEncrypt = new List<DataFile>();
        private static ManualResetEvent _bigFile = new ManualResetEvent(false);
        private static ManualResetEvent _playBreak = new ManualResetEvent(false);

        protected long _totalSize;
        protected const long MAX_SIZE = 1024 * 1024 * 256;

        // constructor
        public BaseJobSaver(IJob job)
        {
            this._job = job;
            _totalSize = 0;
        }

        public void Save(object obj)
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
                    else if (!list.Contains(extend) && dict_rest.Contains(listToChange[j]))
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
                if (p.ProcessName == "Calculator")
                {
                    return true;
                }
            }
            return false;
        }


        protected void CopyFile()
        {
            Stopwatch watch = new Stopwatch();

            List<DataFile> fileToSave = SortList(_fileToSave, DataModel.Instance.AppInfo.PriorityExt);
            List<DataFile> fileToSaveEncrypt = SortList(_fileToSaveEncrypt, DataModel.Instance.AppInfo.PriorityExt);

            if (fileToSave != null && fileToSave.Count > 0)
            {
                foreach (DataFile item in fileToSave)
                {
                    long timeSave = -1;
                    try
                    {
                        WaitPriorityFileRunning(item);
                        _bigFile.WaitOne();
                        if (IsBigFileRunnig(item))
                        {
                            _bigFile.Set();

                        }
                        watch.Restart();
                        IncrementPriorityFile(item);

                        while (IsSoftwareRunning())
                        {
                            //_lastError = Translater.Instance.TranslatedText.ErrorSoftwareIsRunning;
                            Thread.Sleep(100);
                        }
                        File.Copy(item.SrcFile, item.DestFile);

                        DecrementPriorityFile(item);

                        if (IsBigFileRunnig(item))
                            _bigFile.Reset();

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
            if (fileToSaveEncrypt != null && fileToSaveEncrypt.Count > 0)
            {
                foreach (DataFile item in fileToSaveEncrypt)
                {
                    long timeSave = -1;
                    try
                    {
                        WaitPriorityFileRunning(item);
                        _bigFile.WaitOne();
                        if (IsBigFileRunnig(item))
                            _bigFile.Set();
                        watch.Restart();
                        Random rand = new Random();
                        long n = (long)rand.Next(int.MaxValue);
                        n *= (long)rand.Next(int.MaxValue);
                        IncrementPriorityFile(item);

                        while (IsSoftwareRunning())
                        {
                            //_lastError = Translater.Instance.TranslatedText.ErrorSoftwareIsRunning;
                            Thread.Sleep(100);
                        }
                        CrytBaseJobSaver.CryptoSoft(item.SrcFile, item.DestFile, n.ToString());
                        DecrementPriorityFile(item);
                        if (IsBigFileRunnig(item))
                            _bigFile.Reset();
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
        }

        // This method broke off the current thread;
        public static void BreakJob(EState state)
        {
            switch (state)
            {
                case EState.Break:
                    if (Thread.CurrentThread.ThreadState.ToString() == "Running")
                    {
                        try
                        {
                            //_playBreak.WaitOne();
                            //_playBreak.Set();
                            Thread.Sleep(Timeout.Infinite);
                        }
                        catch (ThreadInterruptedException)
                        {
                            Console.WriteLine("Thread '{0}' awoken.",
                                              Thread.CurrentThread.Name);
                        }
                        catch (ThreadAbortException)
                        {
                            Console.WriteLine("Thread '{0}' aborted.",
                                              Thread.CurrentThread.Name);
                        }
                    }
                    break;

                case EState.Stop:
                    if ((Thread.CurrentThread.ThreadState.ToString() == "Suspended") || (Thread.CurrentThread.ThreadState.ToString() == "Running"))
                    {
                        try
                        {
                            Thread.ResetAbort();
                        }
                        catch (ThreadInterruptedException)
                        {
                            Console.WriteLine("Thread '{0}' awoken.",
                                              Thread.CurrentThread.Name);
                        }
                        catch (ThreadAbortException)
                        {
                            Console.WriteLine("Thread '{0}' aborted.",
                                              Thread.CurrentThread.Name);
                        }
                    }
                    break;
                case EState.Play:
                    if (Thread.CurrentThread.ThreadState.ToString() == "Suspended")
                    {
                        try
                        {
                            //_bigFile.WaitOne();
                            Thread.CurrentThread.Interrupt();
                        }
                        catch (ThreadInterruptedException)
                        {
                            Console.WriteLine("Thread '{0}' awoken.",
                                              Thread.CurrentThread.Name);
                        }
                        catch (ThreadAbortException)
                        {
                            Console.WriteLine("Thread '{0}' aborted.",
                                              Thread.CurrentThread.Name);
                        }
                    }
                    break;

                default:
                    break;

            }
            
        }

        protected abstract void SearchFile(string path, string destinationPath);
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
        Break,
        Stop     
    }
}
