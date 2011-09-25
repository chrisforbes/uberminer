using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace uberminer
{
    public class PacketHandler
    {
        public virtual void Handle(Packet packet)
        {
            Handle(packet as Packet);
		}

        public virtual void Handle(KeepAlivePacket packet)
        {
            Handle(packet as Packet);
		}

        public virtual void Handle(LoginRequestPacket packet)
        {
            Handle(packet as Packet);
		}

        public virtual void Handle(HandshakePacket packet)
        {
            Handle(packet as Packet);
		}

        public virtual void Handle(ChatMessagePacket packet)
        {
            Handle(packet as Packet);
		}

        public virtual void Handle(TimeUpdatePacket packet)
        {
            Handle(packet as Packet);
		}

        public virtual void Handle(EntityEquipmentPacket packet)
        {
            Handle(packet as Packet);
		}

        public virtual void Handle(SpawnPositionPacket packet)
        {
            Handle(packet as Packet);
		}

        public virtual void Handle(UseEntityPacket packet)
        {
            Handle(packet as Packet);
		}

        public virtual void Handle(UpdateHealthPacket packet)
        {
            Handle(packet as Packet);
		}

        public virtual void Handle(RespawnPacket packet)
        {
            Handle(packet as Packet);
		}

        public virtual void Handle(PlayerPacket packet)
        {
            Handle(packet as Packet);
		}

        public virtual void Handle(PlayerPositionPacket packet)
        {
            Handle(packet as Packet);
		}

        public virtual void Handle(PlayerLookPacket packet)
        {
            Handle(packet as Packet);
		}

        public virtual void Handle(PlayerPosition_LookPacket packet)
        {
            Handle(packet as Packet);
		}

        public virtual void Handle(PlayerDiggingPacket packet)
        {
            Handle(packet as Packet);
		}

        public virtual void Handle(PlayerBlockPlacementPacket packet)
        {
            Handle(packet as Packet);
		}

        public virtual void Handle(HoldingChangePacket packet)
        {
            Handle(packet as Packet);
		}

        public virtual void Handle(UseBedPacket packet)
        {
            Handle(packet as Packet);
		}

        public virtual void Handle(AnimationPacket packet)
        {
            Handle(packet as Packet);
		}

        public virtual void Handle(EntityActionPacket packet)
        {
            Handle(packet as Packet);
		}

        public virtual void Handle(NamedEntitySpawnPacket packet)
        {
            Handle(packet as Packet);
		}

        public virtual void Handle(PickupSpawnPacket packet)
        {
            Handle(packet as Packet);
		}

        public virtual void Handle(CollectItemPacket packet)
        {
            Handle(packet as Packet);
		}

        public virtual void Handle(AddObject_VehiclePacket packet)
        {
            Handle(packet as Packet);
		}

        public virtual void Handle(MobSpawnPacket packet)
        {
            Handle(packet as Packet);
		}

        public virtual void Handle(EntityPaintingPacket packet)
        {
            Handle(packet as Packet);
		}

        public virtual void Handle(ExperienceOrbPacket packet)
        {
            Handle(packet as Packet);
		}

        public virtual void Handle(StanceupdatePacket packet)
        {
            Handle(packet as Packet);
		}

        public virtual void Handle(EntityVelocityPacket packet)
        {
            Handle(packet as Packet);
		}

        public virtual void Handle(DestroyEntityPacket packet)
        {
            Handle(packet as Packet);
		}

        public virtual void Handle(EntityPacket packet)
        {
            Handle(packet as Packet);
		}

        public virtual void Handle(EntityRelativeMovePacket packet)
        {
            Handle(packet as Packet);
		}

        public virtual void Handle(EntityLookPacket packet)
        {
            Handle(packet as Packet);
		}

        public virtual void Handle(EntityLookandRelativeMovePacket packet)
        {
            Handle(packet as Packet);
		}

        public virtual void Handle(EntityTeleportPacket packet)
        {
            Handle(packet as Packet);
		}

        public virtual void Handle(EntityStatusPacket packet)
        {
            Handle(packet as Packet);
		}

        public virtual void Handle(AttachEntityPacket packet)
        {
            Handle(packet as Packet);
		}

        public virtual void Handle(EntityMetadataPacket packet)
        {
            Handle(packet as Packet);
		}

        public virtual void Handle(EntityEffectPacket packet)
        {
            Handle(packet as Packet);
		}

        public virtual void Handle(RemoveEntityEffectPacket packet)
        {
            Handle(packet as Packet);
		}

        public virtual void Handle(ExperiencePacket packet)
        {
            Handle(packet as Packet);
		}

        public virtual void Handle(PreChunkPacket packet)
        {
            Handle(packet as Packet);
		}

        public virtual void Handle(MapChunkPacket packet)
        {
            Handle(packet as Packet);
		}

        public virtual void Handle(MultiBlockChangePacket packet)
        {
            Handle(packet as Packet);
		}

        public virtual void Handle(BlockChangePacket packet)
        {
            Handle(packet as Packet);
		}

        public virtual void Handle(BlockActionPacket packet)
        {
            Handle(packet as Packet);
		}

        public virtual void Handle(ExplosionPacket packet)
        {
            Handle(packet as Packet);
		}

        public virtual void Handle(SoundeffectPacket packet)
        {
            Handle(packet as Packet);
		}

        public virtual void Handle(New_InvalidStatePacket packet)
        {
            Handle(packet as Packet);
		}

        public virtual void Handle(ThunderboltPacket packet)
        {
            Handle(packet as Packet);
		}

        public virtual void Handle(OpenwindowPacket packet)
        {
            Handle(packet as Packet);
		}

        public virtual void Handle(ClosewindowPacket packet)
        {
            Handle(packet as Packet);
		}

        public virtual void Handle(WindowclickPacket packet)
        {
            Handle(packet as Packet);
		}

        public virtual void Handle(SetslotPacket packet)
        {
            Handle(packet as Packet);
		}

        public virtual void Handle(WindowitemsPacket packet)
        {
            Handle(packet as Packet);
		}

        public virtual void Handle(UpdateprogressbarPacket packet)
        {
            Handle(packet as Packet);
		}

        public virtual void Handle(TransactionPacket packet)
        {
            Handle(packet as Packet);
		}

        public virtual void Handle(CreativeinventoryactionPacket packet)
        {
            Handle(packet as Packet);
		}

        public virtual void Handle(UpdateSignPacket packet)
        {
            Handle(packet as Packet);
		}

        public virtual void Handle(ItemDataPacket packet)
        {
            Handle(packet as Packet);
		}

        public virtual void Handle(IncrementStatisticPacket packet)
        {
            Handle(packet as Packet);
		}

        public virtual void Handle(PlayerListItemPacket packet)
        {
            Handle(packet as Packet);
		}

        public virtual void Handle(ServerListPingPacket packet)
        {
            Handle(packet as Packet);
		}

        public virtual void Handle(Disconnect_KickPacket packet)
        {
            Handle(packet as Packet);
        }
    }
}
