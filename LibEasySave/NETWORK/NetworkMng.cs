using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace LibEasySave.NETWORK
{
    public class NetworkMng
    {

        private static NetworkMng instance;
        private static readonly object _lockobj = new object();
        private int _port;
        private string _ipAddress;


        private NetworkMng() { }

        public static NetworkMng Instance
        {
            get {
                lock (_lockobj)
                {
                    if (instance == null)
                    {
                        instance = new NetworkMng();
                    }
                }
                return instance;
            }
        }

        public void ConnectClientToServer(NetworkInfo<object> networkInfo)
        {
            if (networkInfo == null)
            {
                throw new Exception("object is null");
            }

            WaitCallback callback = new WaitCallback(Task);
            ThreadPool.QueueUserWorkItem(callback, networkInfo);

        }

        private void Task(object obj)
        {
            NetworkInfo<object> networkInfo = (NetworkInfo<object>)obj;
            Client client = new Client(_ipAddress, _port);
            Server server = new Server(_port);
            JsonTools<NetworkInfo<object>> json = new JsonTools<NetworkInfo<object>>(networkInfo);
            Server.ReceiveObject();
            client.SendObject(json.Serialize());
        }

            
        }

    }
}
