using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Reflection;
using Terraria.ModLoader;
using ModLibsCore.Classes.Errors;
using ModLibsCore.Classes.Loadable;
using ModLibsCore.Helpers.Debug;


namespace ModLibsCore.Helpers.DotNET.Reflection {
	/// <summary>
	/// Assorted static "helper" functions pertaining to reflection
	/// </summary>
	public partial class ReflectionHelpers : ILoadable {
		internal static ReflectionHelpers Instance => ModContent.GetInstance<ReflectionHelpers>();



		////////////////

		private IDictionary<string, Type> TypeMap = new ConcurrentDictionary<string, Type>();
		private IDictionary<string, Assembly> AssMap = new ConcurrentDictionary<string, Assembly>();
		private IDictionary<string, IDictionary<string, IList<Type>>> AssClassTypeMap = new ConcurrentDictionary<string, IDictionary<string, IList<Type>>>();
		private IDictionary<string, IDictionary<string, MemberInfo>> FieldPropMap = new ConcurrentDictionary<string, IDictionary<string, MemberInfo>>();



		////////////////
		
		internal ReflectionHelpers() { }

		void ILoadable.OnModsLoad() { }

		void ILoadable.OnPostModsLoad() { }

		void ILoadable.OnModsUnload() { }


		////////////////

		internal MemberInfo GetCachedInfoMember( Type classType, string fieldOrPropName ) {
			string className = classType.FullName;
			MemberInfo result;

			if( !this.FieldPropMap.ContainsKey( className ) ) {
				this.FieldPropMap[className] = new Dictionary<string, MemberInfo>();
			}

			if( !this.FieldPropMap[className].ContainsKey( fieldOrPropName ) ) {
				result = (MemberInfo)classType.GetField( fieldOrPropName, ReflectionHelpers.MostAccess );
				if( result == null ) {
					result = (MemberInfo)classType.GetProperty( fieldOrPropName, ReflectionHelpers.MostAccess );
				}

				this.FieldPropMap[className][fieldOrPropName] = result;
			}

			return this.FieldPropMap[className][fieldOrPropName];
		}
	}
}
