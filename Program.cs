using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.IO;
using System.IO.Pipes;
using System.Text;

namespace uberminer
{
	static class Program
	{
		static void Main(string[] args)
		{
			var s = new TcpClient("minecraft.omeganerd.com", 25565);
			var l = new TcpListener(IPAddress.Any, 25565);
			l.Start();

			var c = l.AcceptTcpClient();

			Console.WriteLine("Connected.");

			var ss = s.GetStream();
			var cs = c.GetStream();

			var sc = 0;
			var cc = 0;

			var sk = new NamedPipeServerStream("serverpipe", PipeDirection.In);
			var ck = new NamedPipeServerStream("clientpipe", PipeDirection.In);

			new Thread(() => ParsePackets("s->c", sk)) { IsBackground = true }.Start();
			new Thread(() => ParsePackets("c->s", ck)) { IsBackground = true }.Start();

			var sj = new NamedPipeClientStream("localhost", "serverpipe", PipeDirection.Out); sj.Connect();
			var cj = new NamedPipeClientStream("localhost", "clientpipe", PipeDirection.Out); cj.Connect();

			new Thread(() => PassData(ss, () => ++sc, cs, sj)) { IsBackground = true }.Start();
			new Thread(() => PassData(cs, () => ++cc, ss, cj)) { IsBackground = true }.Start();

			for (; ; )
			{
	//			Console.Write("Server: {0}      Client: {1}      \r", sc, cc);
				Thread.Sleep(1000);
			}
		}

		static void PassData(Stream from, Action a, params Stream[] to)
		{
			var bs = new List<byte>();
			
			for (; ; )
			{
				var b = (byte)from.ReadByte();

				foreach (var s in to)
					s.WriteByte(b);

				a();
			}
		}

		static void ParsePackets(string label, NamedPipeServerStream s)
		{
			s.WaitForConnection();
			Console.WriteLine("ParsePackets connected: {0}", label);

			var br = new BinaryReader(s);
			bool isBroken = false;

			for (; ; )
			{
				var packetType = (PacketType)br.ReadByte();
				if (!isBroken)
					switch (packetType)
					{
						case PacketType.KeepAlive:
							break;

						case PacketType.Handshake:
							Console.WriteLine("{0}: Handshake: `{1}`", label, br.ReadMcString());
							break;

						case PacketType.Login:
							br.ReadInt32();		// version
							br.ReadMcString();	// name
							br.ReadMcString();	// password
							br.ReadInt64();		// mapseed
							br.ReadByte();		// dimension
							Console.WriteLine("{0}: Login", label);
							break;

						case PacketType.SetSpawnPoint:
							br.ReadInt32();		// spawnpoint x,y,z
							br.ReadInt32();
							br.ReadInt32();
							Console.WriteLine("{0}: SetSpawnPoint", label);
							break;

						case PacketType.EntitySpawn:
							br.ReadInt32();		// id
							br.ReadInt16();		// itemtype
							br.ReadByte();		// count
							br.ReadInt16();		// damage
							br.ReadInt32();		// x, y, z
							br.ReadInt32();
							br.ReadInt32();
							br.ReadByte();		// y, p, r euler angles
							br.ReadByte();
							br.ReadByte();
							Console.WriteLine("{0}: EntitySpawn", label);
							break;

						case PacketType.PreChunk:
							var x = IPAddress.NetworkToHostOrder(br.ReadInt32());		// chunk x
							var z = IPAddress.NetworkToHostOrder(br.ReadInt32());		// chunk z
							var mode = br.ReadByte();		// 0 == delete
							Console.WriteLine("{0}: PreChunk {1} {2} {3}", label, mode == 0 ? "Unspawn" : "Spawn", x, z);
							break;

						case PacketType.MobSpawn:
							br.ReadInt32();		// id
							br.ReadByte();		// type
							br.ReadInt32();		// x,y,z
							br.ReadInt32();
							br.ReadInt32();
							br.ReadByte();		// y, p euler
							br.ReadByte();

							while (br.ReadByte() != 127) { }
							Console.WriteLine("{0}: MobSpawn", label);
							break;

						case PacketType.EntityVelocity:
							br.ReadInt32();		// id
							br.ReadInt16();		// vx,vy,vz
							br.ReadInt16();
							br.ReadInt16();
							Console.WriteLine("{0}: EntityVelocity", label);
							break;
							
						case PacketType.PlayerMoveLook:
							br.ReadBytes(8);	// x,y
							br.ReadBytes(8);
							br.ReadBytes(8);	// stance
							br.ReadBytes(8);	// z
							br.ReadBytes(4);	// y, p euler
							br.ReadBytes(4);
							br.ReadByte();		// flying
							break;

						case PacketType.PlayerPosition:
							br.ReadBytes(8);	// x,y,s,z
							br.ReadBytes(8);
							br.ReadBytes(8);
							br.ReadBytes(8);
							br.ReadByte();		// flying
							break;

						case PacketType.UpdateTime:
							br.ReadBytes(8);	// 64bits
							break;

						case PacketType.PlayerOnGround:
							br.ReadByte();		// "falling with style"
							break;

						case PacketType.PlayerLook:
							br.ReadBytes(4);	// y, p euler
							br.ReadBytes(4);
							br.ReadByte();		// flying
							break;

						case PacketType.Place:
							br.ReadInt16();		// blockid
							br.ReadInt32();		// x
							br.ReadByte();		// y
							br.ReadInt32();		// z
							br.ReadByte();		// dir
							br.ReadByte();		// count	-- uh, what?
							br.ReadInt16();		// damage
							break;

						default:
							Console.WriteLine("{0}: Don't understand packet {1}", label, packetType);
							isBroken = true;
							break;
					}
			}
		}

		static string ReadMcString(this BinaryReader br)
		{
			var len = IPAddress.NetworkToHostOrder(br.ReadInt16());
			return Encoding.UTF8.GetString(br.ReadBytes(len));
		}
	}

	enum PacketType : byte
	{
		KeepAlive = 0x00,
		Login = 0x01,
		Handshake = 0x02,
		Chat = 0x03,
		UpdateTime = 0x04,
		Inventory = 0x05,
		SetSpawnPoint = 0x06,
		Unknown07 = 0x07,
		PlayerOnGround = 0x0a,
		PlayerPosition = 0x0b,
		PlayerLook = 0x0c,
		PlayerMoveLook = 0x0d,
		BlockDig = 0x0e,
		Place = 0x0f,
		BlockItemSwitch = 0x10,
		AddToInventory = 0x11,
		ArmAnimation = 0x12,
		NamedEntitySpawn = 0x14,
		EntitySpawn = 0x15,
		CollectItem = 0x16,
		Unknown17 = 0x17,
		MobSpawn = 0x18,
		EntityVelocity = 0x1c,
		DestroyEntity = 0x1d,
		Entity = 0x1e,
		RelativeEntityMove = 0x1f,
		EntityLook = 0x20,
		RelativeEntityMoveLook = 0x21,
		EntityTeleport = 0x22,
		Unknown27 = 0x27,
		PreChunk = 0x32,
		MapChunk = 0x33,
		MultiBlockChange = 0x34,
		BlockChange = 0x35,
		ComplexEntity = 0x3b,
		Kick = 0xff,
	}
}
