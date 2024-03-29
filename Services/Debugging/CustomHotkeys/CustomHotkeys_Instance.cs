﻿using System;
using System.Collections.Generic;
using Terraria.GameInput;
using Terraria.ModLoader;


namespace ModLibsCore.Services.Debug.CustomHotkeys {
	/// @private
	public partial class CustomHotkeys : ModSystem {
		private readonly ModKeybind Key1;
		private readonly ModKeybind Key2;

		private readonly IDictionary<string, Action> Key1Actions = new Dictionary<string, Action>();
		private readonly IDictionary<string, Action> Key2Actions = new Dictionary<string, Action>();



		////////////////

		internal CustomHotkeys() {
			this.Key1 = KeybindLoader.RegisterKeybind(ModLibsCoreMod.Instance, "Custom Hotkey 1", "K" );
			this.Key2 = KeybindLoader.RegisterKeybind(ModLibsCoreMod.Instance, "Custom Hotkey 2", "L" );
		}

		////////////////

		internal void ProcessTriggers( TriggersSet triggersSet ) {
			if( this.Key1.JustPressed ) {
				foreach( Action act in this.Key1Actions.Values ) {
					act();
				}
			}
			if( this.Key2.JustPressed ) {
				foreach( Action act in this.Key2Actions.Values ) {
					act();
				}
			}
		}
	}
}
