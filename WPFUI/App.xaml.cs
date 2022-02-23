using ConsoleUI;
using LibEasySave;
using LibEasySave.AppInfo;
using LibEasySave.TranslaterSystem;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Runtime.Remoting;
using LibEasySave.Network;

namespace WPFUI
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private JobMng _jobMng;
        // Unique application number 
        const string AppId = "Local\\1DDFB948-19F1-417C-903D-BE05335DB8A4";

        protected override void OnStartup(StartupEventArgs e)
        {      
            base.OnStartup(e);

            DataModel.Instance.Init();
            Translater.Instance.Init();

            _jobMng = new JobMng(new LibEasySave.Job(""));
            ModelViewJobs modelViewJobs = new ModelViewJobs(_jobMng);
            NetworkMng.Instance.Init(modelViewJobs, new ViewDataModel(DataModel.Instance));

            CommandMng commandMng = new CommandMng(modelViewJobs);
            MainWindow mw = new MainWindow(modelViewJobs);
            mw.Show();
        }

        protected static void MonoInstance()
        {
            using (Mutex mutex = new Mutex(false, AppId))
            {
                if (!mutex.WaitOne(0))
                {
                    MessageBox.Show("Une instance existe déjà !");
                    Console.WriteLine("2nd instance");
                    return;
                }
                Console.WriteLine("Started");
                //Console.ReadKey();
            }
        }
    }
}
