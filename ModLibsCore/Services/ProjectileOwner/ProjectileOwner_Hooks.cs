using System;
using Terraria;
using Terraria.ModLoader;
using ModLibsCore.Classes.Loadable;


namespace ModLibsCore.Services.ProjectileOwner {
	/// <summary>
	/// Implements a method to know the 'owner' `Entity` of a given projectile.
	/// </summary>
	public partial class ProjectileOwner : ILoadable {
		/// <summary></summary>
		/// <param name="projectileWho"></param>
		/// <param name="isManuallySet"></param>
		public delegate void OnProjectileOwnerSet( int projectileWho, bool isManuallySet );



		////////////////

		/// <summary></summary>
		/// <param name="hook"></param>
		public static void AddOwnerSetHook( OnProjectileOwnerSet hook ) {
			var self = ModContent.GetInstance<ProjectileOwner>();

			self.Hooks.Add( hook );
		}


		////////////////

		private static void RunOwnerSetHooks( int projectileWho, bool isManuallySet ) {
			var self = ModContent.GetInstance<ProjectileOwner>();

			foreach( OnProjectileOwnerSet hook in self.Hooks ) {
				hook.Invoke( projectileWho, isManuallySet );
			}
		}
	}
}
