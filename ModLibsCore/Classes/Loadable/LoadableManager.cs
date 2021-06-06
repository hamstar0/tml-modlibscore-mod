using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using Terraria;
using Terraria.ModLoader;
using ModLibsCore.Classes.Errors;
using ModLibsCore.Libraries.Debug;
using ModLibsCore.Libraries.DotNET;
using ModLibsCore.Libraries.TModLoader;


namespace ModLibsCore.Classes.Loadable {
	class LoadableManager {
		private ISet<ILoadable> Loadables = new HashSet<ILoadable>();
		private ISet<ISequencedLoadable> SequencedLoadables = new HashSet<ISequencedLoadable>();



		////////////////

		public LoadableManager() { }


		public void RegisterLoadables() {
			Type iLoadableType = typeof( ILoadable );
			Type iSeqLoadableType = typeof( ISequencedLoadable );
			IEnumerable<Assembly> asses = ModLoader.Mods
				.SafeSelect( mod => mod.Code )
				.SafeWhere( code => code != null );

			foreach( Assembly ass in asses ) {
				foreach( Type classType in ass.GetTypes() ) {
					try {
						if( !classType.IsClass || classType.IsAbstract ) {
							continue;
						}

						if( !iLoadableType.IsAssignableFrom(classType) && !iSeqLoadableType.IsAssignableFrom(classType) ) {
							continue;
						}

						var obj = TmlLibraries.SafelyGetInstanceForType( classType );

						if( obj is ILoadable ) {
							this.Loadables.Add( obj as ILoadable );
						} else if( obj is ISequencedLoadable ) {
							this.SequencedLoadables.Add( obj as ISequencedLoadable );
						}
					} catch { }
				}
			}
		}


		////////////////

		public void OnModsLoad() {
			foreach( ILoadable loadable in this.Loadables ) {
				loadable.OnModsLoad();
				//object _;
				//if( !ReflectionLibraries.RunMethod( loadable.GetType(), loadable, "OnModsLoad", new object[] { }, out _ ) ) {
				//	throw new ModLibsException( "Could not call OnModsLoad for "+loadable.GetType() );
				//}
			}

			this.OnModsLoad_Sequenced( this.Loadables );
		}

		private void OnModsLoad_Sequenced( IEnumerable<object> alreadyLoaded ) {
			var loading = new HashSet<ISequencedLoadable>( this.SequencedLoadables );
			var loaded = new HashSet<object>( alreadyLoaded );

			int attempts = 0;

			do {
				foreach( ISequencedLoadable loadable in loading.ToArray() ) {
					if( loadable.OnModsLoad(loaded) ) {
						loaded.Add( loadable );

						loading.Remove( loadable );
					}
				}

				attempts++;
			} while( loading.Count > 0 && attempts < 1000 );

			if( attempts >= 1000 ) {
				LogLibraries.Warn( "Retry attempts exceeded: "+(loading.Count)+" of "+(this.SequencedLoadables.Count)+" remain." );
			}
		}


		////

		public void OnPostModsLoad() {
			foreach( ILoadable loadable in this.Loadables ) {
				loadable.OnPostModsLoad();
			}

			this.OnPostModsLoad_Sequenced( this.Loadables );
		}

		private void OnPostModsLoad_Sequenced( IEnumerable<object> alreadyPostLoaded ) {
			var loading = new HashSet<ISequencedLoadable>( this.SequencedLoadables );
			var loaded = new HashSet<object>( alreadyPostLoaded );

			int attempts = 0;

			do {
				foreach( ISequencedLoadable loadable in loading.ToArray() ) {
					if( loadable.OnPostModsLoad(loaded) ) {
						loaded.Add( loadable );

						loading.Remove( loadable );
					}
				}

				attempts++;
			} while( loading.Count > 0 && attempts < 1000 );

			if( attempts >= 1000 ) {
				LogLibraries.Warn( "Retry attempts exceeded: "+(loading.Count)+" of "+(this.SequencedLoadables.Count)+" remain." );
			}
		}


		////

		public void OnModsUnload() {
			foreach( ILoadable loadable in this.Loadables ) {
				loadable.OnModsUnload();
			}

			this.OnModsUnload_Sequenced( this.Loadables );
		}

		private void OnModsUnload_Sequenced( IEnumerable<object> alreadyUnloaded ) {
			var unloading = new HashSet<ISequencedLoadable>( this.SequencedLoadables );
			var unloaded = new HashSet<object>( alreadyUnloaded );

			int attempts = 0;

			do {
				foreach( ISequencedLoadable loadable in unloading.ToArray() ) {
					if( loadable.OnModsUnload(unloaded) ) {
						unloaded.Add( loadable );

						unloading.Remove( loadable );
					}
				}

				attempts++;
			} while( unloading.Count > 0 && attempts < 1000 );

			if( attempts >= 1000 ) {
				LogLibraries.Warn( "Retry attempts exceeded: "+(unloading.Count)+" of "+(this.SequencedLoadables.Count)+" remain." );
			}
		}
	}
}
