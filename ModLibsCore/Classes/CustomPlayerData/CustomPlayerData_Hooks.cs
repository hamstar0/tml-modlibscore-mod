using System;
using Terraria;
using Terraria.ModLoader;


namespace ModLibsCore.Classes.PlayerData {
	/// <summary>
	/// An alternative to ModPlayer for basic per-player, per-game data storage and Update use.
	/// </summary>
	public partial class CustomPlayerData : ILoadable {
		/// <summary>
		/// Player enters the game.
		/// </summary>
		/// <param name="isCurrentPlayer"></param>
		/// <param name="data">Data loaded for the current player from file.</param>
		protected virtual void OnEnter( bool isCurrentPlayer, object data ) { }

		/// <summary>
		/// Player exits the game.
		/// </summary>
		/// <returns>Data to save for the current player. Return `null` to skip.</returns>
		protected virtual object OnExit() {
			return null;
		}

		/// <summary>
		/// Runs every tick.
		/// </summary>
		protected virtual void Update() { }
	}
}
