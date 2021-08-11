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
				"IN GAME: " + LoadLibraries.IsCurrentPlayerInGame(),
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
			Main.OnTick += ModLibsCoreMod.OutputDebugLoadDataIf;

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
			Main.OnTick -= ModLibsCoreMod.OutputDebugLoadDataIf;
		}


		////////////////
		
		private void DebugModUnloadHook() {
			var loadHooks = ModContent.GetInstance<LoadHooks>();
			int count = loadHooks.ModUnloadHooks.Count - 1;

			//this.DebugModUnloadHookFlag = true;
			LogLibraries.Log( "DEBUG LOAD - ModUnloadHooks: "+count );
		}
		
		private void DebugPostContentLoadHook() {
			var loadHooks = ModContent.GetInstance<LoadHooks>();
			int count = loadHooks.PostContentLoadHooks.Count - 1;

			//this.DebugPostContentLoadHookFlag = true;
			LogLibraries.Log( "DEBUG LOAD - PostContentLoadHooks: "+count );
		}
		
		private void DebugPostModLoadHook() {
			var loadHooks = ModContent.GetInstance<LoadHooks>();
			int count = loadHooks.PostModLoadHooks.Count - 1;

			//this.DebugPostModLoadHookFlag = true;
			LogLibraries.Log( "DEBUG LOAD - PostModLoadHooks: "+count );
		}
		
		private void DebugPostWorldLoadEachHook() {
			var loadHooks = ModContent.GetInstance<LoadHooks>();
			int count = loadHooks.WorldLoadEachHooks.Count - 1;

			//this.DebugPostWorldLoadEachHookFlag = true;
			LogLibraries.Log( "DEBUG LOAD - WorldLoadEachHooks: "+count );
		}
		
		private void DebugPostWorldLoadOnceHook() {
			var loadHooks = ModContent.GetInstance<LoadHooks>();
			int count = loadHooks.PostWorldLoadOnceHooks.Count - 1;

			//this.DebugPostWorldLoadOnceHookFlag = true;
			LogLibraries.Log( "DEBUG LOAD - PostWorldLoadOnceHooks: "+count );

			LoadHooks.AddPostWorldLoadOnceHook( this.DebugPostWorldLoadOnceHook );
		}
		
		private void DebugPostWorldUnloadEachHook() {
			var loadHooks = ModContent.GetInstance<LoadHooks>();
			int count = loadHooks.PostWorldUnloadEachHooks.Count - 1;

			//this.DebugPostWorldUnloadEachHookFlag = true;
			LogLibraries.Log( "DEBUG LOAD - PostWorldUnloadEachHooks: "+count );
		}
		
		private void DebugPostWorldUnloadOnceHook() {
			var loadHooks = ModContent.GetInstance<LoadHooks>();
			int count = loadHooks.PostWorldUnloadOnceHooks.Count - 1;

			//this.DebugPostWorldUnloadOnceHookFlag = true;
			LogLibraries.Log( "DEBUG LOAD - PostWorldUnloadOnceHooks: "+count );

			LoadHooks.AddPostWorldUnloadOnceHook( this.DebugPostWorldUnloadOnceHook );
		}
		
		private void DebugSafeWorldLoadEachHook() {
			var loadHooks = ModContent.GetInstance<LoadHooks>();
			int count = loadHooks.PostWorldUnloadOnceHooks.Count - 1;

			//this.DebugSafeWorldLoadEachHookFlag = true;
			LogLibraries.Log( "DEBUG LOAD - SafeWorldLoadEachHooks: "+count );
		}
		
		private void DebugSafeWorldLoadOnceHook() {
			var loadHooks = ModContent.GetInstance<LoadHooks>();
			int count = loadHooks.SafeWorldLoadOnceHooks.Count - 1;

			//this.DebugSafeWorldLoadOnceHookFlag = true;
			LogLibraries.Log( "DEBUG LOAD - SafeWorldLoadOnceHooks: "+count );

			LoadHooks.AddSafeWorldLoadOnceHook( this.DebugSafeWorldLoadOnceHook );
		}
		
		private void DebugWorldInPlayEachHook() {
			var loadHooks = ModContent.GetInstance<LoadHooks>();
			int count = loadHooks.WorldInPlayEachHooks.Count - 1;

			//this.DebugWorldInPlayEachHookFlag = true;
			LogLibraries.Log( "DEBUG LOAD - WorldInPlayEachHooks: "+count );
		}
		
		private void DebugWorldInPlayOnceHook() {
			var loadHooks = ModContent.GetInstance<LoadHooks>();
			int count = loadHooks.WorldInPlayOnceHooks.Count - 1;

			//this.DebugWorldInPlayOnceHookFlag = true;
			LogLibraries.Log( "DEBUG LOAD - WorldInPlayOnceHooks: "+count );

			LoadHooks.AddWorldInPlayOnceHook( this.DebugWorldInPlayOnceHook );
		}
		
		private void DebugWorldLoadEachHook() {
			var loadHooks = ModContent.GetInstance<LoadHooks>();
			int count = loadHooks.WorldLoadEachHooks.Count - 1;

			//this.DebugWorldLoadEachHookFlag = true;
			LogLibraries.Log( "DEBUG LOAD - WorldLoadEachHooks: "+count );
		}
		
		private void DebugWorldLoadOnceHook() {
			var loadHooks = ModContent.GetInstance<LoadHooks>();
			int count = loadHooks.WorldLoadOnceHooks.Count - 1;

			//this.DebugWorldLoadOnceHookFlag = true;
			LogLibraries.Log( "DEBUG LOAD - WorldLoadOnceHooks: "+count );

			LoadHooks.AddWorldLoadOnceHook( this.DebugWorldLoadOnceHook );
		}
		
		private void DebugWorldUnloadEachHook() {
			var loadHooks = ModContent.GetInstance<LoadHooks>();
			int count = loadHooks.WorldUnloadEachHooks.Count - 1;

			//this.DebugWorldUnloadEachHookFlag = true;
			LogLibraries.Log( "DEBUG LOAD - WorldUnloadEachHooks: "+count );
		}
		
		private void DebugWorldUnloadOnceHook() {
			var loadHooks = ModContent.GetInstance<LoadHooks>();
			int count = loadHooks.WorldUnloadOnceHooks.Count - 1;

			//this.DebugWorldUnloadOnceHookFlag = true;
			LogLibraries.Log( "DEBUG LOAD - WorldUnloadOnceHooks: "+count );

			LoadHooks.AddWorldUnloadOnceHook( this.DebugWorldUnloadOnceHook );
		}
	}
}
