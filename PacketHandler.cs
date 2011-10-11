using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace Uberminer
{
    public sealed class PacketHandler
    {
        public Func<KeepAlivePacket, bool> HandleKeepAlive;

        public Func<LoginRequestPacket, bool> HandleLoginRequest;

        public Func<HandshakePacket, bool> HandleHandshake;

        public Func<ChatMessagePacket, bool> HandleChatMessage;

        public Func<TimeUpdatePacket, bool> HandleTimeUpdate;

        public Func<EntityEquipmentPacket, bool> HandleEntityEquipment;

        public Func<SpawnPositionPacket, bool> HandleSpawnPosition;

        public Func<UseEntityPacket, bool> HandleUseEntity;

        public Func<UpdateHealthPacket, bool> HandleUpdateHealth;

        public Func<RespawnPacket, bool> HandleRespawn;

        public Func<PlayerPacket, bool> HandlePlayer;

        public Func<PlayerPositionPacket, bool> HandlePlayerPosition;

        public Func<PlayerLookPacket, bool> HandlePlayerLook;

        public Func<PlayerPosition_LookPacket, bool> HandlePlayerPosition_Look;

        public Func<PlayerDiggingPacket, bool> HandlePlayerDigging;

        public Func<PlayerBlockPlacementPacket, bool> HandlePlayerBlockPlacement;

        public Func<HoldingChangePacket, bool> HandleHoldingChange;

        public Func<UseBedPacket, bool> HandleUseBed;

        public Func<AnimationPacket, bool> HandleAnimation;

        public Func<EntityActionPacket, bool> HandleEntityAction;

        public Func<NamedEntitySpawnPacket, bool> HandleNamedEntitySpawn;

        public Func<PickupSpawnPacket, bool> HandlePickupSpawn;

        public Func<CollectItemPacket, bool> HandleCollectItem;

        public Func<AddObject_VehiclePacket, bool> HandleAddObject_Vehicle;

        public Func<MobSpawnPacket, bool> HandleMobSpawn;

        public Func<EntityPaintingPacket, bool> HandleEntityPainting;

        public Func<ExperienceOrbPacket, bool> HandleExperienceOrb;

        public Func<StanceupdatePacket, bool> HandleStanceupdate;

        public Func<EntityVelocityPacket, bool> HandleEntityVelocity;

        public Func<DestroyEntityPacket, bool> HandleDestroyEntity;

        public Func<EntityPacket, bool> HandleEntity;

        public Func<EntityRelativeMovePacket, bool> HandleEntityRelativeMove;

        public Func<EntityLookPacket, bool> HandleEntityLook;

        public Func<EntityLookandRelativeMovePacket, bool> HandleEntityLookandRelativeMove;

        public Func<EntityTeleportPacket, bool> HandleEntityTeleport;

        public Func<EntityStatusPacket, bool> HandleEntityStatus;

        public Func<AttachEntityPacket, bool> HandleAttachEntity;

        public Func<EntityMetadataPacket, bool> HandleEntityMetadata;

        public Func<EntityEffectPacket, bool> HandleEntityEffect;

        public Func<RemoveEntityEffectPacket, bool> HandleRemoveEntityEffect;

        public Func<ExperiencePacket, bool> HandleExperience;

        public Func<PreChunkPacket, bool> HandlePreChunk;

        public Func<MapChunkPacket, bool> HandleMapChunk;

        public Func<MultiBlockChangePacket, bool> HandleMultiBlockChange;

        public Func<BlockChangePacket, bool> HandleBlockChange;

        public Func<BlockActionPacket, bool> HandleBlockAction;

        public Func<ExplosionPacket, bool> HandleExplosion;

        public Func<SoundeffectPacket, bool> HandleSoundeffect;

        public Func<New_InvalidStatePacket, bool> HandleNew_InvalidState;

        public Func<ThunderboltPacket, bool> HandleThunderbolt;

        public Func<OpenwindowPacket, bool> HandleOpenwindow;

        public Func<ClosewindowPacket, bool> HandleClosewindow;

        public Func<WindowclickPacket, bool> HandleWindowclick;

        public Func<SetslotPacket, bool> HandleSetslot;

        public Func<WindowitemsPacket, bool> HandleWindowitems;

        public Func<UpdateprogressbarPacket, bool> HandleUpdateprogressbar;

        public Func<TransactionPacket, bool> HandleTransaction;

        public Func<CreativeinventoryactionPacket, bool> HandleCreativeinventoryaction;

        public Func<UpdateSignPacket, bool> HandleUpdateSign;

        public Func<ItemDataPacket, bool> HandleItemData;

        public Func<IncrementStatisticPacket, bool> HandleIncrementStatistic;

        public Func<PlayerListItemPacket, bool> HandlePlayerListItem;

        public Func<ServerListPingPacket, bool> HandleServerListPing;

        public Func<Disconnect_KickPacket, bool> HandleDisconnect_Kick;
    }
}
