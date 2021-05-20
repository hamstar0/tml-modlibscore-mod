using System;
using System.Collections.Generic;
using System.Reflection;
using Terraria;
using Terraria.ModLoader;
using ModLibsCore.Classes.Errors;
using ModLibsCore.Libraries.Debug;
using ModLibsCore.Libraries.DotNET;
using ModLibsCore.Libraries.TModLoader;


namespace ModLibsCore.Classes.Loadable {
	class LoadableManager {
		private ISet<ILoadable> Loadables = new HashSet<ILoadable>();



		////////////////

		public LoadableManager() { }


		public void OnModsLoad() {
			Type iLoadableType = typeof( ILoadable );
			IEnumerable<Assembly> asses = ModLoader.Mods
				.SafeSelect( mod => mod.Code )
				.SafeWhere( code => code != null );

			foreach( Assembly ass in asses ) {
				foreach( Type classType in ass.GetTypes() ) {
					try {
						if( !classType.IsClass || classType.IsAbstract ) {
							continue;
						}

						if( !typeof(ILoadable).IsAssignableFrom(classType) ) {
							continue;
						}

						var loadable = TmlLibraries.SafelyGetInstanceForType(classType) as ILoadable;
						if( loadable == null ) {
							continue;
						}

						this.Loadables.Add( loadable );
					} catch { }
				}
			}
			
			foreach( ILoadable loadable in this.Loadables ) {
				loadable.OnModsLoad();
				//object _;
				//if( !ReflectionLibraries.RunMethod( loadable.GetType(), loadable, "OnModsLoad", new object[] { }, out _ ) ) {
				//	throw new ModLibsException( "Could not call OnModsLoad for "+loadable.GetType() );
				//}
			}
		}


		public void OnPostModsLoad() {
			foreach( ILoadable loadable in this.Loadables ) {
				loadable.OnPostModsLoad();
			}
		}

		public void OnModsUnload() {
			foreach( ILoadable loadable in this.Loadables ) {
				loadable.OnModsUnload();
			}
		}
	}
}
