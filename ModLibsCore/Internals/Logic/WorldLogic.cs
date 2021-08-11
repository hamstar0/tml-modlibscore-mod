using ModLibsCore.Classes.Loadable;
using ModLibsCore.Services.Hooks.LoadHooks;


namespace ModLibsCore.Internals.Logic {
	/// @private
	partial class WorldLogic : ILoadable {
		public static bool IsLoaded { get; private set; } = false;



		////////////////

		void ILoadable.OnModsLoad() { }

		void ILoadable.OnPostModsLoad() {
			void onLoadWorld( On.Terraria.IO.WorldFile.orig_loadWorld orig, bool loadFromCloud ) {
				orig.Invoke( loadFromCloud );

				WorldLogic.IsLoaded = true;
			}

			On.Terraria.IO.WorldFile.loadWorld += onLoadWorld;

			LoadHooks.AddWorldUnloadEachHook( () => WorldLogic.IsLoaded = false );
		}

		void ILoadable.OnModsUnload() { }
	}
}
