using System;
using System.Collections.Generic;


namespace ModLibsCore.Classes.Loadable {
	/// <summary>
	/// Affixed to classes that wish to automatically run functios on mod load, post load, and unload.
	/// </summary>
	public interface ILoadable {
		/// <summary>
		/// Called during `Mod.Load()`.
		/// </summary>
		void OnModsLoad();
		/// <summary>
		/// Called after `Mod.PostSetupContent()`, `Mod.AddRecipeGroups()`, and `Mod.PostAddRecipes()`.
		/// </summary>
		void OnPostModsLoad();
		/// <summary>
		/// Called during `Mod.Unload()`.
		/// </summary>
		void OnModsUnload();
	}




	/// <summary>
	/// Affixed to classes that wish to automatically run functios on mod load, post load, and unload.
	/// </summary>
	public interface ISequencedLoadable {
		/// <summary>
		/// Called during `Mod.Load()`.
		/// </summary>
		/// <param name="hasLoaded"></param>
		/// <returns>`true` when loading complete.</returns>
		bool OnModsLoad( ISet<object> hasLoaded );
		/// <summary>
		/// Called after `Mod.PostSetupContent()`, `Mod.AddRecipeGroups()`, and `Mod.PostAddRecipes()`.
		/// </summary>
		/// <param name="hasPostLoaded"></param>
		/// <returns>`true` when (post) loading complete.</returns>
		bool OnPostModsLoad( ISet<object> hasPostLoaded );
		/// <summary>
		/// Called during `Mod.Unload()`.
		/// </summary>
		/// <param name="hasUnloaded"></param>
		/// <returns>`true` when unloading complete.</returns>
		bool OnModsUnload( ISet<object> hasUnloaded );
	}
}
