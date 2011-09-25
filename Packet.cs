using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace uberminer
{
    public abstract class Packet
    {
        public readonly PacketType Type;

        public static readonly int ProtocolVersion = 17;
        static Dictionary<PacketType, Type> IdTypes = new Dictionary<PacketType, Type>();
        static Dictionary<Type, PacketType> TypeIds = new Dictionary<Type, PacketType>();

        // a successful packet will have had one byte read to determine the packet.
        // start at 1
        public int BytesRead = 1;

        static Packet()
        {
            AddPacketType(PacketType.KeepAlive,                 typeof(KeepAlivePacket));
            AddPacketType(PacketType.LoginRequest,              typeof(LoginRequestPacket));
            AddPacketType(PacketType.Handshake,                 typeof(HandshakePacket));
            AddPacketType(PacketType.ChatMessage,               typeof(ChatMessagePacket));
            AddPacketType(PacketType.TimeUpdate,                typeof(TimeUpdatePacket));
            AddPacketType(PacketType.EntityEquipment,           typeof(EntityEquipmentPacket));
            AddPacketType(PacketType.SpawnPosition,             typeof(SpawnPositionPacket));
            AddPacketType(PacketType.UseEntity,                 typeof(UseEntityPacket));
            AddPacketType(PacketType.UpdateHealth,              typeof(UpdateHealthPacket));
            AddPacketType(PacketType.Respawn,                   typeof(RespawnPacket));
            AddPacketType(PacketType.Player,                    typeof(PlayerPacket));
            AddPacketType(PacketType.PlayerPosition,            typeof(PlayerPositionPacket));
            AddPacketType(PacketType.PlayerLook,                typeof(PlayerLookPacket));
            AddPacketType(PacketType.PlayerPosition_Look,       typeof(PlayerPosition_LookPacket));
            AddPacketType(PacketType.PlayerDigging,             typeof(PlayerDiggingPacket));
            AddPacketType(PacketType.PlayerBlockPlacement,      typeof(PlayerBlockPlacementPacket));
            AddPacketType(PacketType.HoldingChange,             typeof(HoldingChangePacket));
            AddPacketType(PacketType.UseBed,                    typeof(UseBedPacket));
            AddPacketType(PacketType.Animation,                 typeof(AnimationPacket));
            AddPacketType(PacketType.EntityAction,              typeof(EntityActionPacket));
            AddPacketType(PacketType.NamedEntitySpawn,          typeof(NamedEntitySpawnPacket));
            AddPacketType(PacketType.PickupSpawn,               typeof(PickupSpawnPacket));
            AddPacketType(PacketType.CollectItem,               typeof(CollectItemPacket));
            AddPacketType(PacketType.AddObject_Vehicle,         typeof(AddObject_VehiclePacket));
            AddPacketType(PacketType.MobSpawn,                  typeof(MobSpawnPacket));
            AddPacketType(PacketType.EntityPainting,            typeof(EntityPaintingPacket));
            AddPacketType(PacketType.ExperienceOrb,             typeof(ExperienceOrbPacket));
            AddPacketType(PacketType.Stanceupdate,              typeof(StanceupdatePacket));
            AddPacketType(PacketType.EntityVelocity,            typeof(EntityVelocityPacket));
            AddPacketType(PacketType.DestroyEntity,             typeof(DestroyEntityPacket));
            AddPacketType(PacketType.Entity,                    typeof(EntityPacket));
            AddPacketType(PacketType.EntityRelativeMove,        typeof(EntityRelativeMovePacket));
            AddPacketType(PacketType.EntityLook,                typeof(EntityLookPacket));
            AddPacketType(PacketType.EntityLookandRelativeMove, typeof(EntityLookandRelativeMovePacket));
            AddPacketType(PacketType.EntityTeleport,            typeof(EntityTeleportPacket));
            AddPacketType(PacketType.EntityStatus,              typeof(EntityStatusPacket));
            AddPacketType(PacketType.AttachEntity,              typeof(AttachEntityPacket));
            AddPacketType(PacketType.EntityMetadata,            typeof(EntityMetadataPacket));
            AddPacketType(PacketType.EntityEffect,              typeof(EntityEffectPacket));
            AddPacketType(PacketType.RemoveEntityEffect,        typeof(RemoveEntityEffectPacket));
            AddPacketType(PacketType.Experience,                typeof(ExperiencePacket));
            AddPacketType(PacketType.PreChunk,                  typeof(PreChunkPacket));
            AddPacketType(PacketType.MapChunk,                  typeof(MapChunkPacket));
            AddPacketType(PacketType.MultiBlockChange,          typeof(MultiBlockChangePacket));
            AddPacketType(PacketType.BlockChange,               typeof(BlockChangePacket));
            AddPacketType(PacketType.BlockAction,               typeof(BlockActionPacket));
            AddPacketType(PacketType.Explosion,                 typeof(ExplosionPacket));
            AddPacketType(PacketType.Soundeffect,               typeof(SoundeffectPacket));
            AddPacketType(PacketType.New_InvalidState,          typeof(New_InvalidStatePacket));
            AddPacketType(PacketType.Thunderbolt,               typeof(ThunderboltPacket));
            AddPacketType(PacketType.Openwindow,                typeof(OpenwindowPacket));
            AddPacketType(PacketType.Closewindow,               typeof(ClosewindowPacket));
            AddPacketType(PacketType.Windowclick,               typeof(WindowclickPacket));
            AddPacketType(PacketType.Setslot,                   typeof(SetslotPacket));
            AddPacketType(PacketType.Windowitems,               typeof(WindowitemsPacket));
            AddPacketType(PacketType.Updateprogressbar,         typeof(UpdateprogressbarPacket));
            AddPacketType(PacketType.Transaction,               typeof(TransactionPacket));
            AddPacketType(PacketType.Creativeinventoryaction,   typeof(CreativeinventoryactionPacket));
            AddPacketType(PacketType.UpdateSign,                typeof(UpdateSignPacket));
            AddPacketType(PacketType.ItemData,                  typeof(ItemDataPacket));
            AddPacketType(PacketType.IncrementStatistic,        typeof(IncrementStatisticPacket));
            AddPacketType(PacketType.PlayerListItem,            typeof(PlayerListItemPacket));
            AddPacketType(PacketType.ServerListPing,            typeof(ServerListPingPacket));
            AddPacketType(PacketType.Disconnect_Kick,           typeof(Disconnect_KickPacket));


            packetCheck.Add(PacketType.KeepAlive,                   true);
            packetCheck.Add(PacketType.LoginRequest,                true);
            packetCheck.Add(PacketType.Handshake,                   true);
            packetCheck.Add(PacketType.ChatMessage,                 false);
            packetCheck.Add(PacketType.TimeUpdate,                  true);
            packetCheck.Add(PacketType.EntityEquipment,             true);
            packetCheck.Add(PacketType.SpawnPosition,               true);
            packetCheck.Add(PacketType.UseEntity,                   false);
            packetCheck.Add(PacketType.UpdateHealth,                true);
            packetCheck.Add(PacketType.Respawn,                     false);
            packetCheck.Add(PacketType.Player,                      false);
            packetCheck.Add(PacketType.PlayerPosition,              false);
            packetCheck.Add(PacketType.PlayerLook,                  false);
            packetCheck.Add(PacketType.PlayerPosition_Look,         true);
            packetCheck.Add(PacketType.PlayerDigging,               false);
            packetCheck.Add(PacketType.PlayerBlockPlacement,        true);
            packetCheck.Add(PacketType.HoldingChange,               false);
            packetCheck.Add(PacketType.UseBed,                      false);
            packetCheck.Add(PacketType.Animation,                   false);
            packetCheck.Add(PacketType.EntityAction,                true);
            packetCheck.Add(PacketType.NamedEntitySpawn,            false);
            packetCheck.Add(PacketType.PickupSpawn,                 true);
            packetCheck.Add(PacketType.CollectItem,                 false);
            packetCheck.Add(PacketType.AddObject_Vehicle,           false);
            packetCheck.Add(PacketType.MobSpawn,                    true);
            packetCheck.Add(PacketType.EntityPainting,              false);
            packetCheck.Add(PacketType.ExperienceOrb,               false);
            packetCheck.Add(PacketType.Stanceupdate,                false);
            packetCheck.Add(PacketType.EntityVelocity,              true);
            packetCheck.Add(PacketType.DestroyEntity,               true);
            packetCheck.Add(PacketType.Entity,                      true);
            packetCheck.Add(PacketType.EntityRelativeMove,          true);
            packetCheck.Add(PacketType.EntityLook,                  true);
            packetCheck.Add(PacketType.EntityLookandRelativeMove,   true);
            packetCheck.Add(PacketType.EntityTeleport,              true);
            packetCheck.Add(PacketType.EntityStatus,                true);
            packetCheck.Add(PacketType.AttachEntity,                false);
            packetCheck.Add(PacketType.EntityMetadata,              true);
            packetCheck.Add(PacketType.EntityEffect,                false);
            packetCheck.Add(PacketType.RemoveEntityEffect,          false);
            packetCheck.Add(PacketType.Experience,                  true);
            packetCheck.Add(PacketType.PreChunk,                    true);
            packetCheck.Add(PacketType.MapChunk,                    true);
            packetCheck.Add(PacketType.MultiBlockChange,            true);
            packetCheck.Add(PacketType.BlockChange,                 true);
            packetCheck.Add(PacketType.BlockAction,                 true);
            packetCheck.Add(PacketType.Explosion,                   false);
            packetCheck.Add(PacketType.Soundeffect,                 false);
            packetCheck.Add(PacketType.New_InvalidState,            false);
            packetCheck.Add(PacketType.Thunderbolt,                 false);
            packetCheck.Add(PacketType.Openwindow,                  false);
            packetCheck.Add(PacketType.Closewindow,                 false);
            packetCheck.Add(PacketType.Windowclick,                 false);
            packetCheck.Add(PacketType.Setslot,                     true);
            packetCheck.Add(PacketType.Windowitems,                 true);
            packetCheck.Add(PacketType.Updateprogressbar,           false);
            packetCheck.Add(PacketType.Transaction,                 false);
            packetCheck.Add(PacketType.Creativeinventoryaction,     false);
            packetCheck.Add(PacketType.UpdateSign,                  false);
            packetCheck.Add(PacketType.ItemData,                    false);
            packetCheck.Add(PacketType.IncrementStatistic,          false);
            packetCheck.Add(PacketType.PlayerListItem,              true);
            packetCheck.Add(PacketType.ServerListPing,              false);
            packetCheck.Add(PacketType.Disconnect_Kick,             false);
        }

        public Packet(PacketType type)
        {
            Type = type;
        }

        public static void AddPacketType(PacketType id, Type type)
        {
            IdTypes.Add(id, type);
            TypeIds.Add(type, id);
        }

        static List<Packet> packetHistory = new List<Packet>();

        private static Dictionary<PacketType, bool> packetCheck = new Dictionary<PacketType, bool>();

        public static Packet Get(PacketType id)
        {
            Type type;
            if (!IdTypes.TryGetValue(id, out type))
            {
                Program.Log("Unknown packet: 0x" + id.ToString("x"));
                return null;
            }

            var packet = Activator.CreateInstance(type) as Packet;

            packetHistory.Add(packet);

            return packet;
        }

        public static Packet Get(NetworkReader reader)
        {
            var packetId = (PacketType)reader.ReadByte();
            var packet = Get(packetId);
            //try
            {
                if (packet == null)
                    return null;
                //Program.Log("Reading {0}", packet.Type.ToString());
                packet.Read(reader);
                if (packetCheck[packet.Type] == false)
                {
                    Program.Log("{0} read size: {1}", packet.Type, packet.BytesRead);
                }
            }
            //catch (Exception ex)
            {
            //    Program.Log("Packet internal error:");
            //    Program.Log(ex.Message);
            }
            return packet;
        }

        public static void Put(Packet packet, NetworkWriter writer)
        {
            writer.Write((byte)packet.Id);
            packet.Write(writer);
            writer.Flush();
        }

        public PacketType? Id
        {
            get
            {
                PacketType id;
                if (!TypeIds.TryGetValue(GetType(), out id))
                    return null;
                return id;
            }
        }

        public abstract void Handle(PacketHandler handler);
        public abstract void Write(NetworkWriter writer);
        public abstract void Read(NetworkReader reader);
    }
}
