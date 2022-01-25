using System;
using LibEasySave;

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

            Console.WriteLine("Hello World!");
            _jobMng= new JobMng();
            ModelViewJobs modelViewJobs = new ModelViewJobs(_jobMng.Jobs);
            _cmdMng = new CommandMng(modelViewJobs);

            _cmdMng.Start();
        }




    }
}
