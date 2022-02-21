using System;
using System.Collections.Generic;
using System.Text;

namespace LibEasySave.NETWORK
{
    class NetworkInterpreter
    {

        NetworkInfo _networkInfo;
        NetworkInfo _obj;

        public NetworkInterpreter(NetworkInfo networkInfo)
        {
            if (networkInfo == null)
                throw new Exception("networkInfo is null !");

            this._networkInfo = networkInfo;
            _obj = networkInfo.Instance;
        }



    }
}
