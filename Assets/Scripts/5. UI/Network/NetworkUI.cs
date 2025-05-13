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

    public void OnClickHost()
    {
        Managers.Network.StartHost();
    }

    public void OnClickJoin()
    {
        if(inputField.text == "")
            Managers.Network.StartClient();
        else
            Managers.Network.StartClient(inputField.text);
    }

    public void OnClickStart()
    {
        NetworkSceneSystem.Instance?.LoadScene("Game");
    }
}
