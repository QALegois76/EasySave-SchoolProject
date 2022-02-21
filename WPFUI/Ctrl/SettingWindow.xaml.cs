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
    public partial class SettingWindow : Window
    {
        private const int HEIGHT_TB = 25;


        private enum EExtList
        {
            Priority,
            Crypt,
            Allow,
        }

        private List<RoundedControl> _tabs = new List<RoundedControl>();


        // public accesor
        public IViewDataModel ViewModel { get; set; }
        public ITranslatedText TranslatedText { get => Translater.Instance.TranslatedText; }


        // constuctor
        public SettingWindow(IViewDataModel viewModel)
        {
            
            DataContext = this;
            ViewModel = viewModel;

            InitializeComponent();

            scrollPnl.OnItemSelected -= ScrollPnl_OnItemSelected;
            scrollPnl.OnItemSelected += ScrollPnl_OnItemSelected;

            btnExtAllow.Tag = EExtList.Allow;
            btnExtCrypt.Tag = EExtList.Crypt;
            btnExtPriority.Tag = EExtList.Priority;

            _tabs.Add(btnExtAllow);
            _tabs.Add(btnExtCrypt);
            _tabs.Add(btnExtPriority);

            SelectTab(EExtList.Priority);
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
            foreach (RoundedControl rCtrl in _tabs)
            {
                if(rCtrl.IsActiv)
                {
                    // to save
                    ExecuteTabCommand((EExtList)rCtrl.Tag);
                }

                rCtrl.IsActiv = extList == (EExtList)rCtrl.Tag;
            }

            scrollPnl.Clear();

            switch (extList)
            {
                case EExtList.Priority:
                    FillScrollPanel(ViewModel.DataModel.AppInfo.PriorityExt);
                    break;

                case EExtList.Crypt:
                    FillScrollPanel(ViewModel.DataModel.CryptInfo.AllowEtx);
                    break;

                case EExtList.Allow:
                    FillScrollPanel(ViewModel.DataModel.AppInfo.AllowExt);
                    break;

                default:
                    break;
            }

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

                default:
                    return;
                    break;
            }
        }

        private void FillScrollPanel(string[] extList)
        {
            if (extList == null || extList.Length == 0)
                return;

            foreach (string str in extList)
            {
                RoundedTexteBox roundedTexteBox = new RoundedTexteBox();
                roundedTexteBox.Height = HEIGHT_TB;
                roundedTexteBox.Text = str;
                scrollPnl.Add(roundedTexteBox);
            }
        }

        private string[] GetStrings()
        {
            List<string> output = new List<string>();
            foreach (var item in scrollPnl.Controls)
            {
                output.Add((item as RoundedTexteBox).Text);
            }
            return output.ToArray();
        }

        #endregion
    }
}
