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




	/// <summary>
	/// Implements a method to know the 'owner' `Entity` of a given projectile.
	/// </summary>
	public class ProjectileOwner : ILoadable {
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
		}


		////////////////

		internal static void ClaimProjectile( ModLibsProjectile myproj ) {
			myproj.PlayerWho = ProjectileOwner.ClaimingForPlayerWho;

			myproj.NpcWho = ProjectileOwner.ClaimingForProjectileNpcWho != -1
				? ProjectileOwner.ClaimingForProjectileNpcWho
				: ProjectileOwner.ClaimingForNpcWho;
			myproj.PlayerWho = ProjectileOwner.ClaimingForPlayerWho;
		}



		////////////////

		void ILoadable.OnModsLoad() {
			On.Terraria.Player.ItemCheck += ( On.Terraria.Player.orig_ItemCheck orig, Player self, int i ) => {
				ProjectileOwner.ClaimingForPlayerWho = self.whoAmI;
				orig.Invoke( self, i );
				ProjectileOwner.ClaimingForPlayerWho = -1;
			};

			On.Terraria.NPC.AI += ( On.Terraria.NPC.orig_AI orig, NPC self ) => {
				ProjectileOwner.ClaimingForNpcWho = self.whoAmI;
				orig.Invoke( self );
				ProjectileOwner.ClaimingForNpcWho = -1;
			};

			On.Terraria.Projectile.AI += ( On.Terraria.Projectile.orig_AI orig, Projectile self ) => {
				var myproj = self.GetGlobalProjectile<ModLibsProjectile>();

				ProjectileOwner.ClaimingForProjectilePlayerWho = myproj.Owner is Player ? myproj.Owner.whoAmI : -1;
				ProjectileOwner.ClaimingForProjectileNpcWho = myproj.Owner is NPC ? myproj.Owner.whoAmI : -1;
				orig.Invoke( self );
				ProjectileOwner.ClaimingForProjectilePlayerWho = -1;
				ProjectileOwner.ClaimingForProjectileNpcWho = -1;
			};
		}

		void ILoadable.OnModsUnload() { }

		void ILoadable.OnPostModsLoad() { }
	}
}
