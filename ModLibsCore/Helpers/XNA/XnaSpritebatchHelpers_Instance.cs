using System;
using System.Reflection;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using ModLibsCore.Libraries.Debug;
using ModLibsCore.Libraries.DotNET.Reflection;
using ModLibsCore.Classes.Loadable;


namespace ModLibsCore.Libraries.XNA {
	/// @private
	public partial class XNASpritebatchLibraries : ILoadable {
		private FieldInfo SpriteBatchBegunField = null;



		////////////////

		internal XNASpritebatchLibraries() {
			if( Main.dedServ || Main.netMode == NetmodeID.Server ) { return; }

			Type sbType = typeof(SpriteBatch);
			this.SpriteBatchBegunField = sbType.GetField( "inBeginEndPair", ReflectionLibraries.MostAccess );

			if( this.SpriteBatchBegunField == null ) {
				this.SpriteBatchBegunField = sbType.GetField( "_beginCalled", ReflectionLibraries.MostAccess );
			}
			if( this.SpriteBatchBegunField == null ) {
				this.SpriteBatchBegunField = sbType.GetField( "beginCalled", ReflectionLibraries.MostAccess );
			}
		}


		////////////////

		void ILoadable.OnModsLoad() { }

		void ILoadable.OnPostModsLoad() { }

		void ILoadable.OnModsUnload() { }
	}
}
