using Microsoft.Xna.Framework;
using Terraria.ModLoader;
using ModLibsCore.Libraries.Items.Attributes;
using ModLibsCore.Libraries.TModLoader.Commands;


namespace ModLibsCore.Commands {
	/// @private
	public class GetItemIdByNameCommand : ModCommand {
		/// @private
		public override CommandType Type => CommandType.Chat;
		/// @private
		public override string Command => "ml-get-item-id";
		/// @private
		public override string Usage => "/" +this.Command+" \"Gold Pickaxe\"";
		/// @private
		public override string Description => "Gets an item's ID by name. Must be wrapped with quotes.";


		////////////////

		/// @private
		public override void Action( CommandCaller caller, string input, string[] args ) {
			var mymod = (ModLibsCoreMod)this.mod;

			if( args.Length == 0 ) {
				caller.Reply( "No arguments supplied.", Color.Red );
				return;
			}

			int _;
			string itemName;
			if( CommandsLibraries.GetQuotedStringFromArgsAt(args, 0, out _, out itemName) ) {
				if( !ItemAttributeLibraries.DisplayNamesToIds.ContainsKey(itemName) ) {
					throw new UsageException( "Invalid item type." );
				}

				caller.Reply( "Item ID for " + itemName + ": " + ItemAttributeLibraries.DisplayNamesToIds[itemName], Color.Lime );
			}
		}
	}
}
