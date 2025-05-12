using System;
using FishNet.Managing.Scened;
using Steamworks;
using UnityEngine;

public class NetworkUI : MonoBehaviour
{
    public void OnClickHost()
    {
        Managers.Network.CreateLobby();
    }

    public void OnClickStart()
    {
        NetworkSceneSystem.Instance?.LoadScene("Demo");
    }
}
