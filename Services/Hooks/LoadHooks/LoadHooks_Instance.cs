using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using ModLibsCore.Libraries.Debug;


namespace ModLibsCore.Services.Hooks.LoadHooks {
	public partial class LoadHooks : ModSystem {
		internal IList<Action> PostContentLoadHooks = new List<Action>();
		internal IList<Action> PostModLoadHooks = new List<Action>();
		internal IList<Action> ModUnloadHooks = new List<Action>();
		internal IList<Action> WorldLoadOnceHooks = new List<Action>();
		internal IList<Action> WorldLoadEachHooks = new List<Action>();
		internal IList<Action> PostWorldLoadOnceHooks = new List<Action>();
		internal IList<Action> PostWorldLoadEachHooks = new List<Action>();
		internal IList<Action> WorldUnloadOnceHooks = new List<Action>();
		internal IList<Action> WorldUnloadEachHooks = new List<Action>();
		internal IList<Action> PostWorldUnloadOnceHooks = new List<Action>();
		internal IList<Action> PostWorldUnloadEachHooks = new List<Action>();
		internal IList<Action> WorldInPlayOnceHooks = new List<Action>();
		internal IList<Action> WorldInPlayEachHooks = new List<Action>();
		internal IList<Action> SafeWorldLoadOnceHooks = new List<Action>();
		internal IList<Action> SafeWorldLoadEachHooks = new List<Action>();

		private bool PostContentLoadHookConditionsMet = false;
		private bool PostModLoadHookConditionsMet = false;
		private bool WorldLoadHookConditionsMet = false;
		private bool WorldUnloadHookConditionsMet = false;
		private bool PostWorldUnloadHookConditionsMet = false;
		private bool WorldInPlayHookConditionsMet = false;
		private bool SafeWorldLoadHookConditionsMet = false;

		private Func<bool> OnTickGet;



		////////////////

		public override void Load() {
			this.OnTickGet = Timers.Timers.MainOnTickGet();
		}

		public override void OnModLoad() {
			var modsys = ModContent.GetInstance<ModLibsCoreModSystem>();
			modsys.TickUpdates.Add( LoadHooks._Update );

			LoadHooks.AddWorldLoadEachHook( () => {
				this.WorldUnloadHookConditionsMet = false;
				this.PostWorldUnloadHookConditionsMet = false;
			} );
		}

		public override void Unload() {
			this.FulfillModUnloadHooks();

			try {
				var modsys = ModContent.GetInstance<ModLibsCoreModSystem>();
				modsys.TickUpdates.Remove( LoadHooks._Update );

				if ( this.WorldLoadHookConditionsMet && !this.WorldUnloadHookConditionsMet ) {
					this.FulfillWorldUnloadHooks();
					this.FulfillPostWorldUnloadHooks();
				}
			} catch { }
		}


		////////////////

		public override void PreSaveAndQuit() {
			this.FulfillWorldUnloadHooks();
		}

		////////////////

		private static void _Update() { // <- Just in case references are doing something funky...
			var loadHooks = ModContent.GetInstance<LoadHooks>();
			if( loadHooks == null ) { return; }

			if( loadHooks.OnTickGet() ) { // <- Throttles to 60fps
				loadHooks.Update();
			}
		}

		private void Update() {
			if( Main.netMode != NetmodeID.Server ) {
				if( this.WorldLoadHookConditionsMet && Main.gameMenu ) {
					this.WorldLoadHookConditionsMet = false;
					this.SafeWorldLoadHookConditionsMet = false;
				}
			}

			if( this.WorldUnloadHookConditionsMet ) {
				if( Main.gameMenu && Main.menuMode == 0 ) {
					this.FulfillPostWorldUnloadHooks();
				}
			}
		}
	}
}
