using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
            BytesRead += reader.Read(out KAId);
        }

        public override void Write(NetworkWriter writer)
        {
            writer.Write(KAId);
        }

        public override void Handle(PacketHandler handler)
        {
            handler.Handle(this);
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
            BytesRead += reader.Read(out EntityId);
            {
                // disabling this stuff.
                // as we are only reading server to client we need to
                // not read a string as it will make us out of sync with the stream
                // TODO: Determine which stream we're reading.
                short t;
                BytesRead += reader.Read(out t);
                //BytesRead += reader.ReadS16(out Username);
            }
            BytesRead += reader.Read(out MapSeed);
            BytesRead += reader.Read(out ServerMode);
            BytesRead += reader.Read(out Dimension);
            BytesRead += reader.Read(out Difficulty);
            BytesRead += reader.Read(out WorldHeight);
            BytesRead += reader.Read(out MaxPlayers);
        }

        public override void Write(NetworkWriter writer)
        {
            writer.Write(EntityId);
            writer.Write(Username);
            writer.Write(MapSeed);
            writer.Write(Dimension);
            writer.Write(Difficulty);
            writer.Write(WorldHeight);
            writer.Write(MaxPlayers);
        }

        public override void Handle(PacketHandler handler)
        {
            handler.Handle(this);
        }

    }

    public class HandshakePacket : Packet
    {
        public string Hash;

        public HandshakePacket()
            : base(PacketType.Handshake) { }

        public override void Read(NetworkReader reader)
        {
            BytesRead += reader.ReadS16(out Hash);
        }

        public override void Write(NetworkWriter writer)
        {
            writer.Write(Hash);
        }

        public override void Handle(PacketHandler handler)
        {
            handler.Handle(this);
        }
    }

    public class ChatMessagePacket : Packet
    {
        public string Message;

        public ChatMessagePacket()
            : base(PacketType.ChatMessage) { }

        public override void Read(NetworkReader reader)
        {
            BytesRead += reader.ReadS16(out Message);
            Program.Log("Chat: {0}", Message);
        }

        public override void Write(NetworkWriter writer)
        {
            writer.Write(Message);
        }

        public override void Handle(PacketHandler handler)
        {
            handler.Handle(this);
        }
    }

    public class TimeUpdatePacket : Packet
    {
        public long Time;

        public TimeUpdatePacket()
            : base(PacketType.TimeUpdate) { }

        public override void Read(NetworkReader reader)
        {
            BytesRead += reader.Read(out Time);
        }

        public override void Write(NetworkWriter writer)
        {
            writer.Write(Time);
        }

        public override void Handle(PacketHandler handler)
        {
            handler.Handle(this);
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
            BytesRead += reader.Read(out EntityId);
            BytesRead += reader.Read(out Slot);
            BytesRead += reader.Read(out ItemId);
            BytesRead += reader.Read(out Damage);
        }

        public override void Write(NetworkWriter writer)
        {
            writer.Write(EntityId);
            writer.Write(Slot);
            writer.Write(ItemId);
            writer.Write(Damage);

        }

        public override void Handle(PacketHandler handler)
        {
            handler.Handle(this);
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
            BytesRead += reader.Read(out X);
            BytesRead += reader.Read(out Y);
            BytesRead += reader.Read(out Z);
        }

        public override void Write(NetworkWriter writer)
        {
            writer.Write(X);
            writer.Write(Y);
            writer.Write(Z);
        }

        public override void Handle(PacketHandler handler)
        {
            handler.Handle(this);
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
            BytesRead += reader.Read(out User);
            BytesRead += reader.Read(out Target);
            BytesRead += reader.Read(out LeftClick);
        }

        public override void Write(NetworkWriter writer)
        {
            writer.Write(User);
            writer.Write(Target);
            writer.Write(LeftClick);
        }

        public override void Handle(PacketHandler handler)
        {
            handler.Handle(this);
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
            BytesRead += reader.Read(out Health);
            BytesRead += reader.Read(out Food);
            BytesRead += reader.Read(out FoodSaturation);
        }

        public override void Write(NetworkWriter writer)
        {
            writer.Write(Health);
            writer.Write(Food);
            writer.Write(FoodSaturation);
        }

        public override void Handle(PacketHandler handler)
        {
            handler.Handle(this);
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
            BytesRead += reader.Read(out World);
            BytesRead += reader.Read(out Difficulty);
            BytesRead += reader.Read(out Mode);
            BytesRead += reader.Read(out WorldHeight);
            BytesRead += reader.Read(out MapSeed);
        }

        public override void Write(NetworkWriter writer)
        {
            writer.Write(World);
            writer.Write(Difficulty);
            writer.Write(Mode);
            writer.Write(WorldHeight);
            writer.Write(MapSeed);
        }

        public override void Handle(PacketHandler handler)
        {
            handler.Handle(this);
        }
    }

    public class PlayerPacket : Packet
    {
        public bool OnGround;

        public PlayerPacket()
            : base(PacketType.Player) { }

        public override void Read(NetworkReader reader)
        {
            BytesRead += reader.Read(out OnGround);
        }

        public override void Write(NetworkWriter writer)
        {
            writer.Write(OnGround);
        }

        public override void Handle(PacketHandler handler)
        {
            handler.Handle(this);
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
            BytesRead += reader.Read(out X);
            BytesRead += reader.Read(out Y);
            BytesRead += reader.Read(out Stance);
            BytesRead += reader.Read(out Z);
            BytesRead += reader.Read(out OnGround);
        }

        public override void Write(NetworkWriter writer)
        {
            writer.Write(X);
            writer.Write(Y);
            writer.Write(Stance);
            writer.Write(Z);
            writer.Write(OnGround);
        }

        public override void Handle(PacketHandler handler)
        {
            handler.Handle(this);
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
            BytesRead += reader.Read(out Yaw);
            BytesRead += reader.Read(out Pitch);
            BytesRead += reader.Read(out OnGround);
        }

        public override void Write(NetworkWriter writer)
        {
            writer.Write(Yaw);
            writer.Write(Pitch);
            writer.Write(OnGround);
        }

        public override void Handle(PacketHandler handler)
        {
            handler.Handle(this);
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
            BytesRead += reader.Read(out X);
            BytesRead += reader.Read(out Y);
            BytesRead += reader.Read(out Stance);
            BytesRead += reader.Read(out Z);
            BytesRead += reader.Read(out Yaw);
            BytesRead += reader.Read(out Pitch);
            BytesRead += reader.Read(out OnGround);
        }

        public override void Write(NetworkWriter writer)
        {
            writer.Write(X);
            writer.Write(Y);
            writer.Write(Stance);
            writer.Write(Z);
            writer.Write(Yaw);
            writer.Write(Pitch);
            writer.Write(OnGround);
        }

        public override void Handle(PacketHandler handler)
        {
            handler.Handle(this);
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
            BytesRead += reader.Read(out Status);
            BytesRead += reader.Read(out X);
            BytesRead += reader.Read(out Y);
            BytesRead += reader.Read(out Z);
            BytesRead += reader.Read(out Face);
        }

        public override void Write(NetworkWriter writer)
        {
            writer.Write(Status);
            writer.Write(X);
            writer.Write(Y);
            writer.Write(Z);
            writer.Write(Face);
        }

        public override void Handle(PacketHandler handler)
        {
            handler.Handle(this);
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
            BytesRead += reader.Read(out X);
            BytesRead += reader.Read(out Y);
            BytesRead += reader.Read(out Z);
            BytesRead += reader.Read(out Direction);
            BytesRead += reader.Read(out BlockorItemID);
            if (BlockorItemID >= 0)
            {
                BytesRead += reader.Read(out Amount_opt);
                BytesRead += reader.Read(out Damage_opt);
            }
        }

        public override void Write(NetworkWriter writer)
        {
            writer.Write(X);
            writer.Write(Y);
            writer.Write(Z);
            writer.Write(Direction);
            writer.Write(BlockorItemID);
            writer.Write(Amount_opt);
            writer.Write(Damage_opt);
        }

        public override void Handle(PacketHandler handler)
        {
            handler.Handle(this);
        }
    }

    public class HoldingChangePacket : Packet
    {
        public short SlotID;
        public HoldingChangePacket()
            : base(PacketType.HoldingChange) { }

        public override void Read(NetworkReader reader)
        {
            BytesRead += reader.Read(out SlotID);
        }

        public override void Write(NetworkWriter writer)
        {
            writer.Write(SlotID);
        }

        public override void Handle(PacketHandler handler)
        {
            handler.Handle(this);
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
            BytesRead += reader.Read(out EntityID);
            BytesRead += reader.Read(out InBed);
            BytesRead += reader.Read(out Xcoordinate);
            BytesRead += reader.Read(out Ycoordinate);
            BytesRead += reader.Read(out Zcoordinate);
        }

        public override void Write(NetworkWriter writer)
        {
            writer.Write(EntityID);
            writer.Write(InBed);
            writer.Write(Xcoordinate);
            writer.Write(Ycoordinate);
            writer.Write(Zcoordinate);
        }

        public override void Handle(PacketHandler handler)
        {
            handler.Handle(this);
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
            BytesRead += reader.Read(out EID);
            BytesRead += reader.Read(out Animate);
        }

        public override void Write(NetworkWriter writer)
        {
            writer.Write(EID);
            writer.Write(Animate);
        }

        public override void Handle(PacketHandler handler)
        {
            handler.Handle(this);
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
            BytesRead += reader.Read(out EID);
            BytesRead += reader.Read(out ActionID);
        }

        public override void Write(NetworkWriter writer)
        {
            writer.Write(EID);
            writer.Write(ActionID);
        }

        public override void Handle(PacketHandler handler)
        {
            handler.Handle(this);
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
            BytesRead += reader.Read(out EID);
            BytesRead += reader.ReadS16(out PlayerName);
            BytesRead += reader.Read(out X);
            BytesRead += reader.Read(out Y);
            BytesRead += reader.Read(out Z);
            BytesRead += reader.Read(out Rotation);
            BytesRead += reader.Read(out Pitch);
            BytesRead += reader.Read(out CurrentItem);
            Program.Log("Player Name {0}", PlayerName);
        }

        public override void Write(NetworkWriter writer)
        {
            writer.Write(EID);
            writer.Write(PlayerName);
            writer.Write(X);
            writer.Write(Y);
            writer.Write(Z);
            writer.Write(Rotation);
            writer.Write(Pitch);
            writer.Write(CurrentItem);
        }

        public override void Handle(PacketHandler handler)
        {
            handler.Handle(this);
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
            BytesRead += reader.Read(out EID);
            BytesRead += reader.Read(out Item);
            BytesRead += reader.Read(out Count);
            BytesRead += reader.Read(out Damage_Data);
            BytesRead += reader.Read(out X);
            BytesRead += reader.Read(out Y);
            BytesRead += reader.Read(out Z);
            BytesRead += reader.Read(out Rotation);
            BytesRead += reader.Read(out Pitch);
            BytesRead += reader.Read(out Roll);
        }

        public override void Write(NetworkWriter writer)
        {
            writer.Write(EID);
            writer.Write(Item);
            writer.Write(Count);
            writer.Write(Damage_Data);
            writer.Write(X);
            writer.Write(Y);
            writer.Write(Z);
            writer.Write(Rotation);
            writer.Write(Pitch);
            writer.Write(Roll);
        }

        public override void Handle(PacketHandler handler)
        {
            handler.Handle(this);
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
            BytesRead += reader.Read(out CollectedEID);
            BytesRead += reader.Read(out CollectorEID);
        }

        public override void Write(NetworkWriter writer)
        {
            writer.Write(CollectedEID);
            writer.Write(CollectorEID);
        }

        public override void Handle(PacketHandler handler)
        {
            handler.Handle(this);
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
            BytesRead += reader.Read(out EID);
            BytesRead += reader.Read(out ObjectType);
            BytesRead += reader.Read(out X);
            BytesRead += reader.Read(out Y);
            BytesRead += reader.Read(out Z);
            BytesRead += reader.Read(out FireballthrowerEntityID);
            BytesRead += reader.Read(out Unknown1);
            BytesRead += reader.Read(out Unknown2);
            BytesRead += reader.Read(out Unknown3);
        }

        public override void Write(NetworkWriter writer)
        {
            writer.Write(EID);
            writer.Write(ObjectType);
            writer.Write(X);
            writer.Write(Y);
            writer.Write(Z);
            writer.Write(FireballthrowerEntityID);
            writer.Write(Unknown1);
            writer.Write(Unknown2);
            writer.Write(Unknown3);
        }

        public override void Handle(PacketHandler handler)
        {
            handler.Handle(this);
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
            BytesRead += reader.Read(out EID);
            byte mobType;
            BytesRead += reader.Read(out mobType);
            MobType = (MobTypes)mobType;
            BytesRead += reader.Read(out X);
            BytesRead += reader.Read(out Y);
            BytesRead += reader.Read(out Z);
            BytesRead += reader.Read(out Yaw);
            BytesRead += reader.Read(out Pitch);
            BytesRead += reader.Read(out DataStream);
        }

        public override void Write(NetworkWriter writer)
        {
            writer.Write(EID);
            writer.Write((byte)MobType);
            writer.Write(X);
            writer.Write(Y);
            writer.Write(Z);
            writer.Write(Yaw);
            writer.Write(Pitch);
            writer.Write(DataStream);
        }

        public override void Handle(PacketHandler handler)
        {
            handler.Handle(this);
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
            BytesRead += reader.Read(out EntityID);
            BytesRead += reader.ReadS16(out Title);
            BytesRead += reader.Read(out X);
            BytesRead += reader.Read(out Y);
            BytesRead += reader.Read(out Z);
            BytesRead += reader.Read(out Direction);
        }

        public override void Write(NetworkWriter writer)
        {
            writer.Write(EntityID);
            writer.Write(Title);
            writer.Write(X);
            writer.Write(Y);
            writer.Write(Z);
            writer.Write(Direction);
        }

        public override void Handle(PacketHandler handler)
        {
            handler.Handle(this);
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
            BytesRead += reader.Read(out EntityID);
            BytesRead += reader.Read(out x);
            BytesRead += reader.Read(out y);
            BytesRead += reader.Read(out z);
            BytesRead += reader.Read(out count);
        }

        public override void Write(NetworkWriter writer)
        {
            writer.Write(EntityID);
            writer.Write(x);
            writer.Write(y);
            writer.Write(z);
            writer.Write(count);
        }

        public override void Handle(PacketHandler handler)
        {
            handler.Handle(this);
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
            BytesRead += reader.Read(out Unknown1);
            BytesRead += reader.Read(out Unknown2);
            BytesRead += reader.Read(out Unknown3);
            BytesRead += reader.Read(out Unknown4);
            BytesRead += reader.Read(out Unknown5);
            BytesRead += reader.Read(out Unknown6);
        }

        public override void Write(NetworkWriter writer)
        {
            writer.Write(Unknown1);
            writer.Write(Unknown2);
            writer.Write(Unknown3);
            writer.Write(Unknown4);
            writer.Write(Unknown5);
            writer.Write(Unknown6);
        }

        public override void Handle(PacketHandler handler)
        {
            handler.Handle(this);
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
            BytesRead += reader.Read(out EntityID);
            BytesRead += reader.Read(out VelocityX);
            BytesRead += reader.Read(out VelocityY);
            BytesRead += reader.Read(out VelocityZ);
        }

        public override void Write(NetworkWriter writer)
        {
            writer.Write(EntityID);
            writer.Write(VelocityX);
            writer.Write(VelocityY);
            writer.Write(VelocityZ);
        }

        public override void Handle(PacketHandler handler)
        {
            handler.Handle(this);
        }
    }

    public class DestroyEntityPacket : Packet
    {
        public int EID;

        public DestroyEntityPacket()
            : base(PacketType.DestroyEntity) { }

        public override void Read(NetworkReader reader)
        {
            BytesRead += reader.Read(out EID);
        }

        public override void Write(NetworkWriter writer)
        {
            writer.Write(EID);
        }

        public override void Handle(PacketHandler handler)
        {
            handler.Handle(this);
        }
    }

    public class EntityPacket : Packet
    {
        public int EID;

        public EntityPacket()
            : base(PacketType.Entity) { }

        public override void Read(NetworkReader reader)
        {
            BytesRead += reader.Read(out EID);
        }

        public override void Write(NetworkWriter writer)
        {
            writer.Write(EID);
        }

        public override void Handle(PacketHandler handler)
        {
            handler.Handle(this);
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
            BytesRead += reader.Read(out EID);
            BytesRead += reader.Read(out dX);
            BytesRead += reader.Read(out dY);
            BytesRead += reader.Read(out dZ);
        }

        public override void Write(NetworkWriter writer)
        {
            writer.Write(EID);
            writer.Write(dX);
            writer.Write(dY);
            writer.Write(dZ);
        }

        public override void Handle(PacketHandler handler)
        {
            handler.Handle(this);
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
            BytesRead += reader.Read(out EID);
            BytesRead += reader.Read(out Yaw);
            BytesRead += reader.Read(out Pitch);
        }

        public override void Write(NetworkWriter writer)
        {
            writer.Write(EID);
            writer.Write(Yaw);
            writer.Write(Pitch);
        }

        public override void Handle(PacketHandler handler)
        {
            handler.Handle(this);
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
            BytesRead += reader.Read(out EID);
            BytesRead += reader.Read(out dX);
            BytesRead += reader.Read(out dY);
            BytesRead += reader.Read(out dZ);
            BytesRead += reader.Read(out Yaw);
            BytesRead += reader.Read(out Pitch);
        }

        public override void Write(NetworkWriter writer)
        {
            writer.Write(EID);
            writer.Write(dX);
            writer.Write(dY);
            writer.Write(dZ);
            writer.Write(Yaw);
            writer.Write(Pitch);
        }

        public override void Handle(PacketHandler handler)
        {
            handler.Handle(this);
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
            BytesRead += reader.Read(out EID);
            BytesRead += reader.Read(out X);
            BytesRead += reader.Read(out Y);
            BytesRead += reader.Read(out Z);
            BytesRead += reader.Read(out Yaw);
            BytesRead += reader.Read(out Pitch);
        }

        public override void Write(NetworkWriter writer)
        {
            writer.Write(EID);
            writer.Write(X);
            writer.Write(Y);
            writer.Write(Z);
            writer.Write(Yaw);
            writer.Write(Pitch);
        }

        public override void Handle(PacketHandler handler)
        {
            handler.Handle(this);
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
            BytesRead += reader.Read(out EntityID);
            BytesRead += reader.Read(out EntityStatus);
        }

        public override void Write(NetworkWriter writer)
        {
            writer.Write(EntityID);
            writer.Write(EntityStatus);
        }

        public override void Handle(PacketHandler handler)
        {
            handler.Handle(this);
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
            BytesRead += reader.Read(out EntityID);
            BytesRead += reader.Read(out VehicleID);
        }

        public override void Write(NetworkWriter writer)
        {
            writer.Write(EntityID);
            writer.Write(VehicleID);
        }

        public override void Handle(PacketHandler handler)
        {
            handler.Handle(this);
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
            BytesRead += reader.Read(out EntityID);
            BytesRead += reader.Read(out EntityMetadata);
        }

        public override void Write(NetworkWriter writer)
        {
            writer.Write(EntityID);
            writer.Write(EntityMetadata);
        }

        public override void Handle(PacketHandler handler)
        {
            handler.Handle(this);
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
            BytesRead += reader.Read(out EntityID);
            BytesRead += reader.Read(out EffectID);
            BytesRead += reader.Read(out Amplifier);
            BytesRead += reader.Read(out Duration);
        }

        public override void Write(NetworkWriter writer)
        {
            writer.Write(EntityID);
            writer.Write(EffectID);
            writer.Write(Amplifier);
            writer.Write(Duration);
        }

        public override void Handle(PacketHandler handler)
        {
            handler.Handle(this);
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
            BytesRead += reader.Read(out EntityID);
            BytesRead += reader.Read(out EffectID);
        }

        public override void Write(NetworkWriter writer)
        {
            writer.Write(EntityID);
            writer.Write(EffectID);
        }

        public override void Handle(PacketHandler handler)
        {
            handler.Handle(this);
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
            BytesRead += reader.Read(out Currentexperience);
            BytesRead += reader.Read(out Level);
            BytesRead += reader.Read(out Totalexperience);
        }

        public override void Write(NetworkWriter writer)
        {
            writer.Write(Currentexperience);
            writer.Write(Level);
            writer.Write(Totalexperience);
        }

        public override void Handle(PacketHandler handler)
        {
            handler.Handle(this);
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
            BytesRead += reader.Read(out X);
            BytesRead += reader.Read(out Z);
            BytesRead += reader.Read(out Mode);
        }

        public override void Write(NetworkWriter writer)
        {
            writer.Write(X);
            writer.Write(Z);
            writer.Write(Mode);
        }

        public override void Handle(PacketHandler handler)
        {
            handler.Handle(this);
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

        public MapChunkPacket()
            : base(PacketType.MapChunk) { }

        public override void Read(NetworkReader reader)
        {
            BytesRead += reader.Read(out X);
            BytesRead += reader.Read(out Y);
            BytesRead += reader.Read(out Z);
            BytesRead += reader.Read(out Size_X);
            BytesRead += reader.Read(out Size_Y);
            BytesRead += reader.Read(out Size_Z);
            BytesRead += reader.Read(out CompressedSize);
            BytesRead += reader.Read(out CompressedData, CompressedSize);
        }

        public override void Write(NetworkWriter writer)
        {
            writer.Write(X);
            writer.Write(Y);
            writer.Write(Z);
            writer.Write(Size_X);
            writer.Write(Size_Y);
            writer.Write(Size_Z);
            writer.Write(CompressedSize);
            writer.Write(CompressedData);
        }

        public override void Handle(PacketHandler handler)
        {
            handler.Handle(this);
        }
    }

    public class MultiBlockChangePacket : Packet
    {
        public int ChunkX;
        public int ChunkZ;
        public short Arraysize;
        public short[] Coordinatearray;
        public byte[] Typearray;
        public byte[] Metadataarray;

        public MultiBlockChangePacket()
            : base(PacketType.MultiBlockChange) { }

        public override void Read(NetworkReader reader)
        {
            BytesRead += reader.Read(out ChunkX);
            BytesRead += reader.Read(out ChunkZ);
            BytesRead += reader.Read(out Arraysize);
            BytesRead += reader.Read(out Coordinatearray, Arraysize);
            BytesRead += reader.Read(out Typearray, Arraysize);
            BytesRead += reader.Read(out Metadataarray, Arraysize);
        }

        public override void Write(NetworkWriter writer)
        {
            writer.Write(ChunkX);
            writer.Write(ChunkZ);
            writer.Write(Arraysize);
            writer.Write(Coordinatearray);
            writer.Write(Typearray);
            writer.Write(Metadataarray);
        }

        public override void Handle(PacketHandler handler)
        {
            handler.Handle(this);
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
            BytesRead += reader.Read(out X);
            BytesRead += reader.Read(out Y);
            BytesRead += reader.Read(out Z);
            BytesRead += reader.Read(out BlockType);
            BytesRead += reader.Read(out BlockMetadata);
        }

        public override void Write(NetworkWriter writer)
        {
            writer.Write(X);
            writer.Write(Y);
            writer.Write(Z);
            writer.Write(BlockType);
            writer.Write(BlockMetadata);
        }

        public override void Handle(PacketHandler handler)
        {
            handler.Handle(this);
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
            BytesRead += reader.Read(out X);
            BytesRead += reader.Read(out Y);
            BytesRead += reader.Read(out Z);
            BytesRead += reader.Read(out Byte1);
            BytesRead += reader.Read(out Byte2);
        }

        public override void Write(NetworkWriter writer)
        {
            writer.Write(X);
            writer.Write(Y);
            writer.Write(Z);
            writer.Write(Byte1);
            writer.Write(Byte2);
        }

        public override void Handle(PacketHandler handler)
        {
            handler.Handle(this);
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
            BytesRead += reader.Read(out X);
            BytesRead += reader.Read(out Y);
            BytesRead += reader.Read(out Z);
            BytesRead += reader.Read(out Unknown);
            BytesRead += reader.Read(out Recordcount);
            BytesRead += reader.Read(out Records, Recordcount * 3);
        }

        public override void Write(NetworkWriter writer)
        {
            writer.Write(X);
            writer.Write(Y);
            writer.Write(Z);
            writer.Write(Unknown);
            writer.Write(Recordcount);
            writer.Write(Records);
        }

        public override void Handle(PacketHandler handler)
        {
            handler.Handle(this);
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
            BytesRead += reader.Read(out EffectID);
            BytesRead += reader.Read(out X);
            BytesRead += reader.Read(out Y);
            BytesRead += reader.Read(out Z);
            BytesRead += reader.Read(out Sounddata);
        }

        public override void Write(NetworkWriter writer)
        {
            writer.Write(EffectID);
            writer.Write(X);
            writer.Write(Y);
            writer.Write(Z);
            writer.Write(Sounddata);
        }

        public override void Handle(PacketHandler handler)
        {
            handler.Handle(this);
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
            BytesRead += reader.Read(out Reason);
            BytesRead += reader.Read(out Gamemode);
        }

        public override void Write(NetworkWriter writer)
        {
            writer.Write(Reason);
            writer.Write(Gamemode);
        }

        public override void Handle(PacketHandler handler)
        {
            handler.Handle(this);
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
            BytesRead += reader.Read(out EntityID);
            BytesRead += reader.Read(out Unknown);
            BytesRead += reader.Read(out X);
            BytesRead += reader.Read(out Y);
            BytesRead += reader.Read(out Z);
        }

        public override void Write(NetworkWriter writer)
        {
            writer.Write(EntityID);
            writer.Write(Unknown);
            writer.Write(X);
            writer.Write(Y);
            writer.Write(Z);
        }

        public override void Handle(PacketHandler handler)
        {
            handler.Handle(this);
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
            BytesRead += reader.Read(out Windowid);
            BytesRead += reader.Read(out InventoryType);
            BytesRead += reader.ReadS16(out Windowtitle);
            BytesRead += reader.Read(out NumberofSlots);
        }

        public override void Write(NetworkWriter writer)
        {
            writer.Write(Windowid);
            writer.Write(InventoryType);
            writer.Write(Windowtitle);
            writer.Write(NumberofSlots);
        }

        public override void Handle(PacketHandler handler)
        {
            handler.Handle(this);
        }
    }

    public class ClosewindowPacket : Packet
    {
        public byte Windowid;

        public ClosewindowPacket()
            : base(PacketType.Closewindow) { }

        public override void Read(NetworkReader reader)
        {
            BytesRead += reader.Read(out Windowid);
        }

        public override void Write(NetworkWriter writer)
        {
            writer.Write(Windowid);
        }

        public override void Handle(PacketHandler handler)
        {
            handler.Handle(this);
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
            BytesRead += reader.Read(out Windowid);
            BytesRead += reader.Read(out Slot);
            BytesRead += reader.Read(out Right_click);
            BytesRead += reader.Read(out Actionnumber);
            BytesRead += reader.Read(out Shift);
            BytesRead += reader.Read(out ItemID);
            BytesRead += reader.Read(out Itemcount);
            BytesRead += reader.Read(out Itemuses);
        }

        public override void Write(NetworkWriter writer)
        {
            writer.Write(Windowid);
            writer.Write(Slot);
            writer.Write(Right_click);
            writer.Write(Actionnumber);
            writer.Write(Shift);
            writer.Write(ItemID);
            writer.Write(Itemcount);
            writer.Write(Itemuses);
        }

        public override void Handle(PacketHandler handler)
        {
            handler.Handle(this);
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
            BytesRead += reader.Read(out Windowid);
            BytesRead += reader.Read(out Slot);
            BytesRead += reader.Read(out ItemID);
            if (ItemID != -1)
            {
                BytesRead += reader.Read(out ItemCount);
                BytesRead += reader.Read(out Itemuses);
            }
        }

        public override void Write(NetworkWriter writer)
        {
            writer.Write(Windowid);
            writer.Write(Slot);
            writer.Write(ItemID);
            writer.Write(ItemCount);
            writer.Write(Itemuses);
        }

        public override void Handle(PacketHandler handler)
        {
            handler.Handle(this);
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
            BytesRead += reader.Read(out Windowid);
            BytesRead += reader.Read(out Count);

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
            short[] payload;
            BytesRead += reader.Read(out payload, Count);
        }

        public override void Write(NetworkWriter writer)
        {
            writer.Write(Windowid);
            writer.Write(Count);
        }

        public override void Handle(PacketHandler handler)
        {
            handler.Handle(this);
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
            BytesRead += reader.Read(out Windowid);
            BytesRead += reader.Read(out Progressbar);
            BytesRead += reader.Read(out Value);
        }

        public override void Write(NetworkWriter writer)
        {
            writer.Write(Windowid);
            writer.Write(Progressbar);
            writer.Write(Value);
        }

        public override void Handle(PacketHandler handler)
        {
            handler.Handle(this);
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
            BytesRead += reader.Read(out Windowid);
            BytesRead += reader.Read(out Actionnumber);
            BytesRead += reader.Read(out Accepted);
        }

        public override void Write(NetworkWriter writer)
        {
            writer.Write(Windowid);
            writer.Write(Actionnumber);
            writer.Write(Accepted);
        }

        public override void Handle(PacketHandler handler)
        {
            handler.Handle(this);
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
            BytesRead += reader.Read(out Slot);
            BytesRead += reader.Read(out Itemid);
            BytesRead += reader.Read(out Quantity);
            BytesRead += reader.Read(out Damage);
        }

        public override void Write(NetworkWriter writer)
        {
            writer.Write(Slot);
            writer.Write(Itemid);
            writer.Write(Quantity);
            writer.Write(Damage);
        }

        public override void Handle(PacketHandler handler)
        {
            handler.Handle(this);
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
            BytesRead += reader.Read(out X);
            BytesRead += reader.Read(out Y);
            BytesRead += reader.Read(out Z);
            BytesRead += reader.ReadS16(out Text1);
            BytesRead += reader.ReadS16(out Text2);
            BytesRead += reader.ReadS16(out Text3);
            BytesRead += reader.ReadS16(out Text4);
        }

        public override void Write(NetworkWriter writer)
        {
            writer.Write(X);
            writer.Write(Y);
            writer.Write(Z);
            writer.Write(Text1);
            writer.Write(Text2);
            writer.Write(Text3);
            writer.Write(Text4);
        }

        public override void Handle(PacketHandler handler)
        {
            handler.Handle(this);
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
            BytesRead += reader.Read(out ItemType);
            BytesRead += reader.Read(out ItemID);
            BytesRead += reader.Read(out Textlength);
            BytesRead += reader.Read(out Text, Textlength);
        }

        public override void Write(NetworkWriter writer)
        {
            writer.Write(ItemType);
            writer.Write(ItemID);
            writer.Write(Textlength);
            writer.Write(Text);
        }

        public override void Handle(PacketHandler handler)
        {
            handler.Handle(this);
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
            BytesRead += reader.Read(out StatisticID);
            BytesRead += reader.Read(out Amount);
        }

        public override void Write(NetworkWriter writer)
        {
            writer.Write(StatisticID);
            writer.Write(Amount);
        }

        public override void Handle(PacketHandler handler)
        {
            handler.Handle(this);
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
            BytesRead += reader.ReadS16(out Playername);
            BytesRead += reader.Read(out Online);
            BytesRead += reader.Read(out Ping);
        }

        public override void Write(NetworkWriter writer)
        {
            writer.Write(Playername);
            writer.Write(Online);
            writer.Write(Ping);
        }

        public override void Handle(PacketHandler handler)
        {
            handler.Handle(this);
        }
    }

    public class ServerListPingPacket : Packet
    {
        public ServerListPingPacket()
            : base(PacketType.ServerListPing) { }

        public override void Read(NetworkReader reader)
        {
        }

        public override void Write(NetworkWriter writer)
        {
        }

        public override void Handle(PacketHandler handler)
        {
            handler.Handle(this);
        }
    }

    public class Disconnect_KickPacket : Packet
    {
        public string Reason;
        public Disconnect_KickPacket()
            : base(PacketType.Disconnect_Kick) { }

        public override void Read(NetworkReader reader)
        {
            BytesRead += reader.ReadS16(out Reason);
        }

        public override void Write(NetworkWriter writer)
        {
            writer.Write(Reason);
        }

        public override void Handle(PacketHandler handler)
        {
            handler.Handle(this);
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
        public Metadata()
        {
        }
    }
}
