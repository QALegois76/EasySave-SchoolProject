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


        // protected

        protected string _editingJobName = null;
        private readonly IJob JOB_MODEL;
        protected Dictionary<string, IJob> _jobs = new Dictionary<string, IJob>(5);

        public string EditingJobName { get => _editingJobName; set => _editingJobName = value; }
        IJob IJobMng.JOB_MODEL => JOB_MODEL;

        public Dictionary<string, IJob> Jobs => _jobs;





        #endregion


        // constructor
        public JobMng(IJob jobModel)
        {
            if (jobModel == null)
                throw new Exception("jobModel musn't be null");

            JOB_MODEL = jobModel;
        }
    }

    public interface IJobMng
    {

        string EditingJobName { get; set; }

        public IJob JOB_MODEL { get; }

        Dictionary<string,IJob> Jobs { get; }


    }

}
