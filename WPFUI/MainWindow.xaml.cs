using AyoControlLibrary;
using LibEasySave;
using LibEasySave.AppInfo;
using LibEasySave.Network;
using LibEasySave.TranslaterSystem;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
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
using WinFormApp;
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

            ScrollPanel_Network.OnItemSelected -= ScrollPanel_Network_OnItemSelected;
            ScrollPanel_Network.OnItemSelected += ScrollPanel_Network_OnItemSelected;

            DataModel.Instance.AppInfo.PropertyChanged -= PropChanged;
            DataModel.Instance.AppInfo.PropertyChanged += PropChanged;

            DataModel.Instance.PropertyChanged -= PropChanged;
            DataModel.Instance.PropertyChanged += PropChanged;

            NetworkMng.PropertyChanged -= PropChanged;
            NetworkMng.PropertyChanged += PropChanged;

            NetworkMng.OnAddingClient -= NetworkMng_OnAddingClient;
            NetworkMng.OnAddingClient += NetworkMng_OnAddingClient;

            NetworkMng.OnRemovingClient -= NetworkMng_OnRemovingClient;
            NetworkMng.OnRemovingClient += NetworkMng_OnRemovingClient;


            EditJobUC.IsEnabledChanged -= EditJobUC_IsEnabledChanged;
            EditJobUC.IsEnabledChanged += EditJobUC_IsEnabledChanged;

            LogMng.Instance.OnProgressChanged -= LogMng_OnProgressChanged;
            LogMng.Instance.OnProgressChanged += LogMng_OnProgressChanged;

            btnServerStart.IsEnabled = !NetworkMng.IsListening;
            btnServerStop.IsEnabled = NetworkMng.IsListening;

            ScrollPanel_OnItemSelected(null, null);

            if (!DataModel.Instance.IsValid())
            {
                RoundedMessageBox.Show("error !\nData Model not correct\nplease vérify DailyLogPath and StateLogPath");
                EnableJob(false);
            }
        }



        private void LogMng_OnProgressChanged(object sender, ProgressJobEventArgs jobEventArgs)
        {
            Dispatcher.Invoke(delegate
            {
                //double pourcent = ((jobEventArgs.SizeDone / jobEventArgs.SizeToDo) + (jobEventArgs.FilesDone / jobEventArgs.FilesToDo)) / 2d;
                double pourcent = ( (jobEventArgs.FilesDone / jobEventArgs.FilesToDo));

                var temp = (ScrollPanel[jobEventArgs.Guid] as JobChoiceUC);
                if (temp != null)
                    temp.ProgressPourcent =1- pourcent;
                //Debug.Write("Pourcent = " + (1 - pourcent));
                //Console.WriteLine("Pourcent = "+(1- pourcent));
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

        private void PropChanged(object sender, PropertyChangedEventArgs e)
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
                
                if (e.PropertyName == nameof(NetworkMng.Instance.IsListening))
                {
                    btnServerStart.IsEnabled = !NetworkMng.IsListening;
                    btnServerStop.IsEnabled = NetworkMng.IsListening;
                }

                if (e.PropertyName == nameof(DataModel.Instance.IsClientLock))
                    NetworkMng_OnLockClient();

                InvalidateVisual();

            });

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
            SettingWindow settingWindow = new SettingWindow(new ViewDataModel(DataModel.Instance), DataModel.Instance.IsClientLock);
            settingWindow.ShowDialog();
            DataModel.Instance.SaveAppInfo();
            if (!DataModel.Instance.IsClientLock)
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

        private void btnServerStop_OnClick(object sender, EventArgs e)
        {
            NetworkMng.Stop();
        }

        private void btnServerStart_OnClick(object sender, EventArgs e)
        {
            NetworkMng.Start();
        }



        private void EditJobUC_JobNameChanged(object sender, TextChangedEventArgs e)
        {
            Control c = ScrollPanel[_modelView.EditingJob];
            if (!(c is JobChoiceUC))
                return;

            (c as JobChoiceUC).Title = _modelView.Model.BaseJober[_modelView.EditingJob].Job.Name;


        }

        private void btnSave_OnClick(object sender, EventArgs e)
        {
           var temp = WindowsDialog.SaveFile(null, DataModel.Instance.AppInfo.FilterFileDialog, DataModel.Instance.AppInfo.EasySaveFileExt);
            if (temp.ResultDialog == EResultDialog.Ok)
                _modelView.SaveJobFile.Execute(temp.PathSelected);
        }

        private void btnOpen_OnClick(object sender, EventArgs e)
        {
           var temp = WindowsDialog.OpenFile(null, DataModel.Instance.AppInfo.FilterFileDialog, DataModel.Instance.AppInfo.EasySaveFileExt);
            if (temp.ResultDialog == EResultDialog.Ok)
                _modelView.OpenJobFile.Execute(temp.PathSelected);
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
                btnEditJob.IsActiv = true;
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
                _modelView.Model.BaseJober[e.Guid].Job.PropertyChanged -= Job_PropertyChanged;
                _modelView.Model.BaseJober[e.Guid].Job.PropertyChanged += Job_PropertyChanged;

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

        private void Job_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            this.Dispatcher.Invoke(delegate
            {
                if (!(sender is IJob))
                    return;

                if (!_modelView.Model.BaseJober.ContainsKey((sender as IJob).Guid))
                    return;

                (ScrollPanel[(sender as IJob).Guid] as JobChoiceUC).Title = (sender as IJob).Name;
            });

        }

        private void ScrollPanel_OnItemSelected(object sender, GuidSelecEventArg e)
        {
            EditJobUC.SetJob(null);
            EditJobUC.IsEnabled = false;
            (btnEditJob as IActivable).IsActiv = false;
            EditJobUC.InvalidateVisual();
            if (!ScrollPanel.SelectedGuid.HasValue)
                return;
            
            jobInfoUC.SetJobInfo(_modelView.Model.BaseJober[ScrollPanel.SelectedGuid.Value].JobInfo);
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

        private void ScrollPanel_Network_OnItemSelected(object sender, GuidSelecEventArg e)
        {

            NetworkMng.Instance.SelectedGuidClient = e.Guid;
        }

        private void NetworkMng_OnLockClient()
        {
            this.Dispatcher.Invoke(delegate
            {
                bool state = !DataModel.Instance.IsClientLock && DataModel.Instance.IsValid();
                EnableJob(state);
                ScrollPanel.IsEnabled = true;
                foreach (var ctrl in ScrollPanel.Controls)
                {
                    ctrl.IsEnabled = state;
                }
            });
        }

        private void NetworkMng_OnRemovingClient(object sender, GuidSenderEventArg e)
        {
            this.Dispatcher.Invoke(delegate {
                ScrollPanel_Network.Remove(e.Guid);
            });
        }

        private void NetworkMng_OnAddingClient(object sender, AddingClientEventArgs e)
        {
            this.Dispatcher.Invoke(delegate {
                var temp = new NetworkClientUC();
                temp.OnSettingClient -= NetworkClientUC_OnSettingClient;
                temp.OnSettingClient += NetworkClientUC_OnSettingClient;

                temp.OnRefreshClient -= NetworkClientUC_OnRefreshClient;
                temp.OnRefreshClient += NetworkClientUC_OnRefreshClient;

                temp.OnLockUIClient -= NetworkClientUC_OnLockUIClient;
                temp.OnLockUIClient += NetworkClientUC_OnLockUIClient;

                temp.OnCloseClient -= NetworkClientUC_OnCloseClient;
                temp.OnCloseClient += NetworkClientUC_OnCloseClient;
                temp.IPClient = e.IPClient;
                ScrollPanel_Network.Add(temp, e.Guid);
            });
        }

        private void NetworkClientUC_OnLockUIClient(object sender, EventArgs e)
        {
            if (!(sender is NetworkClientUC))
                return;

            NetworkMng.SendNetworkCommad(ENetorkCommand.LockUIClient, (sender as NetworkClientUC).IsLock);
        }

        private void NetworkClientUC_OnRefreshClient(object sender, EventArgs e)
        {
            if (!(sender is Control))
                return;

            if (!((sender as Control).Tag is Guid))
                return;

            var g = (Guid)(sender as Control).Tag;

            NetworkMng.Instance.SendNetworkCommad(ENetorkCommand.GetJobList, null);
        }

        private void NetworkClientUC_OnSettingClient(object sender, EventArgs e)
        {
            NetworkMng.Instance.SendNetworkCommad(ENetorkCommand.GetDataModel, null);
            bool isLock = (ScrollPanel_Network[ScrollPanel_Network.SelectedGuid.Value] as NetworkClientUC).IsLock;
            IViewDataModel vdm = new ViewDataModel(DataModel.InstanceActivClient);
            SettingWindow settingWindow = new SettingWindow(vdm, isLock);
            settingWindow.ShowDialog();
            NetworkMng.Instance.SendNetworkCommad(ENetorkCommand.UpdateDataModel, vdm.DataModel);
        }

        private void NetworkClientUC_OnCloseClient(object sender, EventArgs e)
        {
            if (!(sender is Control))
                return;

            if (!((sender as Control).Tag is Guid))
                return;

            var g = (Guid)(sender as Control).Tag;

            NetworkMng.Instance.Disconnect(g);
        }

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


    }
}
