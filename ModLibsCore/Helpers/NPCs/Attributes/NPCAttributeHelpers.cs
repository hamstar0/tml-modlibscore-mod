using System;
using Terraria;
using Terraria.ModLoader;
using ModLibsCore.Classes.DataStructures;


namespace ModLibsCore.Helpers.NPCs.Attributes {
	/// <summary>
	/// Assorted static "helper" functions pertaining to gameplay attributes of NPCs.
	/// </summary>
	public partial class NPCAttributeHelpers {
		/// <summary>
		/// Table of NPC ids by qualified names.
		/// </summary>
		public static ReadOnlyDictionaryOfSets<string, int> DisplayNamesToIds =>
			ModContent.GetInstance<NPCAttributeHelpers>()._DisplayNamesToIds;



		////////////////

		/// <summary>
		/// Gets an NPC's qualified (human readable) name.
		/// </summary>
		/// <param name="npc"></param>
		/// <returns></returns>
		public static string GetQualifiedName( NPC npc ) {
			return Lang.GetNPCNameValue( npc.netID );
		}

		/// <summary>
		/// Gets an NPC's qualified (human readable) name.
		/// </summary>
		/// <param name="netid"></param>
		/// <returns></returns>
		public static string GetQualifiedName( int netid ) {    //npcType?
			return Lang.GetNPCNameValue( netid );
		}
	}
}
