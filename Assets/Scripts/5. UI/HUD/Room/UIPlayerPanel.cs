using FishNet.Object;
using Steamworks;
using TMPro;
using UnityEngine;

public class UIPlayerPanel : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _playerName;

    public void UpdateUI(ulong steamId)
    {
        _playerName.text = SteamFriends.GetFriendPersonaName(new CSteamID(steamId));
    }
}
