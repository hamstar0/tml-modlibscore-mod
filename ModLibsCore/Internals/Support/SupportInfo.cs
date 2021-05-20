using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.UI;
using ModLibsCore.Classes.Loadable;
using ModLibsCore.Classes.UI.Elements;
using ModLibsCore.Libraries.Debug;
using ModLibsCore.Services.Hooks.LoadHooks;


namespace ModLibsCore.Internals.Menus.Support {
	/// @private
	internal partial class SupportInfoDisplay : ILoadable {
		public static Color HeaderLabelColor = Color.Lerp( Color.White, Color.Gold, 0.25f );



		////////////////

		private float RowHeight;
		private float Scale;

		private UIText HeadLabel;
		private UIWebUrlBasic HeadUrl;

		private IList<UIElement> Elements = new List<UIElement>();

		private bool IsHoveringBox = false;

		private bool IsClicking = false;

		private float Width;

		private bool IsExtended = false;



		////////////////

		internal SupportInfoDisplay( float width = 248f, float yBeg = 8f, float rowHeight = 30f, float scale = 0.8f ) {
			if( Main.dedServ ) { return; }

			var mymod = ModLibsCoreMod.Instance;
			float y = yBeg;
			float row = 0;
			this.Width = width;

			this.RowHeight = rowHeight;
			this.Scale = scale;

			////

			this.HeadLabel = new UIText( "Powered by:", 1.1f * scale );
			this.HeadLabel.Left.Set( -width, 1f );
			this.HeadLabel.Top.Set( (4f + y) * scale, 0f );
			this.HeadLabel.TextColor = SupportInfoDisplay.HeaderLabelColor;
			this.HeadLabel.Recalculate();

			this.HeadUrl = new UIWebUrlBasic( "Mod Libs v" + mymod.Version.ToString(), "https://forums.terraria.org/index.php?threads/.63670/", true, 1.1f * scale );
			this.HeadUrl.Left.Set( -( width - ( 114f * scale ) ), 1f );
			this.HeadUrl.Top.Set( (4f + y) * scale, 0f );
			this.HeadUrl.Recalculate();

			//this.ExtendLabel = new UIThemedText( UITheme.Vanilla, false, "..." );
			//this.ExtendLabel.Left.Set( -(width * 0.5f) - 16f, 1f );
			//this.ExtendLabel.Top.Set( (-14f + y + rowHeight) * scale, 0f );
			//this.ExtendLabel.Recalculate();

			////

			this.Elements.Add( this.HeadLabel );
			this.Elements.Add( this.HeadUrl );

			Main.OnPostDraw += SupportInfoDisplay._Draw;
		}

		void ILoadable.OnModsLoad() { }

		void ILoadable.OnPostModsLoad() {
			// This is deferred to here because of LoadHooks load order (ironically)
			LoadHooks.AddModUnloadHook( () => {
				try {
					Main.OnPostDraw -= SupportInfoDisplay._Draw;
				} catch { }
			} );
		}

		void ILoadable.OnModsUnload() { }


		////////////////

		private void ExpandUI() {
			//this.Elements.Add( this.ModderLabel );
			//this.Elements.Add( this.ModderUrl );

			//this.ModderLabel.Recalculate();
			//this.ModderUrl.Recalculate();
		}


		////////////////

		public Rectangle GetInnerBox() {
			return new Rectangle(
				Main.screenWidth - (int)this.Width - 4,
				4,
				(int)this.Width,
				(this.IsExtended ? 74 : 32 )   //104:40
			);
		}
	}
}
