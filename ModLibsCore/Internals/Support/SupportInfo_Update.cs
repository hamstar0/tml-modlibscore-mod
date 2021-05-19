using System;
using Terraria;
using Terraria.UI;
using Terraria.GameContent.UI.Elements;
using ModLibsCore.Classes.UI.Elements;
using ModLibsCore.Helpers.Debug;


namespace ModLibsCore.Internals.Menus.Support {
	/// @private
	internal partial class SupportInfoDisplay {
		public void Update() {
			if( !Main.hasFocus ) {
				return;
			}

			bool isClicking = Main.mouseLeft && !this.IsClicking;

			this.IsClicking = Main.mouseLeft;
			this.IsHoveringBox = this.GetInnerBox()
				.Contains( Main.mouseX, Main.mouseY );

			this.UpdateElementMouseInteractions( isClicking );

			if( isClicking && this.IsHoveringBox ) {
				if( !this.IsExtended ) {
					//this.IsExtended = true;

					//this.ExtendLabel.Remove();
					//this.Elements.Remove( this.ExtendLabel );
					//this.ExpandUI();
				}
			}
		}


		private void UpdateElementMouseInteractions( bool isClicking ) {
			for( int i = 0; i < this.Elements.Count; i++ ) {
				var elem = this.Elements[i];

				if( !(elem is UIWebUrlBasic) ) {
					if( elem is UIText && ((UIText)elem).Text != "..." ) {
						continue;
					}
					if( elem.GetType().Name != "UIImageUrl" ) {
						continue;
					}
				}

				this.UpdateElementMouseInteraction( elem, isClicking );
			}
		}


		private void UpdateElementMouseInteraction( UIElement elem, bool isClicking ) {
			bool isElementHover = elem.GetOuterDimensions()
				.ToRectangle()
				.Contains( Main.mouseX, Main.mouseY );

			if( isElementHover ) {
				if( isClicking ) { elem.Click( null ); }
				elem.MouseOver( null );
			} else {
				if( elem.IsMouseHovering ) {
					elem.MouseOut( null );
				}
			}
		}
	}
}
