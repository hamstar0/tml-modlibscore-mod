namespace ModLibsCore.Services.Network;

internal static class PacketIds {
	private static int nextId;

	public static int Increment() {
		return nextId++;
	}
}
