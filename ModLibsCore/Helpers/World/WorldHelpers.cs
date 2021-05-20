using System;
using Terraria;
using ModLibsCore.Libraries.Debug;
using ModLibsCore.Libraries.DotNET;


namespace ModLibsCore.Libraries.World {
	/// <summary>
	/// Assorted static "helper" functions pertaining to the current world.
	/// </summary>
	public partial class WorldLibraries {
		/// <summary>
		/// Gets a unique identifier for the current loaded world.
		/// </summary>
		/// <param name="asFileName"></param>
		/// <returns></returns>
		public static string GetUniqueIdForCurrentWorld( bool asFileName ) {
			if( asFileName ) {
				return FileLibraries.SanitizePath( Main.worldName ) + "@" + Main.worldID;
			} else {
				return FileLibraries.SanitizePath( Main.worldName ) + ":" + Main.worldID;
			}
		}
	}
}
