using System;
using Terraria;
using ModLibsCore.Classes.Loadable;


namespace ModLibsCore.Services.ProjectileOwner {
	/// <summary>
	/// Implements a method to know the 'owner' `Entity` of a given projectile.
	/// </summary>
	public partial class ProjectileOwner : ILoadable {
		internal static int ClaimingForPlayerWho = -1;
		internal static int ClaimingForNpcWho = -1;
		internal static int ClaimingForProjectilePlayerWho = -1;
		internal static int ClaimingForProjectileNpcWho = -1;



		////////////////

		/// <summary></summary>
		/// <param name="projectile"></param>
		/// <returns></returns>
		public static Entity GetOwner( Projectile projectile ) {
			return projectile.GetOwner();
		}

		////

		/// <summary>
		/// Allows manually overriding the owner entity of a given projectile. Intended for custom use of projectiles that may
		/// otherwise fail to register an owner. Use with care.
		/// </summary>
		/// <param name="projectile"></param>
		/// <param name="owner"></param>
		public static void SetOwnerManually( Projectile projectile, Entity owner ) {
			var myproj = projectile.GetGlobalProjectile<ModLibsProjectile>();

			myproj.NpcWho = owner is NPC
				? owner.whoAmI
				: -1;
			myproj.PlayerWho = owner is Player
				? owner.whoAmI
				: -1;

			//

			ProjectileOwner.RunOwnerSetHooks( projectile, true );
		}


		////////////////

		internal static void ClaimProjectile( ModLibsProjectile myproj, Projectile projectile ) {
			myproj.PlayerWho = ProjectileOwner.ClaimingForPlayerWho;

			myproj.NpcWho = ProjectileOwner.ClaimingForProjectileNpcWho != -1
				? ProjectileOwner.ClaimingForProjectileNpcWho
				: ProjectileOwner.ClaimingForNpcWho;
			myproj.PlayerWho = ProjectileOwner.ClaimingForPlayerWho;

			//

			ProjectileOwner.RunOwnerSetHooks( projectile, false );
		}
	}
}
