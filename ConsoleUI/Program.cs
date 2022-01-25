using System;
using LibEasySave;
using LibEasySave.Model;

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
            Job job = new Job("groutt",@"C:\Windows\Fonts",@"C:\Windows\Fonts");
            JobSaverStrategy.save(job);
        }




    }
}
