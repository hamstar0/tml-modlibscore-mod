using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using ModLibsCore.Libraries.Debug;
using ModLibsCore.Libraries.TModLoader;


namespace ModLibsCore.Classes.PlayerData {
	/// <summary>
	/// An alternative to ModPlayer for basic per-player, per-game data storage and Update use.
	/// </summary>
	public partial class CustomPlayerData : ILoadable {
		internal static void UpdateAll() {
			var singleton = TmlLibraries.SafelyGetInstance<CustomPlayerData>();
			if( singleton == null ) {
				return;
			}

			bool isNotMenu = Main.netMode == NetmodeID.Server || Main.dedServ
				? true
				: !Main.gameMenu;

			for( int plrWho = 0; plrWho < Main.maxPlayers; plrWho++ ) {
				CustomPlayerData.UpdateForPlayer( plrWho, isNotMenu );
			}

			// On main menu return
			if( Main.netMode != NetmodeID.Server && !Main.dedServ && Main.gameMenu ) {
				singleton.PlayerWhoToTypeToTypeInstanceMap.Clear();
			}
		}


		private static void UpdateForPlayer( int playerWho, bool isNotInMenu ) {
			Player player = Main.player[playerWho];
			var singleton = TmlLibraries.SafelyGetInstance<CustomPlayerData>();
			bool playerHasEnteredGame = singleton.PlayerWhoToTypeToTypeInstanceMap.ContainsKey( playerWho );

			//

			if( player?.active != true ) {
				if( playerHasEnteredGame ) {
					CustomPlayerData.Exit( playerWho );
				}

				return;
			}

			//

			//bool isInGame = Main.netMode == NetmodeID.Server
			//	? true
			//	: plrWho == Main.myPlayer
			//		? LoadLibraries.IsCurrentPlayerInGame()
			//		: false;

			if( isNotInMenu ) {
				CustomPlayerData.UpdateForPlayerInGame( playerWho, playerHasEnteredGame );
			} else {
				if( playerHasEnteredGame ) {
					CustomPlayerData.Exit( playerWho );
				}
			}
		}


		private static void UpdateForPlayerInGame( int playerWho, bool playerHasEnteredGame ) {
			if( !playerHasEnteredGame ) {
				CustomPlayerData.Enter( playerWho );

				return;
			}

			//

			var singleton = TmlLibraries.SafelyGetInstance<CustomPlayerData>();

			IEnumerable<(Type, CustomPlayerData)> plrDataMap = singleton
				.PlayerWhoToTypeToTypeInstanceMap[ playerWho ]
				.Select( kv => (kv.Key, kv.Value) );

			//

			foreach( (Type plrDataType, CustomPlayerData plrData) in plrDataMap ) {
				plrData.Update();
			}
		}
	}
}
