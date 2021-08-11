using Terraria.ModLoader;
using ModLibsCore.Classes.Loadable;
using ModLibsCore.Libraries.TModLoader;
using ModLibsCore.Services.Hooks.LoadHooks;


namespace ModLibsCore.Internals.Logic {
	/// @private
	partial class WorldLogic : ILoadable {
		public void PreUpdateSingle() {
			this.PreUpdateShared();
			this.PreUpdateLocal();
		}

		public void PreUpdateClient() {
			this.PreUpdateShared();
			this.PreUpdateLocal();
		}
		
		public void PreUpdateServer() {
			this.PreUpdateShared();
		}


		////////////////

		private void PreUpdateShared() {
			var loadLibs = ModContent.GetInstance<LoadLibraries>();
			var loadHooks = ModContent.GetInstance<LoadHooks>();

			if( LoadLibraries.IsWorldLoaded() ) {
				loadHooks.FulfillWorldLoadHooks();
			}

			if( LoadLibraries.IsWorldBeingPlayed() ) {
				loadHooks.FulfillWorldInPlayHooks();

				loadLibs.UpdateUponWorldBeingPlayed();
			}

			if( LoadLibraries.IsWorldSafelyBeingPlayed() ) {
				loadHooks.FulfillSafeWorldLoadHook();
			}
		}

		private void PreUpdateLocal() {
		}
	}
}
