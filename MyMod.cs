using System;
using Terraria;
using Terraria.ModLoader;
using ModLibsCore.Libraries.Debug;
using ModLibsCore.Services.Hooks.LoadHooks;


namespace ModLibsCore {
	/// @private
	partial class ModLibsCoreMod : Mod {
		public static ModLibsCoreMod Instance { get; private set; }

		////

		public static string GithubUserName => "hamstar0";
		public static string GithubProjectName => "tml-modlibscore-mod";



		////////////////

		private static void UnhandledLogger( object sender, UnhandledExceptionEventArgs e ) {
			LogLibraries.Log( "UNHANDLED crash? " + e.IsTerminating
				+ " \nSender: " + sender.ToString()
				+ " \nMessage: " + e.ExceptionObject.ToString() );
		}



		////////////////

		private bool HasUnhandledExceptionLogger = false;



		////////////////

		public ModLibsCoreMod() {
			ModLibsCoreMod.Instance = this;

			this.HasSetupContent = false;
			this.HasAddedRecipeGroups = false;
			this.HasAddedRecipes = false;
		}


		public override void Load() {
			//ErrorLogger.Log( "Loading Mod Libs. Ensure you have .NET Framework v4.6+ installed, if you're having problems." );
			//if( Environment.Version < new Version( 4, 0, 30319, 42000 ) ) {
			//	SystemLibraries.OpenUrl( "https://dotnet.microsoft.com/download/dotnet-framework-runtime" );
			//	throw new FileNotFoundException( "Mod Libs "+this.Version+" requires .NET Framework v4.6+ to work." );
			//}

			this.LoadFull();
		}

		////

		public override void Unload() {
			try {
				LogLibraries.Alert( "Unloading mod..." );
			} catch { }

			this.UnloadFull();
		}


		////////////////

		public override void PostSetupContent() {
			this.HasSetupContent = true;

			this.CheckAndProcessLoadFinish();

			ModContent.GetInstance<LoadHooks>().FulfillPostContentLoadHooks();
		}

		////////////////

		public override void AddRecipeGroups() {
			this.HasAddedRecipeGroups = true;

			this.CheckAndProcessLoadFinish();
		}
		
		public override void PostAddRecipes() {
			this.PostAddRecipesFull();

			this.HasAddedRecipes = true;
			this.CheckAndProcessLoadFinish();
		}


		////////////////

		private void CheckAndProcessLoadFinish() {
			if( !this.HasSetupContent ) { return; }
			if( !this.HasAddedRecipeGroups ) { return; }
			if( !this.HasAddedRecipes ) { return; }

			//

			Services.Timers.Timers.SetTimer( 1, true, () => {
				ModContent.GetInstance<LoadHooks>().FulfillPostModLoadHooks();
				return false;
			} );
/*DataDumper.SetDumpSource( "DEBUG", () => {
	var data = Services.DataStore.DataStore.GetAll();
	string str = "";

	foreach( var kv in data ) {
		string key = kv.Key as string;
		if( key == null ) { continue; }

		if( key[key.Length-1] != 'A' ) {
			continue;
		}

		string keyB = key.Substring( 0, key.Length - 1 ) + "B";
		if( !data.ContainsKey(keyB) ) { continue; }

		double valA = (double)kv.Value;
		double valB = (double)data[keyB];
		if( valA != valB ) {
			str += key + " " + valA + " vs " + valB+",\n";
		}
	}

	return str;
} );*/
		}


		////////////////

		//public override void UpdateMusic( ref int music ) { //, ref MusicPriority priority
		//	this.MusicLibraries.UpdateMusic();
		//}
	}
}
