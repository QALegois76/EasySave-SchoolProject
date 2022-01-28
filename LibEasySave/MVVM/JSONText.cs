using System;
using System.Collections.Generic;
using System.Text;
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

        public override string GetSavedStateText(IState state)
        {
            return JsonConvert.SerializeObject(state);
        }
    }
}
