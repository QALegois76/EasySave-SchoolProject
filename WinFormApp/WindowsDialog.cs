using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace WinFormApp
{
    public enum EResultDialog
    {
        Ok,
        Cancel,
        Unkown
    }

    public class WinDialogInfo
    {
        private string pathSelected = null;
        private EResultDialog resultDialog = EResultDialog.Unkown;


        public WinDialogInfo(string pathSelected, EResultDialog resultDialog)
        {
            this.PathSelected = pathSelected;
            this.ResultDialog = resultDialog;
        }

        public string PathSelected { get => pathSelected; internal set => pathSelected = value; }
        public EResultDialog ResultDialog { get => resultDialog;internal set => resultDialog = value; }
    }

    public static class WindowsDialog
    {
        public static WinDialogInfo SearchFolder(string path = null)
        {
            FolderBrowserDialog _folderDialog = new FolderBrowserDialog();
            _folderDialog.SelectedPath = path;
            if (_folderDialog.ShowDialog() == DialogResult.OK)
            {
                return new WinDialogInfo(_folderDialog.SelectedPath, EResultDialog.Ok);
            }
            else
                return new WinDialogInfo(_folderDialog.SelectedPath, EResultDialog.Cancel);
        }
    }
}
