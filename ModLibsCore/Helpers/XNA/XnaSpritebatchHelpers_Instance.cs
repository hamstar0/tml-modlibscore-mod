using System;
using System.Reflection;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using ModLibsCore.Helpers.Debug;
using ModLibsCore.Helpers.DotNET.Reflection;
using ModLibsCore.Classes.Loadable;


namespace ModLibsCore.Helpers.XNA {
	/// @private
	public partial class XNASpritebatchHelpers : ILoadable {
		private FieldInfo SpriteBatchBegunField = null;



		////////////////

		internal XNASpritebatchHelpers() {
			if( Main.dedServ || Main.netMode == NetmodeID.Server ) { return; }

			Type sbType = typeof(SpriteBatch);
			this.SpriteBatchBegunField = sbType.GetField( "inBeginEndPair", ReflectionHelpers.MostAccess );

			if( this.SpriteBatchBegunField == null ) {
				this.SpriteBatchBegunField = sbType.GetField( "_beginCalled", ReflectionHelpers.MostAccess );
			}
			if( this.SpriteBatchBegunField == null ) {
				this.SpriteBatchBegunField = sbType.GetField( "beginCalled", ReflectionHelpers.MostAccess );
			}
		}


		////////////////

		void ILoadable.OnModsLoad() { }

		void ILoadable.OnPostModsLoad() { }

		void ILoadable.OnModsUnload() { }
	}
}
