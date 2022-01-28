using System;
using LibEasySave;
using LibEasySave.Model;
using LibEasySave.TranslaterSystem;

namespace ConsoleUI
{
    class Program
    {
        private static CommandMng _cmdMng;
        private static JobMng _jobMng;

        private static bool _maintainLoop = true;
        private static bool _quitIsAsking = false;


        static void Main(string[] args)
        {
            Translater.Instance.Init();
            _jobMng = new JobMng(new Job(""));
            ModelViewJobs modelViewJobs = new ModelViewJobs(_jobMng);
            _cmdMng = new CommandMng(modelViewJobs);

            _cmdMng.Start();

            //Translater.Instance.TestSave();
        }




    }
}
