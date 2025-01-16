using LibEasySave.AppInfo;
using LibEasySave.TranslaterSystem;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using WinFormApp;
using WPFUI.Themes;

namespace WPFUI.Ctrl
{
    /// <summary>
    /// Interaction logic for SettingWindow.xaml
    /// </summary>
    public partial class SettingWindow : Window, INotifyPropertyChanged
    {
        private const int HEIGHT_TB = 25;


        private enum EExtList
        {
            Priority,
            Crypt,
            Allow,
            JobApp
        }

        private List<RoundedControl> _tabs = new List<RoundedControl>();

        public event PropertyChangedEventHandler PropertyChanged;


        // public accesor
        public IViewDataModel ViewModel { get; set; }
        public ITranslatedText TranslatedText { get => Translater.Instance.TranslatedText; }


        // constuctor
        public SettingWindow(IViewDataModel viewModel , bool isRestricted = false)
        {
            ViewModel = viewModel;
            DataContext = this;

            InitializeComponent();

            scrollPnl.OnItemSelected -= ScrollPnl_OnItemSelected;
            scrollPnl.OnItemSelected += ScrollPnl_OnItemSelected;

            btnExtAllow.Tag = EExtList.Allow;
            btnExtCrypt.Tag = EExtList.Crypt;
            btnExtPriority.Tag = EExtList.Priority;
            btnJobApp.Tag = EExtList.JobApp;

            _tabs.Add(btnExtAllow);
            _tabs.Add(btnExtCrypt);
            _tabs.Add(btnExtPriority);
            _tabs.Add(btnJobApp);

            SelectTab(EExtList.Priority);

            SetRestrictedMode(isRestricted);

            ViewModel.PropertyChanged -= ViewModel_PropertyChanged;
            ViewModel.PropertyChanged += ViewModel_PropertyChanged;

            DataModel.Instance.AppInfo.PropertyChanged -= AppInfo_PropertyChanged;
            DataModel.Instance.AppInfo.PropertyChanged += AppInfo_PropertyChanged;
        }

        private void AppInfo_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(TranslatedText)));
        }

        private void ViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs( nameof(ViewModel)));
        }

        private void ScrollPnl_OnItemSelected(object sender, Themes.GuidSelecEventArg e)
        {
            scrollPnl.SelecteItem(e.Guid);
        }

        private void btnQuit_OnClick(object sender, EventArgs e)
        {
            // did for test
            DataModel.Instance.SaveAppInfo();
            this.Close();
        }


        private void Header_OnDown(object sender, MouseEventArgs e)
        {
            this.DragMove();
        }


        #region scroll panel advanced option

        private void btnUpp_OnClick(object sender, EventArgs e)
        {
            scrollPnl.UpSelection();
        }

        private void btnAdd_OnClick(object sender, EventArgs e)
        {
            if (btnJobApp.IsActiv)
            {
                scrollPnl.Add(new AppJobChoiceUC() { Height = HEIGHT_TB });
            }
            else
                scrollPnl.Add(new RoundedTexteBox() { Height = HEIGHT_TB });
        }

        private void btnRemove_OnClick(object sender, EventArgs e)
        {
            if (scrollPnl.SelectedGuid.HasValue)
                scrollPnl.Remove(scrollPnl.SelectedGuid.Value);
        }

        private void btnDown_OnClick(object sender, EventArgs e)
        {
            scrollPnl.DownSelection();
        }

        #endregion


        #region path Log region
        private void btnStateLogPathOpen_OnClick(object sender, EventArgs e)
        {
            WinDialogInfo info = WindowsDialog.SearchFolder(ViewModel.DataModel.LogInfo.StateLogPath);
            if (info.ResultDialog == EResultDialog.Ok)
                ViewModel.SetStateLogPathCommand.Execute(info.PathSelected);
        }
        private void btnDailyLogPathOpen_OnClick(object sender, EventArgs e)
        {
            WinDialogInfo info = WindowsDialog.SearchFolder(ViewModel.DataModel.LogInfo.DailyLogPath);
            if (info.ResultDialog == EResultDialog.Ok)
                ViewModel.SetDailyLogPathCommand.Execute(info.PathSelected);
        }

        #endregion


        #region Tab Utility
        private void ClinckBtnTab(object sender, EventArgs e) => SelectTab((EExtList)(sender as Control).Tag);

        private void SelectTab(EExtList extList)
        {
            foreach (Control rCtrl in _tabs)
            {
                if (!(rCtrl is IActivable))
                    continue;

                if((rCtrl as IActivable).IsActiv)
                {
                    // to save
                    ExecuteTabCommand((EExtList)rCtrl.Tag);
                }

                (rCtrl as IActivable).IsActiv = extList == (EExtList)rCtrl.Tag;
            }

            scrollPnl.Clear();

            switch (extList)
            {
                case EExtList.Priority:
                    FillScrollPanelExt(ViewModel.DataModel.AppInfo.PriorityExt);
                    break;

                case EExtList.Crypt:
                    FillScrollPanelExt(ViewModel.DataModel.CryptInfo.AllowEtx);
                    break;

                case EExtList.Allow:
                    FillScrollPanelExt(ViewModel.DataModel.AppInfo.AllowExt);
                    break;

                case EExtList.JobApp:
                    FillScrollPanelJobApp(ViewModel.DataModel.AppInfo.JobApps);
                    break;
                default:
                    break;
            }
            EnableExtSet(!DataModel.Instance.IsClientLock);

        }

        private void ExecuteTabCommand(EExtList eExtList)
        {
            switch (eExtList)
            {
                case EExtList.Priority:
                    ViewModel.SetPriorityExtCommand.Execute(GetStrings());
                    break;

                case EExtList.Crypt:
                    ViewModel.SetCryptExtCommand.Execute(GetStrings());
                    break;

                case EExtList.Allow:
                    ViewModel.SetAllowExtCommand.Execute(GetStrings());
                    break;

                case EExtList.JobApp:
                    ViewModel.SetJobAppCommand.Execute(GetPaths());
                    break;

                default:
                    return;
                    break;
            }
        }

        private void FillScrollPanelExt(List<string> extList)
        {
            if (extList == null || extList.Count == 0)
                return;

            foreach (string str in extList)
            {
                RoundedTexteBox roundedTexteBox = new RoundedTexteBox();
                roundedTexteBox.Height = HEIGHT_TB;
                roundedTexteBox.Text = str;
                scrollPnl.Add(roundedTexteBox);
            }
        }
        
        private void FillScrollPanelJobApp(List<string> extList)
        {
            if (extList == null || extList.Count == 0)
                return;

            foreach (string str in extList)
            {
                AppJobChoiceUC roundedTexteBox = new AppJobChoiceUC();
                roundedTexteBox.Height = HEIGHT_TB;
                roundedTexteBox.AppPath = str;
                scrollPnl.Add(roundedTexteBox);
            }
        }

        private List<string> GetStrings()
        {
            List<string> output = new List<string>();
            foreach (var item in scrollPnl.Controls)
            {
                output.Add((item as RoundedTexteBox).Text);
            }
            return output;
        }
        
        private List<string> GetPaths()
        {
            List<string> output = new List<string>();
            foreach (var item in scrollPnl.Controls)
            {
                output.Add((item as AppJobChoiceUC).AppPath);
            }
            return output;
        }

        #endregion


        #region restriction Mode

        private void SetRestrictedMode(bool state)
        {
            if (!state)
            {
                EnableLang(true);
                EnableModeIHM(true);
                EnableLogFormat(true);
                EnableCryptingKey(true);
                EnableLogPaths(true);
                EnableExtManip(true);
                EnableExtTabs(true);
                EnableExtSet(true);
            }
            else
            {
                if (DataModel.Instance.AppInfo.ModeIHM == EModeIHM.Client)
                    // restricted mode for client
                {
                    EnableLang(true);
                    EnableModeIHM(true);
                    EnableLogFormat(false);
                    EnableCryptingKey(false);
                    EnableLogPaths(false);
                    EnableExtManip(false);
                    EnableExtTabs(true);
                    EnableExtSet(false);
                }
                else
                    // restricted mode for server
                {
                    EnableLang(false);
                    EnableModeIHM(false);
                    EnableLogFormat(true);
                    EnableCryptingKey(true);
                    EnableLogPaths(true);
                    EnableExtManip(true);
                    EnableExtTabs(true);
                    EnableExtSet(true);
                }
            }
        }

        private void EnableLang(bool state) 
        {
            lbLang.IsEnabled = state;
            btnLangEn.IsEnabled = state;
            btnLangFr.IsEnabled = state;
        }
        private void EnableModeIHM(bool state)
        {
            lbIHMMode.IsEnabled = state;
            btnIhmModeClient.IsEnabled = state;
            btnIhmModeServer.IsEnabled = state;
        }
        private void EnableLogFormat(bool state) 
        {
            lbLogFormat.IsEnabled = state;
            btnLogFormatJson.IsEnabled = state;
            btnLogFormatXml.IsEnabled = state;
        }

        private void EnableCryptingKey(bool state) 
        {
            lbCryptingKey.IsEnabled = state;
            tbKey.IsEnabled = state;
        }

        private void EnableLogPaths(bool state)
        {
            lbTitleDailyLogPath.IsEnabled = state;
            lbDailyLogPath.IsEnabled = state;
            btnDailyLogPathOpen.IsEnabled = state;

            lbTitleStateLogPath.IsEnabled = state;
            lbStateLogPath.IsEnabled = state;
            btnStateLogPathOpen.IsEnabled = state;
        }

        private void EnableExtTabs(bool state) 
        {
            btnExtPriority.IsEnabled = state;
            btnExtCrypt.IsEnabled = state;
            btnExtAllow.IsEnabled = state;

        }

        private void EnableExtManip(bool state)
        {
            btnUpp.IsEnabled = state;
            btnAdd.IsEnabled = state;
            btnRemove.IsEnabled = state;
            btnDown.IsEnabled = state;
        }

        private void EnableExtSet(bool state)
        {
            foreach (var item in scrollPnl.Controls)
            {
                item.IsEnabled = state;
            }
        }

        #endregion
    }
}
