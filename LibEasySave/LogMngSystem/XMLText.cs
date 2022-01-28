using LibEasySave.Model.LogMng.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace LibEasySave.Model
{
    class XMLText : LogBaseSaver
    {

        public override string GetSavedLogText(ILog log)
        {
            throw new NotImplementedException();
        }

        public override string GetSavedStateText(IStateLog state)
        {
            throw new NotImplementedException();
        }

        public override string GetSavedLogText(IDailyLog dailyLog)
        {
            throw new NotImplementedException();
        }
    }
}
