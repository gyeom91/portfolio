using Unity.Netcode;
using UnityEngine;

public static class FastBufferReaderExtension
{
    public static bool TryGetValue<T>(this FastBufferReader reader, out T value) where T : unmanaged, INetworkMessage
    {
        if (reader.TryBeginReadValue(default(T)) == false)
        {
            value = default;
            return false;
        }

        reader.ReadValue(out value);
        reader.Seek(0);

        return true;
    }
}
