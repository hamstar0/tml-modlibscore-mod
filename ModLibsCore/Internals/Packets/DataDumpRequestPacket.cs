using System;
using Terraria;
using Terraria.ID;
using ModLibsCore.Classes.Errors;
using ModLibsCore.Libraries.Debug;
using ModLibsCore.Libraries.User;
using ModLibsCore.Services.Debug.DataDumper;
using ModLibsCore.Services.Network.SimplePacket;


namespace ModLibsCore.Internals.Packets {
	[Serializable]
	class DataDumpRequestPacket : SimplePacketPayload {	//NetIORequestPayloadFromClient<DataDumpProtocol>
		public static bool QuickRequestIf() {
			if( Main.netMode != NetmodeID.MultiplayerClient ) {
				throw new ModLibsException( "Not server" );
			}

			if( !ModLibsConfig.Instance.DebugModeDumpAlsoServer ) {
				return false;
			}
			if( !UserLibraries.HasBasicServerPrivilege(Main.LocalPlayer) ) {
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

			if( !UserLibraries.HasBasicServerPrivilege(Main.player[fromWho]) ) {
				LogLibraries.Alert( "Player "+Main.player[fromWho].ToString()+" lacks server privilege." );

				return;
			}

			DataDumper.DumpToFile( out string _, false );

			//PreReplyOnClient
		}


		public override void ReceiveOnClient() {
			throw new ModLibsException( "Not implemented" );
		}
	}
}