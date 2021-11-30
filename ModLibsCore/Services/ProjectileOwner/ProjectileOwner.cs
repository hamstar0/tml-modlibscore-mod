using System;
using Terraria;
using ModLibsCore.Classes.Loadable;


namespace ModLibsCore.Services.ProjectileOwner {
	public static class ProjectileExtensions {
		public static Entity GetOwner( this Projectile projectile ) {
			return projectile.GetGlobalProjectile<ModLibsProjectile>()
				.Owner;
		}

		public static Player GetPlayerOwner( this Projectile projectile ) {
			return projectile.GetGlobalProjectile<ModLibsProjectile>()
				.Owner as Player;
		}

		public static NPC GetNPCOwner( this Projectile projectile ) {
			return projectile.GetGlobalProjectile<ModLibsProjectile>()
				.Owner as NPC;
		}
	}




	public class ProjectileOwner : ILoadable {
		internal static int ClaimingForPlayerWho = -1;
		internal static int ClaimingForNpcWho = -1;
		internal static int ClaimingForProjectilePlayerWho = -1;
		internal static int ClaimingForProjectileNpcWho = -1;



		////////////////

		public static Entity GetOwner( Projectile projectile ) {
			return projectile.GetOwner();
		}

		////

		internal static void SetOwnerManually( Projectile projectile, Entity owner ) {
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
