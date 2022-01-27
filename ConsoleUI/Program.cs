using System;
using LibEasySave;
using LibEasySave.Model;

namespace ConsoleUI
{
    class Program
    {
        private static CommandMng _cmdMng;
        private static JobMng _jobMng;

        static void Main(string[] args)
        {
            //_jobMng = new JobMng(new Job(""));
            //ModelViewJobs modelViewJobs = new ModelViewJobs(_jobMng);
            //_cmdMng = new CommandMng(modelViewJobs);
            //_cmdMng.Start();

            Log l = new Log("job1", "mivebavbeari", "fmkjvneavmjea"," 50kpo", int.MaxValue);
            LogBaseSaver b = new JSONText(l);
            var str  =b.GetSavedLogText();
        }




    }
}
