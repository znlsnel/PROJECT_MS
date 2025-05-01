using System;
using Steamworks;
using UnityEngine;

public class NetworkUI : MonoBehaviour
{
    public void OnClickHost()
    {
        NetworkManagerEx.Instance.CreateLobby();
    }

    public void OnClickStart()
    {
        NetworkSystem.Instance.OnStartGameButtonClicked();
    }
}
