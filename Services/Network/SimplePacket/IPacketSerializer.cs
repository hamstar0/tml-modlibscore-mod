using System;
using System.IO;
using System.Reflection;
using NetSerializer;
using Terraria.ModLoader;

namespace ModLibsCore.Services.Network.SimplePacket {
	public interface IPacketSerialzer<T> : ILoadable, IStaticTypeSerializer {
		void Serialize( Serializer serializer, Stream stream, T obj );

		void Deserialize( Serializer serializer, Stream stream, out T obj );

		// Default implementations:

		bool ITypeSerializer.Handles( Type type )
			=> type == typeof( T );

		void ILoadable.Load( Mod mod )
			=> SimplePacket.CustomSerializers.Add( this );

		void ILoadable.Unload()
			=> SimplePacket.CustomSerializers.Remove( this );

		MethodInfo IStaticTypeSerializer.GetStaticWriter( Type type )
			=> this.GetType().GetMethod( nameof( Serialize ) );

		MethodInfo IStaticTypeSerializer.GetStaticReader( Type type )
			=> this.GetType().GetMethod( nameof( Deserialize ) );
	}
}
