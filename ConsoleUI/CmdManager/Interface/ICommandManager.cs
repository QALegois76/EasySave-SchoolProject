using System;
using System.Collections.Generic;
using System.Text;

namespace LibEasySave
{
     public interface ICommandManager
    {
        event CommandEventHandler OnAddingJob;
        event CommandEventHandler OnRemovingJob;
        event CommandEventHandler OnRenameJob;
        event CommandEventHandler OnModifyingJob;
        event CommandEventHandler OnSetingRepSrc;
        event CommandEventHandler OnSetingRepDest;
        event CommandEventHandler OnSetingSavingMode;
        event CommandEventHandler OnGetingRepSrc;
        event CommandEventHandler OnGetingRepDest;
        event CommandEventHandler OnGetingSavingMode;
        event CommandEventHandler OnGetingAllName;
        event CommandEventHandler OnCanceling;
        event CommandEventHandler OnRuning;
    }

    public delegate bool CommandEventHandler(object sender, CommandEventArgs e);

    public class CommandEventArgs : EventArgs
    {
        private string _param = null;

        public string Param => _param;

        public CommandEventArgs(string param)
        {
            _param = param;
        }
    }
}
