using System.Collections.Generic;
using FishNet;
using Steamworks;
using UnityEngine;
using UnityEngine.UI;

public class UIRoom : MonoBehaviour
{
    [SerializeField] private UIPlayerPanel playerPanelPrefab;
    [SerializeField] private Transform playerListRoot;
    [SerializeField] private GameObject _startButton;

    private List<UIPlayerPanel> _playerPanels = new List<UIPlayerPanel>();

    public void OnEnable()
    {
        Managers.Network.OnClientConnected += UpdateUI;

        UpdateUI();
    }

    public void OnDisable()
    {
        Managers.Network.OnClientConnected -= UpdateUI;
    }

    private void CreatePlayerInfoPanel(ulong steamId)
    {
        var roomPanel = Instantiate(playerPanelPrefab, playerListRoot);
        roomPanel.UpdateUI(steamId);
        _playerPanels.Add(roomPanel);
    }
    
    public void UpdateUI()
    {
        ResetUI();

        var memberList = Managers.Steam.RequestLobbyMemberList();

        foreach(var member in memberList)
        {
            CreatePlayerInfoPanel(member.m_SteamID);
        }

        _startButton.SetActive(InstanceFinder.IsServerStarted);
    }

    public void ResetUI()
    {
        foreach(var playerPanel in _playerPanels)
        {
            Destroy(playerPanel.gameObject);
        }

        _playerPanels.Clear();
    }
}
