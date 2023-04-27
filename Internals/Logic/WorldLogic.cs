using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using ModLibsCore.Libraries.Debug;
using ModLibsCore.Services.Hooks.LoadHooks;


namespace ModLibsCore.Internals.Logic {
	/// @private
	partial class WorldLogic : ModSystem {
		public static bool IsLoaded { get; private set; } = false;



		////////////////

		private static void OnEnterWorldClientOnly( Player player ) {
			if( Main.netMode != NetmodeID.MultiplayerClient ) {
				return;
			}

			WorldLogic.IsLoaded = true; // Clients don't 'load' worlds I guess

			LogLibraries.Alert( "World \"loaded\"." );
		}



		////////////////

		public override void OnWorldLoad() {
			Player.Hooks.OnEnterWorld += WorldLogic.OnEnterWorldClientOnly;
			On.Terraria.IO.WorldFile.LoadWorld += this.OnLoadWorld_Inject;

			LogLibraries.Alert( "World load hook loaded." );
		}

		public override void PreSaveAndQuit() {
			LoadHooks.AddWorldUnloadEachHook( () => WorldLogic.IsLoaded = false );
		}

		public override void Unload() {
			Player.Hooks.OnEnterWorld -= WorldLogic.OnEnterWorldClientOnly;
		}

		////

		private void OnLoadWorld_Inject( On.Terraria.IO.WorldFile.orig_LoadWorld orig, bool loadFromCloud ) {
			try {
				orig.Invoke( loadFromCloud );
			} catch( Exception e ) {
				LogLibraries.Warn( "Error loading world: " + e.ToString() );
			}

			WorldLogic.IsLoaded = true; // I guess load it anyway?

			LogLibraries.Alert( "World loaded." );
		}
	}
}
