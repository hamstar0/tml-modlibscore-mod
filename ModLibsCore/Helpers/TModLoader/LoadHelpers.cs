using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using ModLibsCore.Helpers.Debug;
using ModLibsCore.Helpers.DotNET.Reflection;
using ModLibsCore.Internals.Logic;


namespace ModLibsCore.Helpers.TModLoader {
	/// <summary>
	/// Assorted static "helper" functions pertaining to the state of the game.
	/// </summary>
	public partial class LoadHelpers {
		/// <summary>
		/// Indicates if mods Mod Helpers is fully loaded (recipes, content, etc.).
		/// </summary>
		/// <returns></returns>
		public static bool IsModLoaded() {
			var mymod = ModLibsCoreMod.Instance;

			if( !mymod.HasSetupContent ) { return false; }
			if( !mymod.HasAddedRecipeGroups ) { return false; }
			if( !mymod.HasAddedRecipes ) { return false; }

			return true;
		}

		
		/// <summary>
		/// Indicates if the player is playing a game.
		/// </summary>
		/// <returns></returns>
		public static bool IsCurrentPlayerInGame() {
			bool isTimerActive;
			ReflectionHelpers.Get( Main.ActivePlayerFileData, "_isTimerActive", out isTimerActive );

			return !Main.gameMenu && isTimerActive;
		}


		/// <summary>
		/// Indicates if the current world has finished loading, and is ready for play.
		/// </summary>
		/// <returns></returns>
		public static bool IsWorldLoaded() {
			if( !LoadHelpers.IsModLoaded() ) {  return false; }

			return WorldLogic.IsLoaded;
		}


		/// <summary>
		/// Indicates if a given world is being played at present (at least 1 active player).
		/// </summary>
		/// <returns></returns>
		public static bool IsWorldBeingPlayed() {
			var loadHelpers = ModContent.GetInstance<LoadHelpers>();

			if( Main.netMode != NetmodeID.Server && !Main.dedServ ) {
				if( !loadHelpers.IsLocalPlayerInGame_Hackish ) {
					return false;
				}

				return Main.LocalPlayer.active;
			} else {
				if( !LoadHelpers.IsWorldLoaded() ) {
					return false;
				}
				if( !loadHelpers.HasServerBegunHavingPlayers_Hackish ) {
					return false;
				}

				return true;
			}
		}


		/// <summary>
		/// Indicates if a given world is being played (at least 1 active player), and that player has finished all of their
		/// own in-game "loading" stuff (attempts to account for any Terraria/mod hidden loading behaviors).
		/// </summary>
		/// <returns></returns>
		public static bool IsWorldSafelyBeingPlayed() {
			var mymod = ModLibsCoreMod.Instance;
			var loadHelpers = ModContent.GetInstance<LoadHelpers>();
			if( loadHelpers == null ) {
				return false;
			}

			bool notSafelyPlayed = loadHelpers.WorldStartupDelay >= ( 60 * 2 );

			if( ModLibsConfig.Instance.DebugModeHelpersInfo && !notSafelyPlayed ) {
				if( Main.netMode != NetmodeID.Server && !Main.dedServ ) {
					LogHelpers.LogOnce( DebugHelpers.GetCurrentContext( 2 ) + " - IsWorldSafelyBeingPlayed - "
						+ "StartupDelay: "+!(loadHelpers.WorldStartupDelay < (60 * 2))
						+ ", IsLocalPlayerInGame_Hackish: " + loadHelpers.IsLocalPlayerInGame_Hackish+" (true?)"
					);
				} else {
					var myworld = ModContent.GetInstance<ModLibsWorld>();
					LogHelpers.LogOnce( DebugHelpers.GetCurrentContext( 2 ) + " - IsWorldSafelyBeingPlayed - "
						+ "StartupDelay: "+!(loadHelpers.WorldStartupDelay < (60 * 2))
						+ ", IsModLoaded(): "+LoadHelpers.IsModLoaded()+" (true?)"
						+ ", HasServerBegunHavingPlayers_Hackish: " + loadHelpers.HasServerBegunHavingPlayers_Hackish+" (true?)"
						+ ", HasSetupContent: "+mymod.HasSetupContent+" (true?)"
						+ ", HasAddedRecipeGroups: "+mymod.HasAddedRecipeGroups+" (true?)"
						+ ", HasAddedRecipes: "+mymod.HasAddedRecipes+" (true?)" );
				}
			}
			return notSafelyPlayed;
		}
	}
}
