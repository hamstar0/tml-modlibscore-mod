using System;
using System.Reflection;
using Terraria;
using Terraria.ModLoader;
using ModLibsCore.Libraries.Players;
using ModLibsCore.Libraries.World;
using ModLibsCore.Libraries.Debug;
using ModLibsCore.Services.Debug.DataDumper;


namespace ModLibsCore {
	/// @private
	partial class ModLibsCoreMod : Mod {
		private void LoadExceptionBehavior() {
			if( ModLibsConfig.Instance.DebugModeDisableSilentLogging ) {
				var flags = Libraries.DotNET.Reflection.ReflectionLibraries.MostAccess;
				FieldInfo fceField = typeof( AppDomain ).GetField( "FirstChanceException", flags );
				if( fceField == null ) {
					fceField = typeof( AppDomain ).GetField( "_firstChanceException", flags );
				}
				if( fceField != null ) {
					//if( field != null && (field.FieldType == typeof(MulticastDelegate) || field.FieldType.IsSubclassOf( typeof(MulticastDelegate) )) ) {
					fceField.SetValue( AppDomain.CurrentDomain, null );
				}
			}

			if( !this.HasUnhandledExceptionLogger && ModLibsConfig.Instance.DebugModeUnhandledExceptionLogging ) {
				this.HasUnhandledExceptionLogger = true;
				AppDomain.CurrentDomain.UnhandledException += ModLibsCoreMod.UnhandledLogger;
			}
		}


		private void LoadHotkeys() {
			this.DataDumpHotkey = KeybindLoader.RegisterKeybind(this, "Dump Debug Data", "P" );
		}


		private void LoadDataSources() {
			DataDumper.SetDumpSource( "WorldUidWithSeed", () => {
				return "  " + WorldIdentityLibraries.GetUniqueIdForCurrentWorld(true) + " (net mode: " + Main.netMode + ")";
			} );

			DataDumper.SetDumpSource( "PlayerUid", () => {
				if( Main.myPlayer < 0 || Main.myPlayer >= ( Main.player.Length - 1 ) ) {
					return "  Unobtainable";
				}

				return "  " + PlayerIdentityLibraries.GetUniqueId();
			} );
		}
	}
}
