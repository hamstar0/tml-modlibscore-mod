using System;
using Terraria;
using Terraria.ID;
using ModLibsCore.Classes.Errors;
using ModLibsCore.Helpers.Debug;
using ModLibsCore.Helpers.User;
using ModLibsCore.Services.Debug.DataDumper;
using ModLibsCore.Services.Network.SimplePacket;


namespace ModLibsCore.Internals.Packets {
	[Serializable]
	class DataDumpRequestPacket : SimplePacketPayload {	//NetIORequestPayloadFromClient<DataDumpProtocol>
		public static bool QuickRequestIf() {
			if( Main.netMode != NetmodeID.MultiplayerClient ) {
				throw new ModHelpersException( "Not server" );
			}

			if( !ModLibsConfig.Instance.DebugModeDumpAlsoServer ) {
				return false;
			}
			if( !UserHelpers.HasBasicServerPrivilege(Main.LocalPlayer) ) {
				return false;
			}

			SimplePacket.SendToServer( new DataDumpRequestPacket() );
			return true;
		}



		////////////////

		public DataDumpRequestPacket() { }


		////////////////

		public override void ReceiveOnServer( int fromWho ) {
			if( !ModLibsConfig.Instance.DebugModeDumpAlsoServer ) {
				return;
			}

			if( !Main.player[fromWho].active ) {
				return;
			}

			if( !UserHelpers.HasBasicServerPrivilege(Main.player[fromWho]) ) {
				LogHelpers.Alert( "Player "+Main.player[fromWho].ToString()+" lacks server privilege." );

				return;
			}

			DataDumper.DumpToFile( out string _, false );

			//PreReplyOnClient
		}


		public override void ReceiveOnClient() {
			throw new ModHelpersException( "Not implemented" );
		}
	}
}