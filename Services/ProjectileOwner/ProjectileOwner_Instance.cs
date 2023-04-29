using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using ModLibsCore.Libraries.Debug;
using Terraria.DataStructures;

namespace ModLibsCore.Services.ProjectileOwner {
	/// <summary>
	/// Implements a method to know the 'owner' `Entity` of a given projectile.
	/// </summary>
	public partial class ProjectileOwner : ModSystem {
		private IList<OnProjectileOwnerSet> Hooks = new List<OnProjectileOwnerSet>();

		private ISet<int> NewProjectileIdxs = new HashSet<int>();



		////////////////

		public override void Load() {
			On.Terraria.Projectile.NewProjectile_IEntitySource_float_float_float_float_int_int_float_int_float_float +=
				this.Projectile_NewProjectile_IEntitySource_float_float_float_float_int_int_float_int_float_float;
			On.Terraria.MessageBuffer.GetData += this.MessageBuffer_GetData;
			On.Terraria.Player.ItemCheck += this.Player_ItemCheck;
			On.Terraria.NPC.AI += this.NPC_AI;
			On.Terraria.Projectile.AI += this.Projectile_AI;
		}

		////////////////
		
		private int Projectile_NewProjectile_IEntitySource_float_float_float_float_int_int_float_int_float_float(
					On.Terraria.Projectile.orig_NewProjectile_IEntitySource_float_float_float_float_int_int_float_int_float_float orig,
					IEntitySource src,
					float X,
					float Y,
					float SpeedX,
					float SpeedY,
					int Type,
					int Damage,
					float KnockBack,
					int Owner,
					float ai0,
					float ai1 ) {
			int projIdx = orig.Invoke( src, X, Y, SpeedX, SpeedY, Type, Damage, KnockBack, Owner, ai0, ai1 );

			Projectile proj = Main.projectile[ projIdx ];

			if( proj?.active == true ) {
				var myproj = proj.GetGlobalProjectile<ModLibsProjectile>();

				if( ProjectileOwner.ClaimProjectile(myproj, proj) ) {
					this.NewProjectileIdxs.Add( projIdx );
				}
			}

			return projIdx;
		}

		////

		private void MessageBuffer_GetData(
					On.Terraria.MessageBuffer.orig_GetData orig,
					MessageBuffer self,
					int start,
					int length,
					out int messageType ) {
			orig.Invoke( self, start, length, out messageType );

			//

			if( messageType == MessageID.SyncProjectile ) {
				self.reader.BaseStream.Position = start + 1;

				this.AssignProjectileOwnerFromStream( self );
			}
		}

		private void Player_ItemCheck( On.Terraria.Player.orig_ItemCheck orig, Player self, int i ) {
			 ProjectileOwner.ClaimingForPlayerWho = self.whoAmI;
			orig.Invoke( self, i );
			 ProjectileOwner.ClaimingForPlayerWho = -1;

			this.RunOwnerSetHooksForNewProjectiles();
		}

		private void NPC_AI( On.Terraria.NPC.orig_AI orig, NPC self ) {
			 ProjectileOwner.ClaimingForNpcWho = self.whoAmI;
			orig.Invoke( self );
			 ProjectileOwner.ClaimingForNpcWho = -1;

			this.RunOwnerSetHooksForNewProjectiles();
		}

		private void Projectile_AI( On.Terraria.Projectile.orig_AI orig, Projectile self ) {
			var myproj = self.GetGlobalProjectile<ModLibsProjectile>();

			 ProjectileOwner.ClaimingForProjectilePlayerWho = myproj.Owner is Player ? myproj.Owner.whoAmI : -1;
			 ProjectileOwner.ClaimingForProjectileNpcWho = myproj.Owner is NPC ? myproj.Owner.whoAmI : -1;
			orig.Invoke( self );
			 ProjectileOwner.ClaimingForProjectilePlayerWho = -1;
			 ProjectileOwner.ClaimingForProjectileNpcWho = -1;

			this.RunOwnerSetHooksForNewProjectiles();
		}


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


		////////////////

		private void AssignProjectileOwnerFromStream( MessageBuffer stream ) {
			int identity = (int)stream.reader.ReadInt16();
			Vector2 position = stream.reader.ReadVector2();
			Vector2 velocity = stream.reader.ReadVector2();
			float knockBack = stream.reader.ReadSingle();
			int damage = (int)stream.reader.ReadInt16();
			int owner = (int)stream.reader.ReadByte();

			//

			Projectile proj = null;
			int projIdx;

			int projLen = Main.projectile.Length;
			for( projIdx = 0; projIdx < projLen; projIdx++ ) {
				proj = Main.projectile[projIdx];

				if( proj.owner == owner && proj.identity == identity && proj.active ) {
					break;
				}
			}

			//
			
			if( proj?.GetOwner() != null ) {
				return;
			}

			//

			Entity ent = proj.npcProj
				? (Entity)null	//(Entity)Main.npc[proj.owner]
				: (Entity)Main.player[proj.owner];

			if( ent?.active == true ) {
				ProjectileOwner.SetOwnerManually( projIdx, ent );
			}
		}
	}
}
