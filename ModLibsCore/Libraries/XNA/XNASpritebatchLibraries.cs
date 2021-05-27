using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using ModLibsCore.Libraries.Debug;


namespace ModLibsCore.Libraries.XNA {
	/// <summary>
	/// Assorted static "helper" functions pertaining to XNA. 
	/// </summary>
	public partial class XNASpritebatchLibraries {
		/// <summary>
		/// Reports if the given SpriteBatch has begun drawing.
		/// </summary>
		/// <param name="sb"></param>
		/// <param name="isBegun"></param>
		/// <returns>If `false`, could not determine one way or the other.</returns>
		public static bool IsSpriteBatchBegun( SpriteBatch sb, out bool isBegun ) {
			var xna = ModContent.GetInstance<XNASpritebatchLibraries>();
			object isBegunRaw = xna?.SpriteBatchBegunField?.GetValue( sb );

			if( isBegunRaw != null ) {
				isBegun = (bool)isBegunRaw;
				return true;
			} else {
				isBegun = false;
				return false;
			}
		}

		/// <summary>
		/// Reports if `Main.SpriteBatch` has begun drawing.
		/// </summary>
		/// <param name="isBegun"></param>
		/// <returns>If `false`, could not determine one way or the other.</returns>
		public static bool IsMainSpriteBatchBegun( out bool isBegun ) {
			return XNASpritebatchLibraries.IsSpriteBatchBegun( Main.spriteBatch, out isBegun );
		}


		////////////////

		/// <summary>
		/// Draws to the main SpriteBatch by way of a callback. Attempts to resolve when to draw based on the SpriteBatch's
		/// `Begun()` state.
		/// </summary>
		/// <param name="draw"></param>
		/// <param name="isBegun">Indicates that the SpriteBatch was already `Begun()`.</param>
		/// <param name="forceDraw">Forces drawing even when the SpriteBatch is already `Begun()`.</param>
		/// <returns>`true` if no issues occurred with the drawing.</returns>
		public static bool DrawBatch( Action<SpriteBatch> draw, out bool isBegun, bool forceDraw=true ) {
			if( !XNASpritebatchLibraries.IsMainSpriteBatchBegun( out isBegun ) ) {
				return false; // take no chances
			}

			if( !isBegun ) {
				Main.spriteBatch.Begin();

				try {
					draw( Main.spriteBatch );
				} catch( Exception e ) {
					LogLibraries.WarnOnce( e.ToString() );
				}
				
				Main.spriteBatch.End();
			} else {
				if( forceDraw ) {
					LogLibraries.WarnOnce( DebugLibraries.GetCurrentContext( 2 ) + " - SpriteBatch already begun. Drawing anyway..." );
					try {
						draw( Main.spriteBatch );
					} catch( Exception e ) {
						LogLibraries.WarnOnce( e.ToString() );
					}
				}
			}
			
			return true;
		}


		/// <summary>
		/// Draws to the main SpriteBatch by way of a callback. Attempts to resolve when to draw based on the SpriteBatch's
		/// `Begun()` state. If the SpriteBatch needs to be begun anew, the given set of relevant parameters will be applied.
		/// </summary>
		/// <param name="draw"></param>
		/// <param name="sortMode"></param>
		/// <param name="blendState"></param>
		/// <param name="samplerState"></param>
		/// <param name="depthStencilState"></param>
		/// <param name="rasterizerState"></param>
		/// <param name="effect"></param>
		/// <param name="transformMatrix"></param>
		/// <param name="isBegun">Indicates that the SpriteBatch was already `Begun()`.</param>
		/// <param name="forceBeginAnew">Forces the SpriteBatch to `Begin()`.</param>
		/// <param name="forceDraw">Forces drawing even wehn the SpriteBatch is already `Begun()`.</param>
		/// <returns></returns>
		public static bool DrawBatch( Action<SpriteBatch> draw,
				SpriteSortMode sortMode,
				BlendState blendState,
				SamplerState samplerState,
				DepthStencilState depthStencilState,
				RasterizerState rasterizerState,
				Effect effect,
				Matrix transformMatrix,
				out bool isBegun,
				bool forceBeginAnew = false,
				bool forceDraw = true ) {
			if( !XNASpritebatchLibraries.IsMainSpriteBatchBegun( out isBegun ) ) {
				return false; // take no chances
			}

			if( isBegun && forceBeginAnew ) {
				isBegun = false;
				Main.spriteBatch.End();
			}

			if( !isBegun ) {
				Main.spriteBatch.Begin( sortMode, blendState, samplerState, depthStencilState, rasterizerState, effect, transformMatrix );

				try {
					draw( Main.spriteBatch );
				} catch( Exception e ) {
					LogLibraries.WarnOnce( e.ToString() );
				}

				Main.spriteBatch.End();
			} else {
				if( forceDraw ) {
					LogLibraries.WarnOnce( DebugLibraries.GetCurrentContext( 2 ) + " - SpriteBatch already begun. Drawing anyway..." );
					try {
						draw( Main.spriteBatch );
					} catch( Exception e ) {
						LogLibraries.WarnOnce( e.ToString() );
					}
				}
			}

			return true;
		}
	}
}
