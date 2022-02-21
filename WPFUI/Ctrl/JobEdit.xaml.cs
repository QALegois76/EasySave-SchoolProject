using LibEasySave;
using LibEasySave.TranslaterSystem;
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

        internal ITranslatedText TranslatedText => Translater.Instance.TranslatedText;

        private bool _initilized = false;

        private IJob _job;

        public JobEdit()
        {
            _initilized = false;
            InitializeComponent();
            btnDiff.Tag = ESavingMode.DIFF;
            btnFull.Tag = ESavingMode.FULL;
            _initilized = true;
            InvalidateVisual();
        }

        private void SrcPath_OnClick(object sender, EventArgs e)
        {
            WinDialogInfo info =  WindowsDialog.SearchFolder(this.tbSrcPath.Text);

            if (info.ResultDialog == EResultDialog.Ok)
            {
                this.tbSrcPath.Text = info.PathSelected;
                _job.SourceFolder = info.PathSelected;
            }
        }

        private void DestPath_OnClick_1(object sender, EventArgs e)
        {
            WinDialogInfo info = WindowsDialog.SearchFolder(this.tbDestPath.Text);

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
            if (job != null)
            {
                _initilized = false;
                _job = job;

                tbJobName.Text = _job.Name;
                tbSrcPath.Text = _job.SourceFolder;
                tbDestPath.Text = _job.DestinationFolder;

                btnDiff.IsActiv = job.SavingMode == ESavingMode.DIFF;
                btnFull.IsActiv = job.SavingMode == ESavingMode.FULL;


                cBtnCrypt.IsActiv = job.IsEncrypt;

                _initilized = true;
            }
            else
            {
                _initilized = false;

                tbJobName.Text = "";
                tbSrcPath.Text = "";
                tbDestPath.Text = "";

                btnDiff.IsActiv = false;
                btnFull.IsActiv = false;

                cBtnCrypt.IsActiv = false;

                _initilized = true;
            }

        }

        private void tbJobName_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!_initilized)
                return;

            _job.Name = tbJobName.Text;
            JobNameChanged?.Invoke(this, null);
        }

        private void cBtnCrypt_OnActivStateChanged(object sender, EventArgs e)
        {
            if (_job == null || !_initilized)
                return;

            _job.IsEncrypt = cBtnCrypt.IsActiv;
        }

    }
}
