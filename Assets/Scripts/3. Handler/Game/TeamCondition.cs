using FishNet.Connection;
using FishNet.Object;
using FishNet.Observing;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(menuName = "FishNet/ObserverCondition/TeamCondition")]
public class TeamCondition : ObserverCondition
{
    public int teamID = 0;

    public override bool ConditionMet(NetworkConnection connection, bool currentlyAdded, out bool notProcessed)
    {
        notProcessed = false;

        if(NetworkObject.IsOwner) return true;

        int id = NetworkObject.NetworkObserver.GetObserverCondition<TeamCondition>().teamID;
        if(id == teamID)
            return true;
        else
            return false;
    }

    public override ObserverConditionType GetConditionType()
    {
        return ObserverConditionType.Normal;
    }
}
