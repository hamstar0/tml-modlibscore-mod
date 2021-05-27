using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;


namespace ModLibsCore.Libraries.Debug {
	/// <summary>
	/// Assorted static "helper" functions pertaining to log outputs.
	/// </summary>
	public partial class LogLibraries {
		internal static bool CanOutputOnceMessage( string msg, out string formattedMsg ) {
			var logLibs = ModContent.GetInstance<LogLibraries>();
			if( logLibs == null ) {
				formattedMsg = msg;
				return false;
			}

			bool isShown = false;

			if( !logLibs.UniqueMessages.ContainsKey( msg ) ) {
				logLibs.UniqueMessages[msg] = 1;
				formattedMsg = msg;
				isShown = true;
			} else {
				logLibs.UniqueMessages[msg]++;

				if( ( Math.Log10( logLibs.UniqueMessages[msg] ) % 1d ) == 0 ) {
					formattedMsg = "(" + logLibs.UniqueMessages[msg] + "th) " + msg;
					isShown = true;
				} else {
					formattedMsg = msg;
				}
			}

			return isShown;
		}


		////

		/// <summary>
		/// Outputs a plain log message "once" (or rather, once every log10 % 1 == 0 times).
		/// </summary>
		/// <param name="msg"></param>
		public static void LogOnce( string msg ) {
			if( LogLibraries.CanOutputOnceMessage(msg, out msg) ) {
				LogLibraries.Log( "~" + msg );
			}
		}

		/// <summary>
		/// Outputs an "alert" log message "once" (or rather, once every log10 % 1 == 0 times).
		/// </summary>
		/// <param name="msg"></param>
		public static void AlertOnce( string msg = "" ) {
			ModLibsCoreMod mymod = ModLibsCoreMod.Instance;
			(string Context, string Info, string Full) fmtMsg = LogLibraries.FormatMessageFull( msg, 3 );

			string outMsg;
			LogLibraries.CanOutputOnceMessage( fmtMsg.Full, out outMsg );

			if( !LogLibraries.CanOutputOnceMessage( fmtMsg.Context+" "+msg, out _ ) ) {
				return;
			}

			mymod.Logger.Warn( "~" + outMsg );	//was Error(...)
		}

		/// <summary>
		/// Outputs a "warning" log message "once" (or rather, once every log10 % 1 == 0 times).
		/// </summary>
		/// <param name="msg"></param>
		public static void WarnOnce( string msg = "" ) {
			ModLibsCoreMod mymod = ModLibsCoreMod.Instance;
			(string Context, string Info, string Full) fmtMsg = LogLibraries.FormatMessageFull( msg, 3 );

			string outMsg;
			LogLibraries.CanOutputOnceMessage( fmtMsg.Full, out outMsg );

			if( !LogLibraries.CanOutputOnceMessage( fmtMsg.Context + " " + msg, out _ ) ) {
				return;
			}

			mymod.Logger.Error( "~" + outMsg );	//was Fatal(...)
		}

		////

		/// <summary>
		/// Outputs a plain log message "once" (or rather, once every log10 % 1 == 0 times).
		/// </summary>
		/// <param name="msg"></param>
		/// <param name="color"></param>
		public static void LogAndPrintOnce( string msg, Color? color=null ) {
			if( LogLibraries.CanOutputOnceMessage( msg, out msg ) ) {
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
			LogLibraries.CanOutputOnceMessage( fmtMsg.Full, out outMsg );

			if( !LogLibraries.CanOutputOnceMessage( fmtMsg.Context + " " + msg, out _ ) ) {
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
			LogLibraries.CanOutputOnceMessage( fmtMsg.Full, out outMsg );

			if( !LogLibraries.CanOutputOnceMessage( fmtMsg.Context + " " + msg, out _ ) ) {
				return;
			}

			mymod.Logger.Error( "~" + outMsg );	//was Fatal(...)
			Main.NewText( "~!" + fmtMsg.Context + " - " + msg, ( color.HasValue ? color.Value : Color.White ) );
		}


		////////////////

		/// <summary>
		/// Resets a given "once" log, alert, or warn messages.
		/// </summary>
		/// <param name="msg"></param>
		public static void ResetOnceMessage( string msg ) {
			string fmtMsg = LogLibraries.FormatMessage( msg, 3 );
			var logLibs = ModContent.GetInstance<LogLibraries>();

			logLibs.UniqueMessages.Remove( "~" + msg );
			logLibs.UniqueMessages.Remove( "~" + fmtMsg );
		}
	}
}
