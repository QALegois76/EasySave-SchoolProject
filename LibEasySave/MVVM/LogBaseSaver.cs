using System;
using System.Collections.Generic;
using System.Text;

namespace LibEasySave.Model
{
    public abstract class LogBaseSaver
    {
        public LogBaseSaver()
        {
        }

        public abstract string GetSavedLogText(ILog log);

        public abstract string GetSavedStateText(IState state);

    }
}
