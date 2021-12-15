using System;
using Terraria;
using ModLibsCore.Classes.Loadable;


namespace ModLibsCore.Services.ProjectileOwner {
	/// <summary>
	/// Implements extension methods for `Projectile` mirroring `ProjectileOwner` methods.
	/// </summary>
	public static class ProjectileExtensions {
		/// <summary></summary>
		/// <param name="projectile"></param>
		/// <returns></returns>
		public static Entity GetOwner( this Projectile projectile ) {
			return projectile.GetGlobalProjectile<ModLibsProjectile>()
				.Owner;
		}

		/// <summary></summary>
		/// <param name="projectile"></param>
		/// <returns>`Player` instance, if applicable.</returns>
		public static Player GetPlayerOwner( this Projectile projectile ) {
			return projectile.GetGlobalProjectile<ModLibsProjectile>()
				.Owner as Player;
		}

		/// <summary></summary>
		/// <param name="projectile"></param>
		/// <returns>`NPC` instance, if applicable.</returns>
		public static NPC GetNPCOwner( this Projectile projectile ) {
			return projectile.GetGlobalProjectile<ModLibsProjectile>()
				.Owner as NPC;
		}
	}
}
