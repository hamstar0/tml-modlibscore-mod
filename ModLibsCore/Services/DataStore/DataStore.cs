using System;
using System.Collections.Generic;
using System.Linq;
using Terraria.ModLoader;
using ModLibsCore.Classes.Errors;
using ModLibsCore.Libraries.Debug;
using ModLibsCore.Services.Debug.DataDumper;


namespace ModLibsCore.Services.DataStore {
	/// <summary>
	/// Supplies a simple, global-use, object-based key-value dictionary for anyone to use. Nothing more.
	/// </summary>
	public partial class DataStore {
		/// <summary>
		/// Indicates if data is stored with the given key.
		/// </summary>
		/// <param name="key"></param>
		/// <returns></returns>
		public static bool Has( object key ) {
			return ModContent.GetInstance<DataStore>().Data.ContainsKey( key );
		}

		/// <summary>
		/// Gets data stored with the given key, if found.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="key"></param>
		/// <param name="val"></param>
		/// <returns>`true` if found.</returns>
		public static bool Get<T>( object key, out T val ) {
			val = default(T);

			object rawVal = null;
			bool success = ModContent.GetInstance<DataStore>().Data.TryGetValue( key, out rawVal );

			if( !( rawVal is T ) ) {
				success = false;
			} else {
				val = (T)rawVal;
			}
			
			return success;
		}

		/// <summary>
		/// Sets data under a given key.
		/// </summary>
		/// <param name="key"></param>
		/// <param name="val"></param>
		public static void Set( object key, object val ) {
			ModContent.GetInstance<DataStore>().Data[ key ] = val;
		}

		/// <summary>
		/// Removes data at a given key.
		/// </summary>
		/// <param name="key"></param>
		/// <returns></returns>
		public static bool Remove( object key ) {
			return ModContent.GetInstance<DataStore>().Data.Remove( key );
		}


		////////////////
		
		internal static IDictionary<object, object> GetAll() {
			var ds = ModContent.GetInstance<DataStore>();
			IDictionary<object, object> clone;

			clone = ds.Data.ToDictionary( kv => kv.Key, kv => kv.Value );
			clone.Remove( DataDumper.MyDataStorekey );
			return clone;
		}


		////////////////

		/// <summary>
		/// Adds the given amount to any data stored under the given key, if applicable (numeric).
		/// </summary>
		/// <param name="key"></param>
		/// <param name="val"></param>
		public static bool Add( object key, double val ) {
			var ds = ModContent.GetInstance<DataStore>();

			if( !ds.Data.ContainsKey( key ) ) {
				ds.Data[key] = val;
			} else {
				Type dst = ds.Data[key].GetType();

				if( !dst.IsValueType || Type.GetTypeCode(dst) == TypeCode.Boolean ) {
					return false;
				}
				double amt = (double)ds.Data[key] + val;

				if( dst == typeof(double) ) {
					ds.Data[key] = amt;
				} else {
					ds.Data[key] = Convert.ChangeType( amt, dst );
				}
			}

			return true;
		}
	}
}
