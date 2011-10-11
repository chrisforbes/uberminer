using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Threading;

namespace Uberminer
{
    public enum StreamDirection
    {
        ClientToServer,
        ServerToClient,
    }

    class BidirectionalStream
    {
        private Thread readerThread;

        volatile bool stopThread = false;

        private NetworkReader reader;
        private NetworkWriter writer;

        private PacketHandler handler;

        public BidirectionalStream(NetworkReader from, NetworkWriter to, PacketHandler packetHandler)
        {
            reader = from;
            writer = to;

            handler = packetHandler;

            var rStart = new ThreadStart(Run);
            readerThread = new Thread(rStart);
            readerThread.Start();
        }

        ~BidirectionalStream()
        {
            Stop();

            while (readerThread.IsAlive)
            {
            }
        }

        public void Stop()
        {
            stopThread = true;
        }

        private void Run()
        {
            Packet rp = null;
            Packet sp = null;
            while (!stopThread)
            {
                bool logPackets = true;

                rp = Packet.Get(reader, logPackets);
                var sendPacket = rp.Handle(handler);
                
                if (sendPacket)
                {
                    sp = rp;
                    // Uberminer.Log("{0} is writing packet of {1}", threadName, rp.Type.ToString());
                    Packet.Put(sp, writer);
                    //Uberminer.Log("{0} presumably just wrote packet of {1}", threadName, rp.Type.ToString());
                }
                Thread.Sleep(0);
            }
        }
    }
}
