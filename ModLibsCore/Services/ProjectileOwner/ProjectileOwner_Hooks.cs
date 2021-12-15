using System;
using Terraria;
using Terraria.ModLoader;
using ModLibsCore.Classes.Loadable;


namespace ModLibsCore.Services.ProjectileOwner {
	/// <summary>
	/// Implements a method to know the 'owner' `Entity` of a given projectile.
	/// </summary>
	public partial class ProjectileOwner : ILoadable {
		public delegate void OnProjectileOwnerSet( Projectile projectile, bool isManuallySet );



		////////////////

		public static void AddOwnerSetHook( OnProjectileOwnerSet hook ) {
			var self = ModContent.GetInstance<ProjectileOwner>();

			self.Hooks.Add( hook );
		}


		////////////////

		private static void RunOwnerSetHooks( Projectile projectile, bool isManuallySet ) {
			var self = ModContent.GetInstance<ProjectileOwner>();

			foreach( OnProjectileOwnerSet hook in self.Hooks ) {
				hook.Invoke( projectile, isManuallySet );
			}
		}
	}
}
