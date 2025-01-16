using Antlr4.Runtime.Misc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace LibEasySave.Network
{
    public class JSONDeserializer<T> : JSONText where T : class
    {
        public JSONDeserializer() { }

        public static T Deserialize(string fileContent)
        {
            try
            {
                return JsonConvert.DeserializeObject<T>(fileContent);
            } 
            catch (Exception ex)
            {
                return null;
                throw new Exception("Deserialize does'nt works !");
            }
        }
    }
}
