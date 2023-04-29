using System;
using Terraria.ModLoader.Config;
using ModLibsCore.Classes.UI.ModConfig;


namespace ModLibsCore.Classes.UI.Config {
	/// <summary>
	/// Implements a ModConfig wrapper for a float to allow nullable behavior and be able to accept inputs via. text input
	/// or slider.
	/// </summary>
	[NullAllowed]
	public class FloatRef {
		/// <summary></summary>
		[CustomModConfigItem( typeof(FloatInputElement) )]
		public float Value { get; set; }



		////

		/// @private
		public FloatRef() { }

		/// @private
		public FloatRef( float value ) {
			this.Value = value;
		}
	}
}
