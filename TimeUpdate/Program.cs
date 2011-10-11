using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Uberminer;
using System.Threading;

namespace TimeUpdate
{
    class Program
    {
        private static bool changingTime;
        private static int changeTimeTo;

        static void Main(string[] args)
        {
            var s2cHandler = new PacketHandler();
            var c2sHandler = new PacketHandler();

            s2cHandler.HandleTimeUpdate = UpdateTime;

            c2sHandler.HandleChatMessage = C2SChat;

            var miner = new Uberminer("mc.omeganerd.com", s2cHandler, c2sHandler);

            miner.Run();
        }

        static bool UpdateTime(TimeUpdatePacket packet)
        {
            if (changingTime)
            {
                packet.Time = changeTimeTo;
            }

            return true;
        }

        static bool C2SChat(ChatMessagePacket packet)
        {
            var tokens = packet.Message.Split(' ');
            var token = 0;
            if (tokens.Length > 0)
            {
                if (tokens[token] == "/ubtime")
                {
                    var error = false;
                    ++token;
                    if (tokens.Length == 2 && tokens[token] == "toggle")
                    {
                        changingTime = !changingTime;
                    }
                    else if (tokens.Length == 3 && tokens[token] == "set")
                    {
                        ++token;
                        var time = 0;
                        if (int.TryParse(tokens[token], out time))
                        {
                            changingTime = true;
                            changeTimeTo = time;
                        }
                        else
                        {
                            error = true;
                        }
                    }
                    else
                    {
                        error = true;
                    }
                    if (error == true)
                    {
                        // insert a chat message into s2c
                        //p.Message = "§6Invalid ubtime command. Expected [toggle|set 0-24000]";
                        Uberminer.Log(packet.Message);
                    }
                    return false;
                }
            }

            return true;
        }
    }
}


/////// legacy diamond code
//if (direction == StreamDirection.ServerToClient)
//{
//    bool found = false;
//    lock (locker)
//    {
//        Vector3 closest = new Vector3();
//        var d = PacketHandler.diamonds;
//        var player = PacketHandler.player;
//        var closestDist = double.MaxValue;
//        foreach (var diamond in d)
//        {
//            var v = new Vector3();
//            v.X = diamond.Key.X;
//            v.Y = diamond.Key.Y;
//            v.Z = diamond.Key.Z;

//            var compDist = (v.X - player.X) * (v.X - player.X) + (v.Z - player.Z) * (v.Z - player.Z);
//            if (compDist < closestDist)
//            {
//                closest.X = v.X;
//                closest.Y = v.Y;
//                closest.Z = v.Z;
//                closestDist = compDist;
//                found = true;
//            }
//        }

//        if (found)
//        {
//            Uberminer.Log("Found diamond at {0}, {1}, {2}", closest.X, closest.Y, closest.Z);
//        }
//    }
////}

/*
        public virtual void Handle(PlayerPositionPacket packet)
        {
            player.X = packet.X;
            player.Y = packet.Y;
            player.Z = packet.Z;
        }

        public virtual void Handle(PlayerPosition_LookPacket packet)
        {
            player.X = packet.X;
            player.Y = packet.Y;
            player.Z = packet.Z;
        }

        public virtual void Handle(PlayerDiggingPacket packet)
        {
            if (packet.Status == 2)
            {
                bool b;
                var v = new Vector3(packet.X, packet.Y, packet.Z);
                if (diamonds.TryGetValue(v, out b))
                {
                    lock (locker)
                    {
                        diamonds.Remove(v);
                    }
                }
            }
        }

        public virtual void Handle(PlayerBlockPlacementPacket packet)
        {
            //if (packet.BlockorItemID != -1)

            //var cm = world.GetChunkManager();
            //var x = packet.X;
            //var y = packet.Y;
            //var z = packet.Z;

            //switch (packet.Direction)
            //{
            //    case 0:
            //        --y;
            //        break;
            //    case 1:
            //        ++y;
            //        break;
            //    case 2:
            //        --z;
            //        break;
            //    case 3:
            //        ++z;
            //        break;
            //    case 4:
            //        --x;
            //        break;
            //    case 5:
            //        ++x;
            //        break;
            //}

            //var ch = cm.GetChunkRef(x >> 4, z >> 4);
            //if (ch == null)
            //{
            //    ch = cm.CreateChunk(x >> 4, z >> 4);
            //}

            //x = x & 15;
            //y = (byte)(y & 127);
            //z = z & 15;

            //ch.Blocks.AutoFluid = false;
            //ch.Blocks.AutoLight = false;
            //ch.Blocks.SetID(x, y, z, packet.BlockorItemID);
        }

        public virtual void Handle(MapChunkPacket packet)
        {
            var chunk = packet.Chunk;

            if (chunk != null)
            {
                var stride = packet.Size_X * packet.Size_Y * packet.Size_Z;

                var ids = new byte[stride];

                var index = 0;
                for (var i = 0; i < stride; ++i)
                {
                    ids[i] = chunk[i];
                    ++index;
                }

                index = 0;
                for (var x = 0; x < packet.Size_X; x++)
                {
                    for (var z = 0; z < packet.Size_Z; z++)
                    {
                        for (var y = 0; y < packet.Size_Y; y++)
                        {
                            var v = new Vector3();
                            v.X = packet.X + x;
                            v.Y = y;
                            v.Z = packet.Z + z;

                            lock (locker)
                            {
                                if (ids[index] == 56)
                                {
                                    //if ((v.X > -32 && v.X < 32) && (v.Z > -32 && v.Z < 32))
                                    //{
                                    //    continue;
                                    //}
                                    diamonds[v] = true;
                                }
                                else
                                {
                                    bool b;
                                    if (diamonds.TryGetValue(v, out b))
                                    {
                                        diamonds.Remove(v);
                                    }
                                }
                            }
                            ++index;
                        }
                    }
                }
            }

            //-----------------------------------------------------------

            //var stride = packet.Size_X * packet.Size_Y * packet.Size_Z;

            //var ids = new byte[stride];
            //var mds = new byte[stride];
            //var lts = new byte[stride];
            //var sls = new byte[stride];

            //var chunk = packet.Chunk;
            //var cm = world.GetChunkManager();

            //if (chunk != null)
            //{
            //    var ch = cm.GetChunkRef(packet.X >> 4, packet.Z >> 4);
            //    if (ch == null)
            //    {
            //        ch = cm.CreateChunk(packet.X >> 4, packet.Z >> 4);
            //    }
            //    var blocks = ch.Blocks;
            //    blocks.AutoFluid = false;
            //    blocks.AutoLight = false;

            //    var stride = packet.Size_X * packet.Size_Y * packet.Size_Z;

            //    var ids = new byte[stride];
            //    var mds = new byte[stride];
            //    var lts = new byte[stride];
            //    var sls = new byte[stride];

            //    var index = 0;
            //    for (var i = 0; i < stride; ++i)
            //    {
            //        ids[i] = chunk[i];
            //        ++index;
            //    }

            //    for (var i = 0; i < stride; i += 2)
            //    {
            //        var md = chunk[index];
            //        mds[i + 0] = (byte)(md & 0xf);
            //        mds[i + 1] = (byte)(md >> 4);
            //        ++index;
            //    }

            //    for (var i = 0; i < stride; i += 2)
            //    {
            //        var lt = chunk[index];
            //        lts[i + 0] = (byte)(lt & 0xf);
            //        lts[i + 1] = (byte)(lt >> 4);
            //        ++index;
            //    }

            //    for (var i = 0; i < stride; i += 2)
            //    {
            //        var sl = chunk[index];
            //        sls[i + 0] = (byte)(sl & 0xf);
            //        sls[i + 1] = (byte)(sl >> 4);
            //        ++index;
            //    }

            //    index = 0;
            //    for (var x = 0; x < packet.Size_X; x++)
            //    {
            //        for (var z = 0; z < packet.Size_Z; z++)
            //        {
            //            for (var y = 0; y < packet.Size_Y; y++)
            //            {
            //                blocks.SetID(x, y, z, ids[index]);
            //                blocks.SetData(x, y, z, mds[index]);
            //                blocks.SetBlockLight(x, y, z, lts[index]);
            //                blocks.SetSkyLight(x, y, z, sls[index]);
            //                ++index;
            //            }
            //        }
            //    }
            //}
        }

        public virtual void Handle(MultiBlockChangePacket packet)
        {
            //var cm = world.GetChunkManager();

            Vector3 v = new Vector3();
            for (var i = 0; i < packet.ArraySize; ++i)
            {
                var n = packet.CoordinateArray[i];
                var x = n & 0xf;
                var y = n << 8;
                var z = (n & 0xf) << 4;

                v.X = x;
                v.Y = y;
                v.Z = z;

                var t = packet.TypeArray[i];

                lock (locker)
                {
                    if (packet.TypeArray[i] == 56)
                    {
                        diamonds[v] = true;
                    }
                    else
                    {
                        bool b;
                        if (diamonds.TryGetValue(v, out b))
                        {
                            diamonds.Remove(v);
                        }
                    }
                }
            }


            //var m = packet.MetadataArray[i];

            //var ch = cm.GetChunkRef(packet.ChunkX, packet.ChunkZ);
            //if (ch == null)
            //{
            //    ch = cm.CreateChunk(packet.ChunkX, packet.ChunkZ);
            //}

            //ch.Blocks.AutoFluid = false;
            //ch.Blocks.AutoLight = false;

            //ch.Blocks.SetID(x, y, z, t);
            //ch.Blocks.SetData(x, y, z, m);
            //}
        }

        public virtual void Handle(BlockChangePacket packet)
        {
            var v = new Vector3();
            v.X = packet.X;
            v.Y = packet.Y;
            v.Z = packet.Z;

            lock (locker)
            {
                if (packet.BlockType == 56)
                {
                    diamonds[v] = true;
                }
                else
                {
                    bool b;
                    if (diamonds.TryGetValue(v, out b))
                    {
                        diamonds.Remove(v);
                    }
                }
            }

            //var cm = world.GetChunkManager();

            //var ch = cm.GetChunkRef(packet.X >> 4, packet.Z >> 4);

            //if (ch == null)
            //{
            //    ch = cm.CreateChunk(packet.X >> 4, packet.Z >> 4);
            //}

            //ch.Blocks.AutoFluid = false;
            //ch.Blocks.AutoLight = false;

            //ch.Blocks.SetID(packet.X & 15, packet.Y & 127, packet.Z & 15, packet.BlockType);
            //ch.Blocks.SetData(packet.X & 15, packet.Y & 127, packet.Z & 15, packet.BlockMetadata);
        }
*/
