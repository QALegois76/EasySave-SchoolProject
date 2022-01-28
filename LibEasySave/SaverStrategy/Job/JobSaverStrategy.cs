using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace LibEasySave
{
    public static class JobSaverStrategy
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

                temp.Save();
            }
            catch(Exception ex)
            {
                return false;
            }
            return true;
        }
    }
}
