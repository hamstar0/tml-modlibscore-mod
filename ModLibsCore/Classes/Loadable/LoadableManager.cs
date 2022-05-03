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

						//

						object obj = TmlLibraries.SafelyGetInstanceForType( classType );

						if( obj is ILoadable ) {
							this.Loadables.Add( obj as ILoadable );
						} else if( obj is ISequencedLoadable ) {
							this.SequencedLoadables.Add( obj as ISequencedLoadable );
						}
					} catch {
						LogLibraries.Warn( $"Error loading ILoadable {classType.Name}" );
					}
				}
			}
		}


		////////////////

		public void OnModsLoad() {
			this.OnModsLoad_Unsequenced();
			this.OnModsLoad_Sequenced();
		}

		private void OnModsLoad_Unsequenced() {
			foreach( ILoadable loadable in this.Loadables ) {
				loadable.OnModsLoad();
				//object _;
				//if( !ReflectionLibraries.RunMethod( loadable.GetType(), loadable, "OnModsLoad", new object[] { }, out _ ) ) {
				//	throw new ModLibsException( "Could not call OnModsLoad for "+loadable.GetType() );
				//}
			}
		}

		private void OnModsLoad_Sequenced() {
			var loading = new HashSet<ISequencedLoadable>( this.SequencedLoadables );
			var loaded = new HashSet<object>( this.Loadables );

			int attempts = 0;

			do {
				foreach( ISequencedLoadable loadable in loading.ToArray() ) {
					if( loadable.OnModsLoad(loaded) ) {
						loading.Remove( loadable );
						loaded.Add( loadable );
					}
				}

				attempts++;
			} while( loading.Count > 0 && attempts < 1000 );

			if( attempts >= 1000 ) {
				LogLibraries.Warn( "Retry attempts exceeded: "+(loading.Count)+" of "+(this.SequencedLoadables.Count)+" loaded." );
			}
		}


		////

		public void OnPostModsLoad() {
			this.OnPostModsLoad_Unsequenced();
			this.OnPostModsLoad_Sequenced();
		}

		private void OnPostModsLoad_Unsequenced() {
			foreach( ILoadable loadable in this.Loadables ) {
				loadable.OnPostModsLoad();
			}
		}

		private void OnPostModsLoad_Sequenced() {
			var loading = new HashSet<ISequencedLoadable>( this.SequencedLoadables );
			var loaded = new HashSet<object>( this.Loadables );

			int attempts = 0;

			do {
				foreach( ISequencedLoadable loadable in loading.ToArray() ) {
					if( loadable.OnPostModsLoad(loaded) ) {
						loading.Remove( loadable );
						loaded.Add( loadable );
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
			this.OnModsUnload_Unsequenced();
			this.OnModsUnload_Sequenced();
		}

		private void OnModsUnload_Unsequenced() {
			foreach( ILoadable loadable in this.Loadables ) {
				loadable.OnModsUnload();
			}
		}

		private void OnModsUnload_Sequenced() {
			var unloading = new HashSet<ISequencedLoadable>( this.SequencedLoadables );
			var unloaded = new HashSet<object>( this.Loadables );

			int attempts = 0;

			do {
				foreach( ISequencedLoadable loadable in unloading.ToArray() ) {
					if( loadable.OnModsUnload(unloaded) ) {
						unloading.Remove( loadable );
						unloaded.Add( loadable );
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
