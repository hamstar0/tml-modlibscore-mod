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
					out int repeats ) {
			var logLibs = ModContent.GetInstance<LogLibraries>();
			if( logLibs == null ) {
				repeats = -1;
				return false;
			}

			if( !logLibs.UniqueMessages.ContainsKey( msg ) ) {
				logLibs.UniqueMessages[msg] = 0;
			}

			repeats = logLibs.UniqueMessages[msg]++;

			return outputWhen.Invoke( repeats );
		}


		////////////////

		/// <summary>
		/// Outputs a plain log message when allowed.
		/// </summary>
		/// <param name="msg"></param>
		/// <param name="when">A side-effect-free function reporting if the message should output. Accepts a parameter
		/// of the number of times an output attempt (including failures and "once"s) has occurred.</param>
		/// <returns></returns>
		public static int LogWhen( string msg, Func<int, bool> when ) {
			if( LogLibraries.CanOutputMessageWhen(msg, when, out int repeats ) ) {
				LogLibraries.Log( "~~" + msg );
			}

			return repeats;
		}

		/// <summary>
		/// Outputs an "alert" log message when allowed.
		/// </summary>
		/// <param name="msg"></param>
		/// <param name="when">A side-effect-free function reporting if the message should output. Accepts a parameter
		/// of the number of times an output attempt (including failures and "once"s) has occurred.</param>
		/// <returns></returns>
		public static int AlertWhen( string msg, Func<int, bool> when ) {
			ModLibsCoreMod mymod = ModLibsCoreMod.Instance;
			(string Context, string Info, string Full) fmtMsg = LogLibraries.FormatMessageFull( msg, 3 );
			string msgWithCtx = fmtMsg.Context + " " + msg;

			if( LogLibraries.CanOutputMessageWhen(msgWithCtx, when, out int repeats) ) {
				mymod.Logger.Warn( "~~" + msg );    //was Error(...)
			}

			return repeats;
		}

		/// <summary>
		/// Outputs a "warning" log message when allowed.
		/// </summary>
		/// <param name="msg"></param>
		/// <param name="when">A side-effect-free function reporting if the message should output. Accepts a parameter
		/// of the number of times an output attempt (including failures and "once"s) has occurred.</param>
		/// <returns></returns>
		public static int WarnWhen( string msg, Func<int, bool> when ) {
			ModLibsCoreMod mymod = ModLibsCoreMod.Instance;
			(string Context, string Info, string Full) fmtMsg = LogLibraries.FormatMessageFull( msg, 3 );
			string msgWithCtx = fmtMsg.Context + " " + msg;

			if( LogLibraries.CanOutputMessageWhen(msgWithCtx, when, out int repeats) ) {
				mymod.Logger.Error( "~~" + msg ); //was Fatal(...)
			}

			return repeats;
		}
	}
}
