using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.GameInput;
using Terraria.ModLoader;
using ModLibsCore.Classes.Errors;
using ModLibsCore.Helpers.Debug;
using ModLibsCore.Services.Debug.DataDumper;
using ModLibsCore.Services.Debug.CustomHotkeys;


namespace ModLibsCore {
	/// @private
	partial class ModLibsPlayer : ModPlayer {
		public override bool CloneNewInstances => false;



		////////////////

		public override void ProcessTriggers( TriggersSet triggersSet ) {
//DataStore.Add( DebugHelpers.GetCurrentContext()+"_"+this.player.name+":"+this.player.whoAmI+"_A", 1 );
			var mymod = (ModLibsCoreMod)this.mod;

			try {
				if( mymod.DataDumpHotkey != null && mymod.DataDumpHotkey.JustPressed ) {
					string fileName;
					if( DataDumper.DumpToFile( out fileName, true ) ) {
						string msg = "Dumped latest debug data to log file " + fileName;

						Main.NewText( msg, Color.Azure );
						LogHelpers.Log( msg );
					}
				}
			} catch(Exception e ) {
				LogHelpers.Warn( "(2) - " + e.ToString() );
				return;
			}

			try {
				ModContent.GetInstance<CustomHotkeys>()?.ProcessTriggers( triggersSet );
			} catch(Exception e ) {
				LogHelpers.Warn( "(3) - " + e.ToString() );
				return;
			}
			//DataStore.Add( DebugHelpers.GetCurrentContext()+"_"+this.player.name+":"+this.player.whoAmI+"_B", 1 );
		}
	}
}
