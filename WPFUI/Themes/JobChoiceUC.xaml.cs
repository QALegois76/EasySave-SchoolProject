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

namespace WPFUI.Themes
{
    /// <summary>
    /// Interaction logic for JobChoiceUC.xaml
    /// </summary>
    public partial class JobChoiceUC : UserControl , IClickable , IActivable
    {
        public bool IsActiv { get => backPnl.IsActiv; set => backPnl.IsActiv = value; }
        public bool IsSelected { get => cBtn.IsActiv; set => cBtn.IsActiv = value; }

        public event EventHandler OnClick { add => backPnl.OnClick += value; remove => backPnl.OnClick -= value; }
        public event EventHandler OnActivStateChanged { add => backPnl.OnActivStateChanged += value; remove => backPnl.OnActivStateChanged -= value; }

        public string Title { get => backPnl.Text; set => backPnl.Text = value; }

        public JobChoiceUC(string name = "jobName")
        {
            InitializeComponent();
            Title = name;
        }

    }
}
