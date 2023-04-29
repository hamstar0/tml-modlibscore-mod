using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using ModLibsCore.Libraries.Debug;
using ModLibsCore.Libraries.Players;


namespace ModLibsCore.Commands {
	/// @private
	public class MyUserIdCommand : ModCommand {
		/// @private
		public override CommandType Type => CommandType.Chat;
		/// @private
		public override string Command => "ml-my-userid";
		/// @private
		public override string Usage => "/" + this.Command;
		/// @private
		public override string Description => "Displays your user id.";


		////////////////

		/// @private
		public override void Action( CommandCaller caller, string input, string[] args ) {
			string uid = PlayerIdentityLibraries.GetUniqueId();
			
			caller.Reply( "Your user ID is (also see log file): " + uid, Color.Lime );
			LogLibraries.Log( "Your user ID is: "+uid );
		}
	}
}
