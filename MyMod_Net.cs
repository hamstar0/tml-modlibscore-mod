using System;
using System.IO;
using Terraria;
using Terraria.ModLoader;
using ModLibsCore.Libraries.Debug;
using ModLibsCore.Services.Network.SimplePacket;


namespace ModLibsCore {
	/// @private
	partial class ModLibsCoreMod : Mod {
		public override void HandlePacket( BinaryReader reader, int playerWho ) {
//Services.DataStore.DataStore.Add( DebugLibraries.GetCurrentContext()+"_A", 1 );
			SimplePacket.HandlePacket( reader, playerWho );
//Services.DataStore.DataStore.Add( DebugLibraries.GetCurrentContext()+"_B", 1 );
		}
	}
}
