using System;
using Terraria;
using Terraria.ModLoader;
using ModLibsCore.Classes.Loadable;
using ModLibsCore.Libraries.Debug;
using ModLibsCore.Libraries.DotNET.Extensions;
using ModLibsCore.Libraries.TModLoader;
using ModLibsCore.Services.Hooks.LoadHooks;


namespace ModLibsCore {
	/// @private
	partial class ModLibsCoreMod : Mod {
		public static ModLibsCoreMod Instance { get; private set; }



		////////////////

		private static void UnhandledLogger( object sender, UnhandledExceptionEventArgs e ) {
			LogLibraries.Log( "UNHANDLED crash? " + e.IsTerminating
				+ " \nSender: " + sender.ToString()
				+ " \nMessage: " + e.ExceptionObject.ToString() );
		}



		////////////////

		private bool HasUnhandledExceptionLogger = false;



		////////////////
		
		public bool MouseInterface { get; private set; }



		////////////////

		public ModLibsCoreMod() {
			ModLibsCoreMod.Instance = this;

			this.HasSetupContent = false;
			this.HasAddedRecipeGroups = false;
			this.HasAddedRecipes = false;

			this.Loadables = new LoadableManager();
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
			
			ModLibsCoreMod.Instance = null;
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

			Services.Timers.Timers.SetTimer( 1, true, () => {
				this.Loadables.OnPostModsLoad();

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

		public override void PreSaveAndQuit() {
			ModContent.GetInstance<LoadHooks>().PreSaveAndExit();
		}


		////////////////

		public override void PostUpdateEverything() {
			this.MouseInterface = Main.LocalPlayer.mouseInterface;

			if( ModLibsConfig.Instance.DebugModeLoadStages ) {
				string[] loaded = new string[] {
					"mod: " + LoadLibraries.IsModLoaded(),
					"world: " + LoadLibraries.IsWorldLoaded(),
					"world in play: " + LoadLibraries.IsWorldBeingPlayed(),
					"world in play S: " + LoadLibraries.IsWorldSafelyBeingPlayed(),
					"curr plr in game: " + LoadLibraries.IsCurrentPlayerInGame(),
				};
				string loadedStr = loaded.ToStringJoined( ", " );

				if( !Main.gameMenu ) {
					DebugLibraries.Print( "LOADED", loadedStr );
				}

				LogLibraries.LogWhen( "LOAD STATES CHANGED: " + loadedStr, i => i == 0 );

bool? isLPIG = ModContent.GetInstance<LoadLibraries>()?.IsLocalPlayerInGame_Hackish;
LogLibraries.LogWhen( "eh? "+(isLPIG.HasValue?isLPIG.Value.ToString():"null"), i => i == 0 );
			}
		}


		////////////////

		//public override void UpdateMusic( ref int music ) { //, ref MusicPriority priority
		//	this.MusicLibraries.UpdateMusic();
		//}
	}
}
