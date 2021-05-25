using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using ModLibsCore.Classes.Loadable;
using ModLibsCore.Libraries.Debug;


namespace ModLibsCore.Services.Hooks.Draw {
	class DrawHooksInternal : ILoadable {
		internal ISet<Func<bool>> PostDrawTilesHooks = new HashSet<Func<bool>>();



		////////////////

		/// @ private
		void ILoadable.OnModsLoad() { }
		/// @ private
		void ILoadable.OnModsUnload() { }
		/// @ private
		void ILoadable.OnPostModsLoad() { }


		////////////////

		internal void RunPostDrawTilesActions() {
			foreach( Func<bool> action in this.PostDrawTilesHooks.ToArray() ) {
				if( !action() ) {
					this.PostDrawTilesHooks.Remove( action );
				}
			}
		}
	}
}
