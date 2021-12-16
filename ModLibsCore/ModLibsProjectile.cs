using System;
using Terraria;
using Terraria.ModLoader;


namespace ModLibsCore {
	class ModLibsProjectile : GlobalProjectile {
		internal int NpcWho = -1;
		internal int PlayerWho = -1;

		public Entity Owner => this.NpcWho != -1
			? Main.npc[ this.NpcWho ]
			: this.PlayerWho != -1
			? Main.player[ this.PlayerWho ]
			: (Entity)null;


		////////////////

		public override bool InstancePerEntity => true;
	}
}
