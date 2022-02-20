using System;
using System.Collections.Generic;
using System.Text;

namespace LibEasySave.NETWORK
{
    class NetworkInterpreter<T> where T : class, new()
    {

        NetworkInfo<T> _networkInfo;
        T _obj;

        public NetworkInterpreter(NetworkInfo<T> networkInfo)
        {
            if (networkInfo == null)
                throw new Exception("networkInfo is null !");

            this._networkInfo = networkInfo;
            _obj = networkInfo.Instance();
        }



    }
}
