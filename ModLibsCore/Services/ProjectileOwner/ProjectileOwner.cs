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
		/// <param name="projectileWho">`Main.projectile` index.</param>
		/// <param name="owner"></param>
		public static void SetOwnerManually( int projectileWho, Entity owner ) {
			var projectile = Main.projectile[ projectileWho ];
			var myproj = projectile.GetGlobalProjectile<ModLibsProjectile>();

			myproj.NpcWho = owner is NPC
				? owner.whoAmI
				: -1;
			myproj.PlayerWho = owner is Player
				? owner.whoAmI
				: -1;

			//

			ProjectileOwner.RunOwnerSetHooks( projectileWho, true );
		}


		////////////////

		internal static bool ClaimProjectile( ModLibsProjectile myproj, Projectile projectile ) {
			myproj.PlayerWho = ProjectileOwner.ClaimingForProjectilePlayerWho != -1
				? ProjectileOwner.ClaimingForProjectilePlayerWho
				: ProjectileOwner.ClaimingForPlayerWho;

			myproj.NpcWho = ProjectileOwner.ClaimingForProjectileNpcWho != -1
				? ProjectileOwner.ClaimingForProjectileNpcWho
				: ProjectileOwner.ClaimingForNpcWho;

			return myproj.NpcWho != -1 || myproj.PlayerWho != -1;
		}
	}
}
