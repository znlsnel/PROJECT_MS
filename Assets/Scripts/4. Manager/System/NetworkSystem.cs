using System.Linq;
using Cysharp.Threading.Tasks;
using FishNet;
using FishNet.Connection;
using FishNet.Managing.Scened;
using FishNet.Object;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NetworkSystem : NetworkBehaviour
{
    public void Update()
    {
        Debug.Log(Owner);
    }
}
