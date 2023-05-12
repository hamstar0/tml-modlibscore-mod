using System.IO;
using ModLibsCore.Services.Network.ManualPackets;
using ModLibsCore.Services.Network.SimplePacket;
using Terraria.ModLoader;

namespace ModLibsCore {
	/// @private
	partial class ModLibsCoreMod : Mod {
		public override void HandlePacket( BinaryReader reader, int playerWho ) {
			long position = reader.BaseStream.Position;

			if( ManualPacketSystem.TryHandlePacket( reader, playerWho ) ) {
				return;
			}

			reader.BaseStream.Position = position;

			SimplePacket.HandlePacket( reader, playerWho );
		}
	}
}
