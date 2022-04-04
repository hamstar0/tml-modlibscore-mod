using System;
using System.Linq;
using Terraria;
using ModLibsCore.Classes.Loadable;
using ModLibsCore.Libraries.Debug;


namespace ModLibsCore.Services.Hooks.LoadHooks {
	public partial class LoadHooks : ILoadable {
		internal int LastRanHookReportedHookCount = -1;



		////////////////

		internal void FulfillPostContentLoadHooks() {
			if( this.PostContentLoadHookConditionsMet ) { return; }
			this.PostContentLoadHookConditionsMet = true;
			
			Action[] hooks;
			
			lock( LoadHooks.PostContentLoadHookLock ) {
				hooks = this.PostContentLoadHooks.ToArray();
				this.PostContentLoadHooks.Clear();
			}

			this.LastRanHookReportedHookCount = hooks.Length;

			foreach( Action hook in hooks ) {
				hook();
			}
		}
		
		internal void FulfillPostModLoadHooks() {
			if( this.PostModLoadHookConditionsMet ) { return; }

			this.PostModLoadHookConditionsMet = true;

			//

			Action[] hooks;
			
			lock( LoadHooks.PostModLoadHookLock ) {
				hooks = this.PostModLoadHooks.ToArray();

				this.PostModLoadHooks.Clear();
			}

			this.LastRanHookReportedHookCount = hooks.Length;

			foreach( Action hook in hooks ) {
				hook();
			}
		}

		internal void FulfillModUnloadHooks() {
			Action[] hooks;

			lock( LoadHooks.ModUnloadHookLock ) {
				hooks = this.ModUnloadHooks.ToArray();
				this.ModUnloadHooks.Clear();
			}

			this.LastRanHookReportedHookCount = hooks.Length;

			foreach( Action hook in hooks ) {
				hook();
			}
		}


		internal void FulfillWorldLoadHooks() {
			if( this.WorldLoadHookConditionsMet ) { return; }
			this.WorldLoadHookConditionsMet = true;

			Action[] worldLoadOnceHooks;
			Action[] worldLoadEachHooks;
			Action[] postWorldLoadOnceHooks;
			Action[] postWorldLoadEachHooks;

			lock( LoadHooks.WorldLoadOnceHookLock ) {
				worldLoadOnceHooks = this.WorldLoadOnceHooks.ToArray();
				this.WorldLoadOnceHooks.Clear();
			}
			lock( LoadHooks.WorldLoadEachHookLock ) {
				worldLoadEachHooks = this.WorldLoadEachHooks.ToArray();
			}
			lock( LoadHooks.PostWorldLoadOnceHookLock ) {
				postWorldLoadOnceHooks = this.PostWorldLoadOnceHooks.ToArray();
				this.PostWorldLoadOnceHooks.Clear();
			}
			lock( LoadHooks.PostWorldLoadEachHookLock ) {
				postWorldLoadEachHooks = this.PostWorldLoadEachHooks.ToArray();
			}

			this.LastRanHookReportedHookCount = worldLoadOnceHooks.Length;
			foreach( Action hook in worldLoadOnceHooks ) {
				hook();
			}

			this.LastRanHookReportedHookCount = worldLoadEachHooks.Length;
			foreach( Action hook in worldLoadEachHooks ) {
				hook();
			}

			this.LastRanHookReportedHookCount = postWorldLoadOnceHooks.Length;
			foreach( Action hook in postWorldLoadOnceHooks ) {
				hook();
			}

			this.LastRanHookReportedHookCount = postWorldLoadEachHooks.Length;
			foreach( Action hook in postWorldLoadEachHooks ) {
				hook();
			}
		}


		internal void FulfillWorldInPlayHooks() {
			if( this.WorldInPlayHookConditionsMet ) { return; }
			this.WorldInPlayHookConditionsMet = true;

			Action[] inPlayOnceHooks;
			Action[] inPlayEachHooks;

			lock( LoadHooks.WorldInPlayOnceHookLock ) {
				inPlayOnceHooks = this.WorldInPlayOnceHooks.ToArray();
				this.WorldInPlayOnceHooks.Clear();
			}
			lock( LoadHooks.WorldInPlayEachHookLock ) {
				inPlayEachHooks = this.WorldInPlayEachHooks.ToArray();
			}

			this.LastRanHookReportedHookCount = inPlayOnceHooks.Length;
			foreach( Action hook in inPlayOnceHooks ) {
				hook();
			}

			this.LastRanHookReportedHookCount = inPlayEachHooks.Length;
			foreach( Action hook in inPlayEachHooks ) {
				hook();
			}
		}


		internal void FulfillSafeWorldLoadHooks() {
			if( this.SafeWorldLoadHookConditionsMet ) { return; }
			this.SafeWorldLoadHookConditionsMet = true;

			Action[] safeWorldLoadOnceHooks;
			Action[] safeWorldLoadEachHooks;

			lock( LoadHooks.SafeWorldLoadOnceHookLock ) {
				safeWorldLoadOnceHooks = this.SafeWorldLoadOnceHooks.ToArray();
				this.SafeWorldLoadOnceHooks.Clear();
			}
			lock( LoadHooks.SafeWorldLoadEachHookLock ) {
				safeWorldLoadEachHooks = this.SafeWorldLoadEachHooks.ToArray();
			}

			this.LastRanHookReportedHookCount = safeWorldLoadOnceHooks.Length;
			foreach( Action hook in safeWorldLoadOnceHooks ) {
				hook();
			}

			this.LastRanHookReportedHookCount = safeWorldLoadEachHooks.Length;
			foreach( Action hook in safeWorldLoadEachHooks ) {
				hook();
			}
		}


		internal void FulfillWorldUnloadHooks() {
			if( this.WorldUnloadHookConditionsMet ) { return; }
			this.WorldUnloadHookConditionsMet = true;

			Action[] worldUnloadOnceHooks;
			Action[] worldUnloadEachHooks;

			lock( LoadHooks.WorldUnloadOnceHookLock ) {
				worldUnloadOnceHooks = this.WorldUnloadOnceHooks.ToArray();
				this.WorldUnloadOnceHooks.Clear();
			}
			lock( LoadHooks.WorldUnloadEachHookLock ) {
				worldUnloadEachHooks = this.WorldUnloadEachHooks.ToArray();
			}

			this.LastRanHookReportedHookCount = worldUnloadOnceHooks.Length;
			foreach( Action hook in worldUnloadOnceHooks ) {
				hook();
			}

			this.LastRanHookReportedHookCount = worldUnloadEachHooks.Length;
			foreach( Action hook in worldUnloadEachHooks ) {
				hook();
			}
		}


		internal void FulfillPostWorldUnloadHooks() {
			if( this.PostWorldUnloadHookConditionsMet ) { return; }
			this.PostWorldUnloadHookConditionsMet = true;

			Action[] postWorldUnloadOnceHooks;
			Action[] postWorldUnloadEachHooks;

			lock( LoadHooks.PostWorldUnloadOnceHookLock ) {
				postWorldUnloadOnceHooks = this.PostWorldUnloadOnceHooks.ToArray();
				this.PostWorldUnloadOnceHooks.Clear();
			}
			lock( LoadHooks.PostWorldUnloadEachHookLock ) {
				postWorldUnloadEachHooks = this.PostWorldUnloadEachHooks.ToArray();
			}

			this.LastRanHookReportedHookCount = postWorldUnloadOnceHooks.Length;
			foreach( Action hook in postWorldUnloadOnceHooks ) {
				hook();
			}

			this.LastRanHookReportedHookCount = postWorldUnloadEachHooks.Length;
			foreach( Action hook in postWorldUnloadEachHooks ) {
				hook();
			}
		}
	}
}
