using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading;

namespace LibEasySave
{
    /// <summary>
    /// JobSaverFactory is a factory. It permits to create new instance of BaseJobSaver
    /// </summary>
    public class JobSaverFactory
    {

        public static bool Save(IJob job)
        {
            if (job == null)
            {
                Debug.Fail("Veuillez entrer un job");
                return false;
            }

            BaseJobSaver temp;
            
            try
            {
                switch (job.SavingMode)
                {

                    case ESavingMode.DIFF:
                        temp = new DifferentialSaver(job);
                        break;

                    default:
                    case ESavingMode.FULL:
                        temp = new FullSaver(job);
                        break;
                }

                //temp.Save();
                //WaitCallback callback = new WaitCallback(temp.Save);
                //ThreadPool.QueueUserWorkItem(callback);
                //LogMng.Instance.SaveDailyLog();
            }
            catch(Exception ex)
            {
                return false;
            }
            return true;
        }

        public static BaseJobSaver CreateInstance(IJob job)
        {
            BaseJobSaver temp;
            switch (job.SavingMode)
            {

                case ESavingMode.DIFF:
                    temp = new DifferentialSaver(job);
                    break;

                default:
                case ESavingMode.FULL:
                    temp = new FullSaver(job);
                    break;
            }

            return temp;
        }
    }
}
