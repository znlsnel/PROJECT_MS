using System;
using FishNet;
using FishNet.Connection;
using FishNet.Object;
using FishNet.Object.Synchronizing;
using FishNet.Transporting;
using UnityEngine;

public enum ColorType
{
    Black,
    Gray,
    White,
    Red,
    Orange,
    Yellow,
    LightGreen,
    Green,
    Turquoise,
    Blue,
    Indigo,
    Purple,
    Pink,
    Brown,
    Gold,
    Mint,
}

public class NetworkRoomSystem : NetworkSingleton<NetworkRoomSystem>
{
    public readonly SyncDictionary<NetworkConnection, ColorType> PlayerColors = new SyncDictionary<NetworkConnection, ColorType>();

    [ServerRpc(RequireOwnership = false)]
    public void ChangeColor(ColorType colorType, NetworkConnection connection = null)
    {
        if(PlayerColors.TryGetValue(connection, out ColorType prevColor))
        {
            prevColor = colorType;
            PlayerColors[connection] = prevColor;
        }
    }

    [ServerRpc(RequireOwnership = false)]
    public void OnJoinRoom(NetworkConnection conn = null)
    {
        
        PlayerColors.Add(conn, ColorType.Black);
    }
}
