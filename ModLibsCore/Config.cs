using System.ComponentModel;
using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.Config;


namespace ModLibsCore {
	/// <summary>
	/// Defines Mod Helpers config settings.
	/// </summary>
	[Label( "Mod Libs Core - Settings" )]
	public class ModLibsConfig : ModConfig {
		/// <summary>
		/// Gets the stack-merged singleton instance of this config file.
		/// </summary>
		public static ModLibsConfig Instance => ModContent.GetInstance<ModLibsConfig>();



		////////////////

		//public static string ConfigFileName => "Mod Helpers Config.json";

		/// @private
		public override ConfigScope Mode => ConfigScope.ServerSide;



		////////////////

		/// <summary>
		/// Outputs (to log) debug information relevant to specific Helpers functions (where applicable). Developers only.
		/// </summary>
		[Label( "Debug Mode - Helpers Info" )]
		[Tooltip( "Outputs (to log) debug information relevant to Helpers functions (where applicable). Developers only." )]
		public bool DebugModeHelpersInfo { get; set; } = false;

		/// <summary>
		/// Catches and logs unhandled exceptions (before crash).
		/// </summary>
		[Label( "Debug Mode - Unhandled Exception Logging" )]
		[Tooltip( "Catches and logs unhandled exceptions (before crash)." )]
		[DefaultValue( true )]
		public bool DebugModeUnhandledExceptionLogging { get; set; } = true;

		/// <summary>
		/// Allows users to invoke 'data dumps' (see DataDump service) on behalf of the server (without being the 'privileged' user).
		/// </summary>
		[Label( "Debug Mode - Also Server" )]
		[Tooltip( "Allows users to invoke 'data dumps' (see DataDump service) on behalf of the server (without being the 'privileged' user)." )]
		public bool DebugModeDumpAlsoServer { get; set; } = false;

		/// <summary>
		/// Disables logging of "silenced" exceptions.
		/// </summary>
		[Label( "Debug Mode - Silent Logging" )]
		[Tooltip( "Silences silent logging." )]
		public bool DebugModeDisableSilentLogging { get; set; } = false;

		/// <summary>
		/// Displays the current menu's ID in bottom right.
		/// </summary>
		[Label( "Debug Mode - Simple Packet Logging" )]
		[Tooltip( "Logs sent and received SimplePacket messages." )]
		public bool DebugModeNetInfo { get; set; } = false;

		/// <summary>
		/// Displays info at the mouse's location.
		/// </summary>
		[Label( "Debug Mode - Display info at the mouse's location" )]
		[Tooltip( "Displays info at the mouse's location" )]
		public bool DebugModeMouseInfo { get; set; } = false;


		////

		/// <summary>
		/// Save custom player data as (json) text.
		/// </summary>
		[Label( "Save custom player data as (json) text" )]
		[DefaultValue( false )]
		public bool CustomPlayerDataAsText { get; set; } = false;
	}
}
