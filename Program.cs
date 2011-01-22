using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.IO;
using System.IO.Pipes;
using System.Text;
using System.Diagnostics;

namespace uberminer
{
	static class Program
	{
		static void Main(string[] args)
		{
			var s = new TcpClient("localhost", 25565);
			var l = new TcpListener(IPAddress.Any, 25564);
			l.Start();

			var c = l.AcceptTcpClient();

			Log("Connected.");

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
			Log("ParsePackets connected: {0}", label);

			var br = new BinaryReader(s);
			bool isBroken = false;

			var lastPacketType = (PacketType)0;
			var packetType = (PacketType)0;

			for (; ; )
			{
				lastPacketType = packetType;
				packetType = (PacketType)br.ReadByte();
				if (!isBroken)
					switch (packetType)
					{
						case PacketType.KeepAlive:
							Log("{0}: KeepAlive", label);
							break;

						case PacketType.Login:
							br.ReadInt32();		// version
							br.ReadMcString();	// name
							br.ReadMcString();	// password
							br.ReadInt64();		// mapseed
							br.ReadByte();		// dimension
							Log("{0}: Login", label);
							break;

						case PacketType.Handshake:
							Log("{0}: Handshake: `{1}`", label, br.ReadMcString());
							break;

						case PacketType.Chat:
							string chatMessage = ReadMcString(br);
							Log("{0}: Chat - {1}", label, chatMessage);
							break;

						case PacketType.SetSpawnPoint:
							br.ReadInt32();		// spawnpoint x,y,z
							br.ReadInt32();
							br.ReadInt32();
							Log("{0}: SetSpawnPoint", label);
							break;

						case PacketType.UseEntity:
							br.ReadInt32(); // user
							br.ReadInt32(); // target
							br.ReadBoolean(); // left click
							Log("{0}: UseEntity", label);
							break;

						case PacketType.Health:
							br.ReadInt16(); // health
							Log("{0}: Health", label);
							break;

						case PacketType.Respawn:
							Log("{0}: Respawn", label);
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
							Log("{0}: EntitySpawn", label);
							break;

						case PacketType.Entity:
							br.ReadInt32(); // id
							//Log("{0}: Entity", label);
							break;

						case PacketType.RelativeEntityMove:
							br.ReadInt32(); // entity id
							br.ReadByte(); // x
							br.ReadByte(); // y
							br.ReadByte(); // z
							//Log("{0}: RelativeEntityMove", label);
							break;

						case PacketType.EntityLook:
							br.ReadInt32(); // id
							br.ReadByte(); // rotation
							br.ReadByte(); // pitch
							//Log("{0}: EntityLook", label);
							break;

						case PacketType.RelativeEntityMoveLook:
							br.ReadInt32(); // entity id
							br.ReadByte(); // x
							br.ReadByte(); // y
							br.ReadByte(); // z
							br.ReadByte(); // yaw
							br.ReadByte(); // pitch
							//Log("{0}: RelativeEntityMoveLook", label);
							break;

						case PacketType.EntityTeleport:
							br.ReadInt32(); // entity id
							br.ReadInt32(); // x
							br.ReadInt32(); // y
							br.ReadInt32(); // z
							br.ReadByte(); // yaw
							br.ReadByte(); // pitch
							Log("{0}: EntityTeleport", label);
							break;

						case PacketType.EntityStatus:
							br.ReadInt32(); // entity id
							br.ReadByte(); // entity status
							break;

						case PacketType.AttachEntity:
							br.ReadInt32(); // player being attached
							br.ReadInt32(); // vehicle being attached
							break;

						case PacketType.EntityMetadata:
							br.ReadInt32(); // entity id
							while (br.ReadByte() != 127) { } // metadata
							break;

						case PacketType.PreChunk:
							var x = IPAddress.NetworkToHostOrder(br.ReadInt32());		// chunk x
							var z = IPAddress.NetworkToHostOrder(br.ReadInt32());		// chunk z
							var mode = br.ReadByte();		// 0 == delete
							Log("{0}: PreChunk {1} {2} {3}", label, mode == 0 ? "Unspawn" : "Spawn", x, z);
							break;

						case PacketType.MapChunk:
							br.ReadInt32(); // x
							br.ReadInt16(); // y
							br.ReadInt32(); // z
							br.ReadByte(); // size x
							br.ReadByte(); // size y
							br.ReadByte(); // size z
							var compressedSize = IPAddress.NetworkToHostOrder(br.ReadInt32()); // compressed size
							br.ReadBytes(compressedSize); // compressed chunk data
							Log("{0}: Map Chunk - {1}", label, compressedSize);
							break;

						case PacketType.MultiBlockChange:
							br.ReadInt32(); // x
							br.ReadInt32(); // z
							var arraySize = IPAddress.NetworkToHostOrder(br.ReadInt16()); // array size
							for (int i = 0; i < arraySize; ++i)
							{
								br.ReadInt16(); // piece of coordinates array
							}
							br.ReadBytes(arraySize); // types array
							br.ReadBytes(arraySize); // metadata
							Log("{0}: MultiBlockChange", label);
							break;

						case PacketType.BlockChange:
							br.ReadInt32(); // x
							br.ReadByte(); // y
							br.ReadInt32(); // z
							br.ReadByte(); // block type
							br.ReadByte(); // block metadata
							Log("{0}: BlockChange", label);
							break;

						case PacketType.MobSpawn:
							br.ReadInt32();		// id
							br.ReadByte();		// type
							br.ReadInt32();		// x,y,z
							br.ReadInt32();
							br.ReadInt32();
							br.ReadByte();		// y, p euler
							br.ReadByte();

							while (br.ReadByte() != 127) { } // metadata
							Log("{0}: MobSpawn", label);
							break;

						case PacketType.EntityVelocity:
							br.ReadInt32();		// id
							br.ReadInt16();		// vx,vy,vz
							br.ReadInt16();
							br.ReadInt16();
							//Log("{0}: EntityVelocity", label);
							break;
							
						case PacketType.DestroyEntity:
							br.ReadInt32(); // entity id
							break;

						case PacketType.PlayerMoveLook:
							br.ReadBytes(8);	// x,y
							br.ReadBytes(8);
							br.ReadBytes(8);	// stance
							br.ReadBytes(8);	// z
							br.ReadBytes(4);	// y, p euler
							br.ReadBytes(4);
							br.ReadByte();		// flying
							//Log("{0}: PlayerMoveLook", label);
							break;

						case PacketType.BlockDig:
							br.ReadByte(); // status
							br.ReadInt32(); // x
							br.ReadByte(); // y
							br.ReadInt32(); // z
							br.ReadByte(); // face
							//Log("{0}: BlockDig", label);
							break;

						case PacketType.PlayerPosition:
							br.ReadBytes(8);	// x,y,s,z
							br.ReadBytes(8);
							br.ReadBytes(8);
							br.ReadBytes(8);
							br.ReadByte();		// flying
							//Log("{0}: PlayerPosition", label);
							break;

						case PacketType.UpdateTime:
							br.ReadBytes(8);	// 64bits
							Log("{0}: UpdateTime", label);
							break;

						case PacketType.EntityEquipment:
							br.ReadInt32(); // entity id
							br.ReadInt16(); // slot
							br.ReadInt16(); // item id
							br.ReadInt16(); // ? damage
							Log("{0}: EntityEquipment", label);
							break;

						case PacketType.PlayerOnGround:
							br.ReadByte();		// "falling with style"
							//Log("{0}: PlayerOnGround", label);
							break;

						case PacketType.PlayerLook:
							br.ReadBytes(4);	// y, p euler
							br.ReadBytes(4);
							br.ReadByte();		// flying
							//Log("{0}: PlayerLook", label);
							break;

						case PacketType.Place:
							br.ReadInt16();		// blockid
							br.ReadInt32();		// x
							br.ReadByte();		// y
							br.ReadInt32();		// z
							br.ReadByte();		// dir
							br.ReadByte();		// count	-- uh, what?
							br.ReadInt16();		// damage
							Log("{0}: Place", label);
							break;

						case PacketType.ArmAnimation:
							br.ReadInt32(); // entity id
							br.ReadByte(); // forward animation
							Log("{0}: ArmAnimation", label);
							break;

						case PacketType.EntityAction:
							br.ReadInt32(); // entity id
							br.ReadByte(); // entity status
							Log("{0}: EntityAction", label);
							break;

						case PacketType.WindowItems:
							br.ReadByte(); // window id
							var count = IPAddress.NetworkToHostOrder(br.ReadInt16()); // count
							for (int i = 0; i < count; ++i)
							{
								var id = IPAddress.NetworkToHostOrder(br.ReadInt16());
								if (id != -1)
								{
									br.ReadByte(); // count
									br.ReadInt16(); // health
								}
							}
							Log("{0}: WindowItems - {1} items", label, count);
							break;

						case PacketType.CloseWindow:
							br.ReadByte(); // window id
							Log("{0}: CloseWindow", label);
							break;

						case PacketType.WindowClick:
							br.ReadByte(); // window id
							br.ReadInt16(); // slot
							br.ReadByte(); // right click
							br.ReadInt16(); // action num
							var itemid = IPAddress.NetworkToHostOrder(br.ReadInt16()); // item id

							if (itemid != -1)
							{
								br.ReadByte(); // item count
								br.ReadInt16(); // item uses
							}
							Log("{0}: WindowClick", label);
							break;

						case PacketType.SetSlot:
							br.ReadByte(); // window id
							br.ReadInt16(); // slot
							var setslotitemid = IPAddress.NetworkToHostOrder(br.ReadInt16()); // item id

							if (setslotitemid != -1)
							{
								br.ReadByte(); // num items in stack
								br.ReadInt16(); // item uses
							}
							Log("{0}: SetSlot", label);
							break;

						case PacketType.BlockItemSwitch:
							br.ReadInt16(); // block id
							Log("{0}: BlockItemSwitch", label);
							break;

						case PacketType.Kick:
							string reason = ReadMcString(br);
							Log("{0}: Kicked - {1}", label, reason);
							break;

						default:
							Log("{0}: Don't understand packet {1}. Previous packet was {2}", label, packetType, lastPacketType);
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

		static void Log(string format, params object[] arg)
		{
			string outputString = string.Format(format, arg);
			Console.WriteLine(outputString);
			Debug.WriteLine(outputString);
		}
	}

	enum PacketType : byte
	{
		KeepAlive = 0x00,
		Login = 0x01,
		Handshake = 0x02,
		Chat = 0x03,
		UpdateTime = 0x04,
		EntityEquipment = 0x05,
		SetSpawnPoint = 0x06,
		UseEntity = 0x07,
		Health = 0x08,
		Respawn = 0x09,
		PlayerOnGround = 0x0a,
		PlayerPosition = 0x0b,
		PlayerLook = 0x0c,
		PlayerMoveLook = 0x0d,
		BlockDig = 0x0e,
		Place = 0x0f,
		BlockItemSwitch = 0x10,
		AddToInventory = 0x11,
		ArmAnimation = 0x12,
		EntityAction = 0x13,
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
		EntityStatus = 0x26,
		AttachEntity = 0x27,
		EntityMetadata = 0x28,
		PreChunk = 0x32,
		MapChunk = 0x33,
		MultiBlockChange = 0x34,
		BlockChange = 0x35,
		ComplexEntity = 0x3b,
		CloseWindow = 0x65,
		WindowClick = 0x66,
		SetSlot = 0x67,
		WindowItems = 0x68,
		Kick = 0xff,
	}
}
