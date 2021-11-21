using System;
using Terraria;
using Terraria.ModLoader;
using ModLibsCore.Services.ProjectileOwner;


namespace ModLibsCore {
	class ModLibsProjectile : GlobalProjectile {
		private int NpcWho = -1;
		private int PlayerWho = -1;

		public Entity Owner => this.NpcWho != -1
			? Main.npc[ this.NpcWho ]
			: this.PlayerWho != -1
			? Main.player[ this.PlayerWho ]
			: (Entity)null;


		////////////////

		public override bool InstancePerEntity => true;



		////////////////

		public override void SetDefaults( Projectile projectile ) {
			this.PlayerWho = ProjectileOwner.ClaimingForPlayerWho;

			this.NpcWho = ProjectileOwner.ClaimingForProjectileNpcWho != -1
				? ProjectileOwner.ClaimingForProjectileNpcWho
				: ProjectileOwner.ClaimingForNpcWho;
			this.PlayerWho = ProjectileOwner.ClaimingForPlayerWho;
		}
	}
}
