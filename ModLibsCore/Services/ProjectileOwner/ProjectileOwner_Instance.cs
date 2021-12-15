using System;
using System.Collections.Generic;
using Terraria;
using ModLibsCore.Classes.Loadable;


namespace ModLibsCore.Services.ProjectileOwner {
	/// <summary>
	/// Implements a method to know the 'owner' `Entity` of a given projectile.
	/// </summary>
	public partial class ProjectileOwner : ILoadable {
		private IList<OnProjectileOwnerSet> Hooks = new List<OnProjectileOwnerSet>();



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
