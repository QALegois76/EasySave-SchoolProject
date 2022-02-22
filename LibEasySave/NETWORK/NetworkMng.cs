using LibEasySave.AppInfo;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace LibEasySave.Network
{
    public class NetworkMng : ObservableObject
    {
        // private items for singleton
        private const int PORT = 8080;
        private static NetworkMng instance;
        private static readonly object _lockobj = new object();


        // private members
        private bool _isListening = true;

        private string _hostNameIp = "127.0.0.1";

        private IPEndPoint _ipServer;
        private TcpListener _tcpListener;
        private Thread _threadTcpListener;
        private NetworkInterpreter _networkInterpreter;

        private TcpClient _tcpClient = new TcpClient();
        private List<TcpClient> _clients = new List<TcpClient>();


        public bool IsConnected => _tcpClient.Connected;
        public string HostNameIP { get => _hostNameIp; set { _hostNameIp = value; PropChanged(nameof(HostNameIP)); } }


        // public atrribute
        public static NetworkMng Instance
        {
            get
            {
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


        // constructor
        private NetworkMng() 
        {
            _ipServer = new IPEndPoint(IPAddress.Any, PORT);
            _tcpListener = new TcpListener(_ipServer);
            _threadTcpListener = new Thread(ClientAccepter);
        }




        // public method
        public void Init(IModelViewJob modelViewJob, IViewDataModel viewDataModel)
        {
            _networkInterpreter = new NetworkInterpreter(modelViewJob, viewDataModel);
        }


        public void Start()
        {
            if (DataModel.Instance.AppInfo.ModeIHM == EModeIHM.Server)
            {
                _threadTcpListener.Start();
                _isListening = true;
            }
            else
            {
                _tcpClient.Connect(_hostNameIp, PORT);
            }
        }

        public void Stop()
        {
            lock(_lockobj)
            {
                _clients.Clear();
                _isListening = false;
            }
        }
        
        public void SendNetworkCommad(ENetorkCommand networkCommand, object parameter)
        {
            NetworkInfo info = new NetworkInfo(networkCommand, parameter);
            if (DataModel.Instance.AppInfo.ModeIHM == EModeIHM.Server)
            {
                foreach (TcpClient client in _clients)
                {
                    SendThrowNetwork(client, info);
                }
            }
            else
            {
                SendThrowNetwork(_tcpClient, info);
                // send throw network
            }
        }




        // private method

        private void ClientAccepter()
        {
            try
            {
                _tcpListener.Start();
                while (_isListening)
                {
                    var temp = _tcpListener.AcceptTcpClient();
                    _clients.Add(temp);

                    ThreadPool.QueueUserWorkItem(new WaitCallback(ListenClient), temp);
                }
            }
            catch (Exception)
            {
                return;
            }
        }

        private void SendThrowNetwork(TcpClient client, NetworkInfo networkInfo)
        {
            try
            {
                string message = (new JSONText()).GetFormatingText(networkInfo);

                byte[] sendData = new byte[Encoding.UTF8.GetByteCount(message)];
                sendData = Encoding.UTF8.GetBytes(message);
                client.GetStream().Write(sendData);
            }
            catch (Exception)
            {
                return;
            }
        }

        private void ListenClient(object client)
        {
            TcpClient tcpClient = (TcpClient)client;
            try
            {
                while (tcpClient.Connected)
                {
                    byte[] stringBytes = new byte[tcpClient.ReceiveBufferSize];

                    tcpClient.GetStream().Read(stringBytes);
                    string jsonString = Encoding.UTF8.GetString(stringBytes);

                    NetworkInfo networkInfo = JSONDeserializer<NetworkInfo>.Deserialize(jsonString); 
                }
            }
            catch (Exception)
            {
                try
                {
                    tcpClient?.Close();
                }
                catch(Exception)
                {
                }

                if (_clients.Contains(tcpClient))
                    _clients.Remove(tcpClient);


                return;
            }
        }





    }
}

