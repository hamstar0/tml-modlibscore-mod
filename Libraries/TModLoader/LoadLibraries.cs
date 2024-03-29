﻿using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using ModLibsCore.Libraries.Debug;
using ModLibsCore.Libraries.DotNET.Reflection;
using ModLibsCore.Internals.Logic;


namespace ModLibsCore.Libraries.TModLoader {
	/// <summary>
	/// Assorted static "helper" functions pertaining to the state of the game.
	/// </summary>
	public partial class LoadLibraries : ModSystem {
		/// <summary>
		/// Indicates if mods Mod Libs is fully loaded (recipes, content, etc.).
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
			ReflectionLibraries.Get( Main.ActivePlayerFileData, "_isTimerActive", out isTimerActive );

			return !Main.gameMenu && isTimerActive;
		}


		/// <summary>
		/// Indicates if the current world has finished loading, and is ready for play.
		/// </summary>
		/// <returns></returns>
		public static bool IsWorldLoaded() {
			if( !LoadLibraries.IsModLoaded() ) {  return false; }

			return WorldLogic.IsLoaded;
		}


		/// <summary>
		/// Indicates if a given world is being played at present (at least 1 active player).
		/// </summary>
		/// <returns></returns>
		public static bool IsWorldBeingPlayed() {
			var loadLibs = ModContent.GetInstance<LoadLibraries>();

			if( Main.netMode != NetmodeID.Server && !Main.dedServ ) {	// Not server
				// Ugly, but reliable?
				if( loadLibs == null || !loadLibs.IsLocalPlayerInGame_Hackish ) {
					return false;
				}

				return Main.LocalPlayer.active;
			} else {
				if( !LoadLibraries.IsWorldLoaded() ) {
					return false;
				}

				return loadLibs.HasGameBegunHavingPlayers_Hackish;
			}
		}


		/// <summary>
		/// Indicates if a given world is being played (at least 1 active player), and that player has finished all of their
		/// own in-game "loading" stuff (attempts to account for any Terraria/mod hidden loading behaviors).
		/// </summary>
		/// <returns></returns>
		public static bool IsWorldSafelyBeingPlayed() {
			var mymod = ModLibsCoreMod.Instance;
			var loadLibs = ModContent.GetInstance<LoadLibraries>();
			if( loadLibs == null ) {
				return false;
			}

			//

			bool safelyInPlay = loadLibs.WorldStartupDelay >= 120;

			//

			if( ModLibsConfig.Instance.DebugModeLoadStages && !safelyInPlay ) {
				//string ctx = DebugLibraries.GetCurrentContext( 2 );
				int wldStartDelay = ((int)loadLibs.WorldStartupDelay / 50) * 50;

				if( Main.netMode != NetmodeID.Server && !Main.dedServ ) {
					LogLibraries.LogOnce( "!IsWorldSafelyBeingPlayed - "
						+ " WorldStartupDelay: " + wldStartDelay
						+ ", IsLocalPlayerInGame_Hackish: "+loadLibs.IsLocalPlayerInGame_Hackish,
						false
					);
				} else {
					LogLibraries.LogOnce( "!IsWorldSafelyBeingPlayed - "
						+ " IsModLoaded: "+LoadLibraries.IsModLoaded()
						+ ", WorldStartupDelay: " + wldStartDelay
						+ ", HasGameBegunHavingPlayers_Hackish: "+loadLibs.HasGameBegunHavingPlayers_Hackish
						+ ", HasSetupContent: "+mymod.HasSetupContent
						+ ", HasAddedRecipeGroups: "+mymod.HasAddedRecipeGroups
						+ ", HasAddedRecipes: "+mymod.HasAddedRecipes,
						false
					);
				}
			}

			//

			return safelyInPlay;
		}
	}
}
