using ModLibsCore.Internals.Packets;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ModLibsCore.Internals.Logic {
	/// @private
	partial class PlayerIdSynchronization : ModPlayer {
		public override void OnEnterWorld( Player player ) {
			if( Main.netMode == NetmodeID.MultiplayerClient ) {
				PlayerNewIdPacket.QuickSendToServer();
			}
		}
	}
}
