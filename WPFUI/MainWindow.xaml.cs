﻿using AyoControlLibrary;
using LibEasySave;
using LibEasySave.AppInfo;
using LibEasySave.Network;
using LibEasySave.TranslaterSystem;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WPFUI.Ctrl;
using WPFUI.Themes;

namespace WPFUI
{


    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        private const double CLIENT_WIDTH = 850;
        private const double SERVER_WIDTH = 1185;

        // private memeber
        private IModelViewJob _modelView;
        private Dictionary<object, Control> _clientCtrl = new Dictionary<object, Control>();
        public event PropertyChangedEventHandler PropertyChanged;


        public double WidthWidow { get => (DataModel.Instance.AppInfo.ModeIHM == EModeIHM.Client) ? CLIENT_WIDTH : SERVER_WIDTH;}
        public string Test => "Sucess";

        public ITranslatedText TranslatedText { get => Translater.Instance.TranslatedText; }

        public IModelViewJob ModelView => _modelView;

        public NetworkMng NetworkMng => NetworkMng.Instance;





        // constructor
        public MainWindow(IModelViewJob modelView)
        {

            DataContext = this;
            _modelView = modelView;

            //bTestEn.CommandParameter = ELangCode.EN;
            //bTestFr.CommandParameter = ELangCode.FR;

            InitializeComponent();

            _modelView.OnAdding -= ModelView_OnAdding;
            _modelView.OnAdding += ModelView_OnAdding;

            _modelView.OnRemoving -= ModelView_OnRemoving;
            _modelView.OnRemoving += ModelView_OnRemoving;

            _modelView.OnEditing -= ModelView_OnEditing;
            _modelView.OnEditing += ModelView_OnEditing;

            _modelView.OnRunAll -= ModelView_OnRunAll;
            _modelView.OnRunAll += ModelView_OnRunAll;

            EditJobUC.JobNameChanged -= EditJobUC_JobNameChanged;
            EditJobUC.JobNameChanged += EditJobUC_JobNameChanged;

            ScrollPanel.OnItemSelected -= ScrollPanel_OnItemSelected;
            ScrollPanel.OnItemSelected += ScrollPanel_OnItemSelected;

            DataModel.Instance.AppInfo.PropertyChanged -= DataModelPropChanged;
            DataModel.Instance.AppInfo.PropertyChanged += DataModelPropChanged;


            NetworkMng.PropertyChanged -= DataModelPropChanged;
            NetworkMng.PropertyChanged += DataModelPropChanged;

            NetworkMng.Collectionchanged -= NetworkMng_Collectionchanged;
            NetworkMng.Collectionchanged += NetworkMng_Collectionchanged;

            EditJobUC.IsEnabledChanged -= EditJobUC_IsEnabledChanged;
            EditJobUC.IsEnabledChanged += EditJobUC_IsEnabledChanged;

            LogMng.Instance.OnProgressChanged -= LogMng_OnProgressChanged;
            LogMng.Instance.OnProgressChanged += LogMng_OnProgressChanged;


            ScrollPanel_OnItemSelected(null, null);

            if (!DataModel.Instance.IsValid())
            {
                RoundedMessageBox.Show("error !\nData Model not correct\nplease vérify DailyLogPath and StateLogPath");
                EnableJob(false);
            }

           
            //ModeServer(DataModel.Instance.AppInfo.ModeIHM == EModeIHM.Server);

        }

        private void LogMng_OnProgressChanged(object sender, ProgressJobEventArgs jobEventArgs)
        {
            Dispatcher.Invoke(delegate
            {
                double pourcent = ((jobEventArgs.SizeDone / jobEventArgs.SizeToDo) + (jobEventArgs.FilesDone / jobEventArgs.FilesToDo)) / 2d;

                var temp = (ScrollPanel[jobEventArgs.Guid] as JobChoiceUC);
                if (temp != null)
                    temp.ProgressPourcent =1- pourcent;
            });
        }

        private void NetworkMng_Collectionchanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            Dispatcher.Invoke(delegate
            {


                if (e.OldItems != null)
                    foreach (var item in e.OldItems)
                    {
                        this.Dispatcher.Invoke(delegate
                        {
                            if (!_clientCtrl.ContainsKey(item))
                                return;

                            ScrollPanel_Network.Remove(_clientCtrl[item]);
                            _clientCtrl.Remove(item);
                        });

                    }

                if (e.NewItems != null)
                    foreach (var item in e.NewItems)
                    {
                        this.Dispatcher.Invoke(delegate
                        {
                            var temp = new NetworkClientUC();
                            _clientCtrl.Add(item, temp);
                            ScrollPanel_Network.Add(temp);
                        });
                    }
            });
        }

        

        private void EditJobUC_IsEnabledChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (EditJobUC.IsEnabled)
            {
                jobInfoUC.Visibility = Visibility.Hidden;
            }
            else
            {
                jobInfoUC.Visibility = Visibility.Visible;
            }
        }




        private void DataModelPropChanged(object sender, PropertyChangedEventArgs e)
        {
            Dispatcher.Invoke(delegate
            {


                if (e.PropertyName == nameof(DataModel.Instance.AppInfo.ModeIHM))
                {
                    this.Width = WidthWidow;
                    //ModeServer(DataModel.Instance.AppInfo.ModeIHM == EModeIHM.Server);
                    InvalidateVisual();
                }

                if (e.PropertyName == nameof(DataModel.Instance.AppInfo.ActivLang))
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(TranslatedText)));

                if (e.PropertyName == nameof(NetworkMng.Instance.IsConnected))
                {
                    cBtnServerState.IsActiv = true;
                    cBtnServerState.ColorBorderEnable = NetworkMng.Instance.IsConnected ? Color.FromArgb(255, 0, 255, 0) : Color.FromArgb(255, 255, 0, 0);
                    cBtnServerState.ColorBorderActiv = NetworkMng.Instance.IsConnected ? Color.FromArgb(255, 0, 255, 0) : Color.FromArgb(255, 255, 0, 0);
                    btnConnect.Text = NetworkMng.IsConnected ? "Disconnect" : "Connect";
                }

                InvalidateVisual();

            });

        }

        private void ScrollPanel_OnItemSelected(object sender, GuidSelecEventArg e)
        {
            EditJobUC.SetJob(null);
            EditJobUC.IsEnabled = false;
            EditJobUC.InvalidateVisual();
        }

        private void EditJobUC_JobNameChanged(object sender, TextChangedEventArgs e)
        {
            Control c = ScrollPanel[_modelView.EditingJob];
            if (!(c is JobChoiceUC))
                return;

            (c as JobChoiceUC).Title = _modelView.Model.BaseJober[_modelView.EditingJob].Job.Name;


        }




        #region event from UI
        private void btnQuit_OnClick(object sender, EventArgs e)
        {
            Environment.Exit(Environment.ExitCode);
            Application.Current.Shutdown();
        }

        private void btnMax_OnClick(object sender, EventArgs e) => 
            WindowState = (WindowState == WindowState.Normal)? WindowState.Maximized : WindowState.Normal;

        private void btnMin_OnClick(object sender, EventArgs e) => WindowState = WindowState.Minimized;

        private void Header_OnDown(object sender, MouseEventArgs e)
        {
            this.DragMove();
        }

        private void Add_OnClick(object sender, EventArgs e)
        {
            if (_modelView == null)
                return;
            _modelView?.AddJobCommand.Execute(null);
        }

        private void Edit_OnClick(object sender, EventArgs e)
        {
            if (_modelView == null)
                return;
            _modelView?.EditJobCommand.Execute(ScrollPanel.SelectedGuid);
        }

        private void Remove_OnClick(object sender, EventArgs e)
        {
            if (_modelView == null)
                return;
            _modelView?.RemoveJobCommand.Execute(ScrollPanel.SelectedGuid);
        }

        private void RunAll_OnClick(object sender, EventArgs e)
        {
            if (_modelView == null)
                return;
            //_modelView?.RunAllJobCommand.Execute(null);


            List<Guid> guids = new List<Guid>();
            foreach (var item in ScrollPanel.Controls)
            {
                if (!(item is JobChoiceUC))
                    continue;

                if ((item as JobChoiceUC).IsSelected)
                    guids.Add((Guid)(item as JobChoiceUC).Tag);
            }

            foreach (Guid item in guids)
            {
                _modelView.RunJobCommand.Execute(item);
            }
            //_modelView.RunAllJobCommand.Execute(guids);

        }

        private void cBtnActivAll_OnActivStateChanged(object sender, EventArgs e)
        {
            foreach (JobChoiceUC item in ScrollPanel.Controls)
            {
                item.IsSelected = cBtn_ActivAll.IsActiv;
            }
        }

        private void btn_Setting_OnClick(object sender, EventArgs e)
        {
            SettingWindow settingWindow = new SettingWindow(new ViewDataModel(DataModel.Instance));
            settingWindow.ShowDialog();
            DataModel.Instance.SaveAppInfo();
            EnableJob(DataModel.Instance.IsValid());
        }

        private void ConnectClick(object sender , EventArgs e)
        {
            cBtnServerState.ColorBorderEnable = AyoToolsUtility.AyoLightGray;
            cBtnServerState.IsActiv = false;

            if (NetworkMng.IsConnected)
            {
                NetworkMng.Stop();
                (sender as RoundedControl).Text = "Connect";
            }
            else
            {
                NetworkMng.Start();
                (sender as RoundedControl).Text = "Disconnect";
            }


        }
        #endregion

        #region event from model

        private void ModelView_OnRunAll(object sender, GuidSenderEventArg e)
        {
            //List<Guid> guids = new List<Guid>();
            //foreach (var item in ScrollPanel.Controls)
            //{
            //    if (!(item is JobChoiceUC))
            //        continue;

            //    if ((item as JobChoiceUC).IsSelected)
            //        guids.Add((Guid)(item as JobChoiceUC).Tag);
            //}

            //foreach (Guid item in guids)
            //{
            //    _modelView.RunJobCommand.Execute(item);
            //}
            //_modelView.RunAllJobCommand.Execute(guids);
        }

        private void ModelView_OnEditing(object sender, GuidSenderEventArg e)
        {
            Dispatcher.Invoke(delegate
            {

                EditJobUC.IsEnabled = true;
                EditJobUC.SetJob(_modelView.Model.BaseJober[e.Guid].Job);
                EditJobUC.InvalidateVisual();
            });
        }

        private void ModelView_OnRemoving(object sender, GuidSenderEventArg e)
        {
            Dispatcher.Invoke(delegate
            {

                ScrollPanel.Remove(e.Guid);
            });
        }

        private void ModelView_OnAdding(object sender, GuidSenderEventArg e)
        {
            Dispatcher.Invoke(delegate
            {

                var temp = new JobChoiceUC(_modelView.Model.BaseJober[e.Guid].Job.Name);

                temp.OnPlayClick -= JobChoiceUC_OnPlayClick;
                temp.OnPlayClick += JobChoiceUC_OnPlayClick;

                temp.OnPauseClick -= JobChoiceUC_OnPauseClick;
                temp.OnPauseClick += JobChoiceUC_OnPauseClick;

                temp.OnStopClick -= JobChoiceUC_OnStopClick;
                temp.OnStopClick += JobChoiceUC_OnStopClick;

                ScrollPanel.Add(temp, e.Guid);
            });
        }

        private void JobChoiceUC_OnStopClick(object sender, GuidSelecEventArg e)
        {
            _modelView.Model.BaseJober[e.Guid].Stop();
            //BaseJobSaver.BreakJob(EState.Stop);
        }

        private void JobChoiceUC_OnPauseClick(object sender, GuidSelecEventArg e)
        {
            _modelView.Model.BaseJober[e.Guid].Pause();
            //BaseJobSaver.BreakJob(EState.Break);
        }

        private void JobChoiceUC_OnPlayClick(object sender, GuidSelecEventArg e)
        {

            if (_modelView.Model.BaseJober[e.Guid].State == EState.Pause)
                // is pause
            {
                _modelView.Model.BaseJober[e.Guid].Play();
            }
            else
            // is stop
            {
                _modelView.RunJobCommand.Execute(e.Guid);
            }



            //BaseJobSaver.BreakJob(EState.Play);
        }

        #endregion


        #region Network interaction

        public void ModeServer(bool state)
        {
            EnableJob(state);
            EnableNetworkClient(!state);
        }

        #endregion

        #region Utility

        private void EnableJob(bool state)
        {
            cBtn_ActivAll.IsEnabled = state;
            btnAddJob.IsEnabled = state;
            rCtrlActivAll.IsEnabled = state;
            btnEditJob.IsEnabled = state;
            btnRemoveJob.IsEnabled = state;
            btnRunAllJob.IsEnabled = state;
            EditJobUC.IsEnabled = btnEditJob.IsActiv;
            ScrollPanel.IsEnabled = state;
        }

        private void EnableNetworkClient(bool state)
        {
            lbServer.Visibility = state ? Visibility.Visible : Visibility.Hidden;
            tbHostNameIpServer.Visibility = state ? Visibility.Visible : Visibility.Hidden;
            btnConnect.Visibility = state ? Visibility.Visible : Visibility.Hidden;
            cBtnServerState.Visibility = state ? Visibility.Visible : Visibility.Hidden;
            rCtrlStateServer.Visibility = state ? Visibility.Visible : Visibility.Hidden;
        }
        #endregion

        private void btnServerStop_OnClick(object sender, EventArgs e)
        {
            NetworkMng.Stop();
        }

        private void btnServerStart_OnClick(object sender, EventArgs e)
        {
            NetworkMng.Start();
        }
    }
}
