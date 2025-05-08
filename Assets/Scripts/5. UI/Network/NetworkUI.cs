using System;
using Steamworks;
using UnityEngine;

public class NetworkUI : MonoBehaviour
{
    public void OnClickHost()
    {
        SteamManagerEx.Instance.CreateLobby();
    }

    public void OnClickStart()
    {
        Managers.Scene.LoadScene("Demo");
    }
}
