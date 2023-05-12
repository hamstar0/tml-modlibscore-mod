using System;
using Terraria;
using Terraria.ModLoader;
using ModLibsCore.Libraries.Debug;
using ModLibsCore.Libraries.Items.Attributes;
using ModLibsCore.Libraries.NPCs.Attributes;
using ModLibsCore.Libraries.Projectiles.Attributes;


namespace ModLibsCore {
	/// @private
	partial class ModLibsCoreMod : Mod {
		public bool HasSetupContent { get; internal set; }
		public bool HasAddedRecipeGroups { get; internal set; }
		public bool HasAddedRecipes { get; internal set; }


		////////////////


		public ModKeybind DataDumpHotkey = null;



		////////////////

		private void LoadFull() {
			this.LoadExceptionBehavior();

			this.LoadDebug();

			this.LoadHotkeys();
			this.LoadDataSources();
		}


		////////////////

		private void UnloadFull() {
			try {
				this.UnloadDebug();
			} catch( Exception e ) {
				this.Logger.Warn( "!ModLibs.ModLibsMod.UnloadFull - " + e.ToString() );	//was Error(...)
			}

			try {
				if( this.HasUnhandledExceptionLogger ) {
					this.HasUnhandledExceptionLogger = false;
					AppDomain.CurrentDomain.UnhandledException -= ModLibsCoreMod.UnhandledLogger;
				}
			} catch { }
		}
	}
}
