﻿using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.ModLoader;
using ModLibsCore.Libraries.Debug;


namespace ModLibsCore.Services.Hooks.Draw {
	class DrawHooksInternal : ModSystem {
		internal ISet<Func<bool>> PostDrawTilesHooks = new HashSet<Func<bool>>();



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
