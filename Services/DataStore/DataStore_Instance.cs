using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Terraria.ModLoader;
using ModLibsCore.Classes.Errors;
using ModLibsCore.Libraries.Debug;


namespace ModLibsCore.Services.DataStore {
	/// @private
	public partial class DataStore : ModSystem {
		private IDictionary<object, (bool, object)> Data = new Dictionary<object, (bool, object)>();



		////////////////

		internal DataStore() { }

		////////////////

		/// <summary></summary>
		public string Serialize() {
			return JsonConvert.SerializeObject( DataStore.GetAll(), Formatting.Indented );
		}
	}
}
