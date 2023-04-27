using System;
using System.Collections.Generic;
using Terraria.ModLoader;
using ModLibsCore.Classes.Errors;
using ModLibsCore.Libraries.Debug;
using ModLibsCore.Services.Hooks.LoadHooks;


namespace ModLibsCore.Libraries.Players {
	/// @private
	public partial class PlayerIdentityLibraries : ILoadable {
		internal IDictionary<int, string> PlayerIds = new Dictionary<int, string>();



		////////////////
		
		void ILoadable.Load( Mod mod ) {
			LoadHooks.AddPostWorldUnloadEachHook( () => {
				this.PlayerIds = new Dictionary<int, string>();
			} );
		}

		void ILoadable.Unload() { }
	}
}
