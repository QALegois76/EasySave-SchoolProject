using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace LibEasySave.NETWORK
{

    [Serializable]
    public class NetworkInfo<T> where T : class, new()
    {
        [JsonProperty]
        T _obj;

        public NetworkInfo()
        {
        }

        public T Instance()
        {
            if (_obj == null)
                _obj = new T();

            return this._obj;
        }





    }

    
}
