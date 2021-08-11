using System;
using Terraria;
using Terraria.ModLoader;


namespace ModLibsCore.Libraries.Debug {
	/// <summary>
	/// Assorted static "helper" functions pertaining to log outputs.
	/// </summary>
	public partial class LogLibraries {
		private static bool CanOutputOnceMessage( string msg, bool repeatLog10, out int repeats ) {
			Func<int, bool> outputWhen;
			if( repeatLog10 ) {
				outputWhen = (times) => (Math.Log10(times) % 1d) == 0;
			} else {
				outputWhen = (times) => times == 0;
			}

			return LogLibraries.CanOutputMessageWhen( msg, outputWhen, out repeats );
		}


		////////////////

		/// <summary>
		/// Outputs a plain log message "once" (or rather, once every log10 % 1 == 0 times).
		/// </summary>
		/// <param name="msg"></param>
		public static void LogOnce( string msg ) {
			LogLibraries.LogOnce( msg, true );
		}

		/// <summary>
		/// Outputs an "alert" log message "once" (or rather, once every log10 % 1 == 0 times).
		/// </summary>
		/// <param name="msg"></param>
		public static void AlertOnce( string msg = "" ) {
			LogLibraries.AlertOnce( msg, true );
		}

		/// <summary>
		/// Outputs a "warning" log message "once" (or rather, once every log10 % 1 == 0 times).
		/// </summary>
		/// <param name="msg"></param>
		public static void WarnOnce( string msg = "" ) {
			LogLibraries.WarnOnce( msg, true );
		}

		////

		/// <summary>
		/// Outputs a plain log message "once".
		/// </summary>
		/// <param name="msg"></param>
		/// <param name="repeatLog10">Outputs once every log10 % 1 == 0 times.</param>
		/// <returns>Output message, or else `null` if message has already output (and a repeat isn't
		/// occurring).</returns>
		public static string LogOnce( string msg, bool repeatLog10=true ) {
			string outMsg = null;

			if( LogLibraries.CanOutputOnceMessage(msg, repeatLog10, out int repeats) ) {
				outMsg = msg;
				if( repeats >= 1 ) {
					outMsg = "("+repeats+"th) " + outMsg;
				}
				outMsg = "~"+msg;

				LogLibraries.Log( outMsg );
			}

			return outMsg;
		}

		/// <summary>
		/// Outputs an "alert" log message "once".
		/// </summary>
		/// <param name="msg"></param>
		/// <param name="repeatLog10">Outputs once every log10 % 1 == 0 times.</param>
		/// <returns>Output message, or else `null` if message has already output (and a repeat isn't
		/// occurring).</returns>
		public static string AlertOnce( string msg, bool repeatLog10=true ) {
			string outMsg = LogLibraries.RenderOnce( msg, repeatLog10 );
			if( outMsg != null ) {
				ModLibsCoreMod.Instance.Logger.Warn( outMsg );   //was Fatal(...)
			}

			return outMsg;
		}

		/// <summary>
		/// Outputs a "warning" log message "once".
		/// </summary>
		/// <param name="msg"></param>
		/// <param name="repeatLog10">Outputs once every log10 % 1 == 0 times.</param>
		/// <returns>Output message, or else `null` if message has already output (and a repeat isn't
		/// occurring).</returns>
		public static string WarnOnce( string msg, bool repeatLog10=true ) {
			string outMsg = LogLibraries.RenderOnce( msg, repeatLog10 );
			if( outMsg != null ) {
				ModLibsCoreMod.Instance.Logger.Error( outMsg );   //was Fatal(...)
			}

			return outMsg;
		}

		////

		private static string RenderOnce( string msg, bool repeatLog10=true ) {
			(string Context, string Info, string Full) logMsgData = LogLibraries.FormatMessageFull( msg, 3 );

			LogLibraries.CanOutputOnceMessage( logMsgData.Full, repeatLog10, out int repeats );

			string outMsg = null;

			if( LogLibraries.CanOutputOnceMessage(logMsgData.Context + " " + msg, true, out _) ) {
				outMsg = msg;
				if( repeats >= 1 ) {
					outMsg = "("+repeats+"th) " + outMsg;
				}
				outMsg = "~" + outMsg;
			}

			return outMsg;
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
