using System;
using System.Linq;
using Terraria;
using Terraria.ID;
using Terraria.IO;
using Terraria.ModLoader;
using ModLibsCore.Classes.Errors;
using ModLibsCore.Libraries.Debug;
using ModLibsCore.Libraries.Entities;
using ModLibsCore.Libraries.Items;


namespace ModLibsCore.Libraries.Players {
	/// <summary>
	/// Assorted static "helper" functions pertaining to unique player identification.
	/// </summary>
	public partial class PlayerIdentityLibraries {
		/// <summary>
		/// Gets a code to uniquely identify the current player.
		/// </summary>
		/// <returns></returns>
		public static string GetUniqueId() {
			if( Main.netMode == NetmodeID.Server ) {
				throw new ModLibsException( "No 'current' player on a server." );
			}
			if( Main.gameMenu ) {
				throw new ModLibsException( "No 'current' player defined outside of the game." );
			}

			//

			return PlayerIdentityLibraries.GetUniqueId( Main.ActivePlayerFileData );
		}

		/// <summary>
		/// Gets a code to uniquely identify a given player. These are synced to the server in multiplayer.
		/// </summary>
		/// <param name="player"></param>
		/// <returns></returns>
		public static string GetUniqueId( Player player ) {
			var piLibs = ModContent.GetInstance<PlayerIdentityLibraries>();
			string id;

			if( piLibs.PlayerIds.TryGetValue(player.whoAmI, out id) ) {
				return id;
			}

			//

			if( Main.netMode == NetmodeID.Server ) {
				//throw new ModLibsException( "Could not find player " + player.name + "'s id." );
				return null;
			}

			//

			if( !Main.gameMenu ) {
				if( player.whoAmI == Main.myPlayer ) {
					id = PlayerIdentityLibraries.GetUniqueId();
				}
			} else {
				PlayerFileData plrData = Main.PlayerList.FirstOrDefault( pd => pd.Name == player.name );
				if( plrData == null ) {
					return null;
				}

				id = PlayerIdentityLibraries.GetUniqueId( plrData );
			}

			piLibs.PlayerIds[ player.whoAmI ] = id;

			return id;
		}

		////

		private static string GetUniqueId( PlayerFileData plrData ) {
			int plrFileHash = plrData.GetFileName().GetHashCode();
			int plrNameHash = Main.LocalPlayer.name.GetHashCode();
			//int activePlrCloudHashCode = Main.ActivePlayerFileData.IsCloudSave.GetHashCode();

			int hash = Math.Abs( plrFileHash + plrNameHash );

			return Main.clientUUID + "_" + hash;
		}


		////

		/// <summary>
		/// Gets an active player by a given unique id (if present).
		/// </summary>
		/// <param name="uid"></param>
		/// <returns></returns>
		public static Player GetPlayerByUniqueId( string uid ) {
			int len = Main.player.Length;

			for( int i=0; i<len; i++ ) {
				Player plr = Main.player[ i ];
//LogLibraries.Log( "GetPlayerByProperId: "+PlayerIdentityLibraries.GetProperUniqueId( plr )+" == "+uid+": "+plr.name+" ("+plr.whoAmI+")" );
				if( plr == null /*|| !plr.active*/ ) { continue; }	// <- This is WEIRD!
				
				if( PlayerIdentityLibraries.GetUniqueId(plr) == uid ) {
					return plr;
				}
			}

			return null;
		}


		////////////////

		/// <summary>
		/// Gets a unique code for a player's current state. Useful for detecting state changes.
		/// </summary>
		/// <param name="player"></param>
		/// <param name="noContext">Omits team, pvp state, and name.</param>
		/// <param name="looksMatter">Includes appearance elements.</param>
		/// <returns></returns>
		public static int GetVanillaSnapshotHash( Player player, bool noContext, bool looksMatter ) {
			int pow = 1;
			int Pow() {
				pow *= 2;
				if( pow > 16777216 ) { pow = 1; }
				return pow;
			}

			//

			int hash = EntityIdentityLibraries.GetVanillaSnapshotHash( player, noContext );
			int itemHash;

			hash += ( "statLifeMax" + player.statLifeMax ).GetHashCode() * Pow();
			hash += ( "statManaMax" + player.statManaMax ).GetHashCode() * Pow();
			hash += ( "extraAccessory" + player.extraAccessory ).GetHashCode() * Pow();
			hash += ( "difficulty" + player.difficulty ).GetHashCode() * Pow();

			if( !noContext ) {
				hash += ( "team" + player.team ).GetHashCode() * Pow();
				hash += ( "hostile" + player.hostile ).GetHashCode() * Pow();   //pvp?
				hash += ( "name" + player.name ).GetHashCode() * Pow();
			} else {
				Pow();
				Pow();
				Pow();
			}

			if( looksMatter ) {
				hash += ( "Male" + player.Male ).GetHashCode() * Pow();
				hash += ( "skinColor" + player.skinColor ).GetHashCode() * Pow();
				hash += ( "hair" + player.hair ).GetHashCode() * Pow();
				hash += ( "hairColor" + player.hairColor ).GetHashCode() * Pow();
				hash += ( "shirtColor" + player.shirtColor ).GetHashCode() * Pow();
				hash += ( "underShirtColor" + player.underShirtColor ).GetHashCode() * Pow();
				hash += ( "pantsColor" + player.pantsColor ).GetHashCode() * Pow();
				hash += ( "shoeColor" + player.shoeColor ).GetHashCode() * Pow();
			}
			
			for( int i = 0; i < player.inventory.Length; i++ ) {
				Item item = player.inventory[i];
				if( item == null || !item.active || item.stack == 0 ) {
					itemHash = ( "inv" + i ).GetHashCode();
				} else {
					itemHash = i + ItemIdentityLibraries.GetVanillaSnapshotHash( item, noContext, true );
				}
				hash += itemHash * Pow();
			}
			for( int i = 0; i < player.armor.Length; i++ ) {
				Item item = player.armor[i];
				if( item == null || !item.active || item.stack == 0 ) {
					itemHash = ( "arm" + i ).GetHashCode();
				} else {
					itemHash = i + ItemIdentityLibraries.GetVanillaSnapshotHash( item, noContext, true );
				}
				hash += itemHash * Pow();
			}
			for( int i = 0; i < player.bank.item.Length; i++ ) {
				Item item = player.bank.item[i];
				if( item == null || !item.active || item.stack == 0 ) {
					itemHash = ( "bank" + i ).GetHashCode();
				} else {
					itemHash = i + ItemIdentityLibraries.GetVanillaSnapshotHash( item, noContext, true );
				}
				hash += itemHash * Pow();
			}
			for( int i = 0; i < player.bank2.item.Length; i++ ) {
				Item item = player.bank2.item[i];
				if( item == null || !item.active || item.stack == 0 ) {
					itemHash = ( "bank2" + i ).GetHashCode();
				} else {
					itemHash = i + ItemIdentityLibraries.GetVanillaSnapshotHash( item, noContext, true );
				}
				hash += itemHash;
			}
			for( int i = 0; i < player.bank3.item.Length; i++ ) {
				Item item = player.bank3.item[i];
				if( item == null || !item.active || item.stack == 0 ) {
					itemHash = ( "bank3" + i ).GetHashCode();
				} else {
					itemHash = i + ItemIdentityLibraries.GetVanillaSnapshotHash( item, noContext, true );
				}
				hash += itemHash * Pow();
			}
			return hash;
		}
	}
}
