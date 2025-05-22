using System.Collections;
using System.Collections.Generic;
using Steamworks;
using UnityEngine;

public class UILobby : MonoBehaviour
{
    [SerializeField] private UIRoomPanel roomPanelPrefab;
    [SerializeField] private Transform _roomListRoot;

    private List<UIRoomPanel> _roomPanels = new List<UIRoomPanel>();

    public void Start()
    {
        StartCoroutine(UpdateRoomListCoroutine());
    }

    private async void UpdateRoomList()
    {
        var lobbies = await Managers.Steam.RequestLobbyListAsync();

        Debug.Log("UpdateRoomList");

        ClearRoomList();

        if(lobbies == null) return;

        foreach (var lobby in lobbies)
        {
            var roomPanel = Instantiate(roomPanelPrefab, _roomListRoot);
            roomPanel.SetLobbyId(lobby.LobbyId.m_SteamID);
            roomPanel.UpdateUI(lobby);
            _roomPanels.Add(roomPanel);
        }
    }

    private void ClearRoomList()
    {
        foreach (var roomPanel in _roomPanels)
        {
            Destroy(roomPanel.gameObject);
        }
        _roomPanels.Clear();
    }

    private IEnumerator UpdateRoomListCoroutine()
    {
        while (gameObject.activeSelf)
        {
            UpdateRoomList();
            yield return new WaitForSeconds(10f);
        }
    }
}
