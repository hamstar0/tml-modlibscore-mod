using System;
using Terraria;
using Terraria.ID;
using ModLibsCore.Classes.Errors;
using ModLibsCore.Libraries.Players;


namespace ModLibsCore.Libraries.User {
	/// <summary>
	/// Assorted static "helper" functions pertaining to the concept of "users" (corrently supports only a single,
	/// config-defined "priviledged" user).
	/// </summary>
	public class UserLibraries {
		/// <summary>
		/// Indicates if the given player has special priviledge on a server. Currently, this is only defined by a config
		/// setting (`PrivilegedUserId`) using the user's internal unique ID (see `PlayerIdentityLibraries.GetUniqueId()`).
		/// </summary>
		/// <param name="player"></param>
		/// <returns></returns>
		public static bool HasBasicServerPrivilege( Player player ) {
			if( Main.netMode == NetmodeID.SinglePlayer && !Main.dedServ ) {
				throw new ModLibsException( "Not multiplayer." );
			}

			var privConfig = ModLibsCorePrivilegedUserConfig.Instance;
			string uid = PlayerIdentityLibraries.GetUniqueId( player );

			if( string.IsNullOrEmpty(privConfig.PrivilegedUserId) ) {
				return false;
			}

			return privConfig.PrivilegedUserId.Equals( uid );
		}
	}
}
