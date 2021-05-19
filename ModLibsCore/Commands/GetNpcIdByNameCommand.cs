using Microsoft.Xna.Framework;
using Terraria.ModLoader;
using ModLibsCore.Helpers.NPCs.Attributes;
using ModLibsCore.Helpers.TModLoader.Commands;


namespace ModLibsCore.Commands {
	/// @private
	public class GetNpcIdByNameCommand : ModCommand {
		/// @private
		public override CommandType Type => CommandType.Chat;
		/// @private
		public override string Command => "ml-get-item-id";
		/// @private
		public override string Usage => "/" +this.Command+" \"Blue Slime\"";
		/// @private
		public override string Description => "Gets an NPC's ID by name. Must be wrapped with quotes.";


		////////////////

		/// @private
		public override void Action( CommandCaller caller, string input, string[] args ) {
			if( args.Length == 0 ) {
				caller.Reply( "No arguments supplied.", Color.Red );
				return;
			}

			int _;
			string itemName;
			if( CommandsHelpers.GetQuotedStringFromArgsAt(args, 0, out _, out itemName) ) {
				if( !NPCAttributeHelpers.DisplayNamesToIds.ContainsKey(itemName) ) {
					throw new UsageException( "Invalid item type." );
				}

				caller.Reply( "NPC ID for " + itemName + ": " + NPCAttributeHelpers.DisplayNamesToIds[itemName], Color.Lime );
			}
		}
	}
}
