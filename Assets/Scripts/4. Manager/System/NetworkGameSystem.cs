using FishNet.Connection;
using FishNet.Object;
using FishNet.Object.Synchronizing;

public class NetworkGameSystem : NetworkSingleton<NetworkGameSystem>
{
    public readonly SyncVar<bool> IsGameStarted = new SyncVar<bool>(false);
    public readonly SyncVar<GameOptions> GameOptions = new SyncVar<GameOptions>(new GameOptions(1, 300, 3));
    public readonly SyncList<NetworkConnection> Imposters = new SyncList<NetworkConnection>();

    [Server]
    public void StartGame()
    {
        IsGameStarted.Value = true;
    }

    [Server]
    public void EndGame()
    {
        IsGameStarted.Value = false;
    }

    [Server]
    public void SetGameOptions(GameOptions options)
    {
        GameOptions.Value = options;
    }
}

public struct GameOptions
{
    public int imposterCount;
    /// <summary>
    /// expression in seconds
    /// </summary>
    public int dayDuration;
    public int maxDayCount;

    public GameOptions(int imposterCount, int dayDuration, int maxDayCount)
    {
        this.imposterCount = imposterCount;
        this.dayDuration = dayDuration;
        this.maxDayCount = maxDayCount;
    }
}
