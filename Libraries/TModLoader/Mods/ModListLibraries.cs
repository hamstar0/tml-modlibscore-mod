using System;
using System.Collections.Generic;
using Terraria.ModLoader;
using ModLibsCore.Classes.Errors;
using ModLibsCore.Libraries.Debug;
using ModLibsCore.Services.TML;


namespace ModLibsCore.Libraries.TModLoader.Mods {
	/// <summary>
	/// Assorted static "helper" functions pertaining to mod list building.
	/// </summary>
	public partial class ModListLibraries {
		/// <summary>
		/// Gets a map of loaded mods with their build information.
		/// </summary>
		/// <returns></returns>
		public static IDictionary<BuildPropertiesViewer, Mod> GetLoadedModsAndBuildInfo() {
			var mlLibs = ModContent.GetInstance<ModListLibraries>();

			if( mlLibs.ModsByBuildProps != null ) {
				return mlLibs.ModsByBuildProps;
			}

			mlLibs.ModsByBuildProps = mlLibs.GetModsByBuildProps();
			return mlLibs.ModsByBuildProps;
		}

		////////////////

		/// <summary>
		/// Gets a map of all loaded mods by name with their build information.
		/// </summary>
		/// <returns></returns>
		public static IDictionary<string, BuildPropertiesViewer> GetLoadedModNamesWithBuildProps() {
			var mlLibs = ModContent.GetInstance<ModListLibraries>();
			if( mlLibs.BuildPropsByModNames != null ) {
				return mlLibs.BuildPropsByModNames;
			}

			mlLibs.BuildPropsByModNames = mlLibs.GetBuildPropsByModName();
			return mlLibs.BuildPropsByModNames;
		}

		/// <summary>
		/// Gets a map of loaded mods with their authors.
		/// </summary>
		/// <returns></returns>
		public static IDictionary<string, ISet<Mod>> GetLoadedModsByAuthor() {
			var mlLibs = ModContent.GetInstance<ModListLibraries>();
			if( mlLibs.ModsByBuildProps != null ) {
				return mlLibs.ModsByAuthor;
			}

			mlLibs.ModsByAuthor = mlLibs.GetModsByAuthor();
			return mlLibs.ModsByAuthor;
		}
	}
}
