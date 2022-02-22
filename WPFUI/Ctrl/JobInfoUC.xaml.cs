using LibEasySave;
using LibEasySave.Model.LogMng.Interface;
using LibEasySave.TranslaterSystem;
using Microsoft.Win32;
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
using WinFormApp;
//using System.Windows.Forms;

namespace WPFUI.Ctrl
{
    /// <summary>
    /// Interaction logic for JobEdit.xaml
    /// </summary>
    public partial class JobInfoUC : UserControl , INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;


        internal ITranslatedText TranslatedText => Translater.Instance.TranslatedText;



        public JobInfoUC()
        {
            DataContext = this;
            InitializeComponent();
        }

    }
}
