using System;
using Terraria;
using ModLibsCore.Classes.Loadable;


namespace ModLibsCore.Services.ProjectileOwner {
	public static class ProjectileExtensions {
		public static Entity GetOwner( this Projectile projectile ) {
			return projectile.GetGlobalProjectile<ModLibsProjectile>()
				.Owner;
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
