using System;
using Microsoft.Xna.Framework;
using Terraria;


namespace ModLibsCore.Libraries.Projectiles.Attributes {
	/// <summary>
	/// Assorted static "helper" functions pertaining to projectile attributes.
	/// </summary>
	public partial class ProjectileNameAttributeLibraries {
		/// <summary>
		/// Gets the "qualified" (human readable) name of a given projectile.
		/// </summary>
		/// <param name="proj"></param>
		/// <returns></returns>
		public static string GetQualifiedName( Projectile proj ) {
			return ProjectileNameAttributeLibraries.GetQualifiedName( proj.type );
		}

		/// <summary>
		/// Gets the "qualified" (human readable) name of a given projectile.
		/// </summary>
		/// <param name="projType"></param>
		/// <returns></returns>
		public static string GetQualifiedName( int projType ) {
			string name = Lang.GetProjectileName( projType ).Value;
			return name;
		}
	}
}
