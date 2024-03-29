﻿using System;
using System.IO;
using Terraria;
using Terraria.ModLoader;
using ModLibsCore.Classes.Errors;
using ModLibsCore.Libraries.Debug;


namespace ModLibsCore.Libraries.Misc {
	/// <summary>
	/// Assorted static "helper" functions pertaining to file IO for mod custom data.
	/// </summary>
	public partial class ModCustomDataFileLibraries {
		/// <summary>
		/// ModLoader subfolder where custom mod data is stored.
		/// </summary>
		public static string BaseFolder => "Mod Specific Data";



		////////////////

		/// <summary>
		/// Gets the custom data folder of a mod, relative to the ModLoader folder.
		/// </summary>
		/// <param name="mod"></param>
		/// <returns></returns>
		public static string GetRelativeDirectoryPath( Mod mod ) {
			return ModCustomDataFileLibraries.BaseFolder + Path.DirectorySeparatorChar + mod.Name;
		}

		/// <summary>
		/// Gets the full system directory of a mod's custom data folder.
		/// </summary>
		/// <param name="mod"></param>
		/// <returns></returns>
		public static string GetFullDirectoryPath( Mod mod ) {
			return Main.SavePath + Path.DirectorySeparatorChar + ModCustomDataFileLibraries.GetRelativeDirectoryPath(mod);
		}

		/// <summary>
		/// Gets the full system directory and path of a given custom data file of a mod.
		/// </summary>
		/// <param name="mod"></param>
		/// <param name="fileNameHasExt"></param>
		/// <returns></returns>
		public static string GetFullPath( Mod mod, string fileNameHasExt ) {
			return ModCustomDataFileLibraries.GetFullDirectoryPath( mod ) + Path.DirectorySeparatorChar + fileNameHasExt;
		}


		////////////////

		/// <summary>
		/// Creates a mod's data directory, if needed.
		/// </summary>
		/// <param name="mod"></param>
		public static void PrepareDir( Mod mod ) {
			string fullDir = ModCustomDataFileLibraries.GetFullDirectoryPath( mod );
			
			try {
				Directory.CreateDirectory( Main.SavePath );
				Directory.CreateDirectory( Main.SavePath + Path.DirectorySeparatorChar + ModCustomDataFileLibraries.BaseFolder );
				Directory.CreateDirectory( fullDir );
			} catch( IOException e ) {
				LogLibraries.Warn( "Failed to prepare directory: " + fullDir+" - "+e.ToString() );
				throw new IOException( "Failed to prepare directory: " + fullDir, e );
			}
		}
	}
}
