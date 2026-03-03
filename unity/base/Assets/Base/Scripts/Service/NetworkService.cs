using System;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Netcode;
using UnityEngine;

public class NetworkService : Service
{
    private Dictionary<Constants.EHeader, Action<FastBufferReader>> _callbacks = new();

    public void RegisterEvents(Constants.EHeader header, Action<FastBufferReader> callback)
    {
        if (_callbacks.TryGetValue(header, out var action) == false)
            _callbacks.Add(header, null);

        _callbacks[header] -= callback;
        _callbacks[header] += callback;
    }

    public void UnregisterEvents(Constants.EHeader header, Action<FastBufferReader> callback)
    {
        if (_callbacks.ContainsKey(header))
        {
            _callbacks[header] -= callback;
        }
    }

    public void SendToServer<T>(T message, NetworkDelivery networkDelivery) where T : unmanaged, INetworkMessage
    {
        using (var writer = new FastBufferWriter(FastBufferWriter.GetWriteSize(message), Allocator.Temp, 1024))
        {
            writer.WriteValue(message);

            var networkManager = NetworkManager.Singleton;
            var customMessagingManager = networkManager.CustomMessagingManager;
            customMessagingManager.SendUnnamedMessage(0, writer, networkDelivery);
        }
    }

    public void SendToClient<T>(ulong clientId, T message, NetworkDelivery networkDelivery) where T : unmanaged, INetworkMessage
    {
        using (var writer = new FastBufferWriter(FastBufferWriter.GetWriteSize(message), Allocator.Temp, 1024))
        {
            writer.WriteValue(message);

            var networkManager = NetworkManager.Singleton;
            var customMessagingManager = networkManager.CustomMessagingManager;
            customMessagingManager.SendUnnamedMessage(clientId, writer, networkDelivery);
        }
    }

    public void SendToAllClient<T>(T message, NetworkDelivery networkDelivery) where T : unmanaged, INetworkMessage
    {
        using (var writer = new FastBufferWriter(FastBufferWriter.GetWriteSize(message), Allocator.Temp, 1024))
        {
            writer.WriteValue(message);

            var networkManager = NetworkManager.Singleton;
            var customMessagingManager = networkManager.CustomMessagingManager;
            customMessagingManager.SendUnnamedMessageToAll(writer, networkDelivery);
        }
    }

    protected override void Awake()
    {
        base.Awake();

        var networkManager = NetworkManager.Singleton;
        var messagingManager = networkManager.CustomMessagingManager;
        messagingManager.OnUnnamedMessage += OnUnnamedMessage;
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();

        _callbacks.Clear();
        _callbacks = null;

        var networkManager = NetworkManager.Singleton;
        if (networkManager.IsNull())
            return;

        var messagingManager = networkManager.CustomMessagingManager;
        if (messagingManager.IsNull())
            return;

        messagingManager.OnUnnamedMessage -= OnUnnamedMessage;
    }

    private void OnUnnamedMessage(ulong clientId, FastBufferReader reader)
    {
        if (reader.TryGetValue(out DummyMessage message) == false)
            return;

        if (_callbacks.TryGetValue(message.EHeader, out var action) == false)
            return;

        action?.Invoke(reader);
    }
}
