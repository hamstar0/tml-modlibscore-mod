using System;
using Terraria;


namespace ModLibsCore.Libraries.Debug {
	/// <summary>
	/// Assorted static "helper" functions pertaining to log outputs.
	/// </summary>
	public partial class LogLibraries {
		private static void DirectInfo( string msg ) {
			var mymod = ModLibsCoreMod.Instance;

			try {
				mymod.Logger.Info( msg );
				//ErrorLogger.Log( logged + msg );
			} catch( Exception e ) {
				try {
					mymod.Logger.Info( "FALLBACK LOGGER (" + e.GetType().Name + ") " + msg );
					//ErrorLogger.Log( "FALLBACK LOGGER 2 (" + e.GetType().Name + ") " + msg );
				} catch { }
			}
		}

		private static void DirectAlert( string msg ) {
			var mymod = ModLibsCoreMod.Instance;

			try {
				mymod.Logger.Warn( msg );	//was Error(...)
			} catch( Exception e ) {
				try {
					mymod.Logger.Warn( "FALLBACK LOGGER (" + e.GetType().Name + ") " + msg );   //was Error(...)
				} catch { }
			}
		}

		private static void DirectWarn( string msg ) {
			var mymod = ModLibsCoreMod.Instance;

			try {
				mymod.Logger.Error( msg );	//was Fatal(...)
			} catch( Exception e ) {
				try {
					mymod.Logger.Error( "FALLBACK LOGGER (" + e.GetType().Name + ") " + msg );	//was Fatal(...)
				} catch { }
			}
		}
	}
}
