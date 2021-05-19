using ModLibsCore.Services.Hooks.LoadHooks;


namespace ModLibsCore.Internals.Logic {
	/// @private
	partial class WorldLogic {
		public static bool IsLoaded { get; private set; } = false;



		////////////////

		public static void OnWorldLoad() {
			WorldLogic.IsLoaded = true;

			LoadHooks.AddWorldUnloadEachHook( () => WorldLogic.IsLoaded = false );
		}



		////////////////

		public WorldLogic() { }
	}
}
