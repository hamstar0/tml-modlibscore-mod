using System;
using Terraria;
using Terraria.ModLoader;
using ModLibsCore.Libraries.Debug;
using ModLibsCore.Services.Hooks.LoadHooks;


namespace ModLibsCore.Libraries.TModLoader {
	/// @private
	public partial class LoadLibraries : ModSystem {
		private uint WorldStartupDelay = 0;

		internal bool IsLocalPlayerInGame_Hackish = false;
		internal bool HasGameBegunHavingPlayers_Hackish = false;



		////////////////

		internal LoadLibraries() {
			LoadHooks.AddWorldLoadEachHook( () => {
				this.WorldStartupDelay = 0;
			} );
			LoadHooks.AddWorldUnloadEachHook( () => {
				this.WorldStartupDelay = 0;
				this.IsLocalPlayerInGame_Hackish = false;
				this.HasGameBegunHavingPlayers_Hackish = false;
			} );
			LoadHooks.AddPostWorldUnloadEachHook( () => { // Redundant?
				this.WorldStartupDelay = 0;
				this.IsLocalPlayerInGame_Hackish = false;
				this.HasGameBegunHavingPlayers_Hackish = false;
			} );
		}


		////////////////

		internal void UpdateUponPlayerPlaying( Player player ) {
			this.HasGameBegunHavingPlayers_Hackish = true;	// Hackish, but reliable?
		}

		internal void UpdateUponWorldBeingPlayed() {
			this.WorldStartupDelay++;
		}
	}
}
