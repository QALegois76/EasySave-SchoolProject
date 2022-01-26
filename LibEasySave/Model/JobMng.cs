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

        private readonly int MAX_JOB;
        protected string _editingJobName = null;
        private readonly IJob JOB_MODEL;
        protected Dictionary<string, IJob> _jobs = new Dictionary<string, IJob>(5);


        int IJobMng.MAX_JOB => MAX_JOB;

        public string EditingJobName { get => _editingJobName; set => _editingJobName = value; }
        IJob IJobMng.JOB_MODEL => JOB_MODEL;

        public Dictionary<string, IJob> Jobs => _jobs;





        #endregion


        // constructor
        public JobMng(IJob jobModel,int maxJob = 5)
        {
            if (jobModel == null)
                throw new Exception("jobModel musn't be null");

            if (maxJob <1)
                throw new Exception("maxJob musn't be greater than 0");

            JOB_MODEL = jobModel;
            MAX_JOB = maxJob;
        }
    }

    public interface IJobMng
    {
        public int MAX_JOB { get; }

        string EditingJobName { get; set; }

        public IJob JOB_MODEL { get; }

        Dictionary<string,IJob> Jobs { get; }


    }

}
