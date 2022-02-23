using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Threading;
using LibEasySave;
using LibEasySave.AppInfo;
using LibEasySave.Model.LogMng.Interface;
using LibEasySave.Network;

namespace TesteConnection
{
    class Program
    {

        private static List<TcpClient> _clients = new List<TcpClient>();


        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            for (int i = 0; i < 5; i++)
            {
                Console.WriteLine("start in "+(5-i));
                Thread.Sleep(1000);
            }

            Console.WriteLine("wait user signal...");
            Console.ReadKey();



            Console.WriteLine("Start");
            DataModel.Instance.Init();
            DataModel.Instance.AppInfo.ModeIHM = EModeIHM.Client;
            IJobMng model = new JobMng(new Job("model"));
            IModelViewJob modelViewJob = new ModelViewJobs(model);
            NetworkMng.Instance.Init(modelViewJob, new ViewDataModel( DataModel.Instance));
            NetworkMng.Instance.Start();


            Guid g1 = Guid.NewGuid();
            Guid g2 = Guid.NewGuid();
            Guid g3 = Guid.NewGuid();

            modelViewJob.AddJobCommand.Execute(g1);
            modelViewJob.AddJobCommand.Execute(g2);
            modelViewJob.AddJobCommand.Execute(g3);

            LogMng.Instance.SetActivStateLog(g1, 100, 100, null, null);
            LogMng.Instance.SetActivStateLog(g2, 100, 100, null, null);
            LogMng.Instance.SetActivStateLog(g3, 100, 100, null, null);


            List<IJob> jobs = new List<IJob>();
            foreach (var item in model.Jobs)
            {
                jobs.Add(item.Value);
            }

            NetworkMng.Instance.SendNetworkCommad(ENetorkCommand.UpdateJobList, jobs.ToArray());


            Thread.Sleep(1000);

            for (int i = 0; i <= 100; i++)
            {
                try
                {
                    Console.WriteLine("Update "+i);
                    var temp1 = (IActivStateLog)LogMng.Instance.GetStateLog(g1);
                    var temp2 = (IActivStateLog)LogMng.Instance.GetStateLog(g2);
                    var temp3 = (IActivStateLog)LogMng.Instance.GetStateLog(g3);

                    temp1.Progress.UpdateProgress(null, null, 1);
                    Thread.Sleep(5);
                    temp2.Progress.UpdateProgress(null, null, 2);
                    Thread.Sleep(5);
                    temp3.Progress.UpdateProgress(null, null, 1);

                    Thread.Sleep(20);
                }
                catch(Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
                Thread.Sleep(1000);
            }
            Console.WriteLine("End");
            
            

        }

        


    }
}
