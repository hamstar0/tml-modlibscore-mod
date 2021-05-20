using System;
using Terraria;
using Terraria.IO;
using Terraria.ModLoader;
using ModLibsCore.Classes.Loadable;
using ModLibsCore.Libraries.Debug;
using ModLibsCore.Libraries.Items.Attributes;
using ModLibsCore.Libraries.NPCs.Attributes;
using ModLibsCore.Libraries.Projectiles.Attributes;
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
			ModContent.GetInstance<ItemAttributeLibraries>().PopulateNames();
			ModContent.GetInstance<NPCAttributeLibraries>().PopulateNames();
			ModContent.GetInstance<ProjectileAttributeLibraries>().PopulateNames();
		}


		////////////////

		private void UnloadFull() {
			WorldFile.OnWorldLoad -= WorldLogic.OnWorldLoad;

			try {
				this.Loadables.OnModsUnload();
			} catch( Exception e ) {
				this.Logger.Warn( "!ModLibs.ModLibsMod.UnloadFull - " + e.ToString() );	//was Error(...)
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
