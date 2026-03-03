using System;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
using UnityEngine;

public class LobbyManager : Singleton<LobbyManager>
{
    public event Action OnKickedFromLobby;
    public event Action OnLobbyDeleted;
    public event Action<ILobbyChanges> OnLobbyChanged;
    public Lobby Lobby { get; private set; }

    private LobbyEventCallbacks _lobbyEventCallbacks = null;
    private ILobbyEvents _lobbyEvents = null;
    private float _heartbeatTimer = 0;

    public void Clear()
    {
        Lobby = null;
        _lobbyEvents = null;
        _lobbyEventCallbacks.KickedFromLobby -= OnKickedFromLobby;
        _lobbyEventCallbacks.LobbyChanged -= OnChangedLobby;
        _lobbyEventCallbacks = null;
    }

    public bool IsHost(string playerID)
    {
        if (Lobby.IsNull())
            return false;

        return Lobby.HostId == playerID;
    }

    public Player GetPlayer(string playerID)
    {
        if (Lobby.IsNull())
            return null;

        var players = Lobby.Players;
        var player = players.Find(player => player.Id == playerID);
        return player;
    }

    public async Awaitable CreateLobby(string lobbyName, int maxPlayers, CreateLobbyOptions createLobbyOptions)
    {
        try
        {
            Lobby = await LobbyService.Instance.CreateLobbyAsync(lobbyName, maxPlayers, createLobbyOptions);

            _lobbyEventCallbacks = new LobbyEventCallbacks();
            _lobbyEventCallbacks.KickedFromLobby += OnKickedFromLobby;
            _lobbyEventCallbacks.LobbyChanged += OnChangedLobby;
            _lobbyEvents = await LobbyService.Instance.SubscribeToLobbyEventsAsync(Lobby.Id, _lobbyEventCallbacks);
        }
        catch (Exception e)
        {
            Debug.LogError(e);
        }
    }

    public async Awaitable JoinLobby(Lobby lobby, JoinLobbyByIdOptions joinLobbyByIdOptions)
    {
        try
        {
            Lobby = await LobbyService.Instance.JoinLobbyByIdAsync(lobby.Id, joinLobbyByIdOptions);

            _lobbyEventCallbacks = new LobbyEventCallbacks();
            _lobbyEventCallbacks.KickedFromLobby += OnKickedFromLobby;
            _lobbyEventCallbacks.LobbyChanged += OnChangedLobby;
            _lobbyEvents = await LobbyService.Instance.SubscribeToLobbyEventsAsync(Lobby.Id, _lobbyEventCallbacks);
        }
        catch (Exception e)
        {
            Debug.LogError(e);
        }
    }

    public async Awaitable UpdateLobbyOptions(string playerID, UpdateLobbyOptions lobbyOptions)
    {
        if (Lobby.IsNull())
            return;

        if (IsHost(playerID) == false)
            return;

        try
        {
            await LobbyService.Instance.UpdateLobbyAsync(Lobby.Id, lobbyOptions);
        }
        catch (Exception e)
        {
            Debug.LogError(e);
        }
    }

    public string GetPlayerData(string playerID, string key)
    {
        var player = GetPlayer(playerID);
        if (player.IsNull())
            return null;

        var playerData = player.Data;
        if (playerData.IsNull() || playerData.TryGetValue(key, out var playerDataObject) == false)
            return null;

        return playerDataObject.Value;
    }

    public async Awaitable UpdatePlayerData(string playerID, string key, string value)
    {
        if (Lobby.IsNull())
            return;

        var player = GetPlayer(playerID);
        if (player.IsNull())
            return;

        var playerData = player.Data;
        if (playerData.IsNull() || playerData.TryGetValue(key, out var playerDataObject) == false)
            return;

        playerDataObject.Value = value;

        try
        {
            await LobbyService.Instance.UpdatePlayerAsync(Lobby.Id, playerID, new UpdatePlayerOptions() { Data = playerData });
        }
        catch (Exception e)
        {
            Debug.LogError(e);
        }
    }

    public async Awaitable Heartbeat(string playerID, float heartbeatTime)
    {
        if (IsHost(playerID) == false)
            return;

        try
        {
            _heartbeatTimer += Time.deltaTime;
            if (_heartbeatTimer < heartbeatTime)
                return;

            _heartbeatTimer = 0;

            await LobbyService.Instance.SendHeartbeatPingAsync(Lobby.Id);
        }
        catch (Exception e)
        {
            Debug.LogError(e);
        }
    }

    public async Awaitable LeaveOrDelete(string playerID)
    {
        if (Lobby.IsNull())
            return;

        try
        {
            _lobbyEventCallbacks.KickedFromLobby -= OnKickedFromLobby;
            _lobbyEventCallbacks.LobbyChanged -= OnChangedLobby;
            await _lobbyEvents.UnsubscribeAsync();

            if (IsHost(playerID))
            {
                await LobbyService.Instance.DeleteLobbyAsync(Lobby.Id);
            }
            else
            {
                await LobbyService.Instance.RemovePlayerAsync(Lobby.Id, playerID);
            }
        }
        catch (LobbyServiceException e)
        {
            //  lobby not found ---> Unity.Services.Lobbies.Http.HttpException`1[Unity.Services.Lobbies.Models.ErrorStatus]: (404) HTTP/1.1 404 Not Found
            if (e.ErrorCode == 404)
            {
                Debug.LogError(e);
            }
        }
        catch (Exception e)
        {
            Debug.LogError(e);
        }
        finally
        {
            Lobby = null;
            _lobbyEvents = null;
            _lobbyEventCallbacks = null;
        }
    }

    private void OnChangedLobby(ILobbyChanges lobbyChanges)
    {
        if (lobbyChanges.LobbyDeleted)
        {
            OnLobbyDeleted?.Invoke();
        }
        else
        {
            lobbyChanges.ApplyToLobby(Lobby);

            OnLobbyChanged?.Invoke(lobbyChanges);
        }
    }
}
