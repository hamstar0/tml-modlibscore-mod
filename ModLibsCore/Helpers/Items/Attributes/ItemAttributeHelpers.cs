using Terraria;
using Terraria.ModLoader;
using ModLibsCore.Classes.DataStructures;


namespace ModLibsCore.Helpers.Items.Attributes {
	/// <summary>
	/// Assorted static "helper" functions pertaining to gameplay attributes of items.
	/// </summary>
	public partial class ItemAttributeHelpers {
		/// <summary>
		/// Table of item ids by qualified names.
		/// </summary>
		public static ReadOnlyDictionaryOfSets<string, int> DisplayNamesToIds {
			get {
				return ModContent.GetInstance<ItemAttributeHelpers>()._DisplayNamesToIds;
			}
		}



		////////////////

		/// <summary>
		/// Gets an item's qualified (human readable) name.
		/// </summary>
		/// <param name="item"></param>
		/// <returns></returns>
		public static string GetQualifiedName( Item item ) {
			return Lang.GetItemNameValue( item.type );  // not netID?
		}

		/// <summary>
		/// Gets an item's qualified (human readable) name.
		/// </summary>
		/// <param name="itemType"></param>
		/// <returns></returns>
		public static string GetQualifiedName( int itemType ) {
			return Lang.GetItemNameValue( itemType );
		}
	}
}
