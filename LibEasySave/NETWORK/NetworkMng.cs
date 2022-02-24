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
    public class NetworkMng:ObservableObject
    {
        // private items for singleton
        public event AddingClientEventHandler OnAddingClient;
        public event GuidSenderEventHandler OnRemovingClient;
        public event EventHandler<bool> OnLockClient;

        private const int PORT = 8080;
        private static NetworkMng instance;
        private static readonly object _lockobj = new object();
        private ManualResetEvent _mre = new ManualResetEvent(true);



        // private members

        private bool _isListening = false;

        private string _hostNameIp = "127.0.0.1";

        private Guid? _guidSelectedClient = null;

        private IPEndPoint _ipServer;
        private TcpListener _tcpListener;
        private Thread _threadTcpListener;
        private Thread _threadCurrentClient;
        private Thread _threadCheckerClient;
        private NetworkInterpreter _networkInterpreter;
        private TcpClient _tcpClient = new TcpClient();

        private Dictionary<Guid,TcpClient> _clients = new Dictionary<Guid, TcpClient>();


        public bool IsConnected => _tcpClient.Connected;
        public bool IsListening => _isListening;
        public string HostNameIP { get => _hostNameIp; set { _hostNameIp = value; PropChanged(nameof(HostNameIP)); } }
        public Guid? SelectedGuidClient { get => _guidSelectedClient; set => SetSelectedClient( value); }


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
            _threadCheckerClient = new Thread(ClientChecker);
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
                {
                    _threadTcpListener.Start();
                    _threadCheckerClient.Start();
                }
                else
                {
                    _threadTcpListener.Join();
                    _threadCheckerClient.Join();
                }
                _isListening = true;
            }
            else
            {
                try
                {
                    _tcpClient.Connect(_hostNameIp, PORT);
                    _threadCurrentClient = new Thread(new ParameterizedThreadStart(ListenClient));
                    _threadCurrentClient.Start(_tcpClient);

                }
                catch (Exception ex)
                {

                }
            }
            PropChanged(nameof(IsConnected));
            PropChanged(nameof(IsListening));
        }

        public void Stop()
        {
            lock (_lockobj)
            {
                _clients.Clear();
                _isListening = false;
            }
        }

        public void FireLockUI(bool state) => OnLockClient?.Invoke(this, state);

        public void SendNetworkCommad(ENetorkCommand networkCommand, object parameter)
        {

            NetworkInfo info = new NetworkInfo(networkCommand, parameter);
            if (DataModel.Instance.AppInfo.ModeIHM == EModeIHM.Server)
            {
                if (_guidSelectedClient.HasValue)
                    SendThrowNetwork(_clients[_guidSelectedClient.Value], info);
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
                    Guid g = Guid.NewGuid();
                    _clients.Add(g,temp);
                    OnAddingClient?.Invoke(this, new AddingClientEventArgs(g, temp.Client.RemoteEndPoint.ToString()));
                }
            }
            catch (Exception ex)
            {
                _isListening = false;
                PropChanged(nameof(IsListening));
                return;
            }
        }

        private void ClientChecker()
        {

            while (_clients.Count > 0 && _isListening)
            {

                lock (_lockobj)
                {
                    try
                    {
                        List<Guid> toRm = new List<Guid>();
                        foreach (var item in _clients)
                        {
                            if (item.Value == null || !item.Value.Connected)
                                toRm.Add(item.Key);
                        }

                        if (toRm.Count > 0)
                            foreach (Guid g in toRm)
                            {
                                _clients.Remove(g);
                                OnRemovingClient?.Invoke(this, new GuidSenderEventArg( g));
                            }
                    }
                    catch (Exception ex)
                    {
                        return;
                    }

                }
                Thread.Sleep(2000);
            }
        }


        private void SendThrowNetwork(TcpClient client, NetworkInfo networkInfo)
        {
            try
            {
                if (!client.Connected)
                {
                    client = null;
                    return;
                }



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
                        _networkInterpreter.Interprete(networkInfo);
                            
                }
                catch (Exception ex)
                {
                    if (!tcpClient.Connected)
                        tcpClient = null;
                    Console.WriteLine(ex.Message);
                }
            }
        }

        private void SetSelectedClient(Guid? guid)
        {
            if (_guidSelectedClient == guid.Value)
                return;

            _guidSelectedClient = guid;


            if (guid.HasValue)
            {
 


                if (_threadCurrentClient != null && _threadCurrentClient.ThreadState == ThreadState.Running)
                {
                    _threadCurrentClient.Interrupt();
                }

                _threadCurrentClient = new Thread(new ParameterizedThreadStart(ListenClient));
                _threadCurrentClient.Start(_clients[guid.Value]);

                SendNetworkCommad(ENetorkCommand.GetJobList, null);
                SendNetworkCommad(ENetorkCommand.GetDataModel, null);

            }
            else
            {
                _threadCurrentClient.Abort();
            }
        }

        public void Disconnect(Guid g)
        {
            if (!_clients.ContainsKey(g))
                return;

                _clients[g].Close();
        }

        public void LockClient(Guid g, bool state)
        {
            if (!_clients.ContainsKey(g))
                return;
            NetworkInfo netInfo = new NetworkInfo(ENetorkCommand.LockUIClient, state);
            SendThrowNetwork(_clients[g], netInfo);
        }

    }




    public delegate void AddingClientEventHandler(object sender, AddingClientEventArgs e);
    public class AddingClientEventArgs : EventArgs
    {
        private string _ip;
        private Guid _guid;

        public string IPClient => _ip;
        public Guid Guid => _guid;

        public AddingClientEventArgs(Guid guid, string ip)
        {
            _guid = guid;
            _ip = ip;
        }
    }

    
}

