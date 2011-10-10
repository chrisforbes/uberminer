using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace uberminer
{
    public class KeepAlivePacket : Packet
    {
        public int KAId;

        public KeepAlivePacket()
            : base(PacketType.KeepAlive)
        {
        }

        public override void Read(NetworkReader reader)
        {
            MemoryStream ms = new MemoryStream();
            using (BinaryWriter writer = new BinaryWriter(ms))
            {
                writer.Write((byte)Type);
                writer.Write(reader.Read(out KAId));
            }
            BytesRead = ms.ToArray();
            NumBytesRead = BytesRead.Length;
        }

        public override void Write(NetworkWriter writer)
        {
            MemoryStream ms = new MemoryStream();
            using (BinaryWriter tempWriter = new BinaryWriter(ms))
            {
                tempWriter.Write((byte)Type);
                tempWriter.Write(writer.Prepare(KAId));
            }

            BytesWritten = ms.ToArray();
            NumBytesWritten = BytesWritten.Length;

            CheckPacketSanity();

            writer.Write(BytesWritten);
        }

        public override bool Handle(PacketHandler handler)
        {
            if (handler.HandleKeepAlive != null)
            {
                return handler.HandleKeepAlive(this);
            }
            else
            {
                return true;
            }
        }
    }

    public class LoginRequestPacket : Packet
    {
        public int EntityId;
        public string Username;  // only in client->server
        public long MapSeed;
        public int ServerMode;
        public byte Dimension;
        public byte Difficulty;
        public byte WorldHeight;
        public byte MaxPlayers;

        public LoginRequestPacket()
            : base(PacketType.LoginRequest) { }

        public override void Read(NetworkReader reader)
        {
            MemoryStream ms = new MemoryStream();
            using (BinaryWriter writer = new BinaryWriter(ms))
            {
                writer.Write((byte)Type);
                writer.Write(reader.Read(out EntityId));
                writer.Write(reader.Read(out Username));
                writer.Write(reader.Read(out MapSeed));
                writer.Write(reader.Read(out ServerMode));
                writer.Write(reader.Read(out Dimension));
                writer.Write(reader.Read(out Difficulty));
                writer.Write(reader.Read(out WorldHeight));
                writer.Write(reader.Read(out MaxPlayers));

                Uberminer.Log("EntityID: {0}\n Username: {1}\n MapSeed: {2}\n ServerMode: {3}\n",
                    EntityId, Username, MapSeed, ServerMode);
                Uberminer.Log("Dimension: {0}\n Difficulty: {1}\n WorldHeight: {2}\n MaxPlayers: {3}",
                    Dimension, Difficulty, WorldHeight, MaxPlayers);
            }
            BytesRead = ms.ToArray();
            NumBytesRead = BytesRead.Length;
        }

        public override void Write(NetworkWriter writer)
        {
            MemoryStream ms = new MemoryStream();
            using (BinaryWriter tempWriter = new BinaryWriter(ms))
            {
                tempWriter.Write((byte)Type);
                tempWriter.Write(writer.Prepare(EntityId));
                tempWriter.Write(writer.Prepare(Username));
                tempWriter.Write(writer.Prepare(MapSeed));
                tempWriter.Write(writer.Prepare(ServerMode));
                tempWriter.Write(writer.Prepare(Dimension));
                tempWriter.Write(writer.Prepare(Difficulty));
                tempWriter.Write(writer.Prepare(WorldHeight));
                tempWriter.Write(writer.Prepare(MaxPlayers));
            }

            BytesWritten = ms.ToArray();
            NumBytesWritten = BytesWritten.Length;

            CheckPacketSanity();

            writer.Write(BytesWritten);
        }

        public override bool Handle(PacketHandler handler)
        {
            if (handler.HandleLoginRequest != null)
            {
                return handler.HandleLoginRequest(this);
            }
            else
            {
                return true;
            }
        }

    }

    public class HandshakePacket : Packet
    {
        public string Hash;

        public HandshakePacket()
            : base(PacketType.Handshake) { }

        public override void Read(NetworkReader reader)
        {
            MemoryStream ms = new MemoryStream();
            using (BinaryWriter writer = new BinaryWriter(ms))
            {
                writer.Write((byte)Type);
                writer.Write(reader.Read(out Hash));
            }
            BytesRead = ms.ToArray();
            NumBytesRead = BytesRead.Length;
        }

        public override void Write(NetworkWriter writer)
        {
            MemoryStream ms = new MemoryStream();
            using (BinaryWriter tempWriter = new BinaryWriter(ms))
            {
                tempWriter.Write((byte)Type);

                tempWriter.Write(writer.Prepare(Hash));

            }

            BytesWritten = ms.ToArray();
            NumBytesWritten = BytesWritten.Length;

            CheckPacketSanity();

            writer.Write(BytesWritten);
        }

        public override bool Handle(PacketHandler handler)
        {
            if (handler.HandleHandshake != null)
            {
                return handler.HandleHandshake(this);
            }
            else
            {
                return true;
            }
        }
    }

    public class ChatMessagePacket : Packet
    {
        public string Message;

        public ChatMessagePacket()
            : base(PacketType.ChatMessage) { }

        public override void Read(NetworkReader reader)
        {
            MemoryStream ms = new MemoryStream();
            using (BinaryWriter writer = new BinaryWriter(ms))
            {
                writer.Write((byte)Type);
                writer.Write(reader.Read(out Message));
                Uberminer.Log("Chat: {0}", Message);
            }
            BytesRead = ms.ToArray();
            NumBytesRead = BytesRead.Length;
        }

        public override void Write(NetworkWriter writer)
        {
            MemoryStream ms = new MemoryStream();
            using (BinaryWriter tempWriter = new BinaryWriter(ms))
            {
                tempWriter.Write((byte)Type);

                tempWriter.Write(writer.Prepare(Message));

            }

            BytesWritten = ms.ToArray();
            NumBytesWritten = BytesWritten.Length;

            CheckPacketSanity();

            writer.Write(BytesWritten);
        }

        public override bool Handle(PacketHandler handler)
        {
            if (handler.HandleChatMessage != null)
            {
                return handler.HandleChatMessage(this);
            }
            else
            {
                return true;
            }
        }
    }

    public class TimeUpdatePacket : Packet
    {
        public long Time;

        public TimeUpdatePacket()
            : base(PacketType.TimeUpdate) { }

        public override void Read(NetworkReader reader)
        {
            MemoryStream ms = new MemoryStream();
            using (BinaryWriter writer = new BinaryWriter(ms))
            {
                writer.Write((byte)Type);
                writer.Write(reader.Read(out Time));
            }
            BytesRead = ms.ToArray();
            NumBytesRead = BytesRead.Length;
        }

        public override void Write(NetworkWriter writer)
        {
            MemoryStream ms = new MemoryStream();
            using (BinaryWriter tempWriter = new BinaryWriter(ms))
            {
                tempWriter.Write((byte)Type);

                tempWriter.Write(writer.Prepare(Time));

            }

            BytesWritten = ms.ToArray();
            NumBytesWritten = BytesWritten.Length;

            CheckPacketSanity();

            writer.Write(BytesWritten);
        }

        public override bool Handle(PacketHandler handler)
        {
            if (handler.HandleTimeUpdate != null)
            {
                return handler.HandleTimeUpdate(this);
            }
            else
            {
                return true;
            }
        }
    }

    public class EntityEquipmentPacket : Packet
    {
        public int EntityId;
        public short Slot;
        public short ItemId;
        public short Damage;

        public EntityEquipmentPacket()
            : base(PacketType.EntityEquipment) { }

        public override void Read(NetworkReader reader)
        {
            MemoryStream ms = new MemoryStream();
            using (BinaryWriter writer = new BinaryWriter(ms))
            {
                writer.Write((byte)Type);
                writer.Write(reader.Read(out EntityId));
                writer.Write(reader.Read(out Slot));
                writer.Write(reader.Read(out ItemId));
                writer.Write(reader.Read(out Damage));
            }
            BytesRead = ms.ToArray();
            NumBytesRead = BytesRead.Length;
        }

        public override void Write(NetworkWriter writer)
        {
            MemoryStream ms = new MemoryStream();
            using (BinaryWriter tempWriter = new BinaryWriter(ms))
            {
                tempWriter.Write((byte)Type);

                tempWriter.Write(writer.Prepare(EntityId));
                tempWriter.Write(writer.Prepare(Slot));
                tempWriter.Write(writer.Prepare(ItemId));
                tempWriter.Write(writer.Prepare(Damage));


            }

            BytesWritten = ms.ToArray();
            NumBytesWritten = BytesWritten.Length;

            CheckPacketSanity();

            writer.Write(BytesWritten);
        }

        public override bool Handle(PacketHandler handler)
        {
            if (handler.HandleEntityEquipment != null)
            {
                return handler.HandleEntityEquipment(this);
            }
            else
            {
                return true;
            }
        }
    }

    public class SpawnPositionPacket : Packet
    {
        public int X;
        public int Y;
        public int Z;

        public SpawnPositionPacket()
            : base(PacketType.SpawnPosition) { }

        public override void Read(NetworkReader reader)
        {
            MemoryStream ms = new MemoryStream();
            using (BinaryWriter writer = new BinaryWriter(ms))
            {
                writer.Write((byte)Type);
                writer.Write(reader.Read(out X));
                writer.Write(reader.Read(out Y));
                writer.Write(reader.Read(out Z));
            }
            BytesRead = ms.ToArray();
            NumBytesRead = BytesRead.Length;
        }

        public override void Write(NetworkWriter writer)
        {
            MemoryStream ms = new MemoryStream();
            using (BinaryWriter tempWriter = new BinaryWriter(ms))
            {
                tempWriter.Write((byte)Type);

                tempWriter.Write(writer.Prepare(X));
                tempWriter.Write(writer.Prepare(Y));
                tempWriter.Write(writer.Prepare(Z));

            }

            BytesWritten = ms.ToArray();
            NumBytesWritten = BytesWritten.Length;

            CheckPacketSanity();

            writer.Write(BytesWritten);
        }

        public override bool Handle(PacketHandler handler)
        {
            if (handler.HandleSpawnPosition != null)
            {
                return handler.HandleSpawnPosition(this);
            }
            else
            {
                return true;
            }
        }
    }

    public class UseEntityPacket : Packet
    {
        public int User;
        public int Target;
        public bool LeftClick;

        public UseEntityPacket()
            : base(PacketType.UseEntity) { }

        public override void Read(NetworkReader reader)
        {
            MemoryStream ms = new MemoryStream();
            using (BinaryWriter writer = new BinaryWriter(ms))
            {
                writer.Write((byte)Type);
                writer.Write(reader.Read(out User));
                writer.Write(reader.Read(out Target));
                writer.Write(reader.Read(out LeftClick));
            }
            BytesRead = ms.ToArray();
            NumBytesRead = BytesRead.Length;
        }

        public override void Write(NetworkWriter writer)
        {
            MemoryStream ms = new MemoryStream();
            using (BinaryWriter tempWriter = new BinaryWriter(ms))
            {
                tempWriter.Write((byte)Type);

                tempWriter.Write(writer.Prepare(User));
                tempWriter.Write(writer.Prepare(Target));
                tempWriter.Write(writer.Prepare(LeftClick));

            }

            BytesWritten = ms.ToArray();
            NumBytesWritten = BytesWritten.Length;

            CheckPacketSanity();

            writer.Write(BytesWritten);
        }

        public override bool Handle(PacketHandler handler)
        {
            if (handler.HandleUseEntity != null)
            {
                return handler.HandleUseEntity(this);
            }
            else
            {
                return true;
            }
        }
    }

    public class UpdateHealthPacket : Packet
    {
        public short Health;
        public short Food;
        public float FoodSaturation;

        public UpdateHealthPacket()
            : base(PacketType.UpdateHealth) { }

        public override void Read(NetworkReader reader)
        {
            MemoryStream ms = new MemoryStream();
            using (BinaryWriter writer = new BinaryWriter(ms))
            {
                writer.Write((byte)Type);
                writer.Write(reader.Read(out Health));
                writer.Write(reader.Read(out Food));
                writer.Write(reader.Read(out FoodSaturation));
            }
            BytesRead = ms.ToArray();
            NumBytesRead = BytesRead.Length;
        }

        public override void Write(NetworkWriter writer)
        {
            MemoryStream ms = new MemoryStream();
            using (BinaryWriter tempWriter = new BinaryWriter(ms))
            {
                tempWriter.Write((byte)Type);

                tempWriter.Write(writer.Prepare(Health));
                tempWriter.Write(writer.Prepare(Food));
                tempWriter.Write(writer.Prepare(FoodSaturation));

            }

            BytesWritten = ms.ToArray();
            NumBytesWritten = BytesWritten.Length;

            CheckPacketSanity();

            writer.Write(BytesWritten);
        }

        public override bool Handle(PacketHandler handler)
        {
            if (handler.HandleUpdateHealth != null)
            {
                return handler.HandleUpdateHealth(this);
            }
            else
            {
                return true;
            }
        }
    }

    public class RespawnPacket : Packet
    {
        public byte World;
        public byte Difficulty;
        public byte Mode;
        public short WorldHeight;
        public long MapSeed;

        public RespawnPacket()
            : base(PacketType.Respawn) { }

        public override void Read(NetworkReader reader)
        {
            MemoryStream ms = new MemoryStream();
            using (BinaryWriter writer = new BinaryWriter(ms))
            {
                writer.Write((byte)Type);
                writer.Write(reader.Read(out World));
                writer.Write(reader.Read(out Difficulty));
                writer.Write(reader.Read(out Mode));
                writer.Write(reader.Read(out WorldHeight));
                writer.Write(reader.Read(out MapSeed));
            }
            BytesRead = ms.ToArray();
            NumBytesRead = BytesRead.Length;
        }

        public override void Write(NetworkWriter writer)
        {
            MemoryStream ms = new MemoryStream();
            using (BinaryWriter tempWriter = new BinaryWriter(ms))
            {
                tempWriter.Write((byte)Type);

                tempWriter.Write(writer.Prepare(World));
                tempWriter.Write(writer.Prepare(Difficulty));
                tempWriter.Write(writer.Prepare(Mode));
                tempWriter.Write(writer.Prepare(WorldHeight));
                tempWriter.Write(writer.Prepare(MapSeed));

            }

            BytesWritten = ms.ToArray();
            NumBytesWritten = BytesWritten.Length;

            CheckPacketSanity();

            writer.Write(BytesWritten);
        }

        public override bool Handle(PacketHandler handler)
        {
            if (handler.HandleRespawn != null)
            {
                return handler.HandleRespawn(this);
            }
            else
            {
                return true;
            }
        }
    }

    public class PlayerPacket : Packet
    {
        public bool OnGround;

        public PlayerPacket()
            : base(PacketType.Player) { }

        public override void Read(NetworkReader reader)
        {
            MemoryStream ms = new MemoryStream();
            using (BinaryWriter writer = new BinaryWriter(ms))
            {
                writer.Write((byte)Type);
                writer.Write(reader.Read(out OnGround));
            }
            BytesRead = ms.ToArray();
            NumBytesRead = BytesRead.Length;
        }

        public override void Write(NetworkWriter writer)
        {
            MemoryStream ms = new MemoryStream();
            using (BinaryWriter tempWriter = new BinaryWriter(ms))
            {
                tempWriter.Write((byte)Type);

                tempWriter.Write(writer.Prepare(OnGround));

            }

            BytesWritten = ms.ToArray();
            NumBytesWritten = BytesWritten.Length;

            CheckPacketSanity();

            writer.Write(BytesWritten);
        }

        public override bool Handle(PacketHandler handler)
        {
            if (handler.HandlePlayer != null)
            {
                return handler.HandlePlayer(this);
            }
            else
            {
                return true;
            }
        }

    }

    public class PlayerPositionPacket : Packet
    {
        public double X;
        public double Y;
        public double Stance;
        public double Z;
        public bool OnGround;

        public PlayerPositionPacket()
            : base(PacketType.PlayerPosition) { }

        public override void Read(NetworkReader reader)
        {
            MemoryStream ms = new MemoryStream();
            using (BinaryWriter writer = new BinaryWriter(ms))
            {
                writer.Write((byte)Type);
                writer.Write(reader.Read(out X));
                writer.Write(reader.Read(out Y));
                writer.Write(reader.Read(out Stance));
                writer.Write(reader.Read(out Z));
                writer.Write(reader.Read(out OnGround));
            }
            BytesRead = ms.ToArray();
            NumBytesRead = BytesRead.Length;
        }

        public override void Write(NetworkWriter writer)
        {
            MemoryStream ms = new MemoryStream();
            using (BinaryWriter tempWriter = new BinaryWriter(ms))
            {
                tempWriter.Write((byte)Type);

                tempWriter.Write(writer.Prepare(X));
                tempWriter.Write(writer.Prepare(Y));
                tempWriter.Write(writer.Prepare(Stance));
                tempWriter.Write(writer.Prepare(Z));
                tempWriter.Write(writer.Prepare(OnGround));

            }

            BytesWritten = ms.ToArray();
            NumBytesWritten = BytesWritten.Length;

            CheckPacketSanity();

            writer.Write(BytesWritten);
        }

        public override bool Handle(PacketHandler handler)
        {
            if (handler.HandlePlayerPosition != null)
            {
                return handler.HandlePlayerPosition(this);
            }
            else
            {
                return true;
            }
        }

    }

    public class PlayerLookPacket : Packet
    {
        public float Yaw;
        public float Pitch;
        public bool OnGround;

        public PlayerLookPacket()
            : base(PacketType.PlayerLook) { }

        public override void Read(NetworkReader reader)
        {
            MemoryStream ms = new MemoryStream();
            using (BinaryWriter writer = new BinaryWriter(ms))
            {
                writer.Write((byte)Type);
                writer.Write(reader.Read(out Yaw));
                writer.Write(reader.Read(out Pitch));
                writer.Write(reader.Read(out OnGround));
            }
            BytesRead = ms.ToArray();
            NumBytesRead = BytesRead.Length;
        }

        public override void Write(NetworkWriter writer)
        {
            MemoryStream ms = new MemoryStream();
            using (BinaryWriter tempWriter = new BinaryWriter(ms))
            {
                tempWriter.Write((byte)Type);

                tempWriter.Write(writer.Prepare(Yaw));
                tempWriter.Write(writer.Prepare(Pitch));
                tempWriter.Write(writer.Prepare(OnGround));

            }

            BytesWritten = ms.ToArray();
            NumBytesWritten = BytesWritten.Length;

            CheckPacketSanity();

            writer.Write(BytesWritten);
        }

        public override bool Handle(PacketHandler handler)
        {
            if (handler.HandlePlayerLook != null)
            {
                return handler.HandlePlayerLook(this);
            }
            else
            {
                return true;
            }
        }
    }

    public class PlayerPosition_LookPacket : Packet
    {
        public double X;
        public double Y;
        public double Stance;
        public double Z;
        public float Yaw;
        public float Pitch;
        public bool OnGround;

        public PlayerPosition_LookPacket()
            : base(PacketType.PlayerPosition_Look) { }

        public override void Read(NetworkReader reader)
        {
            MemoryStream ms = new MemoryStream();
            using (BinaryWriter writer = new BinaryWriter(ms))
            {
                writer.Write((byte)Type);
                writer.Write(reader.Read(out X));

                if (reader.direction == StreamDirection.ServerToClient)
                {
                    writer.Write(reader.Read(out Stance));
                    writer.Write(reader.Read(out Y));
                }
                else
                {
                    writer.Write(reader.Read(out Y));
                    writer.Write(reader.Read(out Stance));
                }
                writer.Write(reader.Read(out Z));
                writer.Write(reader.Read(out Yaw));
                writer.Write(reader.Read(out Pitch));
                writer.Write(reader.Read(out OnGround));
            }
            BytesRead = ms.ToArray();
            NumBytesRead = BytesRead.Length;
        }

        public override void Write(NetworkWriter writer)
        {
            MemoryStream ms = new MemoryStream();
            using (BinaryWriter tempWriter = new BinaryWriter(ms))
            {
                tempWriter.Write((byte)Type);


                tempWriter.Write(writer.Prepare(X));

                if (writer.direction == StreamDirection.ServerToClient)
                {
                    tempWriter.Write(writer.Prepare(Stance));
                    tempWriter.Write(writer.Prepare(Y));
                }
                else
                {
                    tempWriter.Write(writer.Prepare(Y));
                    tempWriter.Write(writer.Prepare(Stance));
                }
                tempWriter.Write(writer.Prepare(Z));
                tempWriter.Write(writer.Prepare(Yaw));
                tempWriter.Write(writer.Prepare(Pitch));
                tempWriter.Write(writer.Prepare(OnGround));

            }

            BytesWritten = ms.ToArray();
            NumBytesWritten = BytesWritten.Length;

            CheckPacketSanity();

            writer.Write(BytesWritten);
        }

        public override bool Handle(PacketHandler handler)
        {
            if (handler.HandlePlayerPosition_Look != null)
            {
                return handler.HandlePlayerPosition_Look(this);
            }
            else
            {
                return true;
            }
        }
    }

    public class PlayerDiggingPacket : Packet
    {
        public byte Status;
        public int X;
        public byte Y;
        public int Z;
        public byte Face;

        public PlayerDiggingPacket()
            : base(PacketType.PlayerDigging) { }

        public override void Read(NetworkReader reader)
        {
            MemoryStream ms = new MemoryStream();
            using (BinaryWriter writer = new BinaryWriter(ms))
            {
                writer.Write((byte)Type);
                writer.Write(reader.Read(out Status));
                writer.Write(reader.Read(out X));
                writer.Write(reader.Read(out Y));
                writer.Write(reader.Read(out Z));
                writer.Write(reader.Read(out Face));
            }
            BytesRead = ms.ToArray();
            NumBytesRead = BytesRead.Length;
        }

        public override void Write(NetworkWriter writer)
        {
            MemoryStream ms = new MemoryStream();
            using (BinaryWriter tempWriter = new BinaryWriter(ms))
            {
                tempWriter.Write((byte)Type);

                tempWriter.Write(writer.Prepare(Status));
                tempWriter.Write(writer.Prepare(X));
                tempWriter.Write(writer.Prepare(Y));
                tempWriter.Write(writer.Prepare(Z));
                tempWriter.Write(writer.Prepare(Face));

            }

            BytesWritten = ms.ToArray();
            NumBytesWritten = BytesWritten.Length;

            CheckPacketSanity();

            writer.Write(BytesWritten);
        }

        public override bool Handle(PacketHandler handler)
        {
            if (handler.HandlePlayerDigging != null)
            {
                return handler.HandlePlayerDigging(this);
            }
            else
            {
                return true;
            }
        }
    }

    public class PlayerBlockPlacementPacket : Packet
    {
        public int X;
        public byte Y;
        public int Z;
        public byte Direction;
        public short BlockorItemID;
        public byte Amount_opt;
        public short Damage_opt;

        public PlayerBlockPlacementPacket()
            : base(PacketType.PlayerBlockPlacement) { }

        public override void Read(NetworkReader reader)
        {
            MemoryStream ms = new MemoryStream();
            using (BinaryWriter writer = new BinaryWriter(ms))
            {
                writer.Write((byte)Type);
                writer.Write(reader.Read(out X));
                writer.Write(reader.Read(out Y));
                writer.Write(reader.Read(out Z));
                writer.Write(reader.Read(out Direction));
                writer.Write(reader.Read(out BlockorItemID));
                if (BlockorItemID >= 0)
                {
                    writer.Write(reader.Read(out Amount_opt));
                    writer.Write(reader.Read(out Damage_opt));
                }
            }
            BytesRead = ms.ToArray();
            NumBytesRead = BytesRead.Length;
        }

        public override void Write(NetworkWriter writer)
        {
            MemoryStream ms = new MemoryStream();
            using (BinaryWriter tempWriter = new BinaryWriter(ms))
            {
                tempWriter.Write((byte)Type);

                tempWriter.Write(writer.Prepare(X));
                tempWriter.Write(writer.Prepare(Y));
                tempWriter.Write(writer.Prepare(Z));
                tempWriter.Write(writer.Prepare(Direction));
                tempWriter.Write(writer.Prepare(BlockorItemID));
                if (BlockorItemID >= 0)
                {
                    tempWriter.Write(writer.Prepare(Amount_opt));
                    tempWriter.Write(writer.Prepare(Damage_opt));
                }

            }

            BytesWritten = ms.ToArray();
            NumBytesWritten = BytesWritten.Length;

            CheckPacketSanity();

            writer.Write(BytesWritten);
        }

        public override bool Handle(PacketHandler handler)
        {
            if (handler.HandlePlayerBlockPlacement != null)
            {
                return handler.HandlePlayerBlockPlacement(this);
            }
            else
            {
                return true;
            }
        }
    }

    public class HoldingChangePacket : Packet
    {
        public short SlotID;
        public HoldingChangePacket()
            : base(PacketType.HoldingChange) { }

        public override void Read(NetworkReader reader)
        {
            MemoryStream ms = new MemoryStream();
            using (BinaryWriter writer = new BinaryWriter(ms))
            {
                writer.Write((byte)Type);
                writer.Write(reader.Read(out SlotID));
            }
            BytesRead = ms.ToArray();
            NumBytesRead = BytesRead.Length;
        }

        public override void Write(NetworkWriter writer)
        {
            MemoryStream ms = new MemoryStream();
            using (BinaryWriter tempWriter = new BinaryWriter(ms))
            {
                tempWriter.Write((byte)Type);

                tempWriter.Write(writer.Prepare(SlotID));

            }

            BytesWritten = ms.ToArray();
            NumBytesWritten = BytesWritten.Length;

            CheckPacketSanity();

            writer.Write(BytesWritten);
        }

        public override bool Handle(PacketHandler handler)
        {
            if (handler.HandleHoldingChange != null)
            {
                return handler.HandleHoldingChange(this);
            }
            else
            {
                return true;
            }
        }
    }

    public class UseBedPacket : Packet
    {
        public int EntityID;
        public byte InBed;
        public int Xcoordinate;
        public byte Ycoordinate;
        public int Zcoordinate;

        public UseBedPacket()
            : base(PacketType.UseBed) { }

        public override void Read(NetworkReader reader)
        {
            MemoryStream ms = new MemoryStream();
            using (BinaryWriter writer = new BinaryWriter(ms))
            {
                writer.Write((byte)Type);
                writer.Write(reader.Read(out EntityID));
                writer.Write(reader.Read(out InBed));
                writer.Write(reader.Read(out Xcoordinate));
                writer.Write(reader.Read(out Ycoordinate));
                writer.Write(reader.Read(out Zcoordinate));
            }
            BytesRead = ms.ToArray();
            NumBytesRead = BytesRead.Length;
        }

        public override void Write(NetworkWriter writer)
        {
            MemoryStream ms = new MemoryStream();
            using (BinaryWriter tempWriter = new BinaryWriter(ms))
            {
                tempWriter.Write((byte)Type);

                tempWriter.Write(writer.Prepare(EntityID));
                tempWriter.Write(writer.Prepare(InBed));
                tempWriter.Write(writer.Prepare(Xcoordinate));
                tempWriter.Write(writer.Prepare(Ycoordinate));
                tempWriter.Write(writer.Prepare(Zcoordinate));

            }

            BytesWritten = ms.ToArray();
            NumBytesWritten = BytesWritten.Length;

            CheckPacketSanity();

            writer.Write(BytesWritten);
        }

        public override bool Handle(PacketHandler handler)
        {
            if (handler.HandleUseBed != null)
            {
                return handler.HandleUseBed(this);
            }
            else
            {
                return true;
            }
        }
    }

    public class AnimationPacket : Packet
    {
        public int EID;
        public byte Animate;

        public AnimationPacket()
            : base(PacketType.Animation) { }

        public override void Read(NetworkReader reader)
        {
            MemoryStream ms = new MemoryStream();
            using (BinaryWriter writer = new BinaryWriter(ms))
            {
                writer.Write((byte)Type);
                writer.Write(reader.Read(out EID));
                writer.Write(reader.Read(out Animate));
            }
            BytesRead = ms.ToArray();
            NumBytesRead = BytesRead.Length;
        }

        public override void Write(NetworkWriter writer)
        {
            MemoryStream ms = new MemoryStream();
            using (BinaryWriter tempWriter = new BinaryWriter(ms))
            {
                tempWriter.Write((byte)Type);

                tempWriter.Write(writer.Prepare(EID));
                tempWriter.Write(writer.Prepare(Animate));

            }

            BytesWritten = ms.ToArray();
            NumBytesWritten = BytesWritten.Length;

            CheckPacketSanity();

            writer.Write(BytesWritten);
        }

        public override bool Handle(PacketHandler handler)
        {
            if (handler.HandleAnimation != null)
            {
                return handler.HandleAnimation(this);
            }
            else
            {
                return true;
            }
        }
    }

    public class EntityActionPacket : Packet
    {
        public int EID;
        public byte ActionID;

        public EntityActionPacket()
            : base(PacketType.EntityAction) { }

        public override void Read(NetworkReader reader)
        {
            MemoryStream ms = new MemoryStream();
            using (BinaryWriter writer = new BinaryWriter(ms))
            {
                writer.Write((byte)Type);
                writer.Write(reader.Read(out EID));
                writer.Write(reader.Read(out ActionID));
            }
            BytesRead = ms.ToArray();
            NumBytesRead = BytesRead.Length;
        }

        public override void Write(NetworkWriter writer)
        {
            MemoryStream ms = new MemoryStream();
            using (BinaryWriter tempWriter = new BinaryWriter(ms))
            {
                tempWriter.Write((byte)Type);

                tempWriter.Write(writer.Prepare(EID));
                tempWriter.Write(writer.Prepare(ActionID));

            }

            BytesWritten = ms.ToArray();
            NumBytesWritten = BytesWritten.Length;

            CheckPacketSanity();

            writer.Write(BytesWritten);
        }

        public override bool Handle(PacketHandler handler)
        {
            if (handler.HandleEntityAction != null)
            {
                return handler.HandleEntityAction(this);
            }
            else
            {
                return true;
            }
        }
    }

    public class NamedEntitySpawnPacket : Packet
    {
        public int EID;
        public string PlayerName;
        public int X;
        public int Y;
        public int Z;
        public short CurrentItem;
        public byte Rotation;
        public byte Pitch;

        public NamedEntitySpawnPacket()
            : base(PacketType.NamedEntitySpawn) { }

        public override void Read(NetworkReader reader)
        {
            MemoryStream ms = new MemoryStream();
            using (BinaryWriter writer = new BinaryWriter(ms))
            {
                writer.Write((byte)Type);
                writer.Write(reader.Read(out EID));
                writer.Write(reader.Read(out PlayerName));
                writer.Write(reader.Read(out X));
                writer.Write(reader.Read(out Y));
                writer.Write(reader.Read(out Z));
                writer.Write(reader.Read(out Rotation));
                writer.Write(reader.Read(out Pitch));
                writer.Write(reader.Read(out CurrentItem));
                Uberminer.Log("Player Name {0}", PlayerName);
            }
            BytesRead = ms.ToArray();
            NumBytesRead = BytesRead.Length;
        }

        public override void Write(NetworkWriter writer)
        {
            MemoryStream ms = new MemoryStream();
            using (BinaryWriter tempWriter = new BinaryWriter(ms))
            {
                tempWriter.Write((byte)Type);

                tempWriter.Write(writer.Prepare(EID));
                tempWriter.Write(writer.Prepare(PlayerName));
                tempWriter.Write(writer.Prepare(X));
                tempWriter.Write(writer.Prepare(Y));
                tempWriter.Write(writer.Prepare(Z));
                tempWriter.Write(writer.Prepare(Rotation));
                tempWriter.Write(writer.Prepare(Pitch));
                tempWriter.Write(writer.Prepare(CurrentItem));

            }

            BytesWritten = ms.ToArray();
            NumBytesWritten = BytesWritten.Length;

            CheckPacketSanity();

            writer.Write(BytesWritten);
        }

        public override bool Handle(PacketHandler handler)
        {
            if (handler.HandleNamedEntitySpawn != null)
            {
                return handler.HandleNamedEntitySpawn(this);
            }
            else
            {
                return true;
            }
        }
    }

    public class PickupSpawnPacket : Packet
    {
        public int EID;
        public short Item;
        public byte Count;
        public short Damage_Data;
        public int X;
        public int Y;
        public int Z;
        public byte Rotation;
        public byte Pitch;
        public byte Roll;

        public PickupSpawnPacket()
            : base(PacketType.PickupSpawn) { }

        public override void Read(NetworkReader reader)
        {
            MemoryStream ms = new MemoryStream();
            using (BinaryWriter writer = new BinaryWriter(ms))
            {
                writer.Write((byte)Type);
                writer.Write(reader.Read(out EID));
                writer.Write(reader.Read(out Item));
                writer.Write(reader.Read(out Count));
                writer.Write(reader.Read(out Damage_Data));
                writer.Write(reader.Read(out X));
                writer.Write(reader.Read(out Y));
                writer.Write(reader.Read(out Z));
                writer.Write(reader.Read(out Rotation));
                writer.Write(reader.Read(out Pitch));
                writer.Write(reader.Read(out Roll));
            }
            BytesRead = ms.ToArray();
            NumBytesRead = BytesRead.Length;
        }

        public override void Write(NetworkWriter writer)
        {
            MemoryStream ms = new MemoryStream();
            using (BinaryWriter tempWriter = new BinaryWriter(ms))
            {
                tempWriter.Write((byte)Type);

                tempWriter.Write(writer.Prepare(EID));
                tempWriter.Write(writer.Prepare(Item));
                tempWriter.Write(writer.Prepare(Count));
                tempWriter.Write(writer.Prepare(Damage_Data));
                tempWriter.Write(writer.Prepare(X));
                tempWriter.Write(writer.Prepare(Y));
                tempWriter.Write(writer.Prepare(Z));
                tempWriter.Write(writer.Prepare(Rotation));
                tempWriter.Write(writer.Prepare(Pitch));
                tempWriter.Write(writer.Prepare(Roll));

            }

            BytesWritten = ms.ToArray();
            NumBytesWritten = BytesWritten.Length;

            CheckPacketSanity();

            writer.Write(BytesWritten);
        }

        public override bool Handle(PacketHandler handler)
        {
            if (handler.HandlePickupSpawn != null)
            {
                return handler.HandlePickupSpawn(this);
            }
            else
            {
                return true;
            }
        }
    }

    public class CollectItemPacket : Packet
    {
        public int CollectedEID;
        public int CollectorEID;

        public CollectItemPacket()
            : base(PacketType.CollectItem) { }

        public override void Read(NetworkReader reader)
        {
            MemoryStream ms = new MemoryStream();
            using (BinaryWriter writer = new BinaryWriter(ms))
            {
                writer.Write((byte)Type);
                writer.Write(reader.Read(out CollectedEID));
                writer.Write(reader.Read(out CollectorEID));
            }
            BytesRead = ms.ToArray();
            NumBytesRead = BytesRead.Length;
        }

        public override void Write(NetworkWriter writer)
        {
            MemoryStream ms = new MemoryStream();
            using (BinaryWriter tempWriter = new BinaryWriter(ms))
            {
                tempWriter.Write((byte)Type);

                tempWriter.Write(writer.Prepare(CollectedEID));
                tempWriter.Write(writer.Prepare(CollectorEID));

            }

            BytesWritten = ms.ToArray();
            NumBytesWritten = BytesWritten.Length;

            CheckPacketSanity();

            writer.Write(BytesWritten);
        }

        public override bool Handle(PacketHandler handler)
        {
            if (handler.HandleCollectItem != null)
            {
                return handler.HandleCollectItem(this);
            }
            else
            {
                return true;
            }
        }
    }

    public class AddObject_VehiclePacket : Packet
    {
        public int EID;
        public byte ObjectType;
        public int X;
        public int Y;
        public int Z;
        public int FireballthrowerEntityID;
        public short Unknown1;
        public short Unknown2;
        public short Unknown3;

        public AddObject_VehiclePacket()
            : base(PacketType.AddObject_Vehicle) { }

        public override void Read(NetworkReader reader)
        {
            MemoryStream ms = new MemoryStream();
            using (BinaryWriter writer = new BinaryWriter(ms))
            {
                writer.Write((byte)Type);
                writer.Write(reader.Read(out EID));
                writer.Write(reader.Read(out ObjectType));
                writer.Write(reader.Read(out X));
                writer.Write(reader.Read(out Y));
                writer.Write(reader.Read(out Z));
                writer.Write(reader.Read(out FireballthrowerEntityID));
                if (FireballthrowerEntityID > 0)
                {
                    writer.Write(reader.Read(out Unknown1));
                    writer.Write(reader.Read(out Unknown2));
                    writer.Write(reader.Read(out Unknown3));
                }
            }
            BytesRead = ms.ToArray();
            NumBytesRead = BytesRead.Length;
        }

        public override void Write(NetworkWriter writer)
        {
            MemoryStream ms = new MemoryStream();
            using (BinaryWriter tempWriter = new BinaryWriter(ms))
            {
                tempWriter.Write((byte)Type);
                tempWriter.Write(writer.Prepare(EID));
                tempWriter.Write(writer.Prepare(ObjectType));
                tempWriter.Write(writer.Prepare(X));
                tempWriter.Write(writer.Prepare(Y));
                tempWriter.Write(writer.Prepare(Z));
                tempWriter.Write(writer.Prepare(FireballthrowerEntityID));
                if (FireballthrowerEntityID > 0)
                {
                    tempWriter.Write(writer.Prepare(Unknown1));
                    tempWriter.Write(writer.Prepare(Unknown2));
                    tempWriter.Write(writer.Prepare(Unknown3));
                }
            }

            BytesWritten = ms.ToArray();
            NumBytesWritten = BytesWritten.Length;

            CheckPacketSanity();

            writer.Write(BytesWritten);
        }

        public override bool Handle(PacketHandler handler)
        {
            if (handler.HandleAddObject_Vehicle != null)
            {
                return handler.HandleAddObject_Vehicle(this);
            }
            else
            {
                return true;
            }
        }
    }

    public class MobSpawnPacket : Packet
    {
        public int EID;
        public MobTypes MobType;
        public int X;
        public int Y;
        public int Z;
        public byte Yaw;
        public byte Pitch;
        public Metadata DataStream;

        public MobSpawnPacket()
            : base(PacketType.MobSpawn) { }

        public override void Read(NetworkReader reader)
        {
            MemoryStream ms = new MemoryStream();
            using (BinaryWriter writer = new BinaryWriter(ms))
            {
                writer.Write((byte)Type);
                writer.Write(reader.Read(out EID));
                byte mobType;
                writer.Write(reader.Read(out mobType));
                MobType = (MobTypes)mobType;
                writer.Write(reader.Read(out X));
                writer.Write(reader.Read(out Y));
                writer.Write(reader.Read(out Z));
                writer.Write(reader.Read(out Yaw));
                writer.Write(reader.Read(out Pitch));
                writer.Write(reader.Read(out DataStream));
            }
            BytesRead = ms.ToArray();
            NumBytesRead = BytesRead.Length;
        }

        public override void Write(NetworkWriter writer)
        {
            MemoryStream ms = new MemoryStream();
            using (BinaryWriter tempWriter = new BinaryWriter(ms))
            {
                tempWriter.Write((byte)Type);
                tempWriter.Write(writer.Prepare(EID));
                tempWriter.Write(writer.Prepare((byte)MobType));
                tempWriter.Write(writer.Prepare(X));
                tempWriter.Write(writer.Prepare(Y));
                tempWriter.Write(writer.Prepare(Z));
                tempWriter.Write(writer.Prepare(Yaw));
                tempWriter.Write(writer.Prepare(Pitch));
                tempWriter.Write(writer.Prepare(DataStream.data));
            }

            BytesWritten = ms.ToArray();
            NumBytesWritten = BytesWritten.Length;

            CheckPacketSanity();

            writer.Write(BytesWritten);
        }

        public override bool Handle(PacketHandler handler)
        {
            if (handler.HandleMobSpawn != null)
            {
                return handler.HandleMobSpawn(this);
            }
            else
            {
                return true;
            }
        }
    }

    public class EntityPaintingPacket : Packet
    {
        public int EntityID;
        public String Title;
        public int X;
        public int Y;
        public int Z;
        public int Direction;

        public EntityPaintingPacket()
            : base(PacketType.EntityPainting) { }

        public override void Read(NetworkReader reader)
        {
            MemoryStream ms = new MemoryStream();
            using (BinaryWriter writer = new BinaryWriter(ms))
            {
                writer.Write((byte)Type);
                writer.Write(reader.Read(out EntityID));
                writer.Write(reader.Read(out Title));
                writer.Write(reader.Read(out X));
                writer.Write(reader.Read(out Y));
                writer.Write(reader.Read(out Z));
                writer.Write(reader.Read(out Direction));
            }
            BytesRead = ms.ToArray();
            NumBytesRead = BytesRead.Length;
        }

        public override void Write(NetworkWriter writer)
        {
            MemoryStream ms = new MemoryStream();
            using (BinaryWriter tempWriter = new BinaryWriter(ms))
            {
                tempWriter.Write((byte)Type);

                tempWriter.Write(writer.Prepare(EntityID));
                tempWriter.Write(writer.Prepare(Title));
                tempWriter.Write(writer.Prepare(X));
                tempWriter.Write(writer.Prepare(Y));
                tempWriter.Write(writer.Prepare(Z));
                tempWriter.Write(writer.Prepare(Direction));

            }

            BytesWritten = ms.ToArray();
            NumBytesWritten = BytesWritten.Length;

            CheckPacketSanity();

            writer.Write(BytesWritten);
        }

        public override bool Handle(PacketHandler handler)
        {
            if (handler.HandleEntityPainting != null)
            {
                return handler.HandleEntityPainting(this);
            }
            else
            {
                return true;
            }
        }
    }

    public class ExperienceOrbPacket : Packet
    {
        public int EntityID;
        public int x;
        public int y;
        public int z;
        public short count;

        public ExperienceOrbPacket()
            : base(PacketType.ExperienceOrb) { }

        public override void Read(NetworkReader reader)
        {
            MemoryStream ms = new MemoryStream();
            using (BinaryWriter writer = new BinaryWriter(ms))
            {
                writer.Write((byte)Type);
                writer.Write(reader.Read(out EntityID));
                writer.Write(reader.Read(out x));
                writer.Write(reader.Read(out y));
                writer.Write(reader.Read(out z));
                writer.Write(reader.Read(out count));
            }
            BytesRead = ms.ToArray();
            NumBytesRead = BytesRead.Length;
        }

        public override void Write(NetworkWriter writer)
        {
            MemoryStream ms = new MemoryStream();
            using (BinaryWriter tempWriter = new BinaryWriter(ms))
            {
                tempWriter.Write((byte)Type);

                tempWriter.Write(writer.Prepare(EntityID));
                tempWriter.Write(writer.Prepare(x));
                tempWriter.Write(writer.Prepare(y));
                tempWriter.Write(writer.Prepare(z));
                tempWriter.Write(writer.Prepare(count));

            }

            BytesWritten = ms.ToArray();
            NumBytesWritten = BytesWritten.Length;

            CheckPacketSanity();

            writer.Write(BytesWritten);
        }

        public override bool Handle(PacketHandler handler)
        {
            if (handler.HandleExperienceOrb != null)
            {
                return handler.HandleExperienceOrb(this);
            }
            else
            {
                return true;
            }
        }
    }

    public class StanceupdatePacket : Packet
    {
        public float Unknown1;
        public float Unknown2;
        public float Unknown3;
        public float Unknown4;
        public bool Unknown5;
        public bool Unknown6;

        public StanceupdatePacket()
            : base(PacketType.Stanceupdate) { }

        public override void Read(NetworkReader reader)
        {
            MemoryStream ms = new MemoryStream();
            using (BinaryWriter writer = new BinaryWriter(ms))
            {
                writer.Write((byte)Type);
                writer.Write(reader.Read(out Unknown1));
                writer.Write(reader.Read(out Unknown2));
                writer.Write(reader.Read(out Unknown3));
                writer.Write(reader.Read(out Unknown4));
                writer.Write(reader.Read(out Unknown5));
                writer.Write(reader.Read(out Unknown6));
            }
            BytesRead = ms.ToArray();
            NumBytesRead = BytesRead.Length;
        }

        public override void Write(NetworkWriter writer)
        {
            MemoryStream ms = new MemoryStream();
            using (BinaryWriter tempWriter = new BinaryWriter(ms))
            {
                tempWriter.Write((byte)Type);

                tempWriter.Write(writer.Prepare(Unknown1));
                tempWriter.Write(writer.Prepare(Unknown2));
                tempWriter.Write(writer.Prepare(Unknown3));
                tempWriter.Write(writer.Prepare(Unknown4));
                tempWriter.Write(writer.Prepare(Unknown5));
                tempWriter.Write(writer.Prepare(Unknown6));

            }

            BytesWritten = ms.ToArray();
            NumBytesWritten = BytesWritten.Length;

            CheckPacketSanity();

            writer.Write(BytesWritten);
        }

        public override bool Handle(PacketHandler handler)
        {
            if (handler.HandleStanceupdate != null)
            {
                return handler.HandleStanceupdate(this);
            }
            else
            {
                return true;
            }
        }
    }

    public class EntityVelocityPacket : Packet
    {
        public int EntityID;
        public short VelocityX;
        public short VelocityY;
        public short VelocityZ;

        public EntityVelocityPacket()
            : base(PacketType.EntityVelocity) { }

        public override void Read(NetworkReader reader)
        {
            MemoryStream ms = new MemoryStream();
            using (BinaryWriter writer = new BinaryWriter(ms))
            {
                writer.Write((byte)Type);
                writer.Write(reader.Read(out EntityID));
                writer.Write(reader.Read(out VelocityX));
                writer.Write(reader.Read(out VelocityY));
                writer.Write(reader.Read(out VelocityZ));
            }
            BytesRead = ms.ToArray();
            NumBytesRead = BytesRead.Length;
        }

        public override void Write(NetworkWriter writer)
        {
            MemoryStream ms = new MemoryStream();
            using (BinaryWriter tempWriter = new BinaryWriter(ms))
            {
                tempWriter.Write((byte)Type);

                tempWriter.Write(writer.Prepare(EntityID));
                tempWriter.Write(writer.Prepare(VelocityX));
                tempWriter.Write(writer.Prepare(VelocityY));
                tempWriter.Write(writer.Prepare(VelocityZ));

            }

            BytesWritten = ms.ToArray();
            NumBytesWritten = BytesWritten.Length;

            CheckPacketSanity();

            writer.Write(BytesWritten);
        }

        public override bool Handle(PacketHandler handler)
        {
            if (handler.HandleEntityVelocity != null)
            {
                return handler.HandleEntityVelocity(this);
            }
            else
            {
                return true;
            }
        }
    }

    public class DestroyEntityPacket : Packet
    {
        public int EID;

        public DestroyEntityPacket()
            : base(PacketType.DestroyEntity) { }

        public override void Read(NetworkReader reader)
        {
            MemoryStream ms = new MemoryStream();
            using (BinaryWriter writer = new BinaryWriter(ms))
            {
                writer.Write((byte)Type);
                writer.Write(reader.Read(out EID));
            }
            BytesRead = ms.ToArray();
            NumBytesRead = BytesRead.Length;
        }

        public override void Write(NetworkWriter writer)
        {
            MemoryStream ms = new MemoryStream();
            using (BinaryWriter tempWriter = new BinaryWriter(ms))
            {
                tempWriter.Write((byte)Type);

                tempWriter.Write(writer.Prepare(EID));

            }

            BytesWritten = ms.ToArray();
            NumBytesWritten = BytesWritten.Length;

            CheckPacketSanity();

            writer.Write(BytesWritten);
        }

        public override bool Handle(PacketHandler handler)
        {
            if (handler.HandleDestroyEntity != null)
            {
                return handler.HandleDestroyEntity(this);
            }
            else
            {
                return true;
            }
        }
    }

    public class EntityPacket : Packet
    {
        public int EID;

        public EntityPacket()
            : base(PacketType.Entity) { }

        public override void Read(NetworkReader reader)
        {
            MemoryStream ms = new MemoryStream();
            using (BinaryWriter writer = new BinaryWriter(ms))
            {
                writer.Write((byte)Type);
                writer.Write(reader.Read(out EID));
            }
            BytesRead = ms.ToArray();
            NumBytesRead = BytesRead.Length;
        }

        public override void Write(NetworkWriter writer)
        {
            MemoryStream ms = new MemoryStream();
            using (BinaryWriter tempWriter = new BinaryWriter(ms))
            {
                tempWriter.Write((byte)Type);

                tempWriter.Write(writer.Prepare(EID));

            }

            BytesWritten = ms.ToArray();
            NumBytesWritten = BytesWritten.Length;

            CheckPacketSanity();

            writer.Write(BytesWritten);
        }

        public override bool Handle(PacketHandler handler)
        {
            if (handler.HandleEntity != null)
            {
                return handler.HandleEntity(this);
            }
            else
            {
                return true;
            }
        }
    }

    public class EntityRelativeMovePacket : Packet
    {
        public int EID;
        public byte dX;
        public byte dY;
        public byte dZ;

        public EntityRelativeMovePacket()
            : base(PacketType.EntityRelativeMove) { }

        public override void Read(NetworkReader reader)
        {
            MemoryStream ms = new MemoryStream();
            using (BinaryWriter writer = new BinaryWriter(ms))
            {
                writer.Write((byte)Type);
                writer.Write(reader.Read(out EID));
                writer.Write(reader.Read(out dX));
                writer.Write(reader.Read(out dY));
                writer.Write(reader.Read(out dZ));
            }
            BytesRead = ms.ToArray();
            NumBytesRead = BytesRead.Length;
        }

        public override void Write(NetworkWriter writer)
        {
            MemoryStream ms = new MemoryStream();
            using (BinaryWriter tempWriter = new BinaryWriter(ms))
            {
                tempWriter.Write((byte)Type);

                tempWriter.Write(writer.Prepare(EID));
                tempWriter.Write(writer.Prepare(dX));
                tempWriter.Write(writer.Prepare(dY));
                tempWriter.Write(writer.Prepare(dZ));

            }

            BytesWritten = ms.ToArray();
            NumBytesWritten = BytesWritten.Length;

            CheckPacketSanity();

            writer.Write(BytesWritten);
        }

        public override bool Handle(PacketHandler handler)
        {
            if (handler.HandleEntityRelativeMove != null)
            {
                return handler.HandleEntityRelativeMove(this);
            }
            else
            {
                return true;
            }
        }
    }

    public class EntityLookPacket : Packet
    {
        public int EID;
        public byte Yaw;
        public byte Pitch;

        public EntityLookPacket()
            : base(PacketType.EntityLook) { }

        public override void Read(NetworkReader reader)
        {
            MemoryStream ms = new MemoryStream();
            using (BinaryWriter writer = new BinaryWriter(ms))
            {
                writer.Write((byte)Type);
                writer.Write(reader.Read(out EID));
                writer.Write(reader.Read(out Yaw));
                writer.Write(reader.Read(out Pitch));
            }
            BytesRead = ms.ToArray();
            NumBytesRead = BytesRead.Length;
        }

        public override void Write(NetworkWriter writer)
        {
            MemoryStream ms = new MemoryStream();
            using (BinaryWriter tempWriter = new BinaryWriter(ms))
            {
                tempWriter.Write((byte)Type);

                tempWriter.Write(writer.Prepare(EID));
                tempWriter.Write(writer.Prepare(Yaw));
                tempWriter.Write(writer.Prepare(Pitch));

            }

            BytesWritten = ms.ToArray();
            NumBytesWritten = BytesWritten.Length;

            CheckPacketSanity();

            writer.Write(BytesWritten);
        }

        public override bool Handle(PacketHandler handler)
        {
            if (handler.HandleEntityLook != null)
            {
                return handler.HandleEntityLook(this);
            }
            else
            {
                return true;
            }
        }
    }

    public class EntityLookandRelativeMovePacket : Packet
    {
        public int EID;
        public byte dX;
        public byte dY;
        public byte dZ;
        public byte Yaw;
        public byte Pitch;

        public EntityLookandRelativeMovePacket()
            : base(PacketType.EntityLookandRelativeMove) { }

        public override void Read(NetworkReader reader)
        {
            MemoryStream ms = new MemoryStream();
            using (BinaryWriter writer = new BinaryWriter(ms))
            {
                writer.Write((byte)Type);
                writer.Write(reader.Read(out EID));
                writer.Write(reader.Read(out dX));
                writer.Write(reader.Read(out dY));
                writer.Write(reader.Read(out dZ));
                writer.Write(reader.Read(out Yaw));
                writer.Write(reader.Read(out Pitch));
            }
            BytesRead = ms.ToArray();
            NumBytesRead = BytesRead.Length;
        }

        public override void Write(NetworkWriter writer)
        {
            MemoryStream ms = new MemoryStream();
            using (BinaryWriter tempWriter = new BinaryWriter(ms))
            {
                tempWriter.Write((byte)Type);

                tempWriter.Write(writer.Prepare(EID));
                tempWriter.Write(writer.Prepare(dX));
                tempWriter.Write(writer.Prepare(dY));
                tempWriter.Write(writer.Prepare(dZ));
                tempWriter.Write(writer.Prepare(Yaw));
                tempWriter.Write(writer.Prepare(Pitch));

            }

            BytesWritten = ms.ToArray();
            NumBytesWritten = BytesWritten.Length;

            CheckPacketSanity();

            writer.Write(BytesWritten);
        }

        public override bool Handle(PacketHandler handler)
        {
            if (handler.HandleEntityLookandRelativeMove != null)
            {
                return handler.HandleEntityLookandRelativeMove(this);
            }
            else
            {
                return true;
            }
        }
    }

    public class EntityTeleportPacket : Packet
    {
        public int EID;
        public int X;
        public int Y;
        public int Z;
        public byte Yaw;
        public byte Pitch;

        public EntityTeleportPacket()
            : base(PacketType.EntityTeleport) { }

        public override void Read(NetworkReader reader)
        {
            MemoryStream ms = new MemoryStream();
            using (BinaryWriter writer = new BinaryWriter(ms))
            {
                writer.Write((byte)Type);
                writer.Write(reader.Read(out EID));
                writer.Write(reader.Read(out X));
                writer.Write(reader.Read(out Y));
                writer.Write(reader.Read(out Z));
                writer.Write(reader.Read(out Yaw));
                writer.Write(reader.Read(out Pitch));
            }
            BytesRead = ms.ToArray();
            NumBytesRead = BytesRead.Length;
        }

        public override void Write(NetworkWriter writer)
        {
            MemoryStream ms = new MemoryStream();
            using (BinaryWriter tempWriter = new BinaryWriter(ms))
            {
                tempWriter.Write((byte)Type);

                tempWriter.Write(writer.Prepare(EID));
                tempWriter.Write(writer.Prepare(X));
                tempWriter.Write(writer.Prepare(Y));
                tempWriter.Write(writer.Prepare(Z));
                tempWriter.Write(writer.Prepare(Yaw));
                tempWriter.Write(writer.Prepare(Pitch));

            }

            BytesWritten = ms.ToArray();
            NumBytesWritten = BytesWritten.Length;

            CheckPacketSanity();

            writer.Write(BytesWritten);
        }

        public override bool Handle(PacketHandler handler)
        {
            if (handler.HandleEntityTeleport != null)
            {
                return handler.HandleEntityTeleport(this);
            }
            else
            {
                return true;
            }
        }
    }

    public class EntityStatusPacket : Packet
    {
        public int EntityID;
        public Byte EntityStatus;

        public EntityStatusPacket()
            : base(PacketType.EntityStatus) { }

        public override void Read(NetworkReader reader)
        {
            MemoryStream ms = new MemoryStream();
            using (BinaryWriter writer = new BinaryWriter(ms))
            {
                writer.Write((byte)Type);
                writer.Write(reader.Read(out EntityID));
                writer.Write(reader.Read(out EntityStatus));
            }
            BytesRead = ms.ToArray();
            NumBytesRead = BytesRead.Length;
        }

        public override void Write(NetworkWriter writer)
        {
            MemoryStream ms = new MemoryStream();
            using (BinaryWriter tempWriter = new BinaryWriter(ms))
            {
                tempWriter.Write((byte)Type);

                tempWriter.Write(writer.Prepare(EntityID));
                tempWriter.Write(writer.Prepare(EntityStatus));

            }

            BytesWritten = ms.ToArray();
            NumBytesWritten = BytesWritten.Length;

            CheckPacketSanity();

            writer.Write(BytesWritten);
        }

        public override bool Handle(PacketHandler handler)
        {
            if (handler.HandleEntityStatus != null)
            {
                return handler.HandleEntityStatus(this);
            }
            else
            {
                return true;
            }
        }
    }

    public class AttachEntityPacket : Packet
    {
        public int EntityID;
        public int VehicleID;

        public AttachEntityPacket()
            : base(PacketType.AttachEntity) { }

        public override void Read(NetworkReader reader)
        {
            MemoryStream ms = new MemoryStream();
            using (BinaryWriter writer = new BinaryWriter(ms))
            {
                writer.Write((byte)Type);
                writer.Write(reader.Read(out EntityID));
                writer.Write(reader.Read(out VehicleID));
            }
            BytesRead = ms.ToArray();
            NumBytesRead = BytesRead.Length;
        }

        public override void Write(NetworkWriter writer)
        {
            MemoryStream ms = new MemoryStream();
            using (BinaryWriter tempWriter = new BinaryWriter(ms))
            {
                tempWriter.Write((byte)Type);

                tempWriter.Write(writer.Prepare(EntityID));
                tempWriter.Write(writer.Prepare(VehicleID));

            }

            BytesWritten = ms.ToArray();
            NumBytesWritten = BytesWritten.Length;

            CheckPacketSanity();

            writer.Write(BytesWritten);
        }

        public override bool Handle(PacketHandler handler)
        {
            if (handler.HandleAttachEntity != null)
            {
                return handler.HandleAttachEntity(this);
            }
            else
            {
                return true;
            }
        }
    }

    public class EntityMetadataPacket : Packet
    {
        public int EntityID;
        public Metadata EntityMetadata;

        public EntityMetadataPacket()
            : base(PacketType.EntityMetadata) { }

        public override void Read(NetworkReader reader)
        {
            MemoryStream ms = new MemoryStream();
            using (BinaryWriter writer = new BinaryWriter(ms))
            {
                writer.Write((byte)Type);
                writer.Write(reader.Read(out EntityID));
                writer.Write(reader.Read(out EntityMetadata));
            }
            BytesRead = ms.ToArray();
            NumBytesRead = BytesRead.Length;
        }

        public override void Write(NetworkWriter writer)
        {
            MemoryStream ms = new MemoryStream();
            using (BinaryWriter tempWriter = new BinaryWriter(ms))
            {
                tempWriter.Write((byte)Type);

                tempWriter.Write(writer.Prepare(EntityID));
                tempWriter.Write(writer.Prepare(EntityMetadata.data));

            }

            BytesWritten = ms.ToArray();
            NumBytesWritten = BytesWritten.Length;

            CheckPacketSanity();

            writer.Write(BytesWritten);
        }

        public override bool Handle(PacketHandler handler)
        {
            if (handler.HandleEntityMetadata != null)
            {
                return handler.HandleEntityMetadata(this);
            }
            else
            {
                return true;
            }
        }
    }

    public class EntityEffectPacket : Packet
    {
        public int EntityID;
        public byte EffectID;
        public byte Amplifier;
        public short Duration;

        public EntityEffectPacket()
            : base(PacketType.EntityEffect) { }

        public override void Read(NetworkReader reader)
        {
            MemoryStream ms = new MemoryStream();
            using (BinaryWriter writer = new BinaryWriter(ms))
            {
                writer.Write((byte)Type);
                writer.Write(reader.Read(out EntityID));
                writer.Write(reader.Read(out EffectID));
                writer.Write(reader.Read(out Amplifier));
                writer.Write(reader.Read(out Duration));
            }
            BytesRead = ms.ToArray();
            NumBytesRead = BytesRead.Length;
        }

        public override void Write(NetworkWriter writer)
        {
            MemoryStream ms = new MemoryStream();
            using (BinaryWriter tempWriter = new BinaryWriter(ms))
            {
                tempWriter.Write((byte)Type);

                tempWriter.Write(writer.Prepare(EntityID));
                tempWriter.Write(writer.Prepare(EffectID));
                tempWriter.Write(writer.Prepare(Amplifier));
                tempWriter.Write(writer.Prepare(Duration));

            }

            BytesWritten = ms.ToArray();
            NumBytesWritten = BytesWritten.Length;

            CheckPacketSanity();

            writer.Write(BytesWritten);
        }

        public override bool Handle(PacketHandler handler)
        {
            if (handler.HandleEntityEffect != null)
            {
                return handler.HandleEntityEffect(this);
            }
            else
            {
                return true;
            }
        }
    }

    public class RemoveEntityEffectPacket : Packet
    {
        public int EntityID;
        public byte EffectID;

        public RemoveEntityEffectPacket()
            : base(PacketType.RemoveEntityEffect) { }

        public override void Read(NetworkReader reader)
        {
            MemoryStream ms = new MemoryStream();
            using (BinaryWriter writer = new BinaryWriter(ms))
            {
                writer.Write((byte)Type);
                writer.Write(reader.Read(out EntityID));
                writer.Write(reader.Read(out EffectID));
            }
            BytesRead = ms.ToArray();
            NumBytesRead = BytesRead.Length;
        }

        public override void Write(NetworkWriter writer)
        {
            MemoryStream ms = new MemoryStream();
            using (BinaryWriter tempWriter = new BinaryWriter(ms))
            {
                tempWriter.Write((byte)Type);

                tempWriter.Write(writer.Prepare(EntityID));
                tempWriter.Write(writer.Prepare(EffectID));

            }

            BytesWritten = ms.ToArray();
            NumBytesWritten = BytesWritten.Length;

            CheckPacketSanity();

            writer.Write(BytesWritten);
        }

        public override bool Handle(PacketHandler handler)
        {
            if (handler.HandleRemoveEntityEffect != null)
            {
                return handler.HandleRemoveEntityEffect(this);
            }
            else
            {
                return true;
            }
        }
    }

    public class ExperiencePacket : Packet
    {
        public byte Currentexperience;
        public byte Level;
        public short Totalexperience;

        public ExperiencePacket()
            : base(PacketType.Experience) { }

        public override void Read(NetworkReader reader)
        {
            MemoryStream ms = new MemoryStream();
            using (BinaryWriter writer = new BinaryWriter(ms))
            {
                writer.Write((byte)Type);
                writer.Write(reader.Read(out Currentexperience));
                writer.Write(reader.Read(out Level));
                writer.Write(reader.Read(out Totalexperience));
            }
            BytesRead = ms.ToArray();
            NumBytesRead = BytesRead.Length;
        }

        public override void Write(NetworkWriter writer)
        {
            MemoryStream ms = new MemoryStream();
            using (BinaryWriter tempWriter = new BinaryWriter(ms))
            {
                tempWriter.Write((byte)Type);

                tempWriter.Write(writer.Prepare(Currentexperience));
                tempWriter.Write(writer.Prepare(Level));
                tempWriter.Write(writer.Prepare(Totalexperience));

            }

            BytesWritten = ms.ToArray();
            NumBytesWritten = BytesWritten.Length;

            CheckPacketSanity();

            writer.Write(BytesWritten);
        }

        public override bool Handle(PacketHandler handler)
        {
            if (handler.HandleExperience != null)
            {
                return handler.HandleExperience(this);
            }
            else
            {
                return true;
            }
        }
    }

    public class PreChunkPacket : Packet
    {
        public int X;
        public int Z;
        public bool Mode;

        public PreChunkPacket()
            : base(PacketType.PreChunk) { }

        public override void Read(NetworkReader reader)
        {
            MemoryStream ms = new MemoryStream();
            using (BinaryWriter writer = new BinaryWriter(ms))
            {
                writer.Write((byte)Type);
                writer.Write(reader.Read(out X));
                writer.Write(reader.Read(out Z));
                writer.Write(reader.Read(out Mode));
            }
            BytesRead = ms.ToArray();
            NumBytesRead = BytesRead.Length;
        }

        public override void Write(NetworkWriter writer)
        {
            MemoryStream ms = new MemoryStream();
            using (BinaryWriter tempWriter = new BinaryWriter(ms))
            {
                tempWriter.Write((byte)Type);

                tempWriter.Write(writer.Prepare(X));
                tempWriter.Write(writer.Prepare(Z));
                tempWriter.Write(writer.Prepare(Mode));

            }

            BytesWritten = ms.ToArray();
            NumBytesWritten = BytesWritten.Length;

            CheckPacketSanity();

            writer.Write(BytesWritten);
        }

        public override bool Handle(PacketHandler handler)
        {
            if (handler.HandlePreChunk != null)
            {
                return handler.HandlePreChunk(this);
            }
            else
            {
                return true;
            }
        }
    }

    public class MapChunkPacket : Packet
    {
        public int X;
        public short Y;
        public int Z;
        public byte Size_X;
        public byte Size_Y;
        public byte Size_Z;
        public int CompressedSize;
        public byte[] CompressedData;

        private byte[] chunk = null;

        public MapChunkPacket()
            : base(PacketType.MapChunk) { }

        public override void Read(NetworkReader reader)
        {
            MemoryStream ms = new MemoryStream();
            using (BinaryWriter writer = new BinaryWriter(ms))
            {
                writer.Write((byte)Type);
                writer.Write(reader.Read(out X));
                writer.Write(reader.Read(out Y));
                writer.Write(reader.Read(out Z));
                writer.Write(reader.Read(out Size_X));
                writer.Write(reader.Read(out Size_Y));
                writer.Write(reader.Read(out Size_Z));
                writer.Write(reader.Read(out CompressedSize));
                writer.Write(reader.Read(out CompressedData, CompressedSize));

                Size_X++;
                Size_Y++;
                Size_Z++;
            }
            BytesRead = ms.ToArray();
            NumBytesRead = BytesRead.Length;
        }

        public override void Write(NetworkWriter writer)
        {
            // totally cheating some more
            writer.Write(BytesRead);
        }

        public override bool Handle(PacketHandler handler)
        {
            if (handler.HandleMapChunk != null)
            {
                return handler.HandleMapChunk(this);
            }
            else
            {
                return true;
            }
        }

        public byte[] Chunk
        {
            get
            {
                if (chunk == null)
                {
                    chunk = DecompressChunk();
                }
                return chunk;
            }
        }

        private byte[] DecompressChunk()
        {
            int maxSize = (int)(16 * 16 * 128 * 2.5);
            var chunkBytes = new byte[maxSize];

            var stream = new zlib.ZStream();

            var ret = stream.inflateInit();

            if (ret != zlib.zlibConst.Z_OK)
            {
                Uberminer.Log("zlib: inflateInit failed");
                return null;
            }

            stream.avail_in = CompressedSize;
            stream.next_in = CompressedData;
            stream.avail_out = maxSize;
            stream.next_out = chunkBytes;

            ret = stream.inflate(zlib.zlibConst.Z_NO_FLUSH);

            switch (ret)
            {
                case zlib.zlibConst.Z_NEED_DICT:
                case zlib.zlibConst.Z_DATA_ERROR:
                case zlib.zlibConst.Z_MEM_ERROR:
                    Uberminer.Log("zlib: Error inflating");
                    return null;
            }

            stream.inflateEnd();

            return chunkBytes;
        }

    }

    public class MultiBlockChangePacket : Packet
    {
        public int ChunkX;
        public int ChunkZ;
        public short ArraySize;
        public short[] CoordinateArray;
        public byte[] TypeArray;
        public byte[] MetadataArray;

        public MultiBlockChangePacket()
            : base(PacketType.MultiBlockChange) { }

        public override void Read(NetworkReader reader)
        {
            MemoryStream ms = new MemoryStream();
            using (BinaryWriter writer = new BinaryWriter(ms))
            {
                writer.Write((byte)Type);
                writer.Write(reader.Read(out ChunkX));
                writer.Write(reader.Read(out ChunkZ));
                writer.Write(reader.Read(out ArraySize));
                writer.Write(reader.Read(out CoordinateArray, ArraySize));
                writer.Write(reader.Read(out TypeArray, ArraySize));
                writer.Write(reader.Read(out MetadataArray, ArraySize));
            }
            BytesRead = ms.ToArray();
            NumBytesRead = BytesRead.Length;
        }

        public override void Write(NetworkWriter writer)
        {
            MemoryStream ms = new MemoryStream();
            using (BinaryWriter tempWriter = new BinaryWriter(ms))
            {
                tempWriter.Write((byte)Type);

                tempWriter.Write(writer.Prepare(ChunkX));
                tempWriter.Write(writer.Prepare(ChunkZ));
                tempWriter.Write(writer.Prepare(ArraySize));
                foreach (var s in CoordinateArray)
                {
                    tempWriter.Write(writer.Prepare(s));
                }
                tempWriter.Write(writer.Prepare(TypeArray));
                tempWriter.Write(writer.Prepare(MetadataArray));

            }

            BytesWritten = ms.ToArray();
            NumBytesWritten = BytesWritten.Length;

            CheckPacketSanity();

            writer.Write(BytesWritten);
        }

        public override bool Handle(PacketHandler handler)
        {
            if (handler.HandleMultiBlockChange != null)
            {
                return handler.HandleMultiBlockChange(this);
            }
            else
            {
                return true;
            }
        }
    }

    public class BlockChangePacket : Packet
    {
        public int X;
        public byte Y;
        public int Z;
        public byte BlockType;
        public byte BlockMetadata;

        public BlockChangePacket()
            : base(PacketType.BlockChange) { }

        public override void Read(NetworkReader reader)
        {
            MemoryStream ms = new MemoryStream();
            using (BinaryWriter writer = new BinaryWriter(ms))
            {
                writer.Write((byte)Type);
                writer.Write(reader.Read(out X));
                writer.Write(reader.Read(out Y));
                writer.Write(reader.Read(out Z));
                writer.Write(reader.Read(out BlockType));
                writer.Write(reader.Read(out BlockMetadata));
            }
            BytesRead = ms.ToArray();
            NumBytesRead = BytesRead.Length;
        }

        public override void Write(NetworkWriter writer)
        {
            MemoryStream ms = new MemoryStream();
            using (BinaryWriter tempWriter = new BinaryWriter(ms))
            {
                tempWriter.Write((byte)Type);

                tempWriter.Write(writer.Prepare(X));
                tempWriter.Write(writer.Prepare(Y));
                tempWriter.Write(writer.Prepare(Z));
                tempWriter.Write(writer.Prepare(BlockType));
                tempWriter.Write(writer.Prepare(BlockMetadata));

            }

            BytesWritten = ms.ToArray();
            NumBytesWritten = BytesWritten.Length;

            CheckPacketSanity();

            writer.Write(BytesWritten);
        }

        public override bool Handle(PacketHandler handler)
        {
            if (handler.HandleBlockChange != null)
            {
                return handler.HandleBlockChange(this);
            }
            else
            {
                return true;
            }
        }
    }

    public class BlockActionPacket : Packet
    {
        public int X;
        public short Y;
        public int Z;
        public byte Byte1;
        public byte Byte2;

        public BlockActionPacket()
            : base(PacketType.BlockAction) { }

        public override void Read(NetworkReader reader)
        {
            MemoryStream ms = new MemoryStream();
            using (BinaryWriter writer = new BinaryWriter(ms))
            {
                writer.Write((byte)Type);
                writer.Write(reader.Read(out X));
                writer.Write(reader.Read(out Y));
                writer.Write(reader.Read(out Z));
                writer.Write(reader.Read(out Byte1));
                writer.Write(reader.Read(out Byte2));
            }
            BytesRead = ms.ToArray();
            NumBytesRead = BytesRead.Length;
        }

        public override void Write(NetworkWriter writer)
        {
            MemoryStream ms = new MemoryStream();
            using (BinaryWriter tempWriter = new BinaryWriter(ms))
            {
                tempWriter.Write((byte)Type);

                tempWriter.Write(writer.Prepare(X));
                tempWriter.Write(writer.Prepare(Y));
                tempWriter.Write(writer.Prepare(Z));
                tempWriter.Write(writer.Prepare(Byte1));
                tempWriter.Write(writer.Prepare(Byte2));

            }

            BytesWritten = ms.ToArray();
            NumBytesWritten = BytesWritten.Length;

            CheckPacketSanity();

            writer.Write(BytesWritten);
        }

        public override bool Handle(PacketHandler handler)
        {
            if (handler.HandleBlockAction != null)
            {
                return handler.HandleBlockAction(this);
            }
            else
            {
                return true;
            }
        }
    }

    public class ExplosionPacket : Packet
    {
        public double X;
        public double Y;
        public double Z;
        public float Unknown;
        public int Recordcount;
        public byte[] Records;

        public ExplosionPacket()
            : base(PacketType.Explosion) { }

        public override void Read(NetworkReader reader)
        {
            MemoryStream ms = new MemoryStream();
            using (BinaryWriter writer = new BinaryWriter(ms))
            {
                writer.Write((byte)Type);
                writer.Write(reader.Read(out X));
                writer.Write(reader.Read(out Y));
                writer.Write(reader.Read(out Z));
                writer.Write(reader.Read(out Unknown));
                writer.Write(reader.Read(out Recordcount));
                writer.Write(reader.Read(out Records, Recordcount * 3));
            }
            BytesRead = ms.ToArray();
            NumBytesRead = BytesRead.Length;
        }

        public override void Write(NetworkWriter writer)
        {
            MemoryStream ms = new MemoryStream();
            using (BinaryWriter tempWriter = new BinaryWriter(ms))
            {
                tempWriter.Write((byte)Type);

                tempWriter.Write(writer.Prepare(X));
                tempWriter.Write(writer.Prepare(Y));
                tempWriter.Write(writer.Prepare(Z));
                tempWriter.Write(writer.Prepare(Unknown));
                tempWriter.Write(writer.Prepare(Recordcount));
                tempWriter.Write(writer.Prepare(Records));

            }

            BytesWritten = ms.ToArray();
            NumBytesWritten = BytesWritten.Length;

            CheckPacketSanity();

            writer.Write(BytesWritten);
        }

        public override bool Handle(PacketHandler handler)
        {
            if (handler.HandleExplosion != null)
            {
                return handler.HandleExplosion(this);
            }
            else
            {
                return true;
            }
        }
    }

    public class SoundeffectPacket : Packet
    {
        public int EffectID;
        public int X;
        public byte Y;
        public int Z;
        public int Sounddata;

        public SoundeffectPacket()
            : base(PacketType.Soundeffect) { }

        public override void Read(NetworkReader reader)
        {
            MemoryStream ms = new MemoryStream();
            using (BinaryWriter writer = new BinaryWriter(ms))
            {
                writer.Write((byte)Type);
                writer.Write(reader.Read(out EffectID));
                writer.Write(reader.Read(out X));
                writer.Write(reader.Read(out Y));
                writer.Write(reader.Read(out Z));
                writer.Write(reader.Read(out Sounddata));
            }
            BytesRead = ms.ToArray();
            NumBytesRead = BytesRead.Length;
        }

        public override void Write(NetworkWriter writer)
        {
            MemoryStream ms = new MemoryStream();
            using (BinaryWriter tempWriter = new BinaryWriter(ms))
            {
                tempWriter.Write((byte)Type);

                tempWriter.Write(writer.Prepare(EffectID));
                tempWriter.Write(writer.Prepare(X));
                tempWriter.Write(writer.Prepare(Y));
                tempWriter.Write(writer.Prepare(Z));
                tempWriter.Write(writer.Prepare(Sounddata));

            }

            BytesWritten = ms.ToArray();
            NumBytesWritten = BytesWritten.Length;

            CheckPacketSanity();

            writer.Write(BytesWritten);
        }

        public override bool Handle(PacketHandler handler)
        {
            if (handler.HandleSoundeffect != null)
            {
                return handler.HandleSoundeffect(this);
            }
            else
            {
                return true;
            }
        }
    }

    public class New_InvalidStatePacket : Packet
    {
        public byte Reason;
        public byte Gamemode;

        public New_InvalidStatePacket()
            : base(PacketType.New_InvalidState) { }

        public override void Read(NetworkReader reader)
        {
            MemoryStream ms = new MemoryStream();
            using (BinaryWriter writer = new BinaryWriter(ms))
            {
                writer.Write((byte)Type);
                writer.Write(reader.Read(out Reason));
                writer.Write(reader.Read(out Gamemode));
            }
            BytesRead = ms.ToArray();
            NumBytesRead = BytesRead.Length;
        }

        public override void Write(NetworkWriter writer)
        {
            MemoryStream ms = new MemoryStream();
            using (BinaryWriter tempWriter = new BinaryWriter(ms))
            {
                tempWriter.Write((byte)Type);

                tempWriter.Write(writer.Prepare(Reason));
                tempWriter.Write(writer.Prepare(Gamemode));

            }

            BytesWritten = ms.ToArray();
            NumBytesWritten = BytesWritten.Length;

            CheckPacketSanity();

            writer.Write(BytesWritten);
        }

        public override bool Handle(PacketHandler handler)
        {
            if (handler.HandleNew_InvalidState != null)
            {
                return handler.HandleNew_InvalidState(this);
            }
            else
            {
                return true;
            }
        }
    }

    public class ThunderboltPacket : Packet
    {
        public int EntityID;
        public bool Unknown;
        public int X;
        public int Y;
        public int Z;

        public ThunderboltPacket()
            : base(PacketType.Thunderbolt) { }

        public override void Read(NetworkReader reader)
        {
            MemoryStream ms = new MemoryStream();
            using (BinaryWriter writer = new BinaryWriter(ms))
            {
                writer.Write((byte)Type);
                writer.Write(reader.Read(out EntityID));
                writer.Write(reader.Read(out Unknown));
                writer.Write(reader.Read(out X));
                writer.Write(reader.Read(out Y));
                writer.Write(reader.Read(out Z));
            }
            BytesRead = ms.ToArray();
            NumBytesRead = BytesRead.Length;
        }

        public override void Write(NetworkWriter writer)
        {
            MemoryStream ms = new MemoryStream();
            using (BinaryWriter tempWriter = new BinaryWriter(ms))
            {
                tempWriter.Write((byte)Type);

                tempWriter.Write(writer.Prepare(EntityID));
                tempWriter.Write(writer.Prepare(Unknown));
                tempWriter.Write(writer.Prepare(X));
                tempWriter.Write(writer.Prepare(Y));
                tempWriter.Write(writer.Prepare(Z));

            }

            BytesWritten = ms.ToArray();
            NumBytesWritten = BytesWritten.Length;

            CheckPacketSanity();

            writer.Write(BytesWritten);
        }

        public override bool Handle(PacketHandler handler)
        {
            if (handler.HandleThunderbolt != null)
            {
                return handler.HandleThunderbolt(this);
            }
            else
            {
                return true;
            }
        }
    }

    public class OpenwindowPacket : Packet
    {
        public byte Windowid;
        public byte InventoryType;
        public string Windowtitle;
        public byte NumberofSlots;

        public OpenwindowPacket()
            : base(PacketType.Openwindow) { }

        public override void Read(NetworkReader reader)
        {
            MemoryStream ms = new MemoryStream();
            using (BinaryWriter writer = new BinaryWriter(ms))
            {
                writer.Write((byte)Type);
                writer.Write(reader.Read(out Windowid));
                writer.Write(reader.Read(out InventoryType));
                writer.Write(reader.Read(out Windowtitle));
                writer.Write(reader.Read(out NumberofSlots));
            }
            BytesRead = ms.ToArray();
            NumBytesRead = BytesRead.Length;
        }

        public override void Write(NetworkWriter writer)
        {
            MemoryStream ms = new MemoryStream();
            using (BinaryWriter tempWriter = new BinaryWriter(ms))
            {
                tempWriter.Write((byte)Type);

                tempWriter.Write(writer.Prepare(Windowid));
                tempWriter.Write(writer.Prepare(InventoryType));
                tempWriter.Write(writer.Prepare(Windowtitle));
                tempWriter.Write(writer.Prepare(NumberofSlots));

            }

            BytesWritten = ms.ToArray();
            NumBytesWritten = BytesWritten.Length;

            CheckPacketSanity();

            writer.Write(BytesWritten);
        }

        public override bool Handle(PacketHandler handler)
        {
            if (handler.HandleOpenwindow != null)
            {
                return handler.HandleOpenwindow(this);
            }
            else
            {
                return true;
            }
        }
    }

    public class ClosewindowPacket : Packet
    {
        public byte Windowid;

        public ClosewindowPacket()
            : base(PacketType.Closewindow) { }

        public override void Read(NetworkReader reader)
        {
            MemoryStream ms = new MemoryStream();
            using (BinaryWriter writer = new BinaryWriter(ms))
            {
                writer.Write((byte)Type);
                writer.Write(reader.Read(out Windowid));
            }
            BytesRead = ms.ToArray();
            NumBytesRead = BytesRead.Length;
        }

        public override void Write(NetworkWriter writer)
        {
            MemoryStream ms = new MemoryStream();
            using (BinaryWriter tempWriter = new BinaryWriter(ms))
            {
                tempWriter.Write((byte)Type);

                tempWriter.Write(writer.Prepare(Windowid));

            }

            BytesWritten = ms.ToArray();
            NumBytesWritten = BytesWritten.Length;

            CheckPacketSanity();

            writer.Write(BytesWritten);
        }

        public override bool Handle(PacketHandler handler)
        {
            if (handler.HandleClosewindow != null)
            {
                return handler.HandleClosewindow(this);
            }
            else
            {
                return true;
            }
        }
    }

    public class WindowclickPacket : Packet
    {
        public byte Windowid;
        public short Slot;
        public byte Right_click;
        public short Actionnumber;
        public bool Shift;
        public short ItemID;
        public byte Itemcount;
        public short Itemuses;

        public WindowclickPacket()
            : base(PacketType.Windowclick) { }

        public override void Read(NetworkReader reader)
        {
            MemoryStream ms = new MemoryStream();
            using (BinaryWriter writer = new BinaryWriter(ms))
            {
                writer.Write((byte)Type);
                writer.Write(reader.Read(out Windowid));
                writer.Write(reader.Read(out Slot));
                writer.Write(reader.Read(out Right_click));
                writer.Write(reader.Read(out Actionnumber));
                writer.Write(reader.Read(out Shift));
                writer.Write(reader.Read(out ItemID));
                if (ItemID != -1)
                {
                    writer.Write(reader.Read(out Itemcount));
                    writer.Write(reader.Read(out Itemuses));
                }
            }
            BytesRead = ms.ToArray();
            NumBytesRead = BytesRead.Length;
        }

        public override void Write(NetworkWriter writer)
        {
            MemoryStream ms = new MemoryStream();
            using (BinaryWriter tempWriter = new BinaryWriter(ms))
            {
                tempWriter.Write((byte)Type);

                tempWriter.Write(writer.Prepare(Windowid));
                tempWriter.Write(writer.Prepare(Slot));
                tempWriter.Write(writer.Prepare(Right_click));
                tempWriter.Write(writer.Prepare(Actionnumber));
                tempWriter.Write(writer.Prepare(Shift));
                tempWriter.Write(writer.Prepare(ItemID));
                if (ItemID != -1)
                {
                    tempWriter.Write(writer.Prepare(Itemcount));
                    tempWriter.Write(writer.Prepare(Itemuses));
                }
            }

            BytesWritten = ms.ToArray();
            NumBytesWritten = BytesWritten.Length;

            CheckPacketSanity();

            writer.Write(BytesWritten);
        }

        public override bool Handle(PacketHandler handler)
        {
            if (handler.HandleWindowclick != null)
            {
                return handler.HandleWindowclick(this);
            }
            else
            {
                return true;
            }
        }
    }

    public class SetslotPacket : Packet
    {
        public byte Windowid;
        public short Slot;
        public short ItemID;
        public byte ItemCount;
        public short Itemuses;

        public SetslotPacket()
            : base(PacketType.Setslot) { }

        public override void Read(NetworkReader reader)
        {
            MemoryStream ms = new MemoryStream();
            using (BinaryWriter writer = new BinaryWriter(ms))
            {
                writer.Write((byte)Type);
                writer.Write(reader.Read(out Windowid));
                writer.Write(reader.Read(out Slot));
                writer.Write(reader.Read(out ItemID));
                if (ItemID != -1)
                {
                    writer.Write(reader.Read(out ItemCount));
                    writer.Write(reader.Read(out Itemuses));
                }
            }
            BytesRead = ms.ToArray();
            NumBytesRead = BytesRead.Length;
        }

        public override void Write(NetworkWriter writer)
        {
            // cheating on this one as well
            writer.Write(BytesRead);
        }

        public override bool Handle(PacketHandler handler)
        {
            if (handler.HandleSetslot != null)
            {
                return handler.HandleSetslot(this);
            }
            else
            {
                return true;
            }
        }
    }

    public class WindowitemsPacket : Packet
    {
        public byte Windowid;
        public short Count;

        public WindowitemsPacket()
            : base(PacketType.Windowitems) { }

        public override void Read(NetworkReader reader)
        {
            MemoryStream ms = new MemoryStream();
            using (BinaryWriter writer = new BinaryWriter(ms))
            {
                writer.Write((byte)Type);
                writer.Write(reader.Read(out Windowid));
                writer.Write(reader.Read(out Count));

                /*
                 offset = 0
 
                 for slot in count:
                     item_id = payload[offset] as short
                     offset += 2
                     if item_id is not equal to -1:
                         count = payload[offset] as byte
                         offset += 1
                         uses = payload[offset] as short
                         offset += 2
                         inventory[slot] = new item(item_id, count, uses)
                     else:
                         inventory[slot] = None
                        }
                 */
                WindowItemPayload payload;
                writer.Write(reader.Read(out payload, Count));
            }
            BytesRead = ms.ToArray();
            NumBytesRead = BytesRead.Length;
        }

        public override void Write(NetworkWriter writer)
        {
            // going to cheat on this one
            writer.Write(BytesRead);
        }

        public override bool Handle(PacketHandler handler)
        {
            if (handler.HandleWindowitems != null)
            {
                return handler.HandleWindowitems(this);
            }
            else
            {
                return true;
            }
        }
    }

    public class UpdateprogressbarPacket : Packet
    {
        public byte Windowid;
        public short Progressbar;
        public short Value;

        public UpdateprogressbarPacket()
            : base(PacketType.Updateprogressbar) { }

        public override void Read(NetworkReader reader)
        {
            MemoryStream ms = new MemoryStream();
            using (BinaryWriter writer = new BinaryWriter(ms))
            {
                writer.Write((byte)Type);
                writer.Write(reader.Read(out Windowid));
                writer.Write(reader.Read(out Progressbar));
                writer.Write(reader.Read(out Value));
            }
            BytesRead = ms.ToArray();
            NumBytesRead = BytesRead.Length;
        }

        public override void Write(NetworkWriter writer)
        {
            MemoryStream ms = new MemoryStream();
            using (BinaryWriter tempWriter = new BinaryWriter(ms))
            {
                tempWriter.Write((byte)Type);

                tempWriter.Write(writer.Prepare(Windowid));
                tempWriter.Write(writer.Prepare(Progressbar));
                tempWriter.Write(writer.Prepare(Value));

            }

            BytesWritten = ms.ToArray();
            NumBytesWritten = BytesWritten.Length;

            CheckPacketSanity();

            writer.Write(BytesWritten);
        }

        public override bool Handle(PacketHandler handler)
        {
            if (handler.HandleUpdateprogressbar != null)
            {
                return handler.HandleUpdateprogressbar(this);
            }
            else
            {
                return true;
            }
        }
    }

    public class TransactionPacket : Packet
    {
        public byte Windowid;
        public short Actionnumber;
        public bool Accepted;

        public TransactionPacket()
            : base(PacketType.Transaction) { }

        public override void Read(NetworkReader reader)
        {
            MemoryStream ms = new MemoryStream();
            using (BinaryWriter writer = new BinaryWriter(ms))
            {
                writer.Write((byte)Type);
                writer.Write(reader.Read(out Windowid));
                writer.Write(reader.Read(out Actionnumber));
                writer.Write(reader.Read(out Accepted));
            }
            BytesRead = ms.ToArray();
            NumBytesRead = BytesRead.Length;
        }

        public override void Write(NetworkWriter writer)
        {
            MemoryStream ms = new MemoryStream();
            using (BinaryWriter tempWriter = new BinaryWriter(ms))
            {
                tempWriter.Write((byte)Type);

                tempWriter.Write(writer.Prepare(Windowid));
                tempWriter.Write(writer.Prepare(Actionnumber));
                tempWriter.Write(writer.Prepare(Accepted));

            }

            BytesWritten = ms.ToArray();
            NumBytesWritten = BytesWritten.Length;

            CheckPacketSanity();

            writer.Write(BytesWritten);
        }

        public override bool Handle(PacketHandler handler)
        {
            if (handler.HandleTransaction != null)
            {
                return handler.HandleTransaction(this);
            }
            else
            {
                return true;
            }
        }
    }

    public class CreativeinventoryactionPacket : Packet
    {
        public short Slot;
        public short Itemid;
        public short Quantity;
        public short Damage;

        public CreativeinventoryactionPacket()
            : base(PacketType.Creativeinventoryaction) { }

        public override void Read(NetworkReader reader)
        {
            MemoryStream ms = new MemoryStream();
            using (BinaryWriter writer = new BinaryWriter(ms))
            {
                writer.Write((byte)Type);
                writer.Write(reader.Read(out Slot));
                writer.Write(reader.Read(out Itemid));
                writer.Write(reader.Read(out Quantity));
                writer.Write(reader.Read(out Damage));
            }
            BytesRead = ms.ToArray();
            NumBytesRead = BytesRead.Length;
        }

        public override void Write(NetworkWriter writer)
        {
            MemoryStream ms = new MemoryStream();
            using (BinaryWriter tempWriter = new BinaryWriter(ms))
            {
                tempWriter.Write((byte)Type);

                tempWriter.Write(writer.Prepare(Slot));
                tempWriter.Write(writer.Prepare(Itemid));
                tempWriter.Write(writer.Prepare(Quantity));
                tempWriter.Write(writer.Prepare(Damage));

            }

            BytesWritten = ms.ToArray();
            NumBytesWritten = BytesWritten.Length;

            CheckPacketSanity();

            writer.Write(BytesWritten);
        }

        public override bool Handle(PacketHandler handler)
        {
            if (handler.HandleCreativeinventoryaction != null)
            {
                return handler.HandleCreativeinventoryaction(this);
            }
            else
            {
                return true;
            }
        }
    }

    public class UpdateSignPacket : Packet
    {
        public int X;
        public short Y;
        public int Z;
        public string Text1;
        public string Text2;
        public string Text3;
        public string Text4;

        public UpdateSignPacket()
            : base(PacketType.UpdateSign) { }

        public override void Read(NetworkReader reader)
        {
            MemoryStream ms = new MemoryStream();
            using (BinaryWriter writer = new BinaryWriter(ms))
            {
                writer.Write((byte)Type);
                writer.Write(reader.Read(out X));
                writer.Write(reader.Read(out Y));
                writer.Write(reader.Read(out Z));
                writer.Write(reader.Read(out Text1));
                writer.Write(reader.Read(out Text2));
                writer.Write(reader.Read(out Text3));
                writer.Write(reader.Read(out Text4));
            }
            BytesRead = ms.ToArray();
            NumBytesRead = BytesRead.Length;
        }

        public override void Write(NetworkWriter writer)
        {
            MemoryStream ms = new MemoryStream();
            using (BinaryWriter tempWriter = new BinaryWriter(ms))
            {
                tempWriter.Write((byte)Type);

                tempWriter.Write(writer.Prepare(X));
                tempWriter.Write(writer.Prepare(Y));
                tempWriter.Write(writer.Prepare(Z));
                tempWriter.Write(writer.Prepare(Text1));
                tempWriter.Write(writer.Prepare(Text2));
                tempWriter.Write(writer.Prepare(Text3));
                tempWriter.Write(writer.Prepare(Text4));

            }

            BytesWritten = ms.ToArray();
            NumBytesWritten = BytesWritten.Length;

            CheckPacketSanity();

            writer.Write(BytesWritten);
        }

        public override bool Handle(PacketHandler handler)
        {
            if (handler.HandleUpdateSign != null)
            {
                return handler.HandleUpdateSign(this);
            }
            else
            {
                return true;
            }
        }
    }

    public class ItemDataPacket : Packet
    {
        public short ItemType;
        public short ItemID;
        public byte Textlength;
        public byte[] Text;

        public ItemDataPacket()
            : base(PacketType.ItemData) { }

        public override void Read(NetworkReader reader)
        {
            MemoryStream ms = new MemoryStream();
            using (BinaryWriter writer = new BinaryWriter(ms))
            {
                writer.Write((byte)Type);
                writer.Write(reader.Read(out ItemType));
                writer.Write(reader.Read(out ItemID));
                writer.Write(reader.Read(out Textlength));
                writer.Write(reader.Read(out Text, Textlength));
            }
            BytesRead = ms.ToArray();
            NumBytesRead = BytesRead.Length;
        }

        public override void Write(NetworkWriter writer)
        {
            MemoryStream ms = new MemoryStream();
            using (BinaryWriter tempWriter = new BinaryWriter(ms))
            {
                tempWriter.Write((byte)Type);

                tempWriter.Write(writer.Prepare(ItemType));
                tempWriter.Write(writer.Prepare(ItemID));
                tempWriter.Write(writer.Prepare(Textlength));
                tempWriter.Write(writer.Prepare(Text));

            }

            BytesWritten = ms.ToArray();
            NumBytesWritten = BytesWritten.Length;

            CheckPacketSanity();

            writer.Write(BytesWritten);
        }

        public override bool Handle(PacketHandler handler)
        {
            if (handler.HandleItemData != null)
            {
                return handler.HandleItemData(this);
            }
            else
            {
                return true;
            }
        }
    }

    public class IncrementStatisticPacket : Packet
    {
        public int StatisticID;
        public byte Amount;

        public IncrementStatisticPacket()
            : base(PacketType.IncrementStatistic) { }

        public override void Read(NetworkReader reader)
        {
            MemoryStream ms = new MemoryStream();
            using (BinaryWriter writer = new BinaryWriter(ms))
            {
                writer.Write((byte)Type);
                writer.Write(reader.Read(out StatisticID));
                writer.Write(reader.Read(out Amount));
            }
            BytesRead = ms.ToArray();
            NumBytesRead = BytesRead.Length;
        }

        public override void Write(NetworkWriter writer)
        {
            MemoryStream ms = new MemoryStream();
            using (BinaryWriter tempWriter = new BinaryWriter(ms))
            {
                tempWriter.Write((byte)Type);

                tempWriter.Write(writer.Prepare(StatisticID));
                tempWriter.Write(writer.Prepare(Amount));

            }

            BytesWritten = ms.ToArray();
            NumBytesWritten = BytesWritten.Length;

            CheckPacketSanity();

            writer.Write(BytesWritten);
        }

        public override bool Handle(PacketHandler handler)
        {
            if (handler.HandleIncrementStatistic != null)
            {
                return handler.HandleIncrementStatistic(this);
            }
            else
            {
                return true;
            }
        }
    }

    public class PlayerListItemPacket : Packet
    {
        public string Playername;
        public bool Online;
        public short Ping;

        public PlayerListItemPacket()
            : base(PacketType.PlayerListItem) { }

        public override void Read(NetworkReader reader)
        {
            MemoryStream ms = new MemoryStream();
            using (BinaryWriter writer = new BinaryWriter(ms))
            {
                writer.Write((byte)Type);
                writer.Write(reader.Read(out Playername));
                writer.Write(reader.Read(out Online));
                writer.Write(reader.Read(out Ping));
            }
            BytesRead = ms.ToArray();
            NumBytesRead = BytesRead.Length;
        }

        public override void Write(NetworkWriter writer)
        {
            MemoryStream ms = new MemoryStream();
            using (BinaryWriter tempWriter = new BinaryWriter(ms))
            {
                tempWriter.Write((byte)Type);

                tempWriter.Write(writer.Prepare(Playername));
                tempWriter.Write(writer.Prepare(Online));
                tempWriter.Write(writer.Prepare(Ping));

            }

            BytesWritten = ms.ToArray();
            NumBytesWritten = BytesWritten.Length;

            CheckPacketSanity();

            writer.Write(BytesWritten);
        }

        public override bool Handle(PacketHandler handler)
        {
            if (handler.HandlePlayerListItem != null)
            {
                return handler.HandlePlayerListItem(this);
            }
            else
            {
                return true;
            }
        }
    }

    public class ServerListPingPacket : Packet
    {
        public ServerListPingPacket()
            : base(PacketType.ServerListPing) { }

        public override void Read(NetworkReader reader)
        {
            MemoryStream ms = new MemoryStream();
            using (BinaryWriter writer = new BinaryWriter(ms))
            {
                writer.Write((byte)Type);
            }
            BytesRead = ms.ToArray();
            NumBytesRead = BytesRead.Length;
        }

        public override void Write(NetworkWriter writer)
        {
            MemoryStream ms = new MemoryStream();
            using (BinaryWriter tempWriter = new BinaryWriter(ms))
            {
                tempWriter.Write((byte)Type);


            }

            BytesWritten = ms.ToArray();
            NumBytesWritten = BytesWritten.Length;

            CheckPacketSanity();

            writer.Write(BytesWritten);
        }

        public override bool Handle(PacketHandler handler)
        {
            if (handler.HandleServerListPing != null)
            {
                return handler.HandleServerListPing(this);
            }
            else
            {
                return true;
            }
        }
    }

    public class Disconnect_KickPacket : Packet
    {
        public string Reason;
        public Disconnect_KickPacket()
            : base(PacketType.Disconnect_Kick) { }

        public override void Read(NetworkReader reader)
        {
            MemoryStream ms = new MemoryStream();
            using (BinaryWriter writer = new BinaryWriter(ms))
            {
                writer.Write((byte)Type);
                writer.Write(reader.Read(out Reason));
            }
            BytesRead = ms.ToArray();
            NumBytesRead = BytesRead.Length;
        }

        public override void Write(NetworkWriter writer)
        {
            MemoryStream ms = new MemoryStream();
            using (BinaryWriter tempWriter = new BinaryWriter(ms))
            {
                tempWriter.Write((byte)Type);

                tempWriter.Write(writer.Prepare(Reason));

            }

            BytesWritten = ms.ToArray();
            NumBytesWritten = BytesWritten.Length;

            CheckPacketSanity();

            writer.Write(BytesWritten);
        }

        public override bool Handle(PacketHandler handler)
        {
            if (handler.HandleDisconnect_Kick != null)
            {
                return handler.HandleDisconnect_Kick(this);
            }
            else
            {
                return true;
            }
        }
    }

    public enum PacketType : byte
    {
        KeepAlive = 0x00,
        LoginRequest = 0x01,
        Handshake = 0x02,
        ChatMessage = 0x03,
        TimeUpdate = 0x04,
        EntityEquipment = 0x05,
        SpawnPosition = 0x06,
        UseEntity = 0x07,
        UpdateHealth = 0x08,
        Respawn = 0x09,
        Player = 0x0A,
        PlayerPosition = 0x0B,
        PlayerLook = 0x0C,
        PlayerPosition_Look = 0x0D,
        PlayerDigging = 0x0E,
        PlayerBlockPlacement = 0x0F,
        HoldingChange = 0x10,
        UseBed = 0x11,
        Animation = 0x12,
        EntityAction = 0x13,
        NamedEntitySpawn = 0x14,
        PickupSpawn = 0x15,
        CollectItem = 0x16,
        AddObject_Vehicle = 0x17,
        MobSpawn = 0x18,
        EntityPainting = 0x19,
        ExperienceOrb = 0x1A,
        Stanceupdate = 0x1B,
        EntityVelocity = 0x1C,
        DestroyEntity = 0x1D,
        Entity = 0x1E,
        EntityRelativeMove = 0x1F,
        EntityLook = 0x20,
        EntityLookandRelativeMove = 0x21,
        EntityTeleport = 0x22,
        EntityStatus = 0x26,
        AttachEntity = 0x27,
        EntityMetadata = 0x28,
        EntityEffect = 0x29,
        RemoveEntityEffect = 0x2A,
        Experience = 0x2B,
        PreChunk = 0x32,
        MapChunk = 0x33,
        MultiBlockChange = 0x34,
        BlockChange = 0x35,
        BlockAction = 0x36,
        Explosion = 0x3C,
        Soundeffect = 0x3D,
        New_InvalidState = 0x46,
        Thunderbolt = 0x47,
        Openwindow = 0x64,
        Closewindow = 0x65,
        Windowclick = 0x66,
        Setslot = 0x67,
        Windowitems = 0x68,
        Updateprogressbar = 0x69,
        Transaction = 0x6A,
        Creativeinventoryaction = 0x6B,
        UpdateSign = 0x82,
        ItemData = 0x83,
        IncrementStatistic = 0xC8,
        PlayerListItem = 0xC9,
        ServerListPing = 0xFE,
        Disconnect_Kick = 0xFF,
    }

    public enum MobTypes : byte
    {
        Creeper = 50,
        Skeleton = 51,
        Spider = 52,
        GiantZombie = 53,
        Zombie = 54,
        Slime = 55,
        Ghast = 56,
        ZombiePigman = 57,
        Enderman = 58,
        CaveSpider = 59,
        Silverfish = 60,
        Blaze = 61,
        MagmaCube = 62,
        Pig = 90,
        Sheep = 91,
        Cow = 92,
        Hen = 93,
        Squid = 94,
        Wolf = 95,
        Snowman = 97,
        Villager = 120,
    }

    public class Metadata
    {
        public byte[] data;
        public Metadata()
        {
        }
    }

    public class WindowItemPayload
    {
        public byte[] data;
        public WindowItemPayload()
        {
        }
    }

    public class MapChunk
    {
        public MapChunk(byte[] compressedData)
        {
        }
    }
}
