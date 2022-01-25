using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading;
using System.IO;

namespace LibEasySave
{
    public class JobMng
    {
        #region  VARIABLES

        // const
        private const string LOG_FILE_PATH = "";

        private static readonly object pblock = new object();


        // protected
        protected string _editingJobName = null;

        protected Thread thread;

        protected List<ILog> _logs = new List<ILog>();

        protected Dictionary<string, IJob> _jobs = new Dictionary<string, IJob>(5);

        public Dictionary<string, IJob> Jobs => _jobs;


        #endregion


        // constructor
        public JobMng()
        {
        }

        public bool RunJob(string name)
        {
            if (_jobs == null || _jobs.Count == 0)
                return false;

            thread = new Thread(new ParameterizedThreadStart(ExecuteJob));
            thread.Start(name);

            return true;


        }

       // private void ExecuteJob(List<IJob> job)
        private void ExecuteJob(object data)
        {
            if (data == null || !(data is string))
            {
                Debug.Fail("data incorect");
            }

            string jobsName = data.ToString();

            lock (pblock)
            {
                Stopwatch watch = new Stopwatch();

                IJob job = _jobs[jobsName];

                if (!Directory.Exists(job.SourceFolder))
                {
                    Debug.Fail("the folder doesn't exist or it is not accessible");
                    return;
                }

                if (!Directory.Exists(job.DestinationFolder))
                {
                    Debug.Fail("the folder doesn't exist or it is not accessible");
                    return;
                }

                var files = (new DirectoryInfo(job.SourceFolder)).GetFiles();
                var folders = (new DirectoryInfo(job.SourceFolder)).GetDirectories();
                foreach (FileInfo fileInfo in files)
                {
                    ILog log = new Log(jobsName, fileInfo.FullName, Path.Combine(job.DestinationFolder), fileInfo.Length.ToString());
                    try
                    {
                        watch.Start();
                        File.Copy(log.PathFileSource, log.PathFileDestination);
                        watch.Stop();
                        log.TimeSaving = (int)watch.ElapsedMilliseconds;
                    }
                    catch (Exception e)
                    {
                        watch.Stop();
                    }
                    _logs.Add(log);
                }

                foreach (DirectoryInfo folderInfo in folders)
                {
                    File.Copy(folderInfo.FullName, Path.Combine(job.DestinationFolder, folderInfo.Name));
                }

            }

        }

    }
}
