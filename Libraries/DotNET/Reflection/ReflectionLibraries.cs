﻿using System;
using System.Reflection;
using ModLibsCore.Classes.Errors;
using ModLibsCore.Libraries.Debug;


namespace ModLibsCore.Libraries.DotNET.Reflection {
	/// <summary>
	/// Attribute for indicating reference parameters that must be expected to sometimes occur as null
	/// </summary>
	[AttributeUsage( AttributeTargets.All, AllowMultiple = false, Inherited = true )]
	public class NullableAttribute : Attribute { }




	/// <summary>
	/// Assorted static "helper" functions pertaining to reflection
	/// </summary>
	public partial class ReflectionLibraries {
		/*/// <summary>
		/// Defines a parameter (to be passed into RunMethod) to properly identify generic parameters.
		/// </summary>
		public class ReflectionParameter {
			/// <summary></summary>
			public object ParamData;
			/// <summary></summary>
			public Type GenericType;
		}*/



		/// <summary>
		/// Handy preset for accessing any member of the given name (regardless of access restrictions).
		/// </summary>
		public readonly static BindingFlags MostAccess = BindingFlags.Public |
			BindingFlags.NonPublic |
			BindingFlags.Instance |
			BindingFlags.Static;
	}
}
