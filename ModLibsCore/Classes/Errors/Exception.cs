﻿using System;
using System.Collections.Generic;
using Terraria.ModLoader;
using ModLibsCore.Classes.Loadable;
using ModLibsCore.Helpers.Debug;


namespace ModLibsCore.Classes.Errors {
	/// @private
	class ModHelpersExceptionManager : ILoadable {
		internal readonly IDictionary<string, int> MsgCount = new Dictionary<string, int>();



		////////////////

		void ILoadable.OnModsLoad() { }

		void ILoadable.OnPostModsLoad() { }

		void ILoadable.OnModsUnload() { }
	}




	/// <summary>
	/// Specialized exception with added Mod Helpers logging behavior.
	/// </summary>
	public class ModHelpersException : Exception {
		/// <param name="msg">Standard message to output.</param>
		public ModHelpersException( string msg ) : base( msg ) {
			this.Initialize( msg );
		}

		/// <param name="msg">Standard message to output.</param>
		/// <param name="inner">Inner exception to wrap for further output.</param>
		public ModHelpersException( string msg, Exception inner ) : base( msg, inner ) {
			this.Initialize( msg );
		}


		////////////////

		private void Initialize( string msg ) {
			string context = DebugHelpers.GetCurrentContext( 3 );
			var msgCount = ModContent.GetInstance<ModHelpersExceptionManager>().MsgCount;
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
				LogHelpers.Log( "!"+context+" (E#" + count + ") - " + msg + " | " + this.InnerException.Message );
			} else {
				LogHelpers.Log( "!"+context+" (E#" + count + ") - " + msg );
			}
		}
	}
}
