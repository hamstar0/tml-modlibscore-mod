﻿using System;
using System.Collections.Generic;
using System.Reflection;
using Terraria.ModLoader;
using Terraria.ModLoader.Core;
using ModLibsCore.Classes.Errors;
using ModLibsCore.Libraries.Debug;
using ModLibsCore.Libraries.DotNET.Reflection;


namespace ModLibsCore.Services.TML {
	/// <summary>
	/// Supplies a way to peek into other mods' build properties (build.txt) data.
	/// </summary>
	public partial class BuildPropertiesViewer {
		private static Type GetBuildPropertiesClassType() {
			Type myClass;
			if( DataStore.DataStore.Get( "BuildPropertiesClass", out myClass ) ) {
				return myClass;
			}

			try {
				Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();

				foreach( var ass in assemblies ) {
					IEnumerable<Type> types = ReflectionLibraries.GetTypesFromAssembly( ass, "BuildProperties" );

					foreach( Type t in types ) {
						if( t.IsClass && t.Namespace == "Terraria.ModLoader.Core" && t.Name == "BuildProperties" ) {
							DataStore.DataStore.Set( "BuildPropertiesClass", t );
							return t;
						}
					}
				}
			} catch( Exception e ) {
				LogLibraries.Warn( e.ToString() );
				return (Type)null;
			}
			
			DataStore.DataStore.Set( "BuildPropertiesClass", null );
			return null;
		}


		/// <summary>
		/// Produces a 'viewer' object for a TmodFile mod file's build.txt data.
		/// </summary>
		/// <param name="modfile"></param>
		/// <returns></returns>
		public static BuildPropertiesViewer GetBuildPropertiesForModFile( TmodFile modfile ) {
			Type buildPropType = BuildPropertiesViewer.GetBuildPropertiesClassType();
			if( buildPropType == null ) {
				LogLibraries.Alert( $"Could not get `Type` of build properties classes (eventually to get {modfile.Name} props)" );
				return (BuildPropertiesViewer)null;
			}

			MethodInfo method = buildPropType.GetMethod( "ReadModFile", BindingFlags.NonPublic | BindingFlags.Static );
			if( method == null ) {
				LogLibraries.Alert( $"Could not get ReadModFile method of build properties class for {modfile.Name}" );
				return (BuildPropertiesViewer)null;
			}

			object buildProps = method.Invoke( null, new object[] { modfile } );
			if( buildProps == null ) {
				LogLibraries.Log( "BuildProperties has changed." );
				return (BuildPropertiesViewer)null;
			}

			return new BuildPropertiesViewer( buildProps );
		}


		/// <summary>
		/// Produces a 'viewer' object for an active mod file's build.txt data.
		/// </summary>
		/// <param name="modName"></param>
		/// <returns></returns>
		public static BuildPropertiesViewer GetBuildPropertiesForActiveMod( string modName ) {
			Mod mod = ModLoader.GetMod( modName );
			if( mod == null ) {
				return null;
			}

			TmodFile modFile;
			if( !ReflectionLibraries.Get( mod, "File", out modFile ) || modFile == null ) {
				return null;
			}

			return BuildPropertiesViewer.GetBuildPropertiesForModFile( modFile );
		}
	}
}
