﻿using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;


namespace ModLibsCore.Libraries.Debug {
	/// <summary>
	/// Assorted static "helper" functions pertaining to log outputs.
	/// </summary>
	public partial class LogLibraries {
		/// <summary>
		/// Formats a given message as it would appear in the log output.
		/// </summary>
		/// <param name="msg"></param>
		/// <param name="contextDepth">Indicates whether to also output the callstack context at the specified depth.
		/// No output if -1 is set.</param>
		/// <returns></returns>
		public static string FormatMessage( string msg, int contextDepth = -1 ) {
			return LogLibraries.FormatMessageFull( msg, contextDepth ).Full;
		}

		/// <summary>
		/// Formats a given message for conveying its meta data for the log output.
		/// </summary>
		/// <param name="msg"></param>
		/// <param name="contextDepth">Indicates whether to also output the callstack context at the specified depth.
		/// No output if -1 is set.</param>
		/// <returns>Calling context, global info, and full (chained together) output.</returns>
		public static (string Context, string Info, string Full) FormatMessageFull(
					string msg,
					int contextDepth = -1 ) {
			string context, info;
			ModLibsCoreMod mymod = ModLibsCoreMod.Instance;

			if( mymod == null ) {
				contextDepth = contextDepth == -1 ? 2 : contextDepth;
				context = DebugLibraries.GetCurrentContext( contextDepth );

				return ( context, "", "!Mod Libs unloaded. Message called from: "+context );
			}

			//

			var logLibs = ModContent.GetInstance<LogLibraries>();
			string output;
			double nowSeconds;

			// Prepare time stamp
			try {
				var beginning = new DateTime( 1970, 1, 1, 0, 0, 0 );
				TimeSpan nowTotalSpan = DateTime.UtcNow.Subtract( beginning );
				nowSeconds = nowTotalSpan.TotalSeconds - logLibs.StartTime;

				output = "";
			} catch( Exception e ) {
				nowSeconds = 0;
				output = "FORMATTING ERROR 1 (" + e.GetType().Name + ") - " + msg;
			}
			
			// Generate global info output
			try {
				string nowSecondsWhole = ( (int)nowSeconds ).ToString( "D6" );
				string nowSecondsDecimal = ( nowSeconds - (int)nowSeconds ).ToString( "N2" );
				string now = nowSecondsWhole + "." + ( nowSecondsDecimal.Length > 2 ? nowSecondsDecimal.Substring( 2 ) : nowSecondsDecimal );
				
				string from = Main.myPlayer.ToString( "D3" );
				info = Main.netMode + ":" + from + " - " + now;
				if( info.Length < 26 ) {
					info += new String( ' ', 26 - info.Length );
				} else {
					info += "  ";
				}

				//

				output += info + msg;
			} catch( Exception e ) {
				info = "";
				output += "FORMATTING ERROR 2 (" + e.GetType().Name + ") - " + msg;
			}

			//

			// Generate calling context output
			if( contextDepth >= 0 ) {
				try {
					context = DebugLibraries.GetCurrentContext( contextDepth );
				} catch {
					context = "";
				}

				//

				if( output.Length > 0 ) {
					output = context + " - " + output;
				} else {
					output = context;
				}
			} else {
				context = "";
			}

			//

			return ( context, info, output );
		}


		////////////////

		/// <summary>
		/// Outputs a plain log message without added fluff (wraps `Mod.Logger.Info(...)`).
		/// </summary>
		/// <param name="msg"></param>
		public static void Info( string msg = "" ) {
			ModLibsCoreMod.Instance.Logger.Info( msg );
		}

		/// <summary>
		/// Outputs a plain log message without added fluff (wraps `Mod.Logger.Info(...)`).
		/// </summary>
		/// <param name="mod"></param>
		/// <param name="msg"></param>
		public static void Info( Mod mod, string msg = "" ) {
			mod.Logger.Info( msg );
		}

		/// <summary>
		/// Outputs a plain log message.
		/// </summary>
		/// <param name="msg"></param>
		public static void Log( string msg = "" ) {
			try {
				ModLibsCoreMod.Instance.Logger.Info( LogLibraries.FormatMessage( msg ) );
			} catch { }
		}

		/// <summary>
		/// Outputs an "alert" log message (TML considers it an error-type message).
		/// </summary>
		/// <param name="msg"></param>
		public static void Alert( string msg = "" ) {
			try {
				string fmtMsg = LogLibraries.FormatMessage( msg, 4 );

				ModLibsCoreMod.Instance.Logger.Warn( fmtMsg );   //was Error(...)
			} catch { }
			//LogLibraries.Log( DebugLibraries.GetCurrentContext( 2 ) + ((msg != "") ? " - " + msg : "") );
		}

		/// <summary>
		/// Outputs a warning log message (TML considers it a fatal-type message).
		/// </summary>
		/// <param name="msg"></param>
		public static void Warn( string msg = "" ) {
			try {
				string fmtMsg = LogLibraries.FormatMessage( msg, 4 );

				ModLibsCoreMod.Instance.Logger.Error( fmtMsg );	//was Fatal(...)
			} catch { }
			//LogLibraries.Log( DebugLibraries.GetCurrentContext( 2 ) + ((msg != "") ? " - " + msg: "") );
		}


		////

		/// <summary>
		/// Outputs a log message indicating the current context.
		/// </summary>
		/// <param name="msg"></param>
		/// <param name="omitNamespace"></param>
		/// <param name="max"></param>
		/// <param name="separator"></param>
		/// <param name="spacer"></param>
		public static void LogContext( string msg, bool omitNamespace=true, int max=-1, string separator="\n", string spacer="  " ) {
			IList<string> contextSlice = DebugLibraries.GetContextSlice( 3, omitNamespace, max );
			string context = string.Join( separator+spacer, contextSlice );

			LogLibraries.Log( msg + " at " + context );
		}
	}
}
