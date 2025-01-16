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
using System.Windows.Navigation;
using System.Windows.Shapes;
using WPFUI.Themes;

namespace WPFUI.Ctrl
{
    /// <summary>
    /// Interaction logic for NetworkClientUC.xaml
    /// </summary>
    public partial class NetworkClientUC : UserControl , IClickable , IActivable, INotifyPropertyChanged
    {

        private readonly ImageSource _lockImg;
        private readonly ImageSource _unlockImg;
        //public static DependencyProperty LockImageDepency = DependencyProperty.RegisterAttached(nameof(LockImg), typeof(ImageSource), typeof(NetworkClientUC));

        public event EventHandler OnClick;


        public event EventHandler OnActivStateChanged;
        public event PropertyChangedEventHandler PropertyChanged;

        public event EventHandler OnSettingClient;// { add => rbtn_setting.OnClick += value; remove => rbtn_setting.OnClick -= value; }
        public event EventHandler OnRefreshClient;// { add => rbtn_Refresh.OnClick += value; remove => rbtn_Refresh.OnClick -= value; }
        public event EventHandler OnLockUIClient;// { add => rbtn_lockUI.OnClick += value; remove => rbtn_lockUI.OnClick -= value; }
        public event EventHandler OnCloseClient;// { add => rbtn_closeClient.OnClick += value; remove => rbtn_closeClient.OnClick -= value; }

        public bool IsActiv { get => rCtrl_back.IsActiv; set => rCtrl_back.IsActiv = value; }
        public bool IsLock { get => rbtn_lockUI.IsActiv; set => rbtn_lockUI.IsActiv = value; }
        public bool IsAutoCheck { get => rCtrl_back.IsAutoCheck; set => rCtrl_back.IsAutoCheck = value; }
        
        public string IPClient { get => lbClient.Text; set { lbClient.Text = value; } }

        public ImageSource LockImg =>(rbtn_lockUI.IsActiv ? _lockImg : _unlockImg);
        //(rbtn_lockUI.IsActiv ? App.Current.Resources["ImgLock"] : App.Current.Resources["ImgUnlock"]);


        // constructor
        public NetworkClientUC()
        {
            DataContext = this;
            InitializeComponent();

            _lockImg = (ImageSource)App.Current.Resources["ImgLock"];
            _unlockImg = (ImageSource)App.Current.Resources["ImgUnlock"];

            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(LockImg)));


            rbtn_closeClient.OnClick -= Rbtn_closeClient_OnClick;
            rbtn_closeClient.OnClick += Rbtn_closeClient_OnClick;

            //rbtn_lockUI.OnClick -= Rbtn_lockUI_OnClick;
            //rbtn_lockUI.OnClick += Rbtn_lockUI_OnClick;

            rbtn_Refresh.OnClick -= Rbtn_Refresh_OnClick;
            rbtn_Refresh.OnClick += Rbtn_Refresh_OnClick;

            rbtn_setting.OnClick -= Rbtn_setting_OnClick;
            rbtn_setting.OnClick += Rbtn_setting_OnClick;

            rbtn_lockUI.OnActivStateChanged -= Rbtn_lockUI_OnActivStateChanged;
            rbtn_lockUI.OnActivStateChanged += Rbtn_lockUI_OnActivStateChanged;
        }

        private void Rbtn_lockUI_OnActivStateChanged(object sender, EventArgs e)
        {
            OnClick?.Invoke(this, e);
            //SetValue(LockImageDepency, rbtn_lockUI.IsActiv ? App.Current.FindResource("ImgLock") : App.Current.FindResource("ImgUnlock"));
            //LockImg= (Image)(rbtn_lockUI.IsActiv ? App.Current.FindResource("ImgLock") : App.Current.FindResource("ImgUnlock"));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(LockImg)));
            OnLockUIClient?.Invoke(this, e);
        }

        private void Rbtn_setting_OnClick(object sender, EventArgs e) 
        {
            OnClick?.Invoke(this, e);
            OnSettingClient?.Invoke(this, e);
        }

        private void Rbtn_Refresh_OnClick(object sender, EventArgs e)
        {
            OnClick?.Invoke(this, e);
            OnRefreshClient?.Invoke(this, e);
        }

        private void Rbtn_lockUI_OnClick(object sender, EventArgs e)
        {
            OnClick?.Invoke(this, e);
            OnLockUIClient?.Invoke(this, e);
        }

        private void Rbtn_closeClient_OnClick(object sender, EventArgs e)
        {
            OnCloseClient?.Invoke(this, e);
        }




        #region not used
        private void OnOver(object sender, MouseEventArgs e)
        {

        }

        private void OnLeave(object sender, MouseEventArgs e)
        {

        }

        private void OnDown(object sender, MouseEventArgs e)
        {

        }

        private void OnUp(object sender, MouseEventArgs e)
        {

        }

        #endregion
        private void OnClickItem(object sender, EventArgs e) => OnClick?.Invoke(this, e);



    }
}
