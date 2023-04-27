using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;
using Terraria.UI;
using ModLibsCore.Libraries.Debug;
using ModLibsCore.Libraries.TModLoader;


namespace ModLibsCore {
	/// @private
	partial class ModLibsCoreModSystem : ModSystem {
		public override void ModifyInterfaceLayers( List<GameInterfaceLayer> layers )/* tModPorter Note: Removed. Use ModSystem.ModifyInterfaceLayers */ {
			int idx = layers.FindIndex( layer => layer.Name.Equals("Vanilla: Mouse Text") );
			if( idx == -1 ) { return; }

			//

			GameInterfaceDrawMethod internalCallback = () => {
				var loadLibs = ModContent.GetInstance<LoadLibraries>();
				if( loadLibs != null ) {
					loadLibs.IsLocalPlayerInGame_Hackish = true;  // Ugh!

					this.DrawDebug( Main.spriteBatch );
				}
				return true;
			};

			//

			var internalLayer = new LegacyGameInterfaceLayer( "ModLibsCore: Internal",
				internalCallback,
				InterfaceScaleType.UI );
			layers.Insert( 0, internalLayer );
		}
	}
}
