using System;
using System.Collections;
using System.Collections.Generic;
//using Mirror; 
using Steamworks;
using UnityEngine;

public class SteamManagerEx : Manager
{
    
    protected Callback<LobbyCreated_t> _lobbyCreated;
    protected Callback<GameLobbyJoinRequested_t> _gameLobbyJoinRequested;
    protected Callback<LobbyEnter_t> _lobbyEntered;
    protected Callback<LobbyMatchList_t> _lobbyList;
    
    public ELobbyType LobbyType = ELobbyType.k_ELobbyTypePublic;

    public CSteamID CurrentLobbyId { get; private set; } = CSteamID.Nil;

    private const string HostAddressKey = "HostAddress";

    protected override void Init()
    {
        if(!SteamManager.Initialized) return;

        _lobbyCreated = Callback<LobbyCreated_t>.Create(OnLobbyCreated);
        _gameLobbyJoinRequested = Callback<GameLobbyJoinRequested_t>.Create(OnGameLobbyJoinRequested);
        _lobbyEntered = Callback<LobbyEnter_t>.Create(OnLobbyEntered);
        _lobbyList = Callback<LobbyMatchList_t>.Create(OnLobbyList);
    }

    public override void Clear()
    {
         
    }

    public void HostLobby()
    {
        if(CurrentLobbyId != CSteamID.Nil)
        {
            SteamMatchmaking.LeaveLobby(CurrentLobbyId);
        }

        //SteamMatchmaking.CreateLobby(LobbyType, Managers.Network.maxConnections);
    }

    public void JoinLobby(CSteamID lobbyId)
    {
        if(CurrentLobbyId != CSteamID.Nil)
        {
            SteamMatchmaking.LeaveLobby(CurrentLobbyId);
        }

        SteamMatchmaking.JoinLobby(lobbyId);
    }

    public void LeaveLobby()
    {
        if(CurrentLobbyId != CSteamID.Nil)
        {
            SteamMatchmaking.LeaveLobby(CurrentLobbyId);
            CurrentLobbyId = CSteamID.Nil;

         //   Managers.Network.StopHost();
         //   Managers.Network.StopClient();
        }
    }

    private void OnLobbyCreated(LobbyCreated_t callback)
    {
        if(callback.m_eResult != EResult.k_EResultOK)
        {
            Debug.LogError("Lobby 생성 실패");
            return;
        }

       // Managers.Network.StartHost();

        CurrentLobbyId = new CSteamID(callback.m_ulSteamIDLobby);

        SteamMatchmaking.SetLobbyData(CurrentLobbyId, HostAddressKey, SteamUser.GetSteamID().ToString());
    }

    private void OnGameLobbyJoinRequested(GameLobbyJoinRequested_t callback)
    {
        SteamMatchmaking.JoinLobby(callback.m_steamIDLobby);
    }

    private void OnLobbyEntered(LobbyEnter_t callback)
    {
       // if(NetworkServer.active) return;

        string hostAddress = SteamMatchmaking.GetLobbyData(new CSteamID(callback.m_ulSteamIDLobby), HostAddressKey);

        CurrentLobbyId = new CSteamID(callback.m_ulSteamIDLobby);

     //   Managers.Network.networkAddress = hostAddress;
      //  Managers.Network.StartClient();
    }

    private void OnLobbyList(LobbyMatchList_t callback)
    {
        
    }
}
