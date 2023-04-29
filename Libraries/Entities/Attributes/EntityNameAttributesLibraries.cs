using Terraria;
using Terraria.ID;
using ModLibsCore.Libraries.Items.Attributes;
using ModLibsCore.Libraries.NPCs.Attributes;
using ModLibsCore.Libraries.Projectiles.Attributes;


namespace ModLibsCore.Libraries.Entities.Attributes {
	/// <summary>
	/// Assorted static "helper" functions pertaining to `Entity`s (parent class of Item, NPC, Player, and Projectile).
	/// </summary>
	public class EntityNameAttributesLibraries {
		/// <summary>
		/// Gets the "qualified" name (the name the player sees) of a given entity.
		/// </summary>
		/// <param name="ent"></param>
		/// <returns></returns>
		public static string GetQualifiedName( Entity ent ) {
			if( ent is Item ) {
				return ItemNameAttributeLibraries.GetQualifiedName( (Item)ent );
			}
			if( ent is NPC ) {
				return NPCNameAttributeLibraries.GetQualifiedName( (NPC)ent );
			}
			if( ent is Projectile ) {
				return ProjectileNameAttributeLibraries.GetQualifiedName( (Projectile)ent );
			}
			if( ent is Player ) {
				return ( (Player)ent ).name;
			}
			return "...a "+ent.GetType().Name;
		}
	}
}
