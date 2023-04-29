using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;
using ModLibsCore.Classes.Errors;
using ModLibsCore.Libraries.Debug;
using ModLibsCore.Libraries.Players;
using ModLibsCore.Services.Network.SimplePacket;


namespace ModLibsCore.Internals.Packets {
	[Serializable]
	class PlayerNewIdRequestPacket : SimplePacketPayload {	//NetIORequest<PlayerNewIdProtocol>
		public static void QuickRequestToClient( int playerWho ) {
			var protocol = new PlayerNewIdRequestPacket( playerWho );

			SimplePacket.SendToServer( protocol );
		}


		////////////////

		public int PlayerWho;



		////////////////

		public PlayerNewIdRequestPacket() { }

		public PlayerNewIdRequestPacket( int playerWho ) {
			this.PlayerWho = playerWho;
		}


		////////////////

		public override void ReceiveOnServer( int fromWho ) {
			throw new ModLibsException( "Not implemented" );
		}

		public override void ReceiveOnClient() {
			var protocol = new PlayerNewIdPacket(
				(Dictionary<int, string>)ModContent.GetInstance<PlayerIdentityLibraries>().PlayerIds
			);

			SimplePacket.SendToClient( protocol, this.PlayerWho );
		}
	}




	[Serializable]
	class PlayerNewIdPacket : SimplePacketPayload {   //NetIOBidirectionalPayload
		public static void QuickSendToServer() {
			var protocol = new PlayerNewIdPacket(
				(Dictionary<int, string>)ModContent.GetInstance<PlayerIdentityLibraries>().PlayerIds
			);
			protocol.PlayerIds[Main.myPlayer] = PlayerIdentityLibraries.GetUniqueId( Main.LocalPlayer );

			SimplePacket.SendToServer( protocol );
		}



		////////////////

		public Dictionary<int, string> PlayerIds;



		////////////////

		public PlayerNewIdPacket() {
			this.PlayerIds = new Dictionary<int, string>();
		}

		public PlayerNewIdPacket( Dictionary<int, string> playerIds ) {
			if( playerIds == null ) {
				this.PlayerIds = new Dictionary<int, string>();

				LogLibraries.Warn( "Player ids not specified." );
				return;
			}
			this.PlayerIds = playerIds;
		}


		////////////////

		public override void ReceiveOnServer( int fromWho ) {
			try {
				if( this.PlayerIds.TryGetValue( fromWho, out string uid ) ) {
					ModContent.GetInstance<PlayerIdentityLibraries>().PlayerIds[fromWho] = uid;
				} else {
					LogLibraries.Warn( "No UID reported from player id'd " + fromWho );
				}
			} catch {
				this.PlayerIds = new Dictionary<int, string>();
				LogLibraries.Warn( "Deserialization error." );
			}
		}

		public override void ReceiveOnClient() {
			try {
				this.PlayerIds.TryGetValue( 0, out string _ );
			} catch {
				this.PlayerIds = new Dictionary<int, string>();
				LogLibraries.Warn( "Deserialization error." );
			}
			ModContent.GetInstance<PlayerIdentityLibraries>().PlayerIds = this.PlayerIds;
		}
	}
}