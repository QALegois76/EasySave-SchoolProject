using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading;
using System.IO;

namespace LibEasySave
{
    public class JobMng : IJobMng
    {
        #region  VARIABLES
        public const string DEFAULT_NAME = "Job Name";

        // protected

        protected Guid _editingJobName = Guid.Empty;
        private readonly BaseJobSaver JOB_MODEL;
        //protected Dictionary<Guid, IJob> _jobs = new Dictionary<Guid, IJob>();
        protected Dictionary<Guid, BaseJobSaver> _baseJobers = new Dictionary<Guid, BaseJobSaver>();


        public string NextDefaultName => GetNextDefaultName();
        public Guid EditingJob { get => _editingJobName; set => _editingJobName = value; }
        BaseJobSaver IJobMng.JOB_MODEL => JOB_MODEL;

        //public Dictionary<Guid, IJob> Jobs => _jobs;
        public Dictionary<Guid, BaseJobSaver> BaseJober => _baseJobers;





        #endregion


        // constructor
        public JobMng(BaseJobSaver jobModel)
        {
            if (jobModel == null)
                throw new Exception("jobModel musn't be null");

            JOB_MODEL = jobModel;
        }
        public JobMng() { }

        private string GetNextDefaultName()
        {
            int count = 0;
            
            foreach (var item in _baseJobers)
            {
                if (item.Value.Job.Name == DEFAULT_NAME)
                    count++;
            }

            bool exist = false;
            string nextName = DEFAULT_NAME + count;

            do
            {
                exist = false;
                nextName = DEFAULT_NAME + count;
                foreach (var item in _baseJobers)
                {
                    if (item.Value.Job.Name == nextName)
                    {
                        count++;
                        exist = true;
                        break;
                    }
                }
            }
            while (exist);

            return nextName;
        }
    }

    public interface IJobMng
    {
        public string NextDefaultName { get; }
        Guid EditingJob { get; set; }

        public BaseJobSaver JOB_MODEL { get; }

        Dictionary<Guid,BaseJobSaver> BaseJober { get; }


    }

}
