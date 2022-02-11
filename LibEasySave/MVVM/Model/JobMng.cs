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

        protected Guid _editingJobName = Guid.Empty;
        private readonly IJob JOB_MODEL;
        protected Dictionary<Guid, IJob> _jobs = new Dictionary<Guid, IJob>();

        public Guid EditingJob { get => _editingJobName; set => _editingJobName = value; }
        IJob IJobMng.JOB_MODEL => JOB_MODEL;

        public Dictionary<Guid, IJob> Jobs => _jobs;





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

        Guid EditingJob { get; set; }

        public IJob JOB_MODEL { get; }

        Dictionary<Guid,IJob> Jobs { get; }


    }

}
