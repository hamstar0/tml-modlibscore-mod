using System;
using System.Collections.Generic;
using Terraria.ModLoader;
using ModLibsCore.Classes.Errors;
using ModLibsCore.Libraries.Debug;
using ModLibsCore.Services.Hooks.LoadHooks;


namespace ModLibsCore.Libraries.Players {
	/// @private
	public partial class PlayerIdentityLibraries : ModSystem {
		internal IDictionary<int, string> PlayerIds = new Dictionary<int, string>();



		////////////////
		
		public override void OnModLoad() {
			LoadHooks.AddPostWorldUnloadEachHook( () => {
				this.PlayerIds = new Dictionary<int, string>();
			} );
		}
	}
}
