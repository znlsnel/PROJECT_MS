using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using FishNet;
using FishNet.Connection;
using FishNet.Managing.Scened;
using FishNet.Object;
using FishNet.Object.Synchronizing;
using FishNet.Transporting;
using Steamworks;
using UnityEngine;
using Random = UnityEngine.Random;

public class NetworkRoomSystem : NetworkSingleton<NetworkRoomSystem>
{
    public readonly SyncDictionary<NetworkConnection, ColorType> PlayerColors = new SyncDictionary<NetworkConnection, ColorType>();
    public readonly SyncDictionary<NetworkConnection, ulong> NetworkConnectionToSteamId = new SyncDictionary<NetworkConnection, ulong>();
    [SerializeField] private LobbyRoomUI _lobbyRoomUIPrefab;
    public LobbyRoomUI _lobbyRoomUI;

    // 모든 색상을 가져오는 프로퍼티
    private static ColorType[] AllColors => (ColorType[])Enum.GetValues(typeof(ColorType));

    public override void OnStartNetwork()
    {
        base.OnStartNetwork();
        InstanceFinder.SceneManager.OnLoadEnd += OnLoadEnd;
        StartCoroutine(InitializeClientDelayed());
    }

    public override void OnStopNetwork()
    {
        base.OnStopNetwork();
        InstanceFinder.SceneManager.OnLoadEnd -= OnLoadEnd;
        CleanupClient();
    }

    private void OnLoadEnd(SceneLoadEndEventArgs args)
    {
        foreach(UnityEngine.SceneManagement.Scene scene in args.LoadedScenes)
        {
            if(scene.name == "Title")
            {
                CreateRoomUI();
            }
        }
    }

    private void CleanupClient()
    {
        RemovePlayer();
        HandlePlayerLeave(InstanceFinder.ClientManager.Connection);
        DestroyRoomUI(InstanceFinder.ClientManager.Connection);
    }
    #endregion

    #region Client Initialization
    private IEnumerator InitializeClientDelayed()
    {
        Debug.Log("NetworkRoomSystem: Client initialization started");
        
        yield return new WaitForEndOfFrame();
        yield return new WaitForSeconds(INITIAL_DELAY);
        
        if (!IsValidObject())
        {
            Debug.LogWarning("NetworkRoomSystem: Object is invalid or inactive");
            yield break;
        }

        yield return StartCoroutine(WaitForClientReady());
    }

    private IEnumerator WaitForClientReady()
    {
        var clientConnection = GetClientConnection();
        if (clientConnection == null)
        {
            Debug.LogWarning("NetworkRoomSystem: Client connection is null");
            yield break;
        }

        // 연결 활성화 대기
        yield return StartCoroutine(WaitForConnectionActive(clientConnection));
        clientConnection = GetClientConnection();
        if (clientConnection == null || !clientConnection.IsActive)
            yield break;

        // 클라이언트 시작 대기
        yield return StartCoroutine(WaitForClientStarted());
        if (!base.IsClientStarted)
            yield break;

        // // Observer 등록 대기
        // yield return StartCoroutine(WaitForObserverRegistration(clientConnection));
        // clientConnection = GetClientConnection();
        // if (clientConnection == null || !base.Observers.Contains(clientConnection))
        //     yield break;

        // Steam 플레이어 등록
        yield return StartCoroutine(RegisterSteamPlayer());

        // 방 입장 완료
        CompleteRoomJoin();
    }

    private IEnumerator WaitForConnectionActive(NetworkConnection connection)
    {
        Debug.Log("NetworkRoomSystem: Waiting for connection to activate...");
        
        yield return StartCoroutine(WaitForCondition(
            () => {
                connection = GetClientConnection();
                return connection != null && connection.IsActive;
            },
            "Connection activation",
            CONNECTION_TIMEOUT
        ));

        connection = GetClientConnection();
        bool success = connection != null && connection.IsActive;
        Debug.Log(success ? "NetworkRoomSystem: Connection activated successfully" : 
                          "NetworkRoomSystem: Connection activation failed");
    }

    private IEnumerator WaitForClientStarted()
    {
        Debug.Log("NetworkRoomSystem: Waiting for client to start...");
        
        yield return StartCoroutine(WaitForCondition(
            () => base.IsClientStarted && IsValidObject(),
            "Client startup",
            CONNECTION_TIMEOUT
        ));

        bool success = base.IsClientStarted && IsValidObject();
        Debug.Log(success ? "NetworkRoomSystem: Client started successfully" : 
                          "NetworkRoomSystem: Client startup failed");
    }

    private IEnumerator WaitForObserverRegistration(NetworkConnection connection)
    {
        Debug.Log("NetworkRoomSystem: Waiting for observer registration...");
        
        yield return StartCoroutine(WaitForCondition(
            () => {
                connection = GetClientConnection();
                return connection != null && connection.IsValid && 
                       base.Observers.Contains(connection) && IsValidObject();
            },
            "Observer registration",
            CONNECTION_TIMEOUT
        ));

        connection = GetClientConnection();
        bool success = connection != null && connection.IsValid && base.Observers.Contains(connection);
        Debug.Log(success ? "NetworkRoomSystem: Observer registration successful" : 
                          "NetworkRoomSystem: Observer registration failed");
    }

    private IEnumerator RegisterSteamPlayer()
    {
        if (Managers.Network.Type != NetworkType.Steam)
            yield break;

        ulong steamId = SteamUser.GetSteamID().m_SteamID;
        if (steamId == 0)
        {
            Debug.LogWarning("NetworkRoomSystem: Invalid Steam ID");
            yield break;
        }

        Debug.Log($"NetworkRoomSystem: Adding player with Steam ID: {steamId}");
        AddPlayer(steamId);
    }

    private void CompleteRoomJoin()
    {
        Debug.Log("NetworkRoomSystem: Completing room join...");
        HandlePlayerJoin();
        CreateRoomUI();
        Debug.Log("NetworkRoomSystem: Room join completed successfully");
    }
    #endregion

    #region Utility Methods
    private IEnumerator WaitForCondition(Func<bool> condition, string description, float timeout)
    {
        float elapsed = 0f;
        
        while (!condition() && elapsed < timeout)
        {
            Debug.Log($"NetworkRoomSystem: {description} waiting... ({elapsed:F1}s)");
            yield return new WaitForSeconds(WAIT_INTERVAL);
            elapsed += WAIT_INTERVAL;
        }
    }

    public override void OnStopClient()
    {
        base.OnStopClient();
        
        // 클라이언트가 종료될 때 플레이어 제거 요청
        if (base.IsClientStarted && InstanceFinder.ClientManager.Connection != null)
        {
            RemovePlayer();
            HandlePlayerLeave(InstanceFinder.ClientManager.Connection);
            DestroyRoomUI(InstanceFinder.ClientManager.Connection);
        }
    }

    [ServerRpc(RequireOwnership = false)]
    public void AddPlayer(ulong steamId, NetworkConnection connection = null)
    {
        // 연결이 유효한지 확인
        if (connection == null || !connection.IsValid || !connection.IsActive)
        {
            Debug.LogWarning($"NetworkRoomSystem.AddPlayer: Invalid connection - SteamID: {steamId}");
            return;
        }

        // 이미 등록된 연결인지 확인
        if (NetworkConnectionToSteamId.ContainsKey(connection))
        {
            Debug.LogWarning($"NetworkRoomSystem.AddPlayer: Connection {connection.ClientId} already exists");
            NetworkConnectionToSteamId[connection] = steamId; // 업데이트
        }
        else
        {
            NetworkConnectionToSteamId.Add(connection, steamId);
            Debug.Log($"NetworkRoomSystem.AddPlayer: Added player - ClientID: {connection.ClientId}, SteamID: {steamId}");
        }
    }

    [ServerRpc(RequireOwnership = false)]
    public void RemovePlayer(NetworkConnection connection = null)
    {
        // 연결이 유효한지 확인
        if (connection == null)
        {
            Debug.LogWarning("NetworkRoomSystem.RemovePlayer: Connection is null");
            return;
        }

        if (NetworkConnectionToSteamId.ContainsKey(connection))
        {
            ulong steamId = NetworkConnectionToSteamId[connection];
            NetworkConnectionToSteamId.Remove(connection);
            Debug.Log($"NetworkRoomSystem.RemovePlayer: Removed player - ClientID: {connection.ClientId}, SteamID: {steamId}");
        }
        else
        {
            Debug.LogWarning($"NetworkRoomSystem.RemovePlayer: Connection {connection.ClientId} not found");
        }
    }

    public ulong GetSteamId(NetworkConnection connection)
    {
        if(NetworkConnectionToSteamId.TryGetValue(connection, out ulong steamId))
            return steamId;

        return 0;
    }

    private IEnumerator HandleClientJoinDelayed()
    {
        // 클라이언트 연결이 존재하는지 확인
        var clientConnection = InstanceFinder.ClientManager?.Connection;
        if (clientConnection == null)
        {
            Debug.LogWarning("NetworkRoomSystem: Client connection is null");
            yield break;
        }

        // 연결이 완전히 활성화될 때까지 대기
        float timeout = 10f; // 10초 타임아웃
        float elapsed = 0f;
        
        while (!clientConnection.IsActive && elapsed < timeout)
        {
            yield return new WaitForSeconds(0.1f);
            elapsed += 0.1f;
        }

        if (!clientConnection.IsActive)
        {
            Debug.LogWarning($"NetworkRoomSystem: Connection {clientConnection.ClientId} failed to activate within timeout");
            yield break;
        }

        // NetworkBehaviour가 완전히 초기화될 때까지 대기
        elapsed = 0f;
        while (!base.IsClientStarted && elapsed < timeout)
        {
            yield return new WaitForSeconds(0.1f);
            elapsed += 0.1f;
        }

        if (!base.IsClientStarted)
        {
            Debug.LogWarning("NetworkRoomSystem: Client is not started");
            yield break;
        }

        // Observer가 될 때까지 추가 대기
        elapsed = 0f;
        while (!base.Observers.Contains(clientConnection) && elapsed < timeout)
        {
            yield return new WaitForSeconds(0.1f);
            elapsed += 0.1f;
        }

        if (!base.Observers.Contains(clientConnection))
        {
            Debug.LogWarning($"NetworkRoomSystem: Connection {clientConnection.ClientId} is not an observer");
            yield break;
        }

        // Steam ID 유효성 확인
        if(Managers.Network.Type == NetworkType.Steam)
        {
            ulong steamId = SteamUser.GetSteamID().m_SteamID;
            if (steamId == 0)
            {
                Debug.LogWarning("NetworkRoomSystem: Invalid Steam ID");
                yield break;
            }

            Debug.Log($"NetworkRoomSystem: Adding player with Steam ID: {steamId}");
            AddPlayer(steamId);
        }
        
        HandlePlayerJoin();
        CreateRoomUI();
    }

    /// <summary>
    /// 서버에서 플레이어 입장 처리 (내부 메서드)
    /// </summary>
    [ServerRpc(RequireOwnership = false)]
    private void HandlePlayerJoin(NetworkConnection conn = null)
    {
        if (conn == null || !conn.IsValid || !conn.IsActive)
        {
            Debug.LogWarning($"NetworkRoomSystem.HandlePlayerJoin: Invalid connection");
            return;
        }

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
    /// 클라이언트에서 방 입장 요청 (ServerRpc)
    /// </summary>
    [ServerRpc(RequireOwnership = false)]
    public void RequestJoinRoom(NetworkConnection conn = null)
    {
        // 클라이언트에서 서버로 방 입장 요청
        HandlePlayerJoin(conn);
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
    /// 플레이어가 나갈 때 색상을 해제합니다. (내부 메서드)
    /// </summary>
    private void HandlePlayerLeave(NetworkConnection conn)
    {
        if (PlayerColors.ContainsKey(conn))
        {
            ColorType releasedColor = PlayerColors[conn];
            PlayerColors.Remove(conn);
            Debug.Log($"Player {conn.ClientId} left and released color: {releasedColor}");
        }
    }

    /// <summary>
    /// 클라이언트에서 방 퇴장 요청 (ServerRpc)
    /// </summary>
    [ServerRpc(RequireOwnership = false)]
    public void RequestLeaveRoom(NetworkConnection conn = null)
    {
        // 클라이언트에서 서버로 방 퇴장 요청
        HandlePlayerLeave(conn);
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
    private void CreateRoomUI()
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
                ulong m_steamId = GetSteamId(connection.Value);

                if(m_steamId == 0) return;

                CSteamID steamId = new CSteamID(m_steamId);

                string name = SteamFriends.GetFriendPersonaName(steamId);
                Color color = GetPlayerColor(connection.Value);

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
