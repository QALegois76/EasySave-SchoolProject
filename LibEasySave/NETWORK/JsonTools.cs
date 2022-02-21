using Antlr4.Runtime.Misc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace LibEasySave.NETWORK
{
    public class JsonTools : JSONText
    {

        private NetworkInfo _obj;

        public JsonTools() { }

        //public T Instance()
        //{
        //    if (_obj == null)
        //        _obj = new T();

        //    return this._obj;
        //}

        public NetworkInfo Deserialize(string fileName)
        {
            try
            {

                string jsonString = File.ReadAllText(fileName);

                NetworkInfo networkInfo = new NetworkInfo();
                networkInfo = JsonConvert.DeserializeObject<NetworkInfo>(jsonString);


                return networkInfo;
            } 
            catch (Exception ex)
            {
                throw new Exception("Deserialize does'nt works !");
            }
        }

        //public string Serialize()
        //{
        //    try
        //    {

        //        var jsonSerializerSettings = new JsonSerializerSettings();
        //        jsonSerializerSettings.Converters.Add(new Newtonsoft.Json.Converters.StringEnumConverter());

        //        return JsonConvert.SerializeObject(this._obj, Formatting.Indented, jsonSerializerSettings);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception("Serialize doesn't works !");
        //    }
        //}
    }
}
