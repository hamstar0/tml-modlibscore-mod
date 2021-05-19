using System;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.ModLoader;
using ModLibsCore.Classes.UI.Elements;
using ModLibsCore.Helpers.Debug;
using ModLibsCore.Helpers.TModLoader.Menus;


namespace ModLibsCore.Internals.Menus.Support {
	/// @private
	internal partial class SupportInfoDisplay {
		private static bool CanDraw() {
			if( !Main.hasFocus ) { return false; }
			if( !Main.gameMenu ) { return false; }
			if( Main.spriteBatch == null ) {
				return false;
			}
			
			var mymod = ModLibsCoreMod.Instance;
			if( mymod == null || ModLibsConfig.Instance == null || Main.MenuUI == null ) {
				return false;
			}
			
			if( Main.MenuUI.CurrentState != null ) {
				Type uiType = Main.MenuUI.CurrentState.GetType();

				if( !Enum.TryParse(uiType.Name, out MenuUIDefinition menuDef) ) {
					return false;
				}
			}
			
			return true;
		}


		////////////////

		private static void _Draw( GameTime gt ) {
			try {
				if( !SupportInfoDisplay.CanDraw() ) {
					return;
				}

				var sid = ModContent.GetInstance<SupportInfoDisplay>();

				sid?.Update();
				sid?.Draw( Main.spriteBatch );
			} catch( Exception e ) {
				LogHelpers.LogOnce( e.ToString() );
			}
		}


		public void Draw( SpriteBatch sb ) {
			foreach( var elem in this.Elements.ToArray() ) {
				elem.Recalculate();
			}

			//var boxColor = new Color( 256, 0, 32 );
			//var boxEdgeColor = new Color( 255, 224, 224 );
			//float colorMul = 0.25f;
			float textColorMul = 1f;

			if( this.IsHoveringBox ) {
				//this.ExtendLabel.TextColor = Color.White;
				//colorMul = 0.3f;
			} else {
				if( !this.IsExtended ) {
					textColorMul = 0.8f;
				}
				//this.ExtendLabel.TextColor = AnimatedColors.Air.CurrentColor;
			}

			////var rect = new Rectangle( Main.screenWidth - 252, 4, 248, (this.IsExtended ? 104 : 40) );
			//var rect = this.GetInnerBox();
			//HUDHelpers.DrawBorderedRect( sb, boxColor * colorMul, boxEdgeColor * colorMul, rect, 4 );

			//if( this.SupportUrl != null ) {
			//	this.SupportUrl.Theme.UrlColor = Color.Lerp( UITheme.Vanilla.UrlColor, AnimatedColors.Ether.CurrentColor, 0.25f );
			//	this.SupportUrl.Theme.UrlLitColor = Color.Lerp( UITheme.Vanilla.UrlLitColor, AnimatedColors.Strobe.CurrentColor, 0.5f );
			//	this.SupportUrl.Theme.UrlLitColor = Color.Lerp( this.SupportUrl.Theme.UrlLitColor, AnimatedColors.DiscoFast.CurrentColor, 0.75f );
			//	this.SupportUrl.RefreshTheme();
			//}

			foreach( var elem in this.Elements.ToArray() ) {
				if( elem is UIWebUrlBasic ) { continue; }
				if( elem is UIText ) {
					if( elem == this.HeadLabel ) {
						((UIText)elem).TextColor = SupportInfoDisplay.HeaderLabelColor * textColorMul;
					//} else if( elem != this.ExtendLabel && elem != this.EnableModTagsLabel ) {
					//	((UIText)elem).TextColor = Color.White * textColorMul;
					}
				}
				elem.Draw( sb );
			}
			foreach( var elem in this.Elements.Reverse() ) {
				if( !( elem is UIWebUrlBasic ) ) { continue; }
				elem.Draw( sb );
			}
			
			Vector2 bonus = Main.DrawThickCursor( false );
			Main.DrawCursor( bonus, false );
		}
	}
}
