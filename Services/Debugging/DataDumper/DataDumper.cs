﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Terraria;
using Terraria.ID;
using ModLibsCore.Libraries.Debug;
using ModLibsCore.Libraries.DotNET;
using ModLibsCore.Internals.Packets;


namespace ModLibsCore.Services.Debug.DataDumper {
	/// <summary>
	/// Provides a service for supplying data sources that can be pulled from and collected into an on-demand dump as a file.
	/// Uses a dedicated folder at "ModLoader/Logs/Dumps". Primarily used for debugging.
	/// </summary>
	public static class DataDumper {
		internal static object MyDataStorekey = new object();
		private static int Dumps = 0;



		////////////////

		/// <summary></summary>
		/// <param name="data"></param>
		/// <param name="fileNameWithExtension"></param>
		/// <returns></returns>
		public static bool DumpToLocalFile( string data, out string fileNameWithExtension ) {
			fileNameWithExtension = DataDumper.GetFileName( (DataDumper.Dumps++) + "" );
			string relPath = DataDumper.GetRelativePath();
			string fullFolder = Main.SavePath + Path.DirectorySeparatorChar + relPath;
			string fullPath = fullFolder + Path.DirectorySeparatorChar + fileNameWithExtension;

			DataDumper.PrepareDir();
			return FileLibraries.SaveTextFile( data, fullPath, false, false );
		}

		////////////////

		internal static IDictionary<string, Func<string>> GetDumpables() {
			IDictionary<string, Func<string>> dumpables;
			bool success = DataStore.DataStore.Get( DataDumper.MyDataStorekey, out dumpables );

			if( !success ) {
				dumpables = new Dictionary<string, Func<string>>();
				DataStore.DataStore.Set( DataDumper.MyDataStorekey, dumpables );
			}

			return dumpables;
		}


		////////////////

		internal static string GetFileName( string prefix ) {
			string netmode;
			switch( Main.netMode ) {
			case 0:
				netmode = "single";
				break;
			case 1:
				netmode = "client("+Main.LocalPlayer.name+"_"+Main.myPlayer+")";
				break;
			case 2:
				netmode = "server";
				break;
			default:
				netmode = "!";
				break;
			}

			double nowSeconds = DateTime.UtcNow.Subtract( new DateTime( 1970, 1, 1, 0, 0, 0 ) ).TotalSeconds;

			string nowSecondsWhole = ( (int)nowSeconds ).ToString( "D6" );
			string nowSecondsDecimal = ( nowSeconds - (int)nowSeconds ).ToString( "N2" );
			string now = nowSecondsWhole + "." + ( nowSecondsDecimal.Length > 2 ? nowSecondsDecimal.Substring( 2 ) : nowSecondsDecimal );

			return prefix + "_"+netmode +"_"+now + "_dump.txt";
		}

		/// <summary>
		/// Folder path of dump files relative to ModLoader: Logs/Dumps 
		/// </summary>
		/// <returns></returns>
		public static string GetRelativePath() {
			return "Logs" + Path.DirectorySeparatorChar + "Dumps";
		}


		////////////////

		private static void PrepareDir() {
			string fullDir = Main.SavePath + Path.DirectorySeparatorChar + DataDumper.GetRelativePath();

			try {
				Directory.CreateDirectory( Main.SavePath );
				Directory.CreateDirectory( Main.SavePath + Path.DirectorySeparatorChar + "Logs" );
				Directory.CreateDirectory( fullDir );
			} catch( IOException e ) {
				throw new IOException( "Failed to prepare directory: " + fullDir, e );
			}
		}


		////////////////

		/// <summary>
		/// Sets a callback as a data source to output within the dump file with the given label (`name`).
		/// </summary>
		/// <param name="name"></param>
		/// <param name="dump"></param>
		public static void SetDumpSource( string name, Func<string> dump ) {
			var dumpables = DataDumper.GetDumpables();

			dumpables[ name ] = dump;
		}


		////////////////

		/// <summary>
		/// Dumps the outputs to a dump file.
		/// </summary>
		/// <param name="fileName"></param>
		/// <param name="syncIf">Tells the server to dump, also (if from a client).</param>
		/// <returns></returns>
		public static bool DumpToFile( out string fileName, bool syncIf ) {
			IDictionary<string, Func<string>> dumpables = DataDumper.GetDumpables();

			Func<KeyValuePair<string, Func<string>>, string> getKey = kv => {
				try { return kv.Value(); }
				catch( Exception e ) { return "ERROR: "+e.Message; }
			};

			string data = string.Join( "\r\n", dumpables
				.ToDictionary( kv => kv.Key, getKey )
				.SafeSelect( kv=>kv.Key+":\r\n"+kv.Value )
			);
			
			bool success = DataDumper.DumpToLocalFile( data, out fileName );

			if( success && syncIf ) {
				// Allow admins to dump on behalf of server, also
				if( Main.netMode == NetmodeID.MultiplayerClient ) {
					DataDumpRequestPacket.QuickRequestIf();
				}
			}

			return success;
		}
	}
}
