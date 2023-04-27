using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;
using ModLibsCore.Classes.Errors;
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

		void ILoadable.Load( Mod mod ) {
			if( this.CalledOnModsLoad ) {
				return;
			}

			this.CalledOnModsLoad = true;

			//

			CustomPlayerData.InstallCustomPlayerDataCleanup();

			//

			var modsys = ModContent.GetInstance<ModLibsCoreModSystem>();
			modsys.TickUpdates.Add( CustomPlayerData.UpdateAll );
		}

		/// @private

		void ILoadable.Unload() {
			if( this.CalledOnModsUnload ) {
				return;
			}

			this.CalledOnModsUnload = true;

			//

			var modsys = ModContent.GetInstance<ModLibsCoreModSystem>();
			modsys.TickUpdates.Remove( CustomPlayerData.UpdateAll );
		}
	}
}
