using System;
using System.Collections.Generic;
using Terraria;
using ModLibsCore.Classes.Errors;
using ModLibsCore.Classes.Loadable;
using ModLibsCore.Libraries.Debug;


namespace ModLibsCore.Classes.PlayerData {
	/// <summary>
	/// An alternative to ModPlayer for basic per-player, per-game data storage and Update use.
	/// </summary>
	public partial class CustomPlayerData : ILoadable {
		private IDictionary<int, IDictionary<Type, CustomPlayerData>> PlayerWhoToTypeToTypeInstanceMap
			= new Dictionary<int, IDictionary<Type, CustomPlayerData>>();

		private bool CalledOnModsLoad = false;
		private bool CalledOnModsUnload = false;


		////////////////

		/// <summary>
		/// Current player's `whoAmI` (`Main.player` array index) value.
		/// </summary>
		public int PlayerWho { get; private set; }


		////////////////

		/// <summary></summary>
		public Player Player => Main.player[ this.PlayerWho ];



		////////////////

		/// @private
		void ILoadable.OnModsLoad() {
			if( this.CalledOnModsLoad ) {
				throw new ModLibsException( "Attempted multiple calls." );
			}
			this.CalledOnModsLoad = true;

			Main.OnTick += CustomPlayerData.UpdateAll;
		}

		/// @private
		void ILoadable.OnModsUnload() {
			if( this.CalledOnModsUnload ) {
				throw new ModLibsException( "Attempted multiple calls." );
			}
			this.CalledOnModsUnload = true;

			Main.OnTick -= CustomPlayerData.UpdateAll;
		}

		/// @private
		void ILoadable.OnPostModsLoad() { }
	}
}
