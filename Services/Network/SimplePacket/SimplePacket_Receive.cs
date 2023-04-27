using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using ModLibsCore.Classes.Errors;
using ModLibsCore.Libraries.Debug;


namespace ModLibsCore.Services.Network.SimplePacket {
	/// <summary>
	/// Provides functions to neatly send data (via. ModPacket) to server, clients, or both. Abstracts away serialization.
	/// </summary>
	public partial class SimplePacket : ModSystem {
		private static void Receive( SimplePacketPayload data, int playerWho ) {
			if( ModLibsConfig.Instance.DebugModeNetInfo ) {
				Type dataType = data.GetType();
				bool isNoisy = dataType
					.IsDefined( typeof( IsNoisyAttribute ), false );

				if( !isNoisy ) {
					LogLibraries.Log( "<" + dataType.Name );
				}
			}

			//

			if( Main.netMode == NetmodeID.Server ) {
				data.ReceiveOnServer( playerWho );
			} else if( Main.netMode == NetmodeID.MultiplayerClient ) {
				data.ReceiveOnClient();
			} else {
				throw new ModLibsException( "Not MP ("+data.GetType().Name+")" );
			}
		}
	}
}
