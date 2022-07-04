using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;
using ModLibsCore.Classes.DataStructures;


namespace ModLibsCore.Libraries.Projectiles.Attributes {
	/// <summary>
	/// Assorted static "helper" functions pertaining to gameplay attributes of NPCs.
	/// </summary>
	public partial class ProjectileNameAttributeLibraries : ILoadable {
		/// <summary>
		/// Table of NPC ids by qualified names.
		/// </summary>
		public static ReadOnlyDictionaryOfSets<string, int> DisplayNamesToIds =>
			ModContent.GetInstance<ProjectileNameAttributeLibraries>()._DisplayNamesToIds;



		////////////////

		private ReadOnlyDictionaryOfSets<string, int> _DisplayNamesToIds = null;


		////////////////

		internal ProjectileNameAttributeLibraries() { }

		void ILoadable.Load( Mod mod ) { }

		void ILoadable.Unload() { }


		////////////////

		internal void PopulateNames() {
			var dict = new Dictionary<string, ISet<int>>();

			for( int i = 1; i < ProjectileLoader.ProjectileCount; i++ ) {
				string name = ProjectileNameAttributeLibraries.GetQualifiedName( i );

				if( dict.ContainsKey( name ) ) {
					dict[name].Add( i );
				} else {
					dict[name] = new HashSet<int>() { i };
				}
			}

			this._DisplayNamesToIds = new ReadOnlyDictionaryOfSets<string, int>( dict );
		}
	}
}
