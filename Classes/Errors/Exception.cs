using System;
using System.Collections.Generic;
using Terraria.ModLoader;
using ModLibsCore.Libraries.Debug;


namespace ModLibsCore.Classes.Errors {
	/// @private
	class ModLibsExceptionManager : ModSystem {
		internal readonly IDictionary<string, int> MsgCount = new Dictionary<string, int>();
	}




	/// <summary>
	/// Specialized exception with added Mod Libs logging behavior.
	/// </summary>
	public class ModLibsException : Exception {
		/// <param name="msg">Standard message to output.</param>
		public ModLibsException( string msg ) : base( msg ) {
			this.Initialize( msg );
		}

		/// <param name="msg">Standard message to output.</param>
		/// <param name="inner">Inner exception to wrap for further output.</param>
		public ModLibsException( string msg, Exception inner ) : base( msg, inner ) {
			this.Initialize( msg );
		}


		////////////////

		private void Initialize( string msg ) {
			string context = DebugLibraries.GetCurrentContext( 3 );
			var msgCount = ModContent.GetInstance<ModLibsExceptionManager>().MsgCount;
			int count = 0;

			if( msgCount.TryGetValue(msg, out count) ) {
				if( count > 10 && (Math.Log10(count) % 1) != 0 ) {
					return;
				}
			} else {
				msgCount[msg] = 0;
			}
			msgCount[msg]++;

			if( this.InnerException != null ) {
				LogLibraries.Log( "!"+context+" (E#" + count + ") - " + msg + " | " + this.InnerException.Message );
			} else {
				LogLibraries.Log( "!"+context+" (E#" + count + ") - " + msg );
			}
		}
	}
}
