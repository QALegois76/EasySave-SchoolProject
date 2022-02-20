using Antlr4.Runtime.Misc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace LibEasySave.NETWORK
{
    public class JsonTools<NetworkInfo>
    {

        private NetworkInfo _obj;

        public JsonTools(NetworkInfo obj)
        {

            if (obj == null)
                throw new Exception("NetworkInfo must be initialize !");

            this._obj = obj;
        }

        //public T Instance()
        //{
        //    if (_obj == null)
        //        _obj = new T();

        //    return this._obj;
        //}

        public NetworkInfo<object> Deserialize(string fileName)
        {
            try
            {

                string jsonString = File.ReadAllText(fileName);

                NetworkInfo<object> networkInfo = JsonConvert.DeserializeObject<NetworkInfo<object>>(jsonString);


                return networkInfo;
            } 
            catch (Exception ex)
            {
                throw new Exception("Deserialize does'nt works !");
            }
        }

        public string Serialize()
        {
            try
            {

                var jsonSerializerSettings = new JsonSerializerSettings();
                jsonSerializerSettings.Converters.Add(new Newtonsoft.Json.Converters.StringEnumConverter());

                return JsonConvert.SerializeObject(this._obj, Formatting.Indented, jsonSerializerSettings);
            }
            catch (Exception ex)
            {
                throw new Exception("Serialize doesn't works !");
            }
        }
    }
}
