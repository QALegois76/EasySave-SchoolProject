using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace LibEasySave.NETWORK
{
    public class Server
    {

        private readonly int _port;

        private static TcpListener listener;


        public Server(int port)
        {

            this._port = port;
            listener = new TcpListener(System.Net.IPAddress.Any, this._port);

        }
        //TcpListener listener = new TcpListener(System.Net.IPAddress.Any, 1302);
        public static void ReceiveObject()
        {
            Task.Factory.StartNew(() =>
            {
                listener.Start();

                while (true)
                {
                    Console.WriteLine("Waiting for a connection...");
                    TcpClient client = listener.AcceptTcpClient();
                    Console.WriteLine("Client accepted.");

                    NetworkStream stream = client.GetStream();
                    StreamReader reader = new StreamReader(stream);
                    StreamWriter writer = new StreamWriter(stream);

                    try
                    {
                        byte[] buffer = new byte[1024];
                        stream.Read(buffer, 0, buffer.Length);
                        int byteUsed = 0;
                        foreach (byte b in buffer)
                        {
                            if (b != 0)
                            {
                                byteUsed++;
                            }
                        }

                        string request = Encoding.UTF8.GetString(buffer, 0, byteUsed);
                        Console.WriteLine("request received : " + request);
                        writer.WriteLine("Success.");
                        Console.WriteLine("size of buffer : " + buffer.Length);
                        Console.WriteLine("bytes used : " + byteUsed);
                        writer.Flush();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Something went wrong.");
                        writer.WriteLine(ex.ToString());

                    }
                }
            });
        }
    }
}

  
