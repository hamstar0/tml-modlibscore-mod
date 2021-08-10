using System;
using Terraria;
using Terraria.ModLoader;


namespace ModLibsCore.Libraries.Debug {
	/// <summary>
	/// Assorted static "helper" functions pertaining to log outputs.
	/// </summary>
	public partial class LogLibraries {
		private static bool CanOutputMessageWhen(
					string msg,
					Func<int, bool> outputWhen,
					out string formattedMsg ) {
			var logLibs = ModContent.GetInstance<LogLibraries>();
			if( logLibs == null ) {
				formattedMsg = msg;
				return false;
			}

			bool isShown = false;

			if( !logLibs.UniqueMessages.ContainsKey( msg ) ) {
				logLibs.UniqueMessages[msg] = 1;
				formattedMsg = msg;

				isShown = outputWhen.Invoke(0);
			} else {
				int times = logLibs.UniqueMessages[ msg ]++;

				if( outputWhen.Invoke(times) ) {
					formattedMsg = "("+times+"th) " + msg;
					isShown = true;
				} else {
					formattedMsg = msg;
				}
			}

			return isShown;
		}


		////////////////

		/// <summary>
		/// Outputs a plain log message when allowed.
		/// </summary>
		/// <param name="msg"></param>
		/// <param name="when">A side-effect-free function reporting if the message should output. Accepts a parameter
		/// of the number of times an output attempt (including failures and "once"s) has occurred.</param>
		public static void LogWhen( string msg, Func<int, bool> when ) {
			if( LogLibraries.CanOutputMessageWhen(msg, when, out msg) ) {
				LogLibraries.Log( "~" + msg );
			}
		}

		/// <summary>
		/// Outputs an "alert" log message when allowed.
		/// </summary>
		/// <param name="msg"></param>
		/// <param name="when">A side-effect-free function reporting if the message should output. Accepts a parameter
		/// of the number of times an output attempt (including failures and "once"s) has occurred.</param>
		public static void AlertWhen( string msg, Func<int, bool> when ) {
			ModLibsCoreMod mymod = ModLibsCoreMod.Instance;
			(string Context, string Info, string Full) fmtMsg = LogLibraries.FormatMessageFull( msg, 3 );

			string outMsg;
			LogLibraries.CanOutputMessageWhen( fmtMsg.Full, when, out outMsg );
			
			string msgWithCtx = fmtMsg.Context + " " + msg;
			if( !LogLibraries.CanOutputMessageWhen(msgWithCtx, when, out _) ) {
				return;
			}

			mymod.Logger.Warn( "~" + outMsg );	//was Error(...)
		}

		/// <summary>
		/// Outputs a "warning" log message when allowed.
		/// </summary>
		/// <param name="msg"></param>
		/// <param name="when">A side-effect-free function reporting if the message should output. Accepts a parameter
		/// of the number of times an output attempt (including failures and "once"s) has occurred.</param>
		public static void WarnWhen( string msg, Func<int, bool> when ) {
			ModLibsCoreMod mymod = ModLibsCoreMod.Instance;
			(string Context, string Info, string Full) fmtMsg = LogLibraries.FormatMessageFull( msg, 3 );

			string outMsg;
			LogLibraries.CanOutputMessageWhen( fmtMsg.Full, when, out outMsg );

			string msgWithCtx = fmtMsg.Context + " " + msg;
			if( !LogLibraries.CanOutputMessageWhen(msgWithCtx, when, out _) ) {
				return;
			}

			mymod.Logger.Error( "~" + outMsg );	//was Fatal(...)
		}
	}
}
