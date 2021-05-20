using System;
using ModLibsCore.Classes.Loadable;
using ModLibsCore.Libraries.Debug;
using ModLibsCore.Services.Hooks.LoadHooks;


namespace ModLibsCore.Libraries.TModLoader {
	/// @private
	public partial class LoadLibraries : ILoadable {
		internal uint WorldStartupDelay = 0;

		internal bool IsLocalPlayerInGame_Hackish = false;
		internal bool HasServerBegunHavingPlayers_Hackish = false;



		////////////////

		internal LoadLibraries() {
			LoadHooks.AddWorldLoadEachHook( () => {
				this.WorldStartupDelay = 0;
			} );
			LoadHooks.AddWorldUnloadEachHook( () => {
				this.WorldStartupDelay = 0;
				this.IsLocalPlayerInGame_Hackish = false;
			} );
			LoadHooks.AddPostWorldUnloadEachHook( () => { // Redundant?
				this.WorldStartupDelay = 0;
				this.IsLocalPlayerInGame_Hackish = false;
			} );
		}

		void ILoadable.OnModsLoad() { }

		void ILoadable.OnPostModsLoad() { }

		void ILoadable.OnModsUnload() { }


		////////////////

		internal void UpdateUponWorldBeingPlayed() {
			this.WorldStartupDelay++;    // Seems needed for day/night tracking (and possibly other things?)
		}
	}
}
