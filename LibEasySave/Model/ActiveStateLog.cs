using System;
using System.Collections.Generic;
using System.Text;
using LibEasySave.Model.LogMng.Interface;

namespace LibEasySave
{
    class ActiveStateLog : LogMng.StateLog
    {
        public int NbTotalFiles;
        public int SizeTotalFiles;
        public ProgressJob ProgressJob;

    }
}
