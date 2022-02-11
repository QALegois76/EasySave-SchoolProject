﻿using LibEasySave;
using LibEasySave.TranslaterSystem;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
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
            base.OnStartup(e);
            Translater.Instance.Init();

            _jobMng = new JobMng(new LibEasySave.Job(""));
            ModelViewJobs modelViewJobs = new ModelViewJobs(_jobMng);

            MainWindow mw = new MainWindow(modelViewJobs);
            mw.Show();
        }
    }
}
