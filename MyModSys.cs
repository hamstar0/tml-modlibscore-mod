using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.ModLoader;
using ModLibsCore.Libraries.Debug;
using ModLibsCore.Services.Hooks.LoadHooks;
using ModLibsCore.Internals.Logic;
using ModLibsCore.Libraries.Items.Attributes;
using ModLibsCore.Libraries.NPCs.Attributes;
using ModLibsCore.Libraries.Projectiles.Attributes;

namespace ModLibsCore {
	/// @private
	partial class ModLibsCoreModSystem : ModSystem {
		public bool MouseInterface { get; private set; }

		////

		internal IList<Action> TickUpdates = new List<Action>();


		////////////////

		public override void PostUpdateEverything() {
			this.MouseInterface = Main.LocalPlayer.mouseInterface;
		}

		public override void AddRecipeGroups()/* tModPorter Note: Removed. Use ModSystem.AddRecipeGroups */ {
			ModLibsCoreMod.Instance.HasAddedRecipeGroups = true;
			ModLibsCoreMod.Instance.CheckAndProcessLoadFinish();
		}

		public override void PostAddRecipes()/* tModPorter Note: Removed. Use ModSystem.PostAddRecipes */ {
			this.PostAddRecipesFull();

			ModLibsCoreMod.Instance.HasAddedRecipes = true;
			ModLibsCoreMod.Instance.CheckAndProcessLoadFinish();
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

		////////////////

		private void PostAddRecipesFull() {
			ModContent.GetInstance<ItemNameAttributeLibraries>().PopulateNames();
			ModContent.GetInstance<NPCNameAttributeLibraries>().PopulateNames();
			ModContent.GetInstance<ProjectileNameAttributeLibraries>().PopulateNames();
		}
	}
}
