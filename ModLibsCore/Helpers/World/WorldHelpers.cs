using System;
using Terraria;
using ModLibsCore.Helpers.Debug;
using ModLibsCore.Helpers.DotNET;


namespace ModLibsCore.Helpers.World {
	/// <summary>
	/// Assorted static "helper" functions pertaining to the current world.
	/// </summary>
	public partial class WorldHelpers {
		/// <summary>
		/// Gets a unique identifier for the current loaded world.
		/// </summary>
		/// <param name="asFileName"></param>
		/// <returns></returns>
		public static string GetUniqueIdForCurrentWorld( bool asFileName ) {
			if( asFileName ) {
				return FileHelpers.SanitizePath( Main.worldName ) + "@" + Main.worldID;
			} else {
				return FileHelpers.SanitizePath( Main.worldName ) + ":" + Main.worldID;
			}
		}
	}
}
