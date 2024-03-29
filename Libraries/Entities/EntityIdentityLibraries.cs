﻿using Terraria;
using Terraria.ID;
using ModLibsCore.Libraries.Items.Attributes;
using ModLibsCore.Libraries.NPCs.Attributes;
using ModLibsCore.Libraries.Projectiles.Attributes;


namespace ModLibsCore.Libraries.Entities {
	/// <summary>
	/// Assorted static "helper" functions pertaining to `Entity`s (parent class of Item, NPC, Player, and Projectile).
	/// </summary>
	public class EntityIdentityLibraries {
		/// <summary>
		/// Gets a hash value to attempt to uniquely identify a given entity. Not recommended if the specific entity's
		/// `GetVanillaSnapshotHash(...)` (via. the respective Helper) is available.
		/// </summary>
		/// <param name="ent">Entity to attempt to identify.</param>
		/// <param name="noContext">Omits `whoAmI`.</param>
		/// <returns>The identifying hash of the entity.</returns>
		public static int GetVanillaSnapshotHash( Entity ent, bool noContext ) {
			int pow = 1;
			int Pow() {
				pow *= 2;
				if( pow > 16777216 ) { pow = 1; }
				return pow;
			}

			//

			int hash = ("active"+ent.active).GetHashCode();

			if( !noContext ) {
				if( ent is Projectile ) {
					var proj = ent as Projectile;

					hash += ("whoAmI"+Main.projectileIdentity[proj.owner, proj.projUUID]).GetHashCode() + Pow();
				} else {
					hash += ("whoAmI"+ent.whoAmI).GetHashCode() + Pow();
				}
				//hash ^= ("position"+ent.position).GetHashCode();
				//hash ^= ("velocity"+ent.velocity).GetHashCode();
				//hash ^= ("oldPosition"+ent.oldPosition).GetHashCode();
				//hash ^= ("oldVelocity"+ent.oldVelocity).GetHashCode();
				//hash ^= ("oldDirection"+ent.oldDirection).GetHashCode();
				//hash ^= ("direction"+ent.direction).GetHashCode();
				//hash ^= ("wet"+ent.wet).GetHashCode();
				//hash ^= ("honeyWet"+ent.honeyWet).GetHashCode();
				//hash ^= ("wetCount"+ent.wetCount).GetHashCode();
				//hash ^= ("lavaWet"+ent.lavaWet).GetHashCode();
			} else {
				Pow();
			}
			hash += ("width"+ent.width).GetHashCode() + Pow();
			hash += ("height"+ent.height).GetHashCode() + Pow();
			
			return hash;
		}
	}
}
