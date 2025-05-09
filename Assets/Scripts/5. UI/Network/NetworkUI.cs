using System;
using FishNet.Managing.Scened;
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
        SceneManagerEx.Instance.LoadScene("Demo");
    }
}
