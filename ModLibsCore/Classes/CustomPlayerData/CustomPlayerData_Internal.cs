using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using ModLibsCore.Classes.Loadable;
using ModLibsCore.Libraries.Debug;
using ModLibsCore.Libraries.DotNET.Extensions;
using ModLibsCore.Libraries.DotNET.Reflection;
using ModLibsCore.Libraries.Players;
using ModLibsCore.Libraries.TModLoader;


namespace ModLibsCore.Classes.PlayerData {
	/// <summary>
	/// An alternative to ModPlayer for basic per-player, per-game data storage and Update use.
	/// </summary>
	public partial class CustomPlayerData : ILoadable {
		private static void Enter( int playerWho ) {
			Player player = Main.player[playerWho];
			CustomPlayerData singleton = ModContent.GetInstance<CustomPlayerData>();

			IEnumerable<Type> plrDataTypes = ReflectionLibraries.GetAllAvailableSubTypesFromMods( typeof(CustomPlayerData) );
			string uid = PlayerIdentityLibraries.GetUniqueId( player );

			//

			if( ModLibsConfig.Instance.DebugModeLoadStages ) {
				LogLibraries.Alert( "Player "+player.name+" ("+playerWho+"; "+uid+") entered the game." );
			}

			//

			foreach( Type plrDataType in plrDataTypes ) {
				object data = uid != null
					? CustomPlayerData.LoadFileData( plrDataType.Name, uid )
					: null;

				var customPlrDataInst = (CustomPlayerData)Activator.CreateInstance(
					plrDataType,
					BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic,
					null,
					new object[] { },
					null );
				customPlrDataInst.PlayerWho = playerWho;

				//

				singleton.PlayerWhoToTypeToTypeInstanceMap.Set2D( playerWho, plrDataType, customPlrDataInst );

				//

				/*var typedParam = new TypedMethodParameter( typeof( object ), data );

				ReflectionLibraries.RunMethod(
					instance: plrData,
					methodName: "OnEnter",
					args: new object[] { typedParam },
					returnVal: out object _
				);
				ReflectionLibraries.RunMethod(
					instance: plrData,
					methodName: "OnEnter",
					args: new object[] { Main.myPlayer == playerWho, typedParam },
					returnVal: out object _
				);*/	// <- what is this crap?

				customPlrDataInst.OnEnter( Main.myPlayer == playerWho, data );
			}
		}


		////

		private static void Exit( int playerWho ) {
			if( ModLibsConfig.Instance.DebugModeLoadStages ) {
				Player plr = Main.player[playerWho];
				string uid = "";

				if( plr != null ) {
					uid = PlayerIdentityLibraries.GetUniqueId( Main.player[playerWho] );
				}

				LogLibraries.Alert( "Player "+(plr?.name ?? "null")+" ("+playerWho+", "+uid+") exited the game." );
			}

			CustomPlayerData singleton = ModContent.GetInstance<CustomPlayerData>();

			if( Main.netMode != NetmodeID.Server ) {
				IEnumerable<(Type, CustomPlayerData)> plrDataMap = singleton.PlayerWhoToTypeToTypeInstanceMap[ playerWho ]
					.Select( kv => (kv.Key, kv.Value) );

				foreach( (Type plrDataType, CustomPlayerData plrData) in plrDataMap ) {
					object data = plrData.OnExit();

					if( data != null ) {
						CustomPlayerData.SaveFileData(
							plrData.GetType().Name,
							PlayerIdentityLibraries.GetUniqueId(),
							data
						);
					}
				}
			}

			singleton.PlayerWhoToTypeToTypeInstanceMap.Remove( playerWho );
		}


		////////////////

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

		private static void UpdateForPlayer( int plrWho, bool isNotInMenu ) {
			Player player = Main.player[plrWho];
			var singleton = TmlLibraries.SafelyGetInstance<CustomPlayerData>();
			bool playerExists = singleton.PlayerWhoToTypeToTypeInstanceMap.ContainsKey( plrWho );

			if( player?.active != true ) {
				if( playerExists ) {
					CustomPlayerData.Exit( plrWho );
				}

				return;
			}

			//bool isInGame = Main.netMode == NetmodeID.Server
			//	? true
			//	: plrWho == Main.myPlayer
			//		? LoadLibraries.IsCurrentPlayerInGame()
			//		: false;

			if( isNotInMenu ) {
				if( !playerExists ) {
					CustomPlayerData.Enter( plrWho );
				} else {
					IEnumerable<(Type, CustomPlayerData)> plrDataMap = singleton.PlayerWhoToTypeToTypeInstanceMap[ plrWho ]
							.Select( kv => (kv.Key, kv.Value) );

					foreach( (Type plrDataType, CustomPlayerData plrData) in plrDataMap ) {
						plrData.Update();
					}
				}
			} else {
				if( playerExists ) {
					CustomPlayerData.Exit( plrWho );
				}
			}
		}
	}
}
