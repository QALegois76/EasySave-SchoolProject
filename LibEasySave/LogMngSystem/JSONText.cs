using System;
using System.Collections.Generic;
using System.Text;
using LibEasySave.Model.LogMng.Interface;
using Newtonsoft;
using Newtonsoft.Json;

namespace LibEasySave.Model
{
    public class JSONText : LogBaseSaver
    {

        public override string GetSavedLogText(ILog log)
        {
            return JsonConvert.SerializeObject(log);
        }

        public override string GetSavedLogText(IDailyLog log)
        {
            return JsonConvert.SerializeObject(log);
        }

        public override string GetSavedStateText(IStateLog state)
        {
            return JsonConvert.SerializeObject(state);
        }
    }
}
