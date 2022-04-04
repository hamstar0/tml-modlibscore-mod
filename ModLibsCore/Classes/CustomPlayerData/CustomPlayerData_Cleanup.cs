using System;
using System.Collections.Generic;
using Terraria;
using ModLibsCore.Classes.Errors;
using ModLibsCore.Classes.Loadable;
using ModLibsCore.Libraries.Debug;
using ModLibsCore.Libraries.DotNET.Reflection;
using ModLibsCore.Libraries.DotNET.Extensions;
using ModLibsCore.Libraries.Players;
using ModLibsCore.Libraries.TModLoader;


namespace ModLibsCore.Classes.PlayerData {
	/// <summary>
	/// An alternative to ModPlayer for basic per-player, per-game data storage and Update use.
	/// </summary>
	public partial class CustomPlayerData : ILoadable {
		private static void InstallCustomPlayerDataCleanup() {
			On.Terraria.Main.ErasePlayer += ( On.Terraria.Main.orig_ErasePlayer orig, int i ) => {
				CustomPlayerData.CleanupPlayer( Main.PlayerList[i].Player );

				//

				orig.Invoke( i );
			};
		}


		private static void CleanupPlayer( Player player ) {
			var singleton = TmlLibraries.SafelyGetInstance<CustomPlayerData>();

			IEnumerable<Type> plrDataTypes = ReflectionLibraries.GetAllAvailableSubTypesFromMods(
				typeof( CustomPlayerData )
			);
			string uid = PlayerIdentityLibraries.GetUniqueId( player );

			//

			if( ModLibsConfig.Instance.DebugModeLoadStages ) {
				LogLibraries.Alert( $"Player {player.name} ({player.whoAmI}; {uid}) entered the game." );
			}

			//

			singleton.PlayerWhoToTypeToTypeInstanceMap[player.whoAmI] = new Dictionary<Type, CustomPlayerData>();

			foreach( Type plrDataType in plrDataTypes ) {
				CustomPlayerData.DeletePlayerData( plrDataType.Name, uid );

				//

				singleton.PlayerWhoToTypeToTypeInstanceMap.Remove2D( player.whoAmI, plrDataType );
			}
		}
	}
}
