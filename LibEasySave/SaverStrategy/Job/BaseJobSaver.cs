using LibEasySave.AppInfo;
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
        protected List<DataFile> _fileToSave = new List<DataFile>();
        protected List<DataFile> _fileToSaveCrypt = new List<DataFile>();
        protected long _totalSize;

        // constructor
        public BaseJobSaver(IJob job)
        {
            this._job = job;
            _totalSize = 0;
        }

        public void Save()
        {
            SearchFile(_job.SourceFolder, _job.DestinationFolder);

            if (_fileToSave.Count == 0 && _fileToSaveCrypt.Count == 0)
                return;

            string firstSrcFile = (_fileToSave.Count > 0) ? _fileToSave[0].SrcFile : (_fileToSaveCrypt.Count > 0) ? _fileToSaveCrypt[0].SrcFile : null;
            string firstDestFile = (_fileToSave.Count > 0) ? _fileToSave[0].DestFile : (_fileToSaveCrypt.Count > 0) ? _fileToSaveCrypt[0].DestFile : null;

            LogMng.Instance.SetActivStateLog(_job.Guid, _fileToSave.Count + _fileToSaveCrypt.Count, _totalSize, firstSrcFile, firstDestFile);
            this._progressJob = (LogMng.Instance.GetStateLog(_job.Guid) as IActivStateLog)?.Progress;

            if (this._progressJob == null)
            {
                return;
                throw new Exception("activStateJob or ProgressJob is null");
            }

            if(_fileToSave.Count>0)
                CopyFile();
            
            if (_fileToSaveCrypt.Count>0)
                CryptFile();
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
                LogMng.Instance.AddDailyLog(_job.Name, item.SrcFile, item.DestFile, item.SizeFile, timeSave);
            }
        }

        protected void CryptFile()
        {
            Stopwatch watch = new Stopwatch();

            foreach (DataFile file in _fileToSaveCrypt)
            {
                long timeSave = -1;
                try
                {
                    watch.Restart();
                    CripterMng.Instance.ActivCripter.Crypt(file.SrcFile, file.DestFile, DataModel.Instance.CryptInfo.Key);
                    watch.Stop();
                    timeSave = watch.ElapsedMilliseconds;
                }
                catch (Exception ex)
                {
                    watch.Stop();
                }

                _progressJob.UpdateProgress(file.SrcFile, file.DestFile, file.SizeFile);
                LogMng.Instance.AddDailyLog(_job.Name, file.SrcFile, file.DestFile, file.SizeFile, timeSave, true);

            }
        }



        protected abstract void SearchFile(string path, string destinationPath);
    }




    public class CripterMng
    {
        private static CripterMng _instance = new CripterMng();
        public static CripterMng Instance => _instance;


        private ICyptPlugin _activCripter = null;
        public  ICyptPlugin ActivCripter => _activCripter;


        private CripterMng()
        {
            UpdateCryptPlugin();
        }



        private void UpdateCryptPlugin()
        {
            switch (DataModel.Instance.CryptInfo.CryptMode)
            {
                case ECryptMode.XOR:
                    _activCripter = new CryptosoftPlugin();
                    break;
                default:
                    throw new Exception("CryptMode no found");
                    break;
            }
        }


    }

    public interface ICyptPlugin
    {
        void Crypt(string srcFile, string destFile, string key);
        void DeCrypt(string srcFile, string destFile, string key);
    }

    public class CryptosoftPlugin : ICyptPlugin
    {
        private ProcessStartInfo _processStartInfo = new ProcessStartInfo("CrytoSoft.exe");
        

        public void Crypt(string srcFile, string destFile, string key)
        {
            Process process = new Process();
            _processStartInfo.CreateNoWindow = true;

            process.StartInfo.Arguments ="\"" + srcFile + "\" \"" + destFile + "\" " + key + " -D";
            //process.StartInfo = _processStartInfo;
            process.StartInfo.FileName= @"..\..\..\..\LibEasySave\CryptoSoft\net5.0\CryptoSoft.exe";
            process.StartInfo.CreateNoWindow = true;
            process.StartInfo.RedirectStandardOutput = true;
            process.Start();
            string output = "";
            while (!process.StandardOutput.EndOfStream)
            {
                output += process.StandardOutput.ReadLine()+"\n";
            }
        }

        public void DeCrypt(string srcFile, string destFile, string key)
        {
            throw new NotImplementedException();
        }
    }
}
