using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.ModLoader;
using ModLibsCore.Libraries.Debug;
using ModLibsCore.Services.Hooks.LoadHooks;
using ModLibsCore.Internals.Logic;


namespace ModLibsCore {
	/// @private
	partial class ModLibsCoreModSystem : ModSystem {
		public bool MouseInterface { get; private set; }

		////

		internal IList<Action> TickUpdates = new List<Action>();
		


		////////////////

		public override void PreSaveAndQuit() {
			ModContent.GetInstance<LoadHooks>().PreSaveAndExit();
		}


		////////////////

		public override void PostUpdateEverything() {
			this.MouseInterface = Main.LocalPlayer.mouseInterface;
		}


		////////////////

		public override void PostUpdateTime() {
			var logic = ModContent.GetInstance<WorldLogic>();
			if( logic != null ) {
				logic.Update();
			}

			foreach( Action action in this.TickUpdates.ToArray() ) {
				action();
			}
		}
	}
}
