using System.Collections.Generic;
using FishNet;
using FishNet.Connection;
using FishNet.Object;
using UnityEngine;

public class DemeSceneInitializer : SceneInitializer
{
    public override void Initialize()
    {
        NetworkObject[] networkObjects = GetComponentsInChildren<NetworkObject>();

        foreach(NetworkObject networkObject in networkObjects)
        {
            networkObject.Spawn(networkObject.gameObject);
        }
    }
}
