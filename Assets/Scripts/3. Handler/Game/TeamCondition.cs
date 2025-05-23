using FishNet.Connection;
using FishNet.Object;
using FishNet.Observing;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(menuName = "FishNet/ObserverCondition/TeamCondition")]
public class TeamCondition : ObserverCondition
{
    public override bool ConditionMet(NetworkConnection connection, bool currentlyAdded, out bool notProcessed)
    {
        notProcessed = false;

        NetworkObject target = base.NetworkObject;

        if (connection == null || target == null)
            return false;

        NetworkGameSystem.Instance.Players.TryGetValue(target.Owner, out PlayerInfo targetPlayerInfo);
        if(targetPlayerInfo.IsUnityNull())
            return false;

        NetworkGameSystem.Instance.Players.TryGetValue(connection, out PlayerInfo viewerPlayerInfo);
        if(viewerPlayerInfo.IsUnityNull())
            return false;
        
        return targetPlayerInfo.isDead == viewerPlayerInfo.isDead;
    }

    public override ObserverConditionType GetConditionType()
    {
        return ObserverConditionType.Normal;
    }
}
