using System;
using System.Collections.Generic;
using Terraria;


namespace ModLibsCore.Libraries.Debug {
	/// <summary>
	/// Assorted static "helper" functions pertaining to debugging and debug outputs.
	/// </summary>
	public partial class DebugLibraries {
		private static object MyChatLock = new object();

		////////////////

		private static IDictionary<string, string> Chats = new Dictionary<string, string>();
		private static IDictionary<string, int> ChatTimes = new Dictionary<string, int>();



		////////////////

		/// <summary>
		/// Displays a given chat message only once (every log10 = whole number).
		/// </summary>
		/// <param name="msg"></param>
		public static void ChatOnce( string msg ) {
			DebugLibraries.ChatOnce( msg, msg );
		}

		/// <summary>
		/// Displays a given chat message under a given id only once (every log10 = whole number).
		/// </summary>
		/// <param name="id"></param>
		/// <param name="msg"></param>
		public static void ChatOnce( string id, string msg ) {
			int times = 0;
			
			lock( DebugLibraries.MyChatLock ) {
				if( DebugLibraries.Chats.ContainsKey(id) ) {
					times = DebugLibraries.ChatTimes[id]++;
				} else {
					DebugLibraries.Chats[ id ] = msg;
				}
				
				if( (Math.Log10((double)times) % 1d) == 0 ) {
					Main.NewText( "["+times+"] "+msg );
				}
			}
		}
	}
}
