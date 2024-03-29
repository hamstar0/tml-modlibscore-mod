﻿using System;
using Terraria.ModLoader;


namespace ModLibsCore.Services.Debug.CustomHotkeys {
	/// <summary>
	/// Provides a pair of hotkeys that may be dynamically bound with custom functions (mostly for debug use).
	/// </summary>
	public partial class CustomHotkeys {
		/// <summary></summary>
		/// <param name="name"></param>
		/// <param name="action"></param>
		public static void BindActionToKey1( string name, Action action ) {
			ModContent.GetInstance<CustomHotkeys>().Key1Actions[ name ] = action;
		}

		/// <summary></summary>
		/// <param name="name"></param>
		/// <param name="action"></param>
		public static void BindActionToKey2( string name, Action action ) {
			ModContent.GetInstance<CustomHotkeys>().Key2Actions[ name ] = action;
		}
	}
}
