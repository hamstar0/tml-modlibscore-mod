using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent;
using Terraria.GameContent.UI.Elements;
using Terraria.UI;
using ModLibsCore.Libraries.DotNET;
using ModLibsCore.Libraries.Debug;
using ModLibsCore.Services.Timers;


namespace ModLibsCore.Classes.UI.Elements {
	/// @private
	class UIWebUrlBasic : UIElement {
		public static Color Unvisited = new Color( 80, 80, 255 );
		public static Color UnvisitedLit = new Color( 128, 128, 255 );
		public static Color Visited = new Color( 192, 0, 255 );



		////////////////

		public static UIText GetLineElement( string label, float scale, bool large ) {
			float underscoreLen = FontAssets.MouseText.Value.MeasureString("_").X;
			float textLen = FontAssets.MouseText.Value.MeasureString( label ).X;
			int lineLen = (int)Math.Max( 1f, Math.Round(textLen / (underscoreLen - 2)) );

			return new UIText( new String('_', lineLen), scale, large );
		}



		////////////////
		
		public UIText TextElem { get; private set; }
		public UIText LineElem { get; private set; }

		public string Url { get; private set; }
		public bool WillDrawOwnHoverUrl { get; private set; }

		public bool IsVisited { get; private set; }

		public float Scale { get; private set; }
		public bool Large { get; private set; }



		////////////////

		public UIWebUrlBasic( string label, string url, bool hoverUrl = true, float scale = 0.85f, bool large = false ) {
			this.IsVisited = false;
			this.Url = url;
			this.WillDrawOwnHoverUrl = hoverUrl;
			this.Scale = scale;
			this.Large = large;

			this.TextElem = new UIText( label, scale, large );
			this.TextElem.TextColor = UIWebUrlBasic.Unvisited;
			this.Append( this.TextElem );

			this.LineElem = UIWebUrlBasic.GetLineElement( label, scale, large );
			this.LineElem.TextColor = UIWebUrlBasic.Unvisited;
			this.Append( this.LineElem );

			CalculatedStyle labelSize = this.TextElem.GetDimensions();
			this.Width.Set( labelSize.Width, 0f );
			this.Height.Set( labelSize.Height, 0f );

			UIText textElem = this.TextElem;
			UIText lineElem = this.LineElem;

			this.OnMouseOver += delegate ( UIMouseEvent evt, UIElement fromElem ) {
				if( textElem.TextColor != UIWebUrlBasic.Visited ) {
					textElem.TextColor = UIWebUrlBasic.UnvisitedLit;
					textElem.TextColor = UIWebUrlBasic.UnvisitedLit;
				}
			};
			this.OnMouseOut += delegate ( UIMouseEvent evt, UIElement fromElem ) {
				if( textElem.TextColor != UIWebUrlBasic.Visited ) {
					textElem.TextColor = UIWebUrlBasic.Unvisited;
					textElem.TextColor = UIWebUrlBasic.Unvisited;
				}
			};

			this.OnClick += delegate ( UIMouseEvent evt, UIElement fromElem ) {
				try {
					SystemLibraries.OpenUrl( this.Url );
					//System.Diagnostics.Process.Start( this.Url );

					this.IsVisited = true;

					textElem.TextColor = UIWebUrlBasic.Visited;
					lineElem.TextColor = UIWebUrlBasic.Visited;
				} catch( Exception e ) {
					Main.NewText( e.Message );
				}
			};

			this.RefreshTheme();
		}


		////////////////

		public void SetText( string text ) {
			this.TextElem.SetText( text, this.Scale, this.Large );

			Timers.RunNow( () => {
				this.RemoveChild( this.LineElem );
				this.LineElem.Remove();
			} );

			this.LineElem = UIWebUrlBasic.GetLineElement( text, this.Scale, this.Large );
			this.Append( this.LineElem );

			this.Recalculate();
		}


		////////////////

		public override void Draw( SpriteBatch sb ) {
			base.Draw( sb );
			
			if( this.TextElem.IsMouseHovering || this.IsMouseHovering ) {
				if( this.WillDrawOwnHoverUrl ) {
					this.DrawHoverEffects( sb );
				}
			}
		}

		public void DrawHoverEffects( SpriteBatch sb ) {
			if( !string.IsNullOrEmpty(this.Url) ) {
				Vector2 dim = FontAssets.MouseText.Value.MeasureString( this.Url );
				Vector2 pos = new Vector2( Main.mouseX + 10f, Main.mouseY + 10f );

				if( ( pos.X + dim.X ) > Main.screenWidth ) {
					pos.X = Main.screenWidth - dim.X;
				}

				Utils.DrawBorderStringFourWay( sb, FontAssets.MouseText.Value, this.Url, pos.X, pos.Y, Color.White, Color.Black, default(Vector2) );
				//sb.DrawString( Main.fontMouseText, this.Url, UILibraries.GetHoverTipPosition( this.Url ), Color.White );
			}
		}


		////////////////

		public void RefreshTheme() {
			if( this.IsVisited ) {
				this.TextElem.TextColor = UIWebUrlBasic.Visited;
				this.LineElem.TextColor = UIWebUrlBasic.Visited;
			} else {
				if( this.IsMouseHovering ) {
					this.TextElem.TextColor = UIWebUrlBasic.UnvisitedLit;
					this.LineElem.TextColor = UIWebUrlBasic.UnvisitedLit;
				} else {
					this.TextElem.TextColor = UIWebUrlBasic.Unvisited;
					this.LineElem.TextColor = UIWebUrlBasic.Unvisited;
				}
			}
		}
	}
}
