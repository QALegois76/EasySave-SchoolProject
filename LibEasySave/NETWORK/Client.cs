using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace LibEasySave.Network
{
    public class Client
    {
        private readonly string _ipAddress;
        private readonly int _port;

        private TcpClient client;

        public Client(string ipAddress, int port)
        {

            if (ipAddress == null)
            {
                throw new Exception("Address is null");
            }

            this._ipAddress = ipAddress;
            this._port = port;
            this.client = new TcpClient(this._ipAddress, this._port);

            SendObject("oerogiuê");
        }


        public void SendObject(string obj)
        {
        connection:
            try
            {
                String message = obj;

                //int byteCount = Encoding.ASCII.GetByteCount(message + 1);

                byte[] sendData = new byte[Encoding.ASCII.GetByteCount(message)];
                sendData = Encoding.ASCII.GetBytes(message);
                client.GetStream().Write(sendData);


                //Console.WriteLine("sending data to server...");

                //StreamReader reader = new StreamReader(stream);
                //string response = reader.ReadLine();
                ////Console.WriteLine(response);

                //reader.Close();
                //client.Close();
                //Console.ReadKey();
            }
            catch (Exception ex)
            {
                Console.WriteLine("failed to connect...");
                goto connection;
            }
        }
    }
}
