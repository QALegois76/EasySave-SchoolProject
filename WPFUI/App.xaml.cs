using LibEasySave;
using LibEasySave.TranslaterSystem;
using LibEasySave.ProcessMng;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace WPFUI
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private JobMng _jobMng;
        
        protected override void OnStartup(StartupEventArgs e)
        {
            if (!InstanceIsRunning.IsRunning("WPFUI"))
            {
                base.OnStartup(e);
                Translater.Instance.Init();
                DataModel.Instance.Init();

                _jobMng = new JobMng(new Job(""));
                ModelViewJobs modelViewJobs = new ModelViewJobs(_jobMng);

                MainWindow mw = new MainWindow(modelViewJobs);
                mw.Show();
            }else
            {
                MessageBox.Show("Instance running");
                Current.Shutdown();
            }
            
        }
    }
}
