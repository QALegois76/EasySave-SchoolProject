using System;
using System.Collections.Generic;
using System.Text;
using LibEasySave.Model.LogMng.Interface;
using Newtonsoft;
using Newtonsoft.Json;

namespace LibEasySave
{
    public class JSONText : LogBaseSaver
    {
        public override string GetFormatingText(object log)
        {
            var jsonSerializerSettings = new JsonSerializerSettings();
            jsonSerializerSettings.Converters.Add(new Newtonsoft.Json.Converters.StringEnumConverter());

            return JsonConvert.SerializeObject(log, Formatting.Indented, jsonSerializerSettings);
        }
    }
}
