using System;
using System.Collections.Generic;
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
using WinFormApp;
using WPFUI.Themes;

namespace WPFUI.Ctrl
{
    /// <summary>
    /// Interaction logic for AppJobChoiceUC.xaml
    /// </summary>
    public partial class AppJobChoiceUC : UserControl, IClickable , IActivable
    {
        private const string FILTER = "application (*.exe)|*.exe";

        public event EventHandler OnClick;
        public event EventHandler OnActivStateChanged;

        private string _path = "";

        public string TextTitle { get => rCtrl_back.Text; }
        public string AppPath { get => _path; set => SetTitle(value); }


        public bool IsActiv { get => rCtrl_back.IsActiv; set { rCtrl_back.IsActiv = value; btnOpenFile.IsActiv = value; } }
        public bool IsAutoCheck { get => rCtrl_back.IsAutoCheck; set { rCtrl_back.IsAutoCheck = value; btnOpenFile.IsAutoCheck = value; } }


        // constructor
        public AppJobChoiceUC()
        {
            InitializeComponent();
        }

        private void Back_OnClick(object sender, EventArgs e)=>  OnClick?.Invoke(this, e);
        

        private void BtnOpenExe(object sender, EventArgs e)
        {
            var result = WindowsDialog.OpenFile(_path, FILTER);
            if (result.ResultDialog == EResultDialog.Ok)
                SetTitle(result.PathSelected);
        }

        private void SetTitle(string value)
        {
            _path = value;
            rCtrl_back.Text = System.IO.Path.GetFileName(_path);
        }


    }
}
