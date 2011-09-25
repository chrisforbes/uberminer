using System;
using System.Diagnostics;
using System.IO;
using System.IO.Pipes;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace uberminer
{
    public static class Program
    {
        static readonly object _object = new object();
        public static Queue<Byte> streamQueue = new Queue<Byte>();

        static void Main(string[] args)
        {
            var server = new TcpClient("minecraft.omeganerd.com", 25565);
            //var server = new TcpClient("localhost", 25566);
            var listener = new TcpListener(IPAddress.Any, 25565);
            listener.Start();

            var client = listener.AcceptTcpClient();
            Log("Client connected");

            var serverStream = server.GetStream();
            var clientStream = client.GetStream();

            var sk = new NamedPipeServerStream("serverpipe", PipeDirection.In);
            var ck = new NamedPipeServerStream("clientpipe", PipeDirection.In);

            var sj = new NamedPipeClientStream("localhost", "serverpipe", PipeDirection.Out);
            var cj = new NamedPipeClientStream("localhost", "clientpipe", PipeDirection.Out);

            sj.Connect();
            cj.Connect();

            //try
            //{
            new Thread(() =>
            {
                NetworkReader reader = new NetworkReader(sk);
                sk.WaitForConnection();
                while (true)
                {
                    Packet packet = Packet.Get(reader);
                    if (packet != null)
                    {
                        //Log(packet.Type.ToString());
                    }
                }
            }).Start();
            //}
            //catch (Exception ex)
            //{
            //    Log("Packet error:");
            //    Log(ex.Message);
            //}

            try
            {
                new Thread(() =>
                {
                    PassData(serverStream, clientStream, sj);
                }).Start();
            }
            catch (Exception ex)
            {
                Log("S->C error:");
                Log(ex.Message);
            }

            try
            {
                new Thread(() =>
                {
                    PassData(clientStream, serverStream);
                }).Start();
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

        static void PassData(Stream from, params Stream[] to)
        {
            byte b;
            {
                for (; ; )
                {
                    b = (byte)from.ReadByte();
                    foreach (var s in to)
                        s.WriteByte(b);
                }
            }
        }

        public static void Log(string format, params object[] arg)
        {
            string outputString = string.Format(format, arg);
            Console.WriteLine(outputString);
            Debug.WriteLine(outputString);
        }
    }
}
