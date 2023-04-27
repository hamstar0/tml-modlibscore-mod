using System;
using Terraria;
using Terraria.ModLoader;
using ModLibsCore.Libraries.Debug;
using ModLibsCore.Libraries.DotNET.Extensions;
using ModLibsCore.Libraries.TModLoader;
using ModLibsCore.Services.Hooks.LoadHooks;


namespace ModLibsCore {
	/// @private
	partial class ModLibsCoreMod : Mod {
		private static string LastLoadState = null;



		////////////////

		private static void OutputDebugLoadDataIf() {
			if( ModLibsConfig.Instance?.DebugModeLoadStages != true ) {
				return;
			}

			string[] loaded = new string[] {
				"MOD: " + LoadLibraries.IsModLoaded(),
				"WORLD: " + LoadLibraries.IsWorldLoaded(),
				"WORLD (PLAY): " + LoadLibraries.IsWorldBeingPlayed(),
				"WORLD (PLAY S): " + LoadLibraries.IsWorldSafelyBeingPlayed(),
				"CP IN GAME: " + (Main.dedServ ? "N/A" : LoadLibraries.IsCurrentPlayerInGame().ToString()),
			};
			string loadedStr = loaded.ToStringJoined( ", " );

			if( !Main.gameMenu ) {
				DebugLibraries.Print( "LOADED", loadedStr );
			}

			if( ModLibsCoreMod.LastLoadState != loadedStr ) {
				ModLibsCoreMod.LastLoadState = loadedStr;

				LogLibraries.Log( "LOAD STATES CHANGED: "+loadedStr );
			}
		}



		////////////////

		private void LoadDebug() {
			var modsys = ModContent.GetInstance<ModLibsCoreModSystem>();
			modsys.TickUpdates.Add( ModLibsCoreMod.OutputDebugLoadDataIf );

			LoadHooks.AddModUnloadHook( this.DebugModUnloadHook );
			LoadHooks.AddPostContentLoadHook( this.DebugPostContentLoadHook );
			LoadHooks.AddPostModLoadHook( this.DebugPostModLoadHook );
			LoadHooks.AddPostWorldLoadEachHook( this.DebugPostWorldLoadEachHook );
			LoadHooks.AddPostWorldLoadOnceHook( this.DebugPostWorldLoadOnceHook );
			LoadHooks.AddPostWorldUnloadEachHook( this.DebugPostWorldUnloadEachHook );
			LoadHooks.AddPostWorldUnloadOnceHook( this.DebugPostWorldUnloadOnceHook );
			LoadHooks.AddSafeWorldLoadEachHook( this.DebugSafeWorldLoadEachHook );
			LoadHooks.AddSafeWorldLoadOnceHook( this.DebugSafeWorldLoadOnceHook );
			LoadHooks.AddWorldInPlayEachHook( this.DebugWorldInPlayEachHook );
			LoadHooks.AddWorldInPlayOnceHook( this.DebugWorldInPlayOnceHook );
			LoadHooks.AddWorldLoadEachHook( this.DebugWorldLoadEachHook );
			LoadHooks.AddWorldLoadOnceHook( this.DebugWorldLoadOnceHook );
			LoadHooks.AddWorldUnloadEachHook( this.DebugWorldUnloadEachHook );
			LoadHooks.AddWorldUnloadOnceHook( this.DebugWorldUnloadOnceHook );
		}
		
		private void UnloadDebug() {
			var modsys = ModContent.GetInstance<ModLibsCoreModSystem>();
			modsys.TickUpdates.Remove( ModLibsCoreMod.OutputDebugLoadDataIf );
		}


		////////////////
		
		private void DebugModUnloadHook() {
			var loadHooks = ModContent.GetInstance<LoadHooks>();
			int count = loadHooks.LastRanHookReportedHookCount - 1;

			//this.DebugModUnloadHookFlag = true;
			//LogLibraries.Log( "DEBUG LOAD - ModUnloadHooks: "+count );
		}
		
		private void DebugPostContentLoadHook() {
			var loadHooks = ModContent.GetInstance<LoadHooks>();
			int count = loadHooks.LastRanHookReportedHookCount - 1;

			//this.DebugPostContentLoadHookFlag = true;
			LogLibraries.Log( "DEBUG LOAD - PostContentLoadHooks: "+count );
		}
		
		private void DebugPostModLoadHook() {
			var loadHooks = ModContent.GetInstance<LoadHooks>();
			int count = loadHooks.LastRanHookReportedHookCount - 1;

			//this.DebugPostModLoadHookFlag = true;
			LogLibraries.Log( "DEBUG LOAD - PostModLoadHooks: "+count );
		}
		
		private void DebugPostWorldLoadEachHook() {
			var loadHooks = ModContent.GetInstance<LoadHooks>();
			int count = loadHooks.LastRanHookReportedHookCount - 1;

			//this.DebugPostWorldLoadEachHookFlag = true;
			LogLibraries.Log( "DEBUG LOAD - WorldLoadEachHooks: "+count );
		}
		
		private void DebugPostWorldLoadOnceHook() {
			var loadHooks = ModContent.GetInstance<LoadHooks>();
			int count = loadHooks.LastRanHookReportedHookCount - 1;

			//this.DebugPostWorldLoadOnceHookFlag = true;
			LogLibraries.Log( "DEBUG LOAD - PostWorldLoadOnceHooks: "+count );
		}
		
		private void DebugPostWorldUnloadEachHook() {
			var loadHooks = ModContent.GetInstance<LoadHooks>();
			int count = loadHooks.LastRanHookReportedHookCount - 1;

			//this.DebugPostWorldUnloadEachHookFlag = true;
			LogLibraries.Log( "DEBUG LOAD - PostWorldUnloadEachHooks: "+count );
		}
		
		private void DebugPostWorldUnloadOnceHook() {
			var loadHooks = ModContent.GetInstance<LoadHooks>();
			int count = loadHooks.LastRanHookReportedHookCount - 1;

			//this.DebugPostWorldUnloadOnceHookFlag = true;
			LogLibraries.Log( "DEBUG LOAD - PostWorldUnloadOnceHooks: "+count );
		}
		
		private void DebugSafeWorldLoadEachHook() {
			var loadHooks = ModContent.GetInstance<LoadHooks>();
			int count = loadHooks.LastRanHookReportedHookCount - 1;

			//this.DebugSafeWorldLoadEachHookFlag = true;
			LogLibraries.Log( "DEBUG LOAD - SafeWorldLoadEachHooks: "+count );
		}
		
		private void DebugSafeWorldLoadOnceHook() {
			var loadHooks = ModContent.GetInstance<LoadHooks>();
			int count = loadHooks.LastRanHookReportedHookCount - 1;

			//this.DebugSafeWorldLoadOnceHookFlag = true;
			LogLibraries.Log( "DEBUG LOAD - SafeWorldLoadOnceHooks: "+count );
		}
		
		private void DebugWorldInPlayEachHook() {
			var loadHooks = ModContent.GetInstance<LoadHooks>();
			int count = loadHooks.LastRanHookReportedHookCount - 1;

			//this.DebugWorldInPlayEachHookFlag = true;
			LogLibraries.Log( "DEBUG LOAD - WorldInPlayEachHooks: "+count );
		}
		
		private void DebugWorldInPlayOnceHook() {
			var loadHooks = ModContent.GetInstance<LoadHooks>();
			int count = loadHooks.LastRanHookReportedHookCount - 1;

			//this.DebugWorldInPlayOnceHookFlag = true;
			LogLibraries.Log( "DEBUG LOAD - WorldInPlayOnceHooks: "+count );
		}
		
		private void DebugWorldLoadEachHook() {
			var loadHooks = ModContent.GetInstance<LoadHooks>();
			int count = loadHooks.LastRanHookReportedHookCount - 1;

			//this.DebugWorldLoadEachHookFlag = true;
			LogLibraries.Log( "DEBUG LOAD - WorldLoadEachHooks: "+count );
		}
		
		private void DebugWorldLoadOnceHook() {
			var loadHooks = ModContent.GetInstance<LoadHooks>();
			int count = loadHooks.LastRanHookReportedHookCount - 1;

			//this.DebugWorldLoadOnceHookFlag = true;
			LogLibraries.Log( "DEBUG LOAD - WorldLoadOnceHooks: "+count );
		}
		
		private void DebugWorldUnloadEachHook() {
			var loadHooks = ModContent.GetInstance<LoadHooks>();
			int count = loadHooks.LastRanHookReportedHookCount - 1;

			//this.DebugWorldUnloadEachHookFlag = true;
			LogLibraries.Log( "DEBUG LOAD - WorldUnloadEachHooks: "+count );
		}
		
		private void DebugWorldUnloadOnceHook() {
			var loadHooks = ModContent.GetInstance<LoadHooks>();
			int count = loadHooks.LastRanHookReportedHookCount - 1;

			//this.DebugWorldUnloadOnceHookFlag = true;
			LogLibraries.Log( "DEBUG LOAD - WorldUnloadOnceHooks: "+count );
		}
	}
}
