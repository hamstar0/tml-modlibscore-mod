using System;
using Terraria.ModLoader;
using ModLibsCore.Libraries.Debug;


namespace ModLibsCore.Services.Hooks.LoadHooks {
	public partial class LoadHooks : ModSystem {
		/// <summary>
		/// Declares an action to run after mods are loaded (PostSetupContent, PostAddRecipes, AddRecipeGroups).
		/// </summary>
		/// <param name="action"></param>
		public static void AddPostContentLoadHook( Action action ) {
			var loadHooks = ModContent.GetInstance<LoadHooks>();

			if( loadHooks.PostContentLoadHookConditionsMet ) {
				action();
			} else {
				lock( LoadHooks.PostContentLoadHookLock ) {
					loadHooks.PostContentLoadHooks.Add( action );
				}
			}
		}
		
		/// <summary>
		/// Declares an action to run after mods are loaded (PostSetupContent, PostAddRecipes, AddRecipeGroups).
		/// </summary>
		/// <param name="action"></param>
		public static void AddPostModLoadHook( Action action ) {
			var loadHooks = ModContent.GetInstance<LoadHooks>();

			if( loadHooks.PostModLoadHookConditionsMet ) {
				action();
			} else {
				lock( LoadHooks.PostModLoadHookLock ) {
					loadHooks.PostModLoadHooks.Add( action );
				}
			}
		}
		
		/// <summary>
		/// Declares an action to run as mods are unloading.
		/// </summary>
		/// <param name="action"></param>
		public static void AddModUnloadHook( Action action ) {
			var loadHooks = ModContent.GetInstance<LoadHooks>();

			lock( LoadHooks.ModUnloadHookLock ) {
				loadHooks.ModUnloadHooks.Add( action );
			}
		}


		////////////////
		
		/// <summary>
		/// Declares an action to run as the current world loads. Action does not run for subsequent world loads.
		/// </summary>
		/// <param name="action"></param>
		public static void AddWorldLoadOnceHook( Action action ) {
			var loadHooks = ModContent.GetInstance<LoadHooks>();

			if( loadHooks.WorldLoadHookConditionsMet ) {
				action();
			} else {
				lock( LoadHooks.WorldLoadOnceHookLock ) {
					loadHooks.WorldLoadOnceHooks.Add( action );
				}
			}
		}
		
		/// <summary>
		/// Declares an action to run after the current world loads. Action does not run for subsequent world loads.
		/// </summary>
		/// <param name="action"></param>
		public static void AddPostWorldLoadOnceHook( Action action ) {
			var loadHooks = ModContent.GetInstance<LoadHooks>();

			if( loadHooks.WorldLoadHookConditionsMet ) {
				action();
			} else {
				lock( LoadHooks.PostWorldLoadOnceHookLock ) {
					loadHooks.PostWorldLoadOnceHooks.Add( action );
				}
			}
		}

		/// <summary>
		/// Declares an action to run as the current world unloads. Action does not run for subsequent world unloads.
		/// </summary>
		/// <param name="action"></param>
		public static void AddWorldUnloadOnceHook( Action action ) {
			var loadHooks = ModContent.GetInstance<LoadHooks>();

			if( loadHooks.WorldUnloadHookConditionsMet ) {
				action();
			} else {
				lock( LoadHooks.WorldUnloadOnceHookLock ) {
					loadHooks.WorldUnloadOnceHooks.Add( action );
				}
			}
		}
		
		/// <summary>
		/// Declares an action to run after the current world unloads. Action does not run for subsequent world unloads.
		/// </summary>
		/// <param name="action"></param>
		public static void AddPostWorldUnloadOnceHook( Action action ) {
			var loadHooks = ModContent.GetInstance<LoadHooks>();

			if( loadHooks.PostWorldUnloadHookConditionsMet ) {
				action();
			} else {
				lock( LoadHooks.PostWorldUnloadOnceHookLock ) {
					loadHooks.PostWorldUnloadOnceHooks.Add( action );
				}
			}
		}
		
		/// <summary>
		/// Declares an action to run once the current world is in play. Action does not run for subsequent worlds.
		/// </summary>
		/// <param name="action"></param>
		public static void AddWorldInPlayOnceHook( Action action ) {
			var loadHooks = ModContent.GetInstance<LoadHooks>();

			if( loadHooks.WorldInPlayHookConditionsMet ) {
				action();
			} else {
				lock( LoadHooks.WorldInPlayOnceHookLock ) {
					loadHooks.WorldInPlayOnceHooks.Add( action );
				}
			}
		}
		
		/// <summary>
		/// Declares an action to run after the current world is "safely" loaded (waits a few seconds to help avoid confusing
		/// errors). Action does not run for subsequent worlds.
		/// </summary>
		/// <param name="action"></param>
		public static void AddSafeWorldLoadOnceHook( Action action ) {
			var loadHooks = ModContent.GetInstance<LoadHooks>();

			if( loadHooks.SafeWorldLoadHookConditionsMet ) {
				action();
			} else {
				lock( LoadHooks.SafeWorldLoadOnceHookLock ) {
					loadHooks.SafeWorldLoadOnceHooks.Add( action );
				}
			}
		}


		////////////////
		
		/// <summary>
		/// Declares an action to run as the current world loads.
		/// </summary>
		/// <param name="action"></param>
		public static void AddWorldLoadEachHook( Action action ) {
			var loadHooks = ModContent.GetInstance<LoadHooks>();

			if( loadHooks.WorldLoadHookConditionsMet ) {
				action();
			}
			lock( LoadHooks.WorldLoadEachHookLock ) {
				loadHooks.WorldLoadEachHooks.Add( action );
			}
		}

		/// <summary>
		/// Declares an action to run after the current world loads.
		/// </summary>
		/// <param name="action"></param>
		public static void AddPostWorldLoadEachHook( Action action ) {
			var loadHooks = ModContent.GetInstance<LoadHooks>();

			if( loadHooks.WorldLoadHookConditionsMet ) {
				action();
			}
			lock( LoadHooks.PostWorldLoadEachHookLock ) {
				loadHooks.PostWorldLoadEachHooks.Add( action );
			}
		}
		
		/// <summary>
		/// Declares an action to run as the current world unloads.
		/// </summary>
		/// <param name="action"></param>
		public static void AddWorldUnloadEachHook( Action action ) {
			var loadHooks = ModContent.GetInstance<LoadHooks>();

			if( loadHooks.WorldUnloadHookConditionsMet ) {
				action();
			}

			lock( LoadHooks.WorldUnloadEachHookLock ) {
				loadHooks.WorldUnloadEachHooks.Add( action );
			}
		}
		
		/// <summary>
		/// Declares an action to run after the current world unloads.
		/// </summary>
		/// <param name="action"></param>
		public static void AddPostWorldUnloadEachHook( Action action ) {
			var loadHooks = ModContent.GetInstance<LoadHooks>();

			if( loadHooks.PostWorldUnloadHookConditionsMet ) {
				action();
			}
			lock( LoadHooks.PostWorldUnloadEachHookLock ) {
				loadHooks.PostWorldUnloadEachHooks.Add( action );
			}
		}
		
		/// <summary>
		/// Declares an action to run once the current world is in play.
		/// </summary>
		/// <param name="action"></param>
		public static void AddWorldInPlayEachHook( Action action ) {
			var loadHooks = ModContent.GetInstance<LoadHooks>();

			if( loadHooks.WorldInPlayHookConditionsMet ) {
				action();
			}
			lock( LoadHooks.WorldInPlayEachHookLock ) {
				loadHooks.WorldInPlayEachHooks.Add( action );
			}
		}
		
		/// <summary>
		/// Declares an action to run after the current world is "safely" loaded (waits a few seconds to help avoid confusing
		/// errors).
		/// </summary>
		/// <param name="action"></param>
		public static void AddSafeWorldLoadEachHook( Action action ) {
			var loadHooks = ModContent.GetInstance<LoadHooks>();

			if( loadHooks.SafeWorldLoadHookConditionsMet ) {
				action();
			}
			lock( LoadHooks.SafeWorldLoadEachHookLock ) {
				loadHooks.SafeWorldLoadEachHooks.Add( action );
			}
		}
	}
}
