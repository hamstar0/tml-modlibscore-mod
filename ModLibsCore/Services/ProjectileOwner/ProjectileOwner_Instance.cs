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

		private ISet<int> NewProjectileIdxs = new HashSet<int>();



		////////////////

		void ILoadable.OnModsLoad() {
			On.Terraria.Projectile.NewProjectile_float_float_float_float_int_int_float_int_float_float += (
						On.Terraria.Projectile.orig_NewProjectile_float_float_float_float_int_int_float_int_float_float orig,
						float X,
						float Y,
						float SpeedX,
						float SpeedY,
						int Type,
						int Damage,
						float KnockBack,
						int Owner,
						float ai0,
						float ai1 ) => {
				int projIdx = orig.Invoke( X, Y, SpeedX, SpeedY, Type, Damage, KnockBack, Owner, ai0, ai1 );

				Projectile proj = Main.projectile[ projIdx ];

				if( proj?.active == true ) {
					var myproj = proj.GetGlobalProjectile<ModLibsProjectile>();

					if( ProjectileOwner.ClaimProjectile(myproj, proj) ) {
						this.NewProjectileIdxs.Add( projIdx );
					}
				}

				return projIdx;
			};

			//

			On.Terraria.Player.ItemCheck += ( On.Terraria.Player.orig_ItemCheck orig, Player self, int i ) => {
				 ProjectileOwner.ClaimingForPlayerWho = self.whoAmI;
				orig.Invoke( self, i );
				 ProjectileOwner.ClaimingForPlayerWho = -1;

				this.RunOwnerSetHooksForNewProjectiles();
			};

			On.Terraria.NPC.AI += ( On.Terraria.NPC.orig_AI orig, NPC self ) => {
				 ProjectileOwner.ClaimingForNpcWho = self.whoAmI;
				orig.Invoke( self );
				 ProjectileOwner.ClaimingForNpcWho = -1;

				this.RunOwnerSetHooksForNewProjectiles();
			};

			On.Terraria.Projectile.AI += ( On.Terraria.Projectile.orig_AI orig, Projectile self ) => {
				var myproj = self.GetGlobalProjectile<ModLibsProjectile>();

				 ProjectileOwner.ClaimingForProjectilePlayerWho = myproj.Owner is Player ? myproj.Owner.whoAmI : -1;
				 ProjectileOwner.ClaimingForProjectileNpcWho = myproj.Owner is NPC ? myproj.Owner.whoAmI : -1;
				orig.Invoke( self );
				 ProjectileOwner.ClaimingForProjectilePlayerWho = -1;
				 ProjectileOwner.ClaimingForProjectileNpcWho = -1;

				this.RunOwnerSetHooksForNewProjectiles();
			};
		}

		void ILoadable.OnModsUnload() { }

		void ILoadable.OnPostModsLoad() { }


		////////////////
		
		private void RunOwnerSetHooksForNewProjectiles() {
			foreach( int projIdx in this.NewProjectileIdxs ) {
				Projectile proj = Main.projectile[projIdx];
				if( proj?.active != true ) {
					continue;
				}

				ProjectileOwner.RunOwnerSetHooks( projIdx, false );
			}

			//

			this.NewProjectileIdxs.Clear();
		}
	}
}
