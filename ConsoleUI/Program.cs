using System;
using System.Collections.Generic;
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
            //Translater.Instance.Init();
            //_jobMng = new JobMng(new Job(""));
            //ModelViewJobs modelViewJobs = new ModelViewJobs(_jobMng);
            //_cmdMng = new CommandMng(modelViewJobs);

            //_cmdMng.Start();

            List<String> list = new List<string>();
            list.Add(".txt");
            list.Add(".js");
            list.Add(".rar");

            List<String> listToChange = new List<string>();
            list.Add(".txt");
            list.Add(".js");
            list.Add(".rar");
            listToChange.Add(".rar");
            listToChange.Add(".txt");
            listToChange.Add(".lxt");

            //Translater.Instance.TestSave();
        }




    }
}
