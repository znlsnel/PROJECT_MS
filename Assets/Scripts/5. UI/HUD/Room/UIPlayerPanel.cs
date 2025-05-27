using System;
using FishNet;
using FishNet.Connection;
using FishNet.Object;
using FishNet.Object.Synchronizing;
using Steamworks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIPlayerPanel : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _playerName;
    [SerializeField] private Image _playerColor;

    public void UpdateUI(string name, Color color)
    {
        _playerColor.color = color;
        _playerName.text = name;
    }
}
