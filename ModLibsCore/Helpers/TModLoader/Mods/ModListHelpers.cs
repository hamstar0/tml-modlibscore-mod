using System;
using System.Collections.Generic;
using Terraria.ModLoader;
using ModLibsCore.Classes.Errors;
using ModLibsCore.Helpers.Debug;
using ModLibsCore.Services.TML;


namespace ModLibsCore.Helpers.TModLoader.Mods {
	/// <summary>
	/// Assorted static "helper" functions pertaining to mod list building.
	/// </summary>
	public partial class ModListHelpers {
		/// <summary>
		/// Gets a map of loaded mods with their build information.
		/// </summary>
		/// <returns></returns>
		public static IDictionary<BuildPropertiesViewer, Mod> GetLoadedModsAndBuildInfo() {
			var mlHelpers = ModContent.GetInstance<ModListHelpers>();

			if( mlHelpers.ModsByBuildProps != null ) {
				return mlHelpers.ModsByBuildProps;
			}

			mlHelpers.ModsByBuildProps = mlHelpers.GetModsByBuildProps();
			return mlHelpers.ModsByBuildProps;
		}

		////////////////

		/// <summary>
		/// Gets a map of all loaded mods by name with their build information.
		/// </summary>
		/// <returns></returns>
		public static IDictionary<string, BuildPropertiesViewer> GetLoadedModNamesWithBuildProps() {
			var mlHelpers = ModContent.GetInstance<ModListHelpers>();
			if( mlHelpers.BuildPropsByModNames != null ) {
				return mlHelpers.BuildPropsByModNames;
			}

			mlHelpers.BuildPropsByModNames = mlHelpers.GetBuildPropsByModName();
			return mlHelpers.BuildPropsByModNames;
		}

		/// <summary>
		/// Gets a map of loaded mods with their authors.
		/// </summary>
		/// <returns></returns>
		public static IDictionary<string, ISet<Mod>> GetLoadedModsByAuthor() {
			var mlHelpers = ModContent.GetInstance<ModListHelpers>();
			if( mlHelpers.ModsByBuildProps != null ) {
				return mlHelpers.ModsByAuthor;
			}

			mlHelpers.ModsByAuthor = mlHelpers.GetModsByAuthor();
			return mlHelpers.ModsByAuthor;
		}
	}
}
