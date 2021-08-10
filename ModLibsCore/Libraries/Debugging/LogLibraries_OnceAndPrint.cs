using System;
using Microsoft.Xna.Framework;
using Terraria;


namespace ModLibsCore.Libraries.Debug {
	/// <summary>
	/// Assorted static "helper" functions pertaining to log outputs.
	/// </summary>
	public partial class LogLibraries {
		/// <summary>
		/// Outputs a plain log message "once" (or rather, once every log10 % 1 == 0 times).
		/// </summary>
		/// <param name="msg"></param>
		/// <param name="color"></param>
		public static void LogAndPrintOnce( string msg, Color? color=null ) {
			if( LogLibraries.CanOutputOnceMessage( msg, true, out msg ) ) {
				LogLibraries.Log( "~" + msg );
				Main.NewText( "~" + msg, (color.HasValue ? color.Value : Color.White) );
			}
		}

		/// <summary>
		/// Outputs an "alert" log message "once" (or rather, once every log10 % 1 == 0 times).
		/// </summary>
		/// <param name="msg"></param>
		/// <param name="color"></param>
		public static void AlertAndPrintOnce( string msg = "", Color? color = null ) {
			ModLibsCoreMod mymod = ModLibsCoreMod.Instance;
			(string Context, string Info, string Full) fmtMsg = LogLibraries.FormatMessageFull( msg, 3 );

			string outMsg;
			LogLibraries.CanOutputOnceMessage( fmtMsg.Full, true, out outMsg );

			if( !LogLibraries.CanOutputOnceMessage( fmtMsg.Context + " " + msg, true, out _ ) ) {
				return;
			}

			mymod.Logger.Warn( "~" + outMsg ); //was Error(...)
			Main.NewText( "~" + fmtMsg.Context + " - " + msg, ( color.HasValue ? color.Value : Color.White ) );
		}

		/// <summary>
		/// Outputs a "warning" log message "once" (or rather, once every log10 % 1 == 0 times).
		/// </summary>
		/// <param name="msg"></param>
		/// <param name="color"></param>
		public static void WarnAndPrintOnce( string msg = "", Color? color = null ) {
			ModLibsCoreMod mymod = ModLibsCoreMod.Instance;
			(string Context, string Info, string Full) fmtMsg = LogLibraries.FormatMessageFull( msg, 3 );

			string outMsg;
			LogLibraries.CanOutputOnceMessage( fmtMsg.Full, true, out outMsg );

			if( !LogLibraries.CanOutputOnceMessage( fmtMsg.Context + " " + msg, true, out _ ) ) {
				return;
			}

			mymod.Logger.Error( "~" + outMsg );	//was Fatal(...)
			Main.NewText( "~!" + fmtMsg.Context + " - " + msg, ( color.HasValue ? color.Value : Color.White ) );
		}
	}
}
