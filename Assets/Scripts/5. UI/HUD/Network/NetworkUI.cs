using System;
using FishNet;
using FishNet.Managing.Scened;
using Steamworks;
using TMPro;
using UnityEngine;

public class NetworkUI : MonoBehaviour
{
    [SerializeField] private TMP_InputField inputField;
    [SerializeField] private TextMeshProUGUI hostAddressText;
    [SerializeField] private GameObject lobbyPanel;
    [SerializeField] private GameObject createRoomPanel;
    [SerializeField] private GameObject roomPanel;
    [SerializeField] private GameObject testPanel;

    private void Start()
    {
        Managers.Steam.OnJoinLobby += OpenRoomPanel;

        if(Managers.Network.Type == NetworkType.TCP_UDP)
        {
            OpenTestPanel();
        }
        else
        {
            OpenLobbyPanel();
        }
    }

    public void OnClickHost()
    {
        Managers.Network.StartHost();
    }

    public void OnClickJoin()
    {
        bool isSuccess = false;

        if(inputField.text == "")
            isSuccess = Managers.Network.StartClient();
        else
            isSuccess = Managers.Network.StartClient(inputField.text);

        if(isSuccess)
        {
            OpenRoomPanel();
        }
        else
        {
            Debug.LogError("Failed to join lobby");
        }
    }

    public void OnClickStart()
    {
        NetworkSceneSystem.Instance?.LoadScene("Game");
    }

    public void OnClickLeave()
    {
        Managers.Network.StopClient();
    }

    public void OnClickCreateRoom()
    {
        OpenCreateRoomPanel();
    }

    public void OpenLobbyPanel()
    {
        ResetPanel();
        lobbyPanel.SetActive(true);
    }

    public void OpenCreateRoomPanel()
    {
        ResetPanel();
        createRoomPanel.SetActive(true);
    }

    public void OpenRoomPanel()
    {
        ResetPanel();
        roomPanel.SetActive(true);
    }

    public void OpenTestPanel()
    {
        ResetPanel();
        testPanel.SetActive(true);
    }

    private void ResetPanel()
    {
        lobbyPanel.SetActive(false);
        createRoomPanel.SetActive(false);
        roomPanel.SetActive(false);
        testPanel.SetActive(false);
    }
    
}
