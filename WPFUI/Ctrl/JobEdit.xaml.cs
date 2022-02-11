using LibEasySave;
using Microsoft.Win32;
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
//using System.Windows.Forms;

namespace WPFUI.Ctrl
{
    /// <summary>
    /// Interaction logic for JobEdit.xaml
    /// </summary>
    public partial class JobEdit : UserControl
    {
        public event TextChangedEventHandler JobNameChanged;

        private IJob _job;

        public JobEdit()
        {
            InitializeComponent();
            btnDiff.Tag = ESavingMode.DIFF;
            btnFull.Tag = ESavingMode.FULL;
        }

        private void SrcPath_OnClick(object sender, EventArgs e)
        {
            WinDialogInfo info =  WindowsDialog.Instance.SearchFolder(this.tbSrcPath.Text);

            if (info.ResultDialog == EResultDialog.Ok)
            {
                this.tbSrcPath.Text = info.PathSelected;
                _job.SourceFolder = info.PathSelected;
            }
        }

        private void DestPath_OnClick_1(object sender, EventArgs e)
        {
            WinDialogInfo info = WindowsDialog.Instance.SearchFolder(this.tbDestPath.Text);

            if (info.ResultDialog == EResultDialog.Ok)
            {
                this.tbDestPath.Text = info.PathSelected;
                _job.DestinationFolder = info.PathSelected;
            }
        }

        private void ChoseSavingMode(object sender, EventArgs e)
        {
            if (!((sender as Control).Tag is ESavingMode))
                return;
            btnDiff.IsActiv = false;
            btnFull.IsActiv = false;

            switch ((ESavingMode)(sender as Control).Tag)
            {
                case ESavingMode.FULL:
                    btnFull.IsActiv = true;
                    _job.SavingMode = ESavingMode.FULL;
                    break;

                case ESavingMode.DIFF:
                    _job.SavingMode = ESavingMode.DIFF;
                    btnDiff.IsActiv = true;
                    break;

                default:
                    throw new Exception("error saving mode not find");
                    break;
            }
        }

        public void SetJob(IJob job)
        {
            _job = job;

            tbJobName.Text = _job.Name;
            tbSrcPath.Text = _job.SourceFolder;
            tbDestPath.Text = _job.DestinationFolder;

            btnDiff.IsActiv = job.SavingMode == ESavingMode.DIFF;
            btnFull.IsActiv = job.SavingMode == ESavingMode.FULL;

            //cBtnCrypt.IsActiv = job.MustBeCrypt;
        }

        private void tbJobName_TextChanged(object sender, TextChangedEventArgs e)
        {
            _job.Name = tbJobName.Text;
            JobNameChanged.Invoke(this, new TextChangedEventArgs(null,UndoAction.None));
        }
    }
}
