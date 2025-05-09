using System;
using FishNet.Managing.Scened;
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
        Managers.Scene.LoadScene("Demo");
    }
}
