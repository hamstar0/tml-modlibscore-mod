﻿using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using ModLibsCore.Libraries.Debug;
using ModLibsCore.Classes.Loadable;


namespace ModLibsCore.Services.Hooks.LoadHooks {
	public partial class LoadHooks : ILoadable {
		private IList<Action> PostContentLoadHooks = new List<Action>();
		private IList<Action> PostModLoadHooks = new List<Action>();
		private IList<Action> ModUnloadHooks = new List<Action>();
		private IList<Action> WorldLoadOnceHooks = new List<Action>();
		private IList<Action> WorldLoadEachHooks = new List<Action>();
		private IList<Action> PostWorldLoadOnceHooks = new List<Action>();
		private IList<Action> PostWorldLoadEachHooks = new List<Action>();
		private IList<Action> WorldUnloadOnceHooks = new List<Action>();
		private IList<Action> WorldUnloadEachHooks = new List<Action>();
		private IList<Action> PostWorldUnloadOnceHooks = new List<Action>();
		private IList<Action> PostWorldUnloadEachHooks = new List<Action>();
		private IList<Action> WorldInPlayOnceHooks = new List<Action>();
		private IList<Action> WorldInPlayEachHooks = new List<Action>();
		private IList<Action> SafeWorldLoadOnceHooks = new List<Action>();
		private IList<Action> SafeWorldLoadEachHooks = new List<Action>();

		private bool PostContentLoadHookConditionsMet = false;
		private bool PostModLoadHookConditionsMet = false;
		private bool WorldLoadHookConditionsMet = false;
		private bool WorldUnloadHookConditionsMet = false;
		private bool PostWorldUnloadHookConditionsMet = false;
		private bool WorldInPlayHookConditionsMet = false;
		private bool SafeWorldLoadHookConditionsMet = false;

		private Func<bool> OnTickGet;



		////////////////

		internal LoadHooks() {
			this.OnTickGet = Timers.Timers.MainOnTickGet();
			Main.OnTick += LoadHooks._Update;
		}

		void ILoadable.OnModsLoad() { }

		void ILoadable.OnPostModsLoad() {
			LoadHooks.AddWorldLoadEachHook( () => {
				this.WorldUnloadHookConditionsMet = false;
				this.PostWorldUnloadHookConditionsMet = false;
			} );
		}

		void ILoadable.OnModsUnload() {
			this.FulfillModUnloadHooks();

			try {
				Main.OnTick -= LoadHooks._Update;

				if( this.WorldLoadHookConditionsMet && !this.WorldUnloadHookConditionsMet ) {
					this.FulfillWorldUnloadHooks();
					this.FulfillPostWorldUnloadHooks();
				}
			} catch { }
		}


		////////////////

		internal void PreSaveAndExit() {
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
