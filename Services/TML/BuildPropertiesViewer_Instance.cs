﻿using System;
using System.Collections.Generic;
using System.Reflection;
using Terraria.ModLoader;
using ModLibsCore.Classes.Errors;
using ModLibsCore.Libraries.Debug;
using ModLibsCore.Libraries.DotNET.Reflection;


namespace ModLibsCore.Services.TML {
	/// <summary>
	/// Supplies a way to peek into other mods' build properties (build.txt) data.
	/// </summary>
	public partial class BuildPropertiesViewer : ModSystem {
		/// <summary></summary>
		public string[] DllReferences => (string[])this.GetField( "dllReferences" );
		//public string[] ModReferences => ((object[])this.GetField( "modReferences" )).Select( m=>m.ToString() ).ToArray();
		//public string[] WeakReferences => ((object[])this.GetField( "weakReferences" ) ).Select( m=>m.ToString() ).ToArray();

		/// <summary></summary>
		public string[] SortAfter => (string[])this.GetField( "sortAfter" );
		/// <summary></summary>
		public string[] SortBefore => (string[])this.GetField( "sortBefore" );
		/// <summary></summary>
		public string[] BuildIgnores => (string[])this.GetField( "buildIgnores" );
		/// <summary></summary>
		public string Author => (string)this.GetField( "author" );
		/// <summary></summary>
		public Version Version => (Version)this.GetField( "version" );
		/// <summary></summary>
		public string DisplayName => (string)this.GetField( "displayName" );
		/// <summary></summary>
		public bool NoCompile => (bool)this.GetField( "noCompile" );
		/// <summary></summary>
		public bool HideCode => (bool)this.GetField( "hideCode" );
		/// <summary></summary>
		public bool HideResources => (bool)this.GetField( "hideResources" );
		/// <summary></summary>
		public bool IncludeSource => (bool)this.GetField( "includeSource" );
		/// <summary></summary>
		public bool IncludePDB => (bool)this.GetField( "includePDB" );
		/// <summary></summary>
		public bool EditAndContinue => (bool)this.GetField( "editAndContinue" );
		/// <summary></summary>
		public bool Beta => (bool)this.GetField( "beta" );
		/// <summary></summary>
		public int LanguageVersion => (int)this.GetField( "languageVersion" );
		/// <summary></summary>
		public string Homepage => (string)this.GetField( "homepage" );
		/// <summary></summary>
		public string Description => (string)this.GetField( "description" );
		/// <summary></summary>
		public ModSide Side => (ModSide)this.GetField( "side" );

		/// <summary></summary>
		public IDictionary<string, Version> ModReferences {
			get {
				var modRefsRaw = (object)this.GetField( "modReferences" );
				int length;

				if( !ReflectionLibraries.Get( modRefsRaw, "Length", out length ) ) {
					throw new ModLibsException( "Invalid modReferences" );
				}
				
				var dict = new Dictionary<string, Version>( length );
				string name;
				Version vers;
				
				for( int i=0; i<length; i++ ) {
					object modRef;
					if( !ReflectionLibraries.RunMethod( modRefsRaw, "GetValue", new object[] { i }, out modRef ) ) {
						throw new ModLibsException( "Invalid modReference array value " + i );
					}

					if( !ReflectionLibraries.Get( modRef, "mod", out name ) ) { continue; }
					if( !ReflectionLibraries.Get( modRef, "target", out vers ) ) { continue; }
					dict[ name ] = vers;
				}
				return dict;
			}
		}

		/// <summary></summary>
		public IDictionary<string, Version> WeakReferences {
			get {
				var modRefsRaw = (object)this.GetField( "modReferences" );
				int length;

				if( !ReflectionLibraries.Get( modRefsRaw, "Length", out length ) ) {
					throw new ModLibsException( "Invalid modReferences" );
				}

				var dict = new Dictionary<string, Version>( length );
				string name;
				Version vers;

				for( int i = 0; i < length; i++ ) {
					object modRef;
					if( !ReflectionLibraries.RunMethod( modRefsRaw, "GetValue", new object[] { i }, out modRef ) ) {
						throw new ModLibsException( "Invalid modReference array value " + i );
					}

					if( !ReflectionLibraries.Get( modRef, "mod", out name ) ) { continue; }
					if( !ReflectionLibraries.Get( modRef, "target", out vers ) ) { continue; }
					dict[name] = vers;
				}
				return dict;
			}
		}

		////////////////

		internal object BuildProps;



		////////////////

		private BuildPropertiesViewer() { }

		internal BuildPropertiesViewer( object buildProps ) {
			this.BuildProps = buildProps;
		}

		////////////////

		/// <summary>
		/// Gets the value of a given build.txt field.
		/// </summary>
		/// <param name="propName"></param>
		/// <returns></returns>
		public object GetField( string propName ) {
			Type modPropsType = this.BuildProps.GetType();
			FieldInfo fieldInfo = modPropsType.GetField( propName, BindingFlags.NonPublic | BindingFlags.Instance );
			if( fieldInfo == null ) { return null; }

			return fieldInfo.GetValue( this.BuildProps );
		}
	}
}
