using System;
using System.Collections.Generic;
using ModLibsCore.Classes.Errors;
using ModLibsCore.Classes.Loadable;
using ModLibsCore.Helpers.Debug;
using ModLibsCore.Services.Hooks.LoadHooks;


namespace ModLibsCore.Helpers.Players {
	/// @private
	public partial class PlayerIdentityHelpers : ILoadable {
		internal IDictionary<int, string> PlayerIds = new Dictionary<int, string>();



		////////////////
		
		void ILoadable.OnModsLoad() { }

		void ILoadable.OnPostModsLoad() {
			LoadHooks.AddPostWorldUnloadEachHook( () => {
				this.PlayerIds = new Dictionary<int, string>();
			} );
		}

		void ILoadable.OnModsUnload() { }
	}
}
