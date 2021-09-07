using System;
using Terraria;
using Terraria.ID;
using ModLibsCore.Classes.Loadable;
using ModLibsCore.Libraries.Debug;
using ModLibsCore.Services.Hooks.LoadHooks;


namespace ModLibsCore.Internals.Logic {
	/// @private
	partial class WorldLogic : ILoadable {
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

		void ILoadable.OnModsLoad() {
			void onLoadWorld( On.Terraria.IO.WorldFile.orig_loadWorld orig, bool loadFromCloud ) {
				try {
					orig.Invoke( loadFromCloud );
				} catch( Exception e ) {
					LogLibraries.Warn( "Error loading world: " + e.ToString() );
				}
				
				WorldLogic.IsLoaded = true;	// I guess load it anyway?

				LogLibraries.Alert( "World loaded." );
			}

			//

			Player.Hooks.OnEnterWorld += WorldLogic.OnEnterWorldClientOnly;
			On.Terraria.IO.WorldFile.loadWorld += onLoadWorld;

			LogLibraries.Alert( "World load hook loaded." );
		}

		void ILoadable.OnPostModsLoad() {
			LoadHooks.AddWorldUnloadEachHook( () => WorldLogic.IsLoaded = false );
		}

		void ILoadable.OnModsUnload() {
			Player.Hooks.OnEnterWorld -= WorldLogic.OnEnterWorldClientOnly;
		}
	}
}
