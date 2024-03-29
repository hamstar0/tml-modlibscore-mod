﻿using System;
using Terraria;
using Terraria.ModLoader;


namespace ModLibsCore.Libraries.Debug {
	/// <summary>
	/// Assorted static "helper" functions pertaining to log outputs.
	/// </summary>
	public partial class LogLibraries {
		private static bool CanOutputOnceMessage(
					string msg,
					bool repeatLog10,
					bool incrementOutputCount,
					out int repeats ) {
			Func<int, bool> outputWhen;
			if( repeatLog10 ) {
				outputWhen = (times) => times == 0 || (Math.Log10(times) % 1d) == 0d;
			} else {
				outputWhen = (times) => times == 0;
			}

			return LogLibraries.CanOutputMessageWhen( msg, outputWhen, incrementOutputCount, out repeats );
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
			string outMsg = LogLibraries.RenderOnce( msg, true, repeatLog10 );
			if( outMsg != null ) {
				ModLibsCoreMod.Instance.Logger.Info( "~"+outMsg );
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
			string outMsg = LogLibraries.RenderOnce( msg, true, repeatLog10 );
			if( outMsg != null ) {
				ModLibsCoreMod.Instance.Logger.Warn( "~"+outMsg );   //was Fatal(...)
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
			string outMsg = LogLibraries.RenderOnce( msg, true, repeatLog10 );
			if( outMsg != null ) {
				ModLibsCoreMod.Instance.Logger.Error( "~"+outMsg );   //was Fatal(...)
			}

			return outMsg;
		}

		////

		private static string RenderOnce( string msg, bool outputContext, bool repeatLog10 ) {
			string outMsg = null;
			(string msgContext, string _, string _) = LogLibraries.FormatMessageFull( msg, 4 );
			string internalMsg = msgContext + " " + msg;

			bool canOutput = LogLibraries.CanOutputOnceMessage( internalMsg, repeatLog10, true, out int repeats );

			// Render formatted message
			if( canOutput ) {
				outMsg = msg;
				if( repeats > 1 ) {
					outMsg = "("+repeats+"th) " + outMsg;
				}
				if( outputContext ) {
					outMsg = msgContext + " - " + outMsg;
				}
			}
			
//ModLibsCoreMod.Instance.Logger.Info( "RenderOnce ("+repeats+") src: "+msg+", out: "+outMsg );
			return outMsg;
		}


		////////////////

		/// <summary>
		/// Resets a given "once" log, alert, or warn message.
		/// </summary>
		/// <param name="msg"></param>
		public static void ResetOnceMessage( string msg ) {
			string fmtMsg = LogLibraries.FormatMessage( msg, 4 );
			var logLibs = ModContent.GetInstance<LogLibraries>();

			logLibs.UniqueMessages.Remove( "~" + msg );
			logLibs.UniqueMessages.Remove( "~" + fmtMsg );
		}
	}
}
