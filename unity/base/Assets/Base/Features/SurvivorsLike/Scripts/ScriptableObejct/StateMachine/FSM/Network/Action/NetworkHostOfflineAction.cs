using System;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu(fileName = "NetworkHostOfflineAction", menuName = PATH + "Network/NetworkHostOfflineAction")]
public class NetworkHostOfflineAction : NetworkHostAction
{
    [SerializeField] private string _localhost = "127.0.0.1";
    [SerializeField] private ushort _port = 7777;

    public override void OnClientConnectedCallback(ulong obj)
    {
        base.OnClientConnectedCallback(obj);

        var networkManager = NetworkManager.Singleton;
        var sceneManager = networkManager.SceneManager;
        sceneManager.LoadScene("SurvivorsLikeBattle", LoadSceneMode.Single);
    }

    public override async Awaitable Enter()
    {
        OfflineMode = true;

        var sceneController = SceneController.Instance;
        var uIService = sceneController.GetService<UIService>();
        uIService.Open<UIProgressPanel>(false);

        var networkManager = NetworkManager.Singleton;
        var networkConfig = networkManager.NetworkConfig;
        var utp = (UnityTransport)networkConfig.NetworkTransport;
        utp.SetConnectionData(_localhost, _port);

        try
        {
            await base.Enter();
        }
        catch (Exception e)
        {
            Debug.LogError(e);
        }
    }

    protected override async void OnLoadEventCompleted(string sceneName, LoadSceneMode loadSceneMode, List<ulong> clientsCompleted, List<ulong> clientsTimedOut)
    {
        await Awaitable.NextFrameAsync();

        var sceneController = SceneController.Instance;
        var poolService = sceneController.GetService<NetworkPoolService>();
        var character = await poolService.LoadAsync<GameObject>($"Battle_{PlayerPrefs.GetString("CharacterName", "Archer")}");
        if (character.TryGetComponent<NetworkObject>(out var networkObject) == false)
            return;

        var networkManager = NetworkManager.Singleton;
        var spawnManager = networkManager.SpawnManager;
        var client = networkManager.LocalClient;
        var battleWorldService = sceneController.GetService<BattleWorldService>();
        var randomPos = battleWorldService.GetRandomPosition();
        spawnManager.InstantiateAndSpawn(networkPrefab: networkObject, ownerClientId: client.ClientId, destroyWithScene: true, position: randomPos);

        base.OnLoadEventCompleted(sceneName, loadSceneMode, clientsCompleted, clientsTimedOut);
    }
}
