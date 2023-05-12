using System;
using System.IO;
using Terraria.ModLoader;

namespace ModLibsCore.Services.Network.ManualPackets;

public abstract class ManualPacket : IDisposable {
	public int Id { get; internal set; }

	protected BinaryWriter Writer { get; private set; }

	private MemoryStream stream;

	protected ManualPacket() {
		if( !ManualPacketSystem.TryGetPacket( this.GetType(), out var basePacket ) ) {
			throw new InvalidOperationException( $"Unable to acquire id of packet {this.GetType().Name}!" );
		}

		this.Id = basePacket.Id;
		this.Writer = new BinaryWriter( this.stream = new MemoryStream() );
	}

	public abstract void Read( BinaryReader reader, int sender );

	public void WriteAndDispose( BinaryWriter writer ) {
		writer.Write( this.stream.ToArray() );

		this.Dispose();
	}

	public void Dispose() {
		GC.SuppressFinalize( this );

		this.Writer?.Dispose();
		this.stream?.Dispose();

		this.Writer = null!;
		this.stream = null!;
	}
}
