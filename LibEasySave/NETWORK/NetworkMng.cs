using LibEasySave.AppInfo;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace LibEasySave.Network
{
    public class NetworkMng : ObservableObject
    {
        // private items for singleton
        private static event InternEventHandler OnClientListened;

        private const int PORT = 8080;
        private static NetworkMng instance;
        private static readonly object _lockobj = new object();
        private ManualResetEvent _mre = new ManualResetEvent(true);



        // private members
        public event NotifyCollectionChangedEventHandler Collectionchanged;

        private bool _isListening = true;

        private string _hostNameIp = "127.0.0.1";

        private IPEndPoint _ipServer;
        private TcpListener _tcpListener;
        private Thread _threadTcpListener;
        private NetworkInterpreter _networkInterpreter;

        private TcpClient _tcpClient = new TcpClient();
        private ObservableCollection<TcpClient> _clients = new ObservableCollection<TcpClient>();


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

            _clients.CollectionChanged -= Clients_CollectionChanged;
            _clients.CollectionChanged += Clients_CollectionChanged;

            OnClientListened -= NetworkMng_OnClientListened;
            OnClientListened += NetworkMng_OnClientListened;
        }

        private void NetworkMng_OnClientListened(object sender, InternEventArgs netInfoEventArg)
        {
            _networkInterpreter.Interprete(netInfoEventArg.NetInfo);
        }

        private void Clients_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            _mre.Set();
            Collectionchanged?.Invoke(this, e);
            _mre.Reset();
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
                if (_threadTcpListener.ThreadState == ThreadState.Unstarted)
                    _threadTcpListener.Start();
                else
                    _threadTcpListener.Join();
                _isListening = true;
            }
            else
            {
                try
                {
                    _tcpClient.Connect(_hostNameIp, PORT);

                }
                catch (Exception)
                {

                }
                PropChanged(nameof(IsConnected));
            }
        }

        public void Stop()
        {
            lock (_lockobj)
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

                    Thread t = new Thread( new ParameterizedThreadStart(ListenClient));
                    t.SetApartmentState(ApartmentState.STA);
                    t.Start(temp);
                }
            }
            catch (Exception ex)
            {
                return;
            }
        }

        private void SendThrowNetwork(TcpClient client, NetworkInfo networkInfo)
        {
            try
            {
                string message = (new JSONText()).GetFormatingText(networkInfo,true);

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
            while (tcpClient != null && tcpClient.Connected)
            {
                try
                {
                    byte[] stringBytes = new byte[tcpClient.ReceiveBufferSize];

                    tcpClient.GetStream().Read(stringBytes);
                    string jsonString = Encoding.UTF8.GetString(stringBytes);

                    NetworkInfo networkInfo = JSONDeserializer<NetworkInfo>.Deserialize(jsonString);

                    if (networkInfo != null)
                        OnClientListened?.Invoke(this, new InternEventArgs(networkInfo));

                            
                           


                }
                catch (Exception ex)
                {
                    if (!tcpClient.Connected)
                        tcpClient = null;
                    Console.WriteLine(ex.Message);
                }
            }
        }

        private delegate void InternEventHandler(object sender, InternEventArgs netInfoEventArg);

        private class InternEventArgs : EventArgs
        {
            NetworkInfo _netInfo;

            public NetworkInfo NetInfo { get => _netInfo; }

            public InternEventArgs(NetworkInfo netInfo)
            {
                _netInfo = netInfo;
            }
        }





    }
}

