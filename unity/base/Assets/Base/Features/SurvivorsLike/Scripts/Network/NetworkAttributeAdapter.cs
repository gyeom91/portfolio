using System;
using CycloneGames.GameplayAbilities.Runtime;
using Unity.Netcode;
using UnityEngine;

public class NetworkAttributeAdapter : BaseNetworkBehaviour
{
    public event Action<NetworkListEvent<NetworkAttributeData>, NetworkList<NetworkAttributeData>> OnChangedNetworkAttributeList;

    private NetworkList<NetworkAttributeData> _networkAttributeDatas = new();

    public void SetAttributeValue(string attributeName, GameplayAttribute gameplayAttribute)
    {
        if (IsServer == false)
            return;

        var newAttributeData = new NetworkAttributeData();
        newAttributeData.AttributeName = attributeName;

        var index = _networkAttributeDatas.IndexOf(newAttributeData);
        if (index == -1)
        {
            newAttributeData.BaseValue = gameplayAttribute.BaseValue;
            newAttributeData.CurrentValue = gameplayAttribute.CurrentValue;
            _networkAttributeDatas.Add(newAttributeData);
        }
        else
        {
            var networkAttributeData = _networkAttributeDatas[index];
            networkAttributeData.BaseValue = gameplayAttribute.BaseValue;
            networkAttributeData.CurrentValue = gameplayAttribute.CurrentValue;

            _networkAttributeDatas[index] = networkAttributeData;
        }
    }

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();

        _networkAttributeDatas.OnListChanged += OnChangedNetworkList;
    }

    public override void OnNetworkDespawn()
    {
        base.OnNetworkDespawn();

        _networkAttributeDatas.OnListChanged -= OnChangedNetworkList;
    }

    protected override void Awake()
    {
        base.Awake();

        _networkAttributeDatas.Initialize(this);
    }

    private void OnChangedNetworkList(NetworkListEvent<NetworkAttributeData> changeEvent)
    {
        OnChangedNetworkAttributeList?.Invoke(changeEvent, _networkAttributeDatas);
    }
}
