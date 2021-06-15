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
			IEnumerable<Type> plrDataTypes = ReflectionLibraries.GetAllAvailableSubTypesFromMods( typeof( CustomPlayerData ) );
			string uid = PlayerIdentityLibraries.GetUniqueId( player );

			if( ModLibsConfig.Instance.DebugModeMiscInfo ) {
				LogLibraries.Alert( "Player "+player.name+" ("+playerWho+"; "+uid+") entered the game." );
			}

			foreach( Type plrDataType in plrDataTypes ) {
				object data = CustomPlayerData.LoadFileData( plrDataType.Name, uid );
				var plrData = (CustomPlayerData)Activator.CreateInstance(
					plrDataType,
					BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic,
					null,
					new object[] { },
					null );
				plrData.PlayerWho = playerWho;

				singleton.DataMap.Set2D( playerWho, plrDataType, plrData );

				var typedParam = new TypedMethodParameter( typeof( object ), data );

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
				);

				plrData.OnEnter( Main.myPlayer == playerWho, data );
			}
		}

		private static void Exit( int playerWho ) {
			if( ModLibsConfig.Instance.DebugModeMiscInfo ) {
				Player plr = Main.player[playerWho];
				string uid = "";

				if( plr != null ) {
					uid = PlayerIdentityLibraries.GetUniqueId( Main.player[playerWho] );
				}

				LogLibraries.Alert( "Player "+(plr?.name ?? "null")+" ("+playerWho+", "+uid+") exited the game." );
			}

			CustomPlayerData singleton = ModContent.GetInstance<CustomPlayerData>();

			if( Main.netMode != NetmodeID.Server ) {
				IEnumerable<(Type, CustomPlayerData)> plrDataMap = singleton.DataMap[playerWho].Select( kv => (kv.Key, kv.Value) );

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

			singleton.DataMap.Remove( playerWho );
		}


		////////////////

		internal static void UpdateAll() {
			var singleton = TmlLibraries.SafelyGetInstance<CustomPlayerData>();
			if( singleton == null ) {
				return;
			}

			bool isNotMenu = Main.netMode == NetmodeID.Server
				? true
				: !Main.gameMenu;
			Player player;

			for( int plrWho = 0; plrWho < Main.maxPlayers; plrWho++ ) {
				player = Main.player[plrWho];

				bool containsKey = singleton.DataMap.ContainsKey( plrWho );

				if( player?.active != true ) {
					if( containsKey ) {
						CustomPlayerData.Exit( plrWho );
					}

					continue;
				}

				//bool isInGame = Main.netMode == NetmodeID.Server
				//	? true
				//	: plrWho == Main.myPlayer
				//		? LoadLibraries.IsCurrentPlayerInGame()
				//		: false;

				if( isNotMenu ) {
					if( !containsKey ) {
						CustomPlayerData.Enter( plrWho );
					} else {
						IEnumerable<(Type, CustomPlayerData)> plrDataMap = singleton.DataMap[plrWho]
								.Select( kv => (kv.Key, kv.Value) );

						foreach( (Type plrDataType, CustomPlayerData plrData) in plrDataMap ) {
							plrData.Update();
						}
					}
				} else {
					if( containsKey ) {
						CustomPlayerData.Exit( plrWho );
					}
				}
			}

			if( Main.netMode != NetmodeID.Server && Main.gameMenu ) {
				singleton.DataMap.Clear();
			}
		}
	}
}
