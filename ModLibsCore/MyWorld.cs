using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using ModLibsCore.Classes.Errors;
using ModLibsCore.Helpers.Debug;
using ModLibsCore.Internals.Logic;


namespace ModLibsCore {
	/// @private
	class ModLibsWorld : ModWorld {
		internal WorldLogic WorldLogic { get; private set; }



		////////////////

		public override void Initialize() {
			this.WorldLogic = new WorldLogic();
		}


		////////////////

		public override void PreUpdate() {
			if( this.WorldLogic != null ) {
				if( Main.netMode == NetmodeID.SinglePlayer ) { // Single
					this.WorldLogic.PreUpdateSingle();
				} else if( Main.netMode == NetmodeID.Server ) {
					this.WorldLogic.PreUpdateServer();
				}
			}
		}
	}
}
