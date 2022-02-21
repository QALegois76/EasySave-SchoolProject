using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace LibEasySave.NETWORK
{

    [Serializable]
    public class NetworkInfo
    {
        [JsonProperty]
        NetworkInfo _obj;

        public NetworkInfo()
        {
        }

        public NetworkInfo Instance
        {
            get
            {
                {
                    if (_obj == null)
                        _obj = new NetworkInfo();

                    return this._obj;
                }
            }
            set
            {
                this._obj = value;
            }
        }
    }
}


