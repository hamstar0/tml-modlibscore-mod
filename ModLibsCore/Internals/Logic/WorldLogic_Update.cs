using Terraria.ModLoader;
using ModLibsCore.Helpers.TModLoader;
using ModLibsCore.Services.Hooks.LoadHooks;


namespace ModLibsCore.Internals.Logic {
	/// @private
	partial class WorldLogic {
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
			var loadHelpers = ModContent.GetInstance<LoadHelpers>();
			var loadHooks = ModContent.GetInstance<LoadHooks>();

			if( LoadHelpers.IsWorldLoaded() ) {
				loadHooks.FulfillWorldLoadHooks();
			}

			if( LoadHelpers.IsWorldBeingPlayed() ) {
				loadHooks.FulfillWorldInPlayHooks();

				loadHelpers.UpdateUponWorldBeingPlayed();
			}

			if( LoadHelpers.IsWorldSafelyBeingPlayed() ) {
				loadHooks.FulfillSafeWorldLoadHook();
			}
		}

		private void PreUpdateLocal() {
		}
	}
}
