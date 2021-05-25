using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;
using Terraria.UI;
using ModLibsCore.Libraries.Debug;
using ModLibsCore.Libraries.TModLoader;


namespace ModLibsCore {
	/// @private
	partial class ModLibsCoreMod : Mod {
		public override void ModifyInterfaceLayers( List<GameInterfaceLayer> layers ) {
			var loadLibs = ModContent.GetInstance<LoadLibraries>();
			if( loadLibs == null ) { return; }

			//

			int idx = layers.FindIndex( layer => layer.Name.Equals( "Vanilla: Mouse Text" ) );
			if( idx == -1 ) { return; }

			//

			GameInterfaceDrawMethod internalCallback = () => {
				if( loadLibs != null ) {
					loadLibs.IsLocalPlayerInGame_Hackish = true;  // Ugh!
				}
				return true;
			};

			GameInterfaceDrawMethod debugDrawCallback = () => {
				this.DrawDebug( Main.spriteBatch );
				return true;
			};

			//

			if( !loadLibs.IsLocalPlayerInGame_Hackish ) {
				var internalLayer = new LegacyGameInterfaceLayer( "ModLibsCore: Internal",
					internalCallback,
					InterfaceScaleType.UI );
				layers.Insert( 0, internalLayer );
			}

			if( loadLibs.IsLocalPlayerInGame_Hackish ) {
				var debugLayer = new LegacyGameInterfaceLayer( "ModLibsCore: Debug Display",
					debugDrawCallback,
					InterfaceScaleType.UI );
				layers.Insert( idx, debugLayer );
			}
		}
	}
}
