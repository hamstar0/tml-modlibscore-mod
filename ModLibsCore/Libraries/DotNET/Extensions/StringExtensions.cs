using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;


namespace ModLibsCore.Libraries.DotNET.Extensions {
	/// <summary>
	/// Extensions for strings.
	/// </summary>
	public static class StringExtensions {
		/// <summary>
		/// Safely truncates an input string.
		/// </summary>
		/// <param name="value"></param>
		/// <param name="maxLength"></param>
		/// <returns></returns>
		public static string Trunc( this string value, int maxLength ) {
			return value?.Substring( 0, Math.Min( value.Length, maxLength ) );
		}
	}




	/// <summary>
	/// Extensions for Enumerable collections to output as a string.
	/// </summary>
	public static class EnumerableExtensions {
		/// <summary>
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		public static string ToStringJoined<T>( this IEnumerable<T> collection, string delim ) {
			return string.Join( delim, collection );
		}
	}




	/// <summary>
	/// Extensions for Vector2 to output shorter strings.
	/// </summary>
	public static class Vector2StringExtensions {
		/// <summary>
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		public static string ToShortString( this Vector2 value ) {
			return "{"+value.X.ToString("N0")+", "+value.Y.ToString("N0")+"}";
		}
	}
}
