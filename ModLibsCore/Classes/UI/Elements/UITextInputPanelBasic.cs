using System;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameInput;
using Terraria.UI;
using Terraria.GameContent.UI.Elements;
using ModLibsCore.Services.Timers;


namespace ModLibsCore.Classes.UI.Elements {
	/// @private
	class UITextInputPanelBasic : UIPanel {
		public delegate bool TextEventHandler( StringBuilder input );
		public delegate void FocusHandler();



		////////////////

		public event TextEventHandler OnTextChange;
		public event FocusHandler OnUnfocus;


		////////////////

		public Color TextColor;

		public bool IsHidden { get; protected set; }

		public float Opacity { get; set; } = 1f;


		////////////////

		private string Text = "";
		private uint CursorAnimation;
		private bool IsSelected = false;


		////////////////

		public string HintText { get; private set; }

		public bool IsInteractive { get; private set; } = true;



		////////////////

		public UITextInputPanelBasic( string hintText ) {
			this.HintText = hintText;

			this.SetPadding( 6f );
		}


		////////////////

		public string GetText() {
			return this.Text;
		}

		public void SetText( string text ) {
			this.Text = text;
		}


		////////////////

		public void Enable() {
			this.IsInteractive = true;
		}

		public void Disable() {
			this.IsInteractive = false;
		}


		////////////////

		public void Hide() {
			this.IsHidden = true;
		}

		public void Show() {
			this.IsHidden = false;
		}


		////////////////

		private void UpdateInteractivity( CalculatedStyle dim ) {
			// Detect if user selects this element
			if( Main.mouseLeft ) {
				bool isNowSelected = false;

				if( Main.mouseX >= dim.X && Main.mouseX < ( dim.X + dim.Width ) ) {
					if( Main.mouseY >= dim.Y && Main.mouseY < ( dim.Y + dim.Height ) ) {
						isNowSelected = true;
						Main.keyCount = 0;
					}
				}

				if( this.IsSelected && !isNowSelected ) {
					Timers.RunNow( () => {
						this.OnUnfocus?.Invoke();
					} );
				}
				this.IsSelected = isNowSelected;
			}

			// Apply text inputs
			if( this.IsSelected ) {
				PlayerInput.WritingText = true;
				Main.instance.HandleIME();

				string newStr = Main.GetInputText( this.Text );

				if( !newStr.Equals( this.Text ) ) {
					var newStrMuta = new StringBuilder( newStr );

					Timers.RunNow( () => {
						if( this.OnTextChange?.Invoke( newStrMuta ) ?? true ) {
							this.Text = newStrMuta.ToString();
						}
					} );
				}
			}
		}


		////////////////

		/// @private
		public override void Draw( SpriteBatch spriteBatch ) {
			if( this.IsHidden ) {
				return;
			}

			float opacity = this.Opacity;

			Color oldBg = this.BackgroundColor;
			Color oldBord = this.BorderColor;

			this.BackgroundColor *= opacity;
			this.BorderColor *= opacity;

			base.Draw( spriteBatch );

			this.BackgroundColor = oldBg;
			this.BorderColor = oldBord;
		}


		////////////////

		protected override void DrawSelf( SpriteBatch sb ) {
			/*float opacity = this.ComputeCurrentOpacity();
			Color oldTextColor = this.TextColor;
			
			this.TextColor.R = (byte)((float)this.TextColor.R * opacity);
			this.TextColor.G = (byte)((float)this.TextColor.G * opacity);
			this.TextColor.B = (byte)((float)this.TextColor.B * opacity);
			this.TextColor.A = (byte)((float)this.TextColor.A * opacity);
			*/
			base.DrawSelf( sb );

			CalculatedStyle dim = this.GetDimensions();

			if( this.IsInteractive ) {
				this.UpdateInteractivity( dim );
			}

			this.DrawText( dim, sb );
		}


		private void DrawText( CalculatedStyle dim, SpriteBatch sb ) {
			var pos = new Vector2( dim.X + this.PaddingLeft, dim.Y + this.PaddingTop );

			// Draw text
			if( this.Text.Length == 0 ) {
				Utils.DrawBorderString( sb, this.HintText, pos, Color.Gray, 1f );
			} else {
				string displayStr = this.Text;

				// Draw cursor
				if( this.IsSelected ) {
					if( ++this.CursorAnimation % 40 < 20 ) {
						displayStr = displayStr + "|";
					}
				}

				Utils.DrawBorderString( sb, displayStr, pos, this.TextColor, 1f );
			}

			//this.TextColor = oldTextColor;
		}
	}
}
