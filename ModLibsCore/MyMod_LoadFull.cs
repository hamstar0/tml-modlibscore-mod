using System;
using Terraria;
using Terraria.IO;
using Terraria.ModLoader;
using ModLibsCore.Classes.Loadable;
using ModLibsCore.Helpers.Debug;
using ModLibsCore.Helpers.Items.Attributes;
using ModLibsCore.Helpers.NPCs.Attributes;
using ModLibsCore.Helpers.Projectiles.Attributes;
using ModLibsCore.Internals.Logic;


namespace ModLibsCore {
	/// @private
	partial class ModLibsCoreMod : Mod {
		public bool HasSetupContent { get; private set; }
		public bool HasAddedRecipeGroups { get; private set; }
		public bool HasAddedRecipes { get; private set; }


		////////////////

		// Classes
		internal LoadableManager Loadables;


		public ModHotKey ControlPanelHotkey = null;
		public ModHotKey DataDumpHotkey = null;



		////////////////

		private void LoadFull() {
			this.LoadExceptionBehavior();

			this.Loadables.OnModsLoad();

			this.LoadHotkeys();
			this.LoadDataSources();

			WorldFile.OnWorldLoad += WorldLogic.OnWorldLoad;
		}


		////////////////

		private void PostAddRecipesFull() {
			ModContent.GetInstance<ItemAttributeHelpers>().PopulateNames();
			ModContent.GetInstance<NPCAttributeHelpers>().PopulateNames();
			ModContent.GetInstance<ProjectileAttributeHelpers>().PopulateNames();
		}


		////////////////

		private void UnloadFull() {
			WorldFile.OnWorldLoad -= WorldLogic.OnWorldLoad;

			try {
				this.Loadables.OnModsUnload();
			} catch( Exception e ) {
				this.Logger.Warn( "!ModHelpers.ModHelpersMod.UnloadFull - " + e.ToString() );	//was Error(...)
			}

			try {
				if( this.HasUnhandledExceptionLogger ) {
					this.HasUnhandledExceptionLogger = false;
					AppDomain.CurrentDomain.UnhandledException -= ModLibsCoreMod.UnhandledLogger;
				}
			} catch { }

			this.Loadables = null;
			this.ControlPanelHotkey = null;
			this.DataDumpHotkey = null;
		}
	}
}
