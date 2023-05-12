using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Runtime.Serialization;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ModLibsCore.Services.Network.ManualPackets;

public sealed class ManualPacketSystem : ModSystem {
	public static ManualPacketSystem Instance => ModContent.GetInstance<ManualPacketSystem>();

	private static readonly Dictionary<Type, ManualPacket> packetsByType = new();
	private static ManualPacket[] packets = Array.Empty<ManualPacket>();

	public override void PostSetupContent() {
		foreach( var type in ModLoader.Mods.SelectMany(m => m.Code.GetTypes().Where(t => !t.IsAbstract && typeof(ManualPacket).IsAssignableFrom(t))) ) {
			var instance = (ManualPacket)FormatterServices.GetUninitializedObject( type );

			instance.Id = PacketIds.Increment();
			packetsByType[type] = instance;

			if( instance.Id >= packets.Length ) {
				Array.Resize( ref packets, (int)BitOperations.RoundUpToPowerOf2( (uint)instance.Id + 1 ) );
			}

			packets[instance.Id] = instance;

			ContentInstance.Register( instance );
		}
	}

	public override void Unload() {
		packetsByType?.Clear();
		packets = Array.Empty<ManualPacket>();
	}

	// Get
	public static bool TryGetPacket( int id, out ManualPacket result ) {
		result = id >= 0 && id < packets.Length ? packets[id] : null;

		return result != null;
	}

	public static bool TryGetPacket( Type type, out ManualPacket result )
		=> packetsByType.TryGetValue( type, out result );

	public static T GetPacket<T>() where T : ManualPacket
		=> ModContent.GetInstance<T>();

	// Send
	public static void SendPacket<T>( T packet, int toClient = -1, int ignoreClient = -1, Func<Player, bool> sendDelegate = null ) where T : ManualPacket {
		if( Main.netMode == NetmodeID.SinglePlayer ) {
			return;
		}

		ModPacket modPacket = Instance.Mod.GetPacket();

		modPacket.Write7BitEncodedInt( packet.Id );
		packet.WriteAndDispose( modPacket );

		try {
			if( Main.netMode == NetmodeID.MultiplayerClient ) {
				modPacket.Send();
			} else if( toClient != -1 ) {
				modPacket.Send( toClient, ignoreClient );
			} else {
				for( int i = 0; i < Main.player.Length; i++ ) {
					var player = Main.player[i];

					if( i != ignoreClient && Netplay.Clients[i].State >= 10 && (sendDelegate?.Invoke( player ) ?? true) ) {
						modPacket.Send( i );
					}
				}
			}
		} catch { }
	}

	internal static bool TryHandlePacket( BinaryReader reader, int sender ) {
		try {
			int packetId = reader.Read7BitEncodedInt();

			if( !TryGetPacket( packetId, out var packet ) ) {
				return false;
			}

			packet.Read( reader, sender );

			return true;
		} catch {
			//TODO: Log
		}

		return false;
	}
}
