using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
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
        private static TcpClient _tcpClient = new TcpClient();

        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

           
            while(!_tcpClient.Connected)
            {
                try
                {
                    _tcpClient.Connect("127.0.0.1", 8080);
                }
                catch (Exception e)
                {
                    Console.WriteLine("ERROR : "+e.Message);
                }
            }

            Console.WriteLine("Client connected !");


            while (_tcpClient.Connected)
            {
                try
                {
                    byte[] buffer = new byte[_tcpClient.ReceiveBufferSize];
                    _tcpClient.GetStream().Read(buffer);
                    string str = Encoding.UTF8.GetString(buffer);
                    var temp = JSONDeserializer<NetworkInfo>.Deserialize(str);
                    if (temp == null)
                        Console.WriteLine("Error Deserialisation");
                    else
                        Console.WriteLine("NetCommand : " + temp.Command + "   |   " + temp.Parameter);

                    Console.WriteLine("");
                }
                catch (Exception ex)
                {
                    Console.WriteLine("ERROR : "+ex.Message);
                }
            }

            Console.WriteLine("Client Disconnected !");

            Console.WriteLine("End Programme");

        }




    }


    public class TestClass
    {
        public TestClass()
        {
            new Thread(ThreadMethod).Start();
        }
        private void ThreadMethod()
        {
            DataModel.Instance.Init();
            DataModel.Instance.AppInfo.ModeIHM = EModeIHM.Client;
            IJobMng model = new JobMng(new FullSaver(new Job("name1")));
            IModelViewJob modelViewJob = new ModelViewJobs(model);
            NetworkMng.Instance.Init(modelViewJob, new ViewDataModel(DataModel.Instance));
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
            foreach (var item in model.BaseJober)
            {
                jobs.Add(item.Value.Job);
            }

            NetworkMng.Instance.SendNetworkCommad(ENetorkCommand.UpdateJobList, jobs.ToArray());


            Thread.Sleep(1000);

            for (int i = 0; i <= 100; i++)
            {
                try
                {
                    Console.WriteLine("Update " + i);
                    var temp1 = (IActivStateLog)LogMng.Instance.GetStateLog(g1);
                    var temp2 = (IActivStateLog)LogMng.Instance.GetStateLog(g2);
                    var temp3 = (IActivStateLog)LogMng.Instance.GetStateLog(g3);

                    temp1.Progress.UpdateProgress(null, null, 1);
                    Thread.Sleep(5);
                    temp2.Progress.UpdateProgress(null, null, 2);
                    Thread.Sleep(5);
                    temp3.Progress.UpdateProgress(null, null, 1);

                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
                Thread.Sleep(500);
            }
        }
    }
}
