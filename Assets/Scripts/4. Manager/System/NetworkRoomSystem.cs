using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using FishNet;
using FishNet.Connection;
using FishNet.Object;
using FishNet.Object.Synchronizing;
using FishNet.Transporting;
using Steamworks;
using UnityEngine;
using Random = UnityEngine.Random;

public class NetworkRoomSystem : NetworkSingleton<NetworkRoomSystem>
{
    public readonly SyncDictionary<NetworkConnection, ColorType> PlayerColors = new SyncDictionary<NetworkConnection, ColorType>();

    [SerializeField] private LobbyRoomUI _lobbyRoomUIPrefab;
    private LobbyRoomUI _lobbyRoomUI;

    // 모든 색상을 가져오는 프로퍼티
    private static ColorType[] AllColors => (ColorType[])Enum.GetValues(typeof(ColorType));

    public override void OnStartServer()
    {
        base.OnStartServer();
        // 서버에서 클라이언트 연결/해제 이벤트 구독
        base.ServerManager.OnRemoteConnectionState += OnRemoteConnectionState;
    }

    public override void OnStopServer()
    {
        base.OnStopServer();
        // 이벤트 구독 해제
        if (base.ServerManager != null)
            base.ServerManager.OnRemoteConnectionState -= OnRemoteConnectionState;
    }

    /// <summary>
    /// 원격 연결 상태가 변경될 때 호출됩니다.
    /// </summary>
    private void OnRemoteConnectionState(NetworkConnection conn, RemoteConnectionStateArgs args)
    {
        // 연결이 유효하지 않으면 처리하지 않음
        if (conn == null || !conn.IsValid)
        {
            Debug.LogWarning("NetworkRoomSystem: Invalid connection received");
            return;
        }

        switch(args.ConnectionState)
        {
            case RemoteConnectionState.Started:
                // 지연 호출을 통해 클라이언트가 완전히 초기화될 때까지 대기
                StartCoroutine(HandleClientJoinDelayed(conn));
            break;
            case RemoteConnectionState.Stopped:
                OnLeaveRoom(conn);
                DestroyRoomUI(conn);
                break;
        }
    }

    /// <summary>
    /// 클라이언트 연결을 지연 처리합니다.
    /// </summary>
    private IEnumerator HandleClientJoinDelayed(NetworkConnection conn)
    {
        // 연결이 완전히 활성화될 때까지 대기
        float timeout = 10f; // 10초 타임아웃
        float elapsed = 0f;
        
        while (!conn.IsActive && elapsed < timeout)
        {
            yield return new WaitForSeconds(0.1f);
            elapsed += 0.1f;
        }

        if (!conn.IsActive)
        {
            Debug.LogWarning($"NetworkRoomSystem: Connection {conn.ClientId} failed to activate within timeout");
            yield break;
        }

        // 서버가 시작되었고 유효한 연결인지 확인
        if (!base.IsServerStarted)
        {
            Debug.LogWarning("NetworkRoomSystem: Server is not started");
            yield break;
        }

        // Observer가 될 때까지 추가 대기
        elapsed = 0f;
        while (!base.Observers.Contains(conn) && elapsed < timeout)
        {
            yield return new WaitForSeconds(0.1f);
            elapsed += 0.1f;
        }

        if (!base.Observers.Contains(conn))
        {
            Debug.LogWarning($"NetworkRoomSystem: Connection {conn.ClientId} is not an observer");
            yield break;
        }

        // 이제 안전하게 RPC 호출
        OnJoinRoom(conn);
        CreateRoomUI(conn);
    }

    [ServerRpc(RequireOwnership = false)]
    public void ChangeColor(ColorType colorType, NetworkConnection connection = null)
    {
        if (!IsColorAvailable(colorType, connection))
        {
            Debug.LogWarning($"Color {colorType} is already in use!");
            return;
        }

        if(PlayerColors.ContainsKey(connection))
        {
            PlayerColors[connection] = colorType;
            UpdateUI();
        }
    }

    [ServerRpc(RequireOwnership = false)]
    public void OnJoinRoom(NetworkConnection conn = null)
    {
        ColorType assignedColor = GetAvailableRandomColor(conn);
        
        if (PlayerColors.ContainsKey(conn))
        {
            PlayerColors[conn] = assignedColor;
        }
        else
        {
            PlayerColors.Add(conn, assignedColor);
        }
        
        Debug.Log($"Player {conn.ClientId} assigned color: {assignedColor}");
    }

    /// <summary>
    /// 사용 가능한 랜덤 색상을 반환합니다.
    /// </summary>
    private ColorType GetAvailableRandomColor(NetworkConnection excludeConnection = null)
    {
        // 현재 사용 중인 색상들 가져오기 (excludeConnection 제외)
        var usedColors = PlayerColors
            .Where(kvp => kvp.Key != excludeConnection)
            .Select(kvp => kvp.Value)
            .ToHashSet();

        // 사용 가능한 색상들 필터링
        var availableColors = AllColors
            .Where(color => !usedColors.Contains(color))
            .ToArray();

        // 사용 가능한 색상이 없으면 첫 번째 색상 반환 (fallback)
        if (availableColors.Length == 0)
        {
            Debug.LogWarning("모든 색상이 사용 중입니다! 첫 번째 색상을 할당합니다.");
            return AllColors[0];
        }

        // 사용 가능한 색상 중에서 랜덤 선택
        int randomIndex = Random.Range(0, availableColors.Length);
        return availableColors[randomIndex];
    }

    /// <summary>
    /// 특정 색상이 사용 가능한지 확인합니다.
    /// </summary>
    private bool IsColorAvailable(ColorType color, NetworkConnection excludeConnection = null)
    {
        var usedColors = PlayerColors
            .Where(kvp => kvp.Key != excludeConnection)
            .Select(kvp => kvp.Value)
            .ToHashSet();

        return !usedColors.Contains(color);
    }

    /// <summary>
    /// 사용 가능한 모든 색상의 리스트를 반환합니다.
    /// </summary>
    public List<ColorType> GetAvailableColors(NetworkConnection excludeConnection = null)
    {
        var usedColors = PlayerColors
            .Where(kvp => kvp.Key != excludeConnection)
            .Select(kvp => kvp.Value)
            .ToHashSet();

        return AllColors
            .Where(color => !usedColors.Contains(color))
            .ToList();
    }

    /// <summary>
    /// 플레이어가 나갈 때 색상을 해제합니다.
    /// </summary>
    [ServerRpc(RequireOwnership = false)]
    public void OnLeaveRoom(NetworkConnection conn = null)
    {
        if (PlayerColors.ContainsKey(conn))
        {
            ColorType releasedColor = PlayerColors[conn];
            PlayerColors.Remove(conn);
            Debug.Log($"Player {conn.ClientId} left and released color: {releasedColor}");
        }
    }

    /// <summary>
    /// 클라이언트에서 사용 가능한 색상 목록을 요청합니다.
    /// </summary>
    [ServerRpc(RequireOwnership = false)]
    public void RequestAvailableColors(NetworkConnection conn = null)
    {
        List<ColorType> availableColors = GetAvailableColors(conn);
        TargetSendAvailableColors(conn, availableColors.ToArray());
    }

    /// <summary>
    /// 특정 클라이언트에게 사용 가능한 색상 목록을 전송합니다.
    /// </summary>
    [TargetRpc]
    private void TargetSendAvailableColors(NetworkConnection conn, ColorType[] availableColors)
    {
        // UI에서 사용할 수 있도록 이벤트 발생
        OnAvailableColorsReceived?.Invoke(availableColors);
    }

    /// <summary>
    /// 플레이어의 색상을 Unity Color로 반환합니다.
    /// </summary>
    /// <param name="connection">플레이어 연결</param>
    /// <returns>플레이어의 Unity Color</returns>
    public Color GetPlayerColor(NetworkConnection connection)
    {
        if (PlayerColors.TryGetValue(connection, out ColorType colorType))
        {
            return ColorSO.GetColor(colorType);
        }
        return Color.white; // 기본 색상
    }

    /// <summary>
    /// 사용 가능한 색상 목록을 받았을 때 발생하는 이벤트 (클라이언트용)
    /// </summary>
    public static event System.Action<ColorType[]> OnAvailableColorsReceived;


    #region Room UI

    [TargetRpc]
    private void CreateRoomUI(NetworkConnection conn)
    {
        _lobbyRoomUI = Instantiate(_lobbyRoomUIPrefab);
        UpdateUI();
    }

    [TargetRpc]
    private void DestroyRoomUI(NetworkConnection conn)
    {
        Destroy(_lobbyRoomUI.gameObject);
    }

    [ServerRpc(RequireOwnership = false)]
    private void UpdateUI()
    {
        ResetUI();

        if(Managers.Network.Type == NetworkType.Steam)
        {
            foreach(var connection in InstanceFinder.ServerManager.Clients)
            {
                ulong m_steamId = Managers.Steam.GetSteamId(connection.Value);

                if(m_steamId == 0) return;

                CSteamID steamId = new CSteamID(m_steamId);

                string name = SteamFriends.GetFriendPersonaName(steamId);
                Color color = NetworkRoomSystem.Instance.GetPlayerColor(connection.Value);

                CreateUI(name, color);
            }
        }
    }

    [ObserversRpc]
    private void ResetUI()
    {
        if(_lobbyRoomUI == null) return;
        
        _lobbyRoomUI.ResetUI();
    }

    [ObserversRpc]
    private void CreateUI(string name, Color color)
    {
        if(_lobbyRoomUI == null) return;

        _lobbyRoomUI.CreateUI(name, color);
    }
    #endregion
}
