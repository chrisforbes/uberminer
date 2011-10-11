using System;
using System.Diagnostics;
using System.IO;
using System.IO.Pipes;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Collections.Generic;

namespace uberminer
{
    public class Uberminer
    {
        string host;
        PacketHandler s2cHandler;
        PacketHandler c2sHandler;

        public Uberminer(string ServerHost, PacketHandler ServerToClientHandler, PacketHandler ClientToServerHandler)
        {
            host = ServerHost;
            s2cHandler = ServerToClientHandler;
            c2sHandler = ClientToServerHandler;
        }

        public void Run()
        {
            var serverClient = new TcpClient(host, 25565);
            var clientListener = new TcpListener(IPAddress.Any, 25565);

            clientListener.Start();

            var clientClient = clientListener.AcceptTcpClient();
            Log("Client connected");

            var serverStream = serverClient.GetStream();
            var clientStream = clientClient.GetStream();

            var serverToClientStream = new UberStream();
            var clientToServerStream = new UberStream();

            var s2cBi = new BidirectionalStream(
                new NetworkReader(serverToClientStream, StreamDirection.ServerToClient),
                new NetworkWriter(clientStream, StreamDirection.ServerToClient),
                s2cHandler);

            var c2sBi = new BidirectionalStream(
                new NetworkReader(clientToServerStream, StreamDirection.ClientToServer),
                new NetworkWriter(serverStream, StreamDirection.ClientToServer),
                c2sHandler);

            try
            {
                var s2cTS = new ThreadStart(() =>
                {
                    PassData(serverStream, serverToClientStream);
                });
                var s2cThread = new Thread(s2cTS);
                s2cThread.Name = "RootS2C";
                s2cThread.Start();
            }
            catch (Exception ex)
            {
                Log("S->C error:");
                Log(ex.Message);
            }

            try
            {
                var c2sTS = new ThreadStart(() =>
                {
                    PassData(clientStream, clientToServerStream);
                });
                var c2sThread = new Thread(c2sTS);
                c2sThread.Name = "RootC2S";
                c2sThread.Start();
            }
            catch (Exception ex)
            {
                Log("C->S error:");
                Log(ex.Message);
            }


            while (true)
            {
                Thread.Sleep(1000);
            }
        }
        
        private void PassData(NetworkReader from, NetworkWriter to)
        {
            Packet packet;
            {
                for (; ; )
                {
                    packet = Packet.Get(from);
                    packet.Write(to);
                }
            }
        }

        private void PassData(Stream from, Stream to)
        {
            byte[] b = new byte[512];
            {
                for (; ; )
                {
                    var read = from.Read(b, 0, 512);
                    if (read > 0)
                    {
                        to.Write(b, 0, read);
                    }
                }
            }
        }

        public static void Log(string format, params object[] arg)
        {
            string outputString = string.Format(format, arg);
            var time = string.Format("[{0}]: ", DateTime.Now.ToString("hh:mm:ss"));
            Console.WriteLine(time + outputString);
            Debug.WriteLine(time + outputString);
        }
    }
}
