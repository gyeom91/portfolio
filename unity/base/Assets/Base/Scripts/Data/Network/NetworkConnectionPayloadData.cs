using System.Collections.Generic;
using UnityEngine;

public class NetworkConnectionPayloadData
{
    public int Count { get { return _networkConnectionPayloadList.Count; } }

    private Dictionary<ulong, NetworkConnectionPayload> _networkConnectionPayloadList = new();

    public void Add(ulong clientID, NetworkConnectionPayload networkConnectionPayload)
    {
        if (_networkConnectionPayloadList.ContainsKey(clientID))
            return;

        _networkConnectionPayloadList.Add(clientID, networkConnectionPayload);
    }

    public NetworkConnectionPayload Get(ulong clientID)
    {
        if (_networkConnectionPayloadList.ContainsKey(clientID) == false)
            return default(NetworkConnectionPayload);

        return _networkConnectionPayloadList[clientID];
    }
}
