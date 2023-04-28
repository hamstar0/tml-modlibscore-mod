using System;
using Terraria;
using Terraria.ModLoader;


namespace ModLibsCore.Services.ProjectileOwner {
	/// <summary>
	/// Implements extension methods for `Projectile` mirroring `ProjectileOwner` methods.
	/// </summary>
	public static class ProjectileExtensions {
		/// <summary></summary>
		/// <param name="projectile"></param>
		/// <returns></returns>
		public static Entity GetOwner( this Projectile projectile ) {
			return projectile.TryGetGlobalProjectile(out ModLibsProjectile modProjectile)
				&& modProjectile.Owner is Entity { active: true } result ? result : null;
		}

		/// <summary></summary>
		/// <param name="projectile"></param>
		/// <returns>`Player` instance, if applicable.</returns>
		public static Player GetPlayerOwner( this Projectile projectile ) {
			return projectile.GetOwner() as Player;
		}

		/// <summary></summary>
		/// <param name="projectile"></param>
		/// <returns>`NPC` instance, if applicable.</returns>
		public static NPC GetNPCOwner( this Projectile projectile ) {
			return projectile.GetOwner() as NPC;
		}
	}
}
