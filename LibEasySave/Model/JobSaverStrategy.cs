using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace LibEasySave.Model
{
    public class JobSaverStrategy
    {
        IJob jober;



        public JobSaverStrategy()
        {
        }

        public static bool save(IJob job)
        {
            if (job == null)
            {
                Debug.Fail("Veuillez entrer un job");
                return false;
            }

            BaseJobSaver temp;

            switch (job.SavingMode)
            {

                case ESavingMode.Diferential:
                    temp = new DifferentialSaver(job);
                    break;

                default:
                case ESavingMode.Full:
                    temp = new FullSaver(job);
                    break;
            }

            temp.copyfile();
            return true;
        }


        // copy 




    }
}
