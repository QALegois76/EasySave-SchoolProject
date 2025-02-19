﻿using ConsoleUI;
using LibEasySave;
using LibEasySave.AppInfo;
using LibEasySave.TranslaterSystem;
using LibEasySave.ProcessMng;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
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
        private const string MULTI_INST_PARAM = "-M";

        protected override void OnStartup(StartupEventArgs e)
        {      
            base.OnStartup(e);


            //DataModel.Instance.Init();
            //Translater.Instance.Init();
            List<string> param = e.Args.ToList();

            if (!InstanceIsRunning.IsRunning("WPFUI") || param.Contains(MULTI_INST_PARAM))
            {
                param.Remove(MULTI_INST_PARAM);
                base.OnStartup(e);
                Translater.Instance.Init();
                DataModel.Instance.Init();

                _jobMng = new JobMng();
                ModelViewJobs modelViewJobs = new ModelViewJobs(_jobMng);
                NetworkMng.Instance.Init(modelViewJobs, new ViewDataModel(DataModel.Instance));
                MainWindow mw = new MainWindow(modelViewJobs);
                if (e.Args.Length > 0 && e.Args[0] != null && e.Args[0] != MULTI_INST_PARAM)
                    modelViewJobs.OpenJobFile.Execute(e.Args[0]);
                mw.Show();
            }else
            {
                MessageBox.Show("Instance running");
                Current.Shutdown();
            }
            
        }
    }
}
