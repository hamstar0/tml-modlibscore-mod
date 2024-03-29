﻿using Terraria.ModLoader;
using ModLibsCore.Libraries.TModLoader;
using ModLibsCore.Services.Hooks.LoadHooks;


namespace ModLibsCore.Internals.Logic {
	/// @private
	partial class WorldLogic : ModSystem {
		public void Update() {
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
				loadHooks.FulfillSafeWorldLoadHooks();
			}
		}

		//private void PreUpdateLocal() {
		//}
	}
}
